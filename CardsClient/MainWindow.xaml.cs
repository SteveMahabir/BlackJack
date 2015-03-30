/*
 * Program:         CardsClient.exe
 * Module:          MainWindow.xaml.cs
 * Author:          T. Haworth
 * Date:            January 20, 2015
 * Description:     Complete Windows WPF client that will use and demonstrate
 *                  the features/sevices of the CardsLibrary.dll assembly.
 * Modifications:   Mar 9 - Modified to use CardsLibrary.Shoe as a WCF service.
 *                  Mar 17 - Modified to use CardsLibrary.Card as a WCF Data Transfer Object
 */

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
using CardsLibrary;         // namespace for Shoe and Card

namespace CardsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext =true)]
    public partial class MainWindow : Window, ICallback
    {
        // C'tor
        private IShoe shoe = null;
        private int mycallbackNumber = -1;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                // Configure the Endpoint details
                DuplexChannelFactory<IShoe> channel = new DuplexChannelFactory<IShoe>( this,  "Shoe" );
                              

                // Activate a remote Shoe object
                shoe = channel.CreateChannel();

                //register for callbacks from the server
                mycallbackNumber = shoe.RegisterForCallbacks();

                // Set-up the slider control
                sliderDecks.Minimum = 1;
                sliderDecks.Maximum = 10;
                sliderDecks.Value = shoe.NumDecks;

                updateCardCounts();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // Event handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Card card = shoe.Draw();
                lstCards.Items.Insert(0, card.Name);

                updateCardCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // Shuffle the shoe
                shoe.Shuffle();

                // Reset the "hand" listbox
                // lstCards.Items.Clear();

                updateCardCounts();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sliderDecks_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try 
            {
                if (shoe != null)
                {
                    // Set the number of decks
                    shoe.NumDecks = (int)(sliderDecks.Value);
                    if (shoe.NumDecks == 1)
                        txtDeckCount.Text = "1 Deck";
                    else
                        txtDeckCount.Text = shoe.NumDecks + " Decks";

                    // Reset the "hand" listbox
                    lstCards.Items.Clear();

                    updateCardCounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Helper methods

        private void updateCardCounts()
        {
            txtHandCount.Text = lstCards.Items.Count.ToString();
            //txtShoeCount.Text = shoe.NumCards.ToString(); - redundent
        }

        private delegate void ClientUpdateDelegate(CallbackInfo info);

        //Implement the callback contract
        //callback will run on a different - the tread running this method cannot MOD the GUI
        //will have to call a dispatch method in the main
        public void UpdateGui(CallbackInfo info)
        {
            //check if this is the dispatch thread
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                //thread trying to update the gui is the thread executing the GUI - so update the GUI
                txtShoeCount.Text = info.NumCards.ToString();
                sliderDecks.Value = info.NumDecks;
                txtDeckCount.Text = info.NumDecks == 1 ? "1 Deck" : info.NumCards + "Decks";

                if (info.EmptyHand)
                {
                    lstCards.Items.Clear();
                    txtHandCount.Text = "0";
                }
            }
            else
            {
                //as the dispatch thread to make this call
                //only the main dispatch thread can change the GUI
                this.Dispatcher.BeginInvoke( new ClientUpdateDelegate( UpdateGui ),info );

            }

            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            shoe.UnregisteredForCallbacks(mycallbackNumber);
        }
    }
}
