using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;  // WCF namespace

namespace CardsLibrary
{

    [ServiceContract]
    interface IGame
    {
        [OperationContract]
        public Card Hit();

        [OperationContract]
        public void Stay();

        [OperationContract]
        public void Bet();

        [OperationContract]
        public void StartGame();

        private void Shuffle();

        private void setNumDecks();


    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class Game : IGame
    {

        //Public Data Members - Visible to Clients
        public enum round { work = 0, bet, deal, play, dealer, win };


        //Private Data Members - In-visible to Clients
        private int NumDecks;
        private Shoe gameDeck;

        //Constructor
        private Game()
        {
            Console.WriteLine("Starting a new game");
            // Create the Shoe Object used for the Game
            gameDeck = new Shoe();

            // Populate the Shoe Object
            gameDeck.NumDecks = 2;

        }


        #region Public Methods - Client Methods : Main Logic

        // Hit - Draws a Card from the Master Shoe Object
        public Card Hit() 
        {
            return gameDeck.Draw();
        }

        void Stay() 
        {

        }

        void Bet()
        {

        }


        void Shuffle() {}

        void setNumDecks() {}

        #endregion

        #region Private Methods - Helper Methods Only Used by the Service
        private int CalculateHandScore()
        {
            return 0;
        }


        //
        

        #endregion
    }
}
