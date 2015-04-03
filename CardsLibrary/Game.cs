using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;  // WCF namespace

namespace CardsLibrary
{

    [ServiceContract]
    public interface IGame
    {
        [OperationContract]
        Card Hit();

        [OperationContract]
        void Stay();

        [OperationContract]
        void Bet();

        [OperationContract]
        void StartGame();

    }

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
	

            //// at this point everyone has bet, so we need to deal cards to all the registered playerd
	
            //for( int i = 0; i < players.length; ++i ){
            //    //deal 2 card	
            //} 
            ////give dealer two cards
	
	
            //for( 5 times ){
            //    //wait?
            //    foreach( player ){
            //        //each player, check if they have stayed			
            //    }
            //    //if all players have stayed, break;
            //}
	
            //dealer plays{
            //    //dealer score logic
            //}
	
            //int daler score = calculate dealer score
	
	
            //foreach( player ){
            //    //if score is bigger then the dealer, payout the bet amount
		
            //}
		
            //work round;
            //make a new master deck with the number of registered players + 1
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
