﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BlackJackContracts;

namespace CardsLibrary
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        //Public Data Members - Visible to Clients
        public enum round { work = 0, bet, deal, play, dealer, win };


        //Private Data Members - In-visible to Clients
        private Shoe gameDeck;

        //Constructor
        private Game()
        {
            Console.WriteLine("Starting a new game");
            // Create the Shoe Object used for the Game
            gameDeck = new Shoe(2);

        }


        #region Public Methods - Client Methods : Main Logic
        public void StartGame()
        {

            //make a new list of players from the people who have joined.
            //// this way is someone registers in the middle of a game they are ignored untill the next round starts
            Dictionary<int, ICallback> players = clientCallbacks;
            Player dealer = new Player();

            //// at this point everyone has bet, so we need to deal cards to all the registered playerd

            //deal cards
            foreach (var p in players.Values)
            {
                //p.hand.add(gameDeck.Draw());
                //p.hand.add(gameDeck.Draw());
            }

            ////give dealer two cards
            dealer.hand.Add(gameDeck.Draw());
            dealer.hand.Add(gameDeck.Draw());

            bool allPlayersStay = true;
            //wait 25 seconds
            for (int i = 0; i < 5; i++)
            {
                //sleep for 5 seconds
                System.Threading.Thread.Sleep(5000);
                allPlayersStay = true;
                foreach (var p in players.Values)
                {
                    //if (!p.stay)
                    //{
                    //    allPlayersStay = false;
                    //break;
                    //}
                }
                if (allPlayersStay)
                {
                    break;
                }
            }

            //time up message?

            //dealer plays
            //int daler score = calculate dealer score
            int dealerScore = CalculateHandScore(dealer.hand);

            //dealer hits on 16 stays on 17
            while (dealerScore < 16)
            {
                dealer.hand.Add(gameDeck.Draw());
                dealerScore = CalculateHandScore(dealer.hand);
            }

            //foreach( player ){
            //    //if score is bigger then the dealer, payout the bet amount

            //}

            foreach (var p in players.Values)
            {
                int playerScore = 0 /* CalculateHandScore( p.hand )*/;
                //if not bust
                if (!(playerScore > 21))
                {
                    if (playerScore > dealerScore)
                    {
                        //p.money += p.bet;
                        //p.message = "You won " + p.bet + "$!";
                    }
                }
                else
                {
                    //bust
                    //p.message = "Bust!"
                }
            }

            //work round;
            //make a new master deck with the number of registered players + 1
            //reset all players bet amount to 0;

            gameDeck.NumDecks = clientCallbacks.Values.Count() + 1;

            foreach (var p in clientCallbacks)
            {
                //p.bet = 0;
            }

        }

        // Hit - Draws a Card from the Master Shoe Object
        public Card Hit()
        {
            return gameDeck.Draw();
        }

        public void Stay()
        {

        }

        public void Bet()
        {

        }

        #endregion

        #region Private Methods - Helper Methods Only Used by the Service
        private int CalculateHandScore()
        {
            return 0;
        }

        void Shuffle()
        {
            gameDeck.Shuffle();
        }

        void setNumDecks(int _numDecks)
        {
            gameDeck.NumDecks = _numDecks;
        }


        //


        #endregion

    }
}
