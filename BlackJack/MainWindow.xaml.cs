using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ServiceModel;  // WCF namespace
using BlackJackContracts;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public partial class MainWindow : Window, ICallback
    {
        private IGame game = null;
        private int myCallbackId = -1;
        private Player me = null;
        private Player dealer = null;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                // Configure the Endpoint details
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "Game");

                // Activate a remote game object
                game = channel.CreateChannel();

                // Register for callbacks
                myCallbackId = game.RegisterForCallbacks();
                me = game.GetPlayerbyId(myCallbackId);

                // User Prompt
                MessageBox.Show("Welcome Player " + myCallbackId + "\nPlease place a bet then start the game", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

                //Toggle UI Controls
                btn_Hit.IsEnabled = false;
                btn_StarRound.IsEnabled = false;
                btn_Stay.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Bet_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(lbl_BetAmount.Content) > Convert.ToInt32(lbl_MoneyAmount.Content))
                MessageBox.Show("Low on Funds", "Nice Try...", MessageBoxButton.OK, MessageBoxImage.Stop);
            else
            { 
                // Toggle UI Controls
                btn_StarRound.IsEnabled = true;
                btn_Bet.IsEnabled = false;
                sldr_BetAmount.IsEnabled = false;
                game.Bet(myCallbackId, Convert.ToInt32(lbl_PlayerScore.Content));
            }
        }

        private void btn_StartRound_Click(object sender, RoutedEventArgs e)
        {
            // Toggle UI Controls
            btn_Bet.IsEnabled = false;
            btn_StarRound.IsEnabled = false;
            btn_Hit.IsEnabled = true;
            btn_Stay.IsEnabled = true;

            // Call the Game Start
            game.StartGame(myCallbackId);
        }

        private void btn_Hit_Click(object sender, RoutedEventArgs e)
        {
            // Call the Game Object
            game.Hit(myCallbackId);

            // Update me
            UpdateMe();
            
            // Handle the Score Logic
            if (me.handScore > 21)
            {
                MessageBox.Show("Bust! Sorry you have lost this round", "Dealer Wins!", MessageBoxButton.OK, MessageBoxImage.Error);
                btn_Hit.IsEnabled = false;
                btn_Stay.IsEnabled = false;
                btn_Stay_Click(sender, e);
            }
        }

        // 
        private void btn_Stay_Click(object sender, RoutedEventArgs e)
        {
            btn_Stay.IsEnabled = false;
            btn_Hit.IsEnabled = false;
            game.Stay(myCallbackId);
        }

        public void UpdateMe()
        {
            // Get the new Cards in Hand
            me = game.GetPlayerbyId(myCallbackId);
            lst_PlayerCards.Items.Clear();
            foreach (Card c in me.hand)
                lst_PlayerCards.Items.Add(c.Name);
            
            // Update the UI
            lbl_PlayerScore.Content = me.handScore.ToString();
        }
        public void UpdateDealer(bool hidden)
        {
            dealer = game.GetDealer();
            lst_DealerCards.Items.Clear();

            if(hidden)
            {
                lst_DealerCards.Items.Add("[Hidden]");
                lst_DealerCards.Items.Add(dealer.hand[1].Name);
                lbl_DealerScore.Content = "Unknown";
            }
            else
            { 
                foreach (Card c in dealer.hand)
                    lst_DealerCards.Items.Add(c.Name);
                lbl_DealerScore.Content = dealer.handScore.ToString();
            }
        }

        // Implement the callback contract
        private delegate void ClientUpdateDelegate(CallbackInfo info);

        public void UpdateGui(CallbackInfo info)
        {
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                if (info.generalMessage)
                {
                    // Game has started, lets begin the round!
                    MessageBox.Show("All Players are now ready!");
                    UpdateMe();
                    UpdateDealer(true);
                }
                else
                { 
                    me = info.Players[myCallbackId];
                    UpdateMe();
                    UpdateDealer(true);

                    if (info.gameFinished)
                        FinishRound();
                }
            }
            else
            {
                // Only the main (dispatch) thread can change the GUI
                this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGui), info);
            }

        }

        // This Round has Finished
        public void FinishRound()
        {
            // Show the Dealer Cards and Score
            UpdateDealer(false);

            // Player Outcome Logic
            
            if (dealer.handScore > 21 && me.handScore > 21) // Both Lost
            {
                MessageBox.Show("Both you and the dealer busts... No one wins", "Tie!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (dealer.handScore > 21) // Dealer Busts
            {
                MessageBox.Show("Congratulations, You Won!", "Winner!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                lbl_MoneyAmount.Content = (Convert.ToInt32(lbl_MoneyAmount.Content) + me.bet).ToString();
            }
            else if (me.handScore > 21)
            {
                lbl_MoneyAmount.Content = (Convert.ToInt32(lbl_MoneyAmount.Content) - me.bet).ToString();
            }
            else if (dealer.handScore < me.handScore) // Both are under 22
            {
                MessageBox.Show("Congratulations, You Won!", "Winner!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                lbl_MoneyAmount.Content = (Convert.ToInt32(lbl_MoneyAmount.Content) + me.bet).ToString();
            }
            else
            { 
                MessageBox.Show("Sorry, Dealer Wins...", "Lost!", MessageBoxButton.OK, MessageBoxImage.Warning);
                lbl_MoneyAmount.Content = (Convert.ToInt32(lbl_MoneyAmount.Content) - me.bet).ToString();
            }

            // Clear Dealer and Player
            lst_DealerCards.Items.Clear();
            lst_PlayerCards.Items.Clear();
            lbl_DealerScore.Content = "0";
            lbl_PlayerScore.Content = "0";

            // Enable bets
            btn_Bet.IsEnabled = true;
            sldr_BetAmount.IsEnabled = true;

            // Disable Plays
            btn_Hit.IsEnabled = false;
            btn_Stay.IsEnabled = false;
            btn_StarRound.IsEnabled = false;

            game.ClearMe(myCallbackId);
        }
    }




   

}
