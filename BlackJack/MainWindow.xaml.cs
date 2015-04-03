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
        }



        public enum round { bet = 0, deal, play, dealer, win };
        public void disableUIforRound( round r ) {
            switch (r)
            {
                case round.bet:
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
                case round.win:
                    btn_Hit.IsEnabled = false;
                    btn_Stay.IsEnabled = false;
                    btn_Bet.IsEnabled = false;
                    sldr_BetAmount.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

    }




   

}
