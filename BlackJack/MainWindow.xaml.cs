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
            // Toggle UI Controls
            btn_StarRound.IsEnabled = true;
            btn_Bet.IsEnabled = false;
            game.Bet(myCallbackId, Convert.ToInt32(lbl_PlayerScore.Content));
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

            // User Prompt
            MessageBox.Show("Game started, waiting for other players to join...", "Waiting...", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Bust!", "Sorry!", MessageBoxButton.OK, MessageBoxImage.Error);
                btn_Hit.IsEnabled = false;
                btn_Stay.IsEnabled = false;
            }
        }

        // 
        private void btn_Stay_Click(object sender, RoutedEventArgs e)
        {
            btn_Stay.IsEnabled = false;
            btn_Hit.IsEnabled = false;
            MessageBox.Show("Waiting for other players to finish","CURRENT SCORE: " +me.handScore.ToString(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
            game.Stay(myCallbackId);
            UpdateDealer(false);
            
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
                foreach (Card c in dealer.hand)
                    lst_DealerCards.Items.Add(c.Name);
        }

        // Implement the callback contract
        private delegate void ClientUpdateDelegate(CallbackInfo info);

        public void UpdateGui(CallbackInfo info)
        {
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                me = info.Players[myCallbackId];
                UpdateMe();
                UpdateDealer(true);

                if (info.gameFinished)
                    FinishRound();
            }
            else
            {
                // Only the main (dispatch) thread can change the GUI
                this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGui), info);
            }

        }

        //TODO  make a void method to END GAME
        public void FinishRound()
        {
            lbl_MoneyAmount.Content = (Convert.ToInt32(lbl_PlayerScore.Content) + me.bet).ToString();
            
        }
    }




   

}
