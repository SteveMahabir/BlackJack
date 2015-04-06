using System;
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
        #region Data Members

        // Callback Data Members
        private Dictionary<int, ICallback> clientCallbacks = new Dictionary<int, ICallback>();
        private int nextCallbackId = 1;

        // Dictionary Holding all Players
        public Dictionary<int, Player> players = new Dictionary<int, Player>();

        //Public Data Members - Visible to Clients
        public enum round { work = 0, bet, deal, play, dealer, win };

        //Private Data Members - In-visible to Clients
        private Shoe gameDeck;

        // Global Flags
        private bool roundStarted = false;
        
        #endregion

        //Constructor
        private Game()
        {
            Console.WriteLine("Game Started, waiting for players...");
            // Create the Shoe Object used for the Game
            gameDeck = new Shoe(2);

            // Create a Dealer
            players.Add(0, new Player());
            players[0].isReady = true;
        }


        #region Public Methods - Client Methods : Main Logic
        public void StartGame(int playerId)
        {
            // Player is Ready!
            players[playerId].isReady = true;

            bool allPlayersReady = false;
            foreach (Player p in players.Values)
                if (p.isReady == true)
                    allPlayersReady = true;
                else
                    allPlayersReady = false;

            if (allPlayersReady)
                BeginRound();
        }

        // All Players Ready!
        private void BeginRound()
        {
            // Deal two Cards to Everyone
            foreach (var p in players.Values)
            {
                p.hand.Add(gameDeck.Draw());
                p.hand.Add(gameDeck.Draw());
                p.handScore = CalculateHandScore(p.hand);
            }

            // Update Clients
            updateAllClients(false, true);

            roundStarted = true;
        }


        // Hit - Draws a Card from the Master Shoe Object
        public void Hit(int id)
        {
            try
            {
                Card temp = gameDeck.Draw();
                players[id].hand.Add(temp);
                players[id].handScore = CalculateHandScore(players[id].hand);
                if (players[id].handScore > 21)
                    players[id].stay = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // Stay - Holds the Card
        public void Stay(int id)
        {
            players[id].stay = true;

            bool allPlayersStay = false;
            foreach(Player p in players.Values)
            {
                if (p.stay)
                    allPlayersStay = true;
                else
                    allPlayersStay = false;
            }

            if (allPlayersStay)
                FinishRound();

        }

        public void FinishRound()
        {
            // Dealer Plays
            while (players[0].handScore < 16)
            {
                players[0].hand.Add(gameDeck.Draw());
                players[0].handScore = CalculateHandScore(players[0].hand);
            }

            foreach (Player p in players.Values)
            {
                if (p.handScore > players[0].handScore)
                {
                    p.message = "You Won!";
                }
                else
                    p.message = "Sorry, Dealer Wins";
            }

            updateAllClients(true, false);

            roundStarted = false;
        }

        public void ClearMe(int id)
        {
            players[id] = new Player();
        }

        public void Bet(int id, int betAmount)
        {
            players[id].bet = betAmount;
        }

        // Returns a specific player
        public Player GetPlayerbyId(int _id)
        {
            return players[_id];
        }

        // Returns the Dealers Info
        public Player GetDealer()
        {
            return players[0];
        }


        #endregion

        #region Private Methods - Helper Methods Only Used by the Service

        // Used to obtain a score based on a players hand
        private int CalculateHandScore(List<Card> hand)
        {
            int ret_score = 0;
            bool aceFound = false;
            int aceCount = 0;

            //get toal scre from cards in hand
            foreach (Card c in hand)
            {
                if (c.Rank == Card.RankID.Ace)
                {
                    aceFound = true;
                    aceCount++;
                }

                ret_score += c.score;
            }

            //adjust for aces
            if (aceFound & ret_score > 21)
            {
                while (ret_score > 21)
                {
                    if (aceCount == 0)
                    {
                        break;
                    }
                    ret_score -= 10;
                    aceCount--;

                }
            }

            return ret_score;
        }

        // Shuffle the Deck
        void Shuffle()
        {
            gameDeck.Shuffle();
        }

        // Sets the Number of Decks
        void setNumDecks(int _numDecks)
        {
            gameDeck.NumDecks = _numDecks;
        }

        // Register a Player for the game
        public int RegisterForCallbacks()
        {
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            clientCallbacks.Add(nextCallbackId, cb);

            players.Add(nextCallbackId, new Player());

            return nextCallbackId++;
        }

        // Unregister a Player
        public void UnregisterForCallbacks(int id)
        {
            clientCallbacks.Remove(id);
        }

        private void updateAllClients(bool finished, bool generalMessage)
        {
            // Create and initialize the data transfer object
            CallbackInfo info = new CallbackInfo(players, finished, generalMessage);

            // Update all clients via the callback contract
            foreach (ICallback cb in clientCallbacks.Values)
                cb.UpdateGui(info);
        }

        #endregion

    }
}
