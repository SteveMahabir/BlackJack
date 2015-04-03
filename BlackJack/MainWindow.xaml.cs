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


namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            try
            {
                // Configure the Endpoint details
                //DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "Game");

                //// Activate a remote Shoe object
                //shoe = channel.CreateChannel();

                //// Register for callbacks
                //myCallbackId = shoe.RegisterForCallbacks();

                //// Set-up the slider control
                //sliderDecks.Minimum = 1;
                //sliderDecks.Maximum = 10;
                //sliderDecks.Value = shoe.NumDecks;

                //updateCardCounts();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public enum round { bet = 0, deal, play, dealer, work };
        public void disableUIforRound( round r ) {
            switch (r)
            {
                case round.bet:
                    btn_StarRound.IsEnabled = false;
                    btn_Bet.IsEnabled = true;
                    sldr_BetAmount.IsEnabled = true;
                    btn_Hit.IsEnabled = false;
                    btn_Stay.IsEnabled = false;
                    break;
                case round.deal:
                    btn_Bet.IsEnabled = false;
                    sldr_BetAmount.IsEnabled = false;
                    break;
                case round.play:
                     btn_Hit.IsEnabled = true;
                     btn_Stay.IsEnabled = true;
                    break;
                case round.dealer:
                    btn_Hit.IsEnabled = false;
                    btn_Stay.IsEnabled = false;
                    btn_Bet.IsEnabled = false;
                    sldr_BetAmount.IsEnabled = false;
                    break;
                case round.work:
                    btn_Hit.IsEnabled = false;
                    btn_Stay.IsEnabled = false;
                    btn_Bet.IsEnabled = false;
                    sldr_BetAmount.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private void btn_Bet_Click(object sender, RoutedEventArgs e)
        {
            btn_StarRound.IsEnabled = true;
            btn_Bet.IsEnabled = false;
        }

        private void btn_StarRound_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Hit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Stay_Click(object sender, RoutedEventArgs e)
        {

        }

    }




   

}
