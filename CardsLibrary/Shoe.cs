/*
 * Program:         CardsLibrary.dll
 * Module:          Shoe.cs
 * Author:          T. Haworth
 * Date:            January 13, 2015
 * Description:     Defines the Shoe type that manages a collection of Card 
 *                  objects simulating a casino-style "shoe" consisting of
 *                  multiple decks of standard playing cards.
 * Modifications:   Jan 20 - Modified repopulate() to invoke Shuffle() as final step
 *                  Mar 3  - Modified to expose Shoe as a basic WCF service.
 *                           Note that the Draw() method has been temporarily modified 
 *                           to return a string instead of a Card object reference.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;  // WCF namespace

namespace CardsLibrary
{
    // This is the Callback Contract that each client will implement
    [ServiceContract]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateGui(CallbackInfo info);
    }


    // This is the Contract for the Shoe service endpoint
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IShoe
    {
        [OperationContract]
        Card Draw();

        [OperationContract(IsOneWay = true)]
        void Shuffle();

        int NumDecks
        {
            [OperationContract]
            get;
            [OperationContract(IsOneWay = true)]
            set;
        }
        int NumCards
        {
            [OperationContract]
            get;
        }

        [OperationContract]
        int RegisterForCallbacks();
        
        
    }

    // This is the implementation of the service 
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Shoe : IShoe
    {
        // ---------------- Private member variables ----------------

        private List<Card> cards = new List<Card>();    // A container of all cards in the shoe
        private int cardIdx;                            // The index of the next card to be drawn
        private int numDecks = 1;                       // Number of decks in the shoe
        private Dictionary<int, ICallback> clientCallbacks = new Dictionary<int, ICallback>();
        private int nextCallbackId = 1;

        // ---------------------- Constructor -----------------------

        public Shoe()
        {
            Console.WriteLine("Constructing a Shoe object");

            cardIdx = 0;
            repopulate();
        }

        // --------------- Public methods / properties --------------

        // A public method that returns the next Card object which is the object at index
        // cardIdx, and also increments the cardIdx so that a different card will be returned
        // the next time this method is called. If there are no more cards (cardIdx is > the 
        // last index) it throws an exception.
        public Card Draw()
        {
            if (cardIdx == cards.Count)
                throw new System.IndexOutOfRangeException("The Shoe is empty. Please reset.");

            Console.WriteLine("Dealing: " + cards[cardIdx].Name);
            Card card = cards[cardIdx++];

            UpdateAllClients(false);

            return card;
        }

        // A public method that will randomize the order of the cards in the shoe
        // and will reset the cardIdx to zero.
        public void Shuffle()
        {
            Console.WriteLine("Shuffling the Shoe");

            Random rand = new Random();
            Card temp;

            for (int i = 0; i < cards.Count; ++i)
            {
                // Choose a random index
                int randIdx = rand.Next(cards.Count);

                if (randIdx != i)
                {
                    // Swap
                    temp = cards[i];
                    cards[i] = cards[randIdx];
                    cards[randIdx] = temp;
                }
            }

            // Reset the cardIdx
            cardIdx = 0;

            UpdateAllClients(true);
        }

        // A public property that allows the client to read the number of decks in 
        // the shoe AND to reset the number of decks.
        public int NumDecks
        {
            get
            {
                return numDecks;
            }
            set
            {
                if (numDecks != value)
                {
                    numDecks = value;
                    repopulate();
                }
            }

        }

        // Read-only public property that returns the number of cards
        // in the shoe that haven't yet been "drawn" 
        public int NumCards
        {
            get { return cards.Count - cardIdx; }
        }

        public int RegisterForCallbacks()
        {
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            clientCallbacks.Add(nextCallbackId, cb);
            return nextCallbackId++;
        }

        public void UnregisterForCallbacks(int id) {
            clientCallbacks.Remove(id);
        }
        
        // --------------------- Helper methods ---------------------

        private void repopulate()
        {
            Console.WriteLine("Repopulating the Shoe with {0} deck(s)", numDecks);

            // Remove "old" cards
            cards.Clear();

            // Populate with new cards
            for (int d = 0; d < numDecks; ++d)
            {
                foreach (Card.SuitID s in Enum.GetValues(typeof(Card.SuitID)))
                {
                    foreach (Card.RankID r in Enum.GetValues(typeof(Card.RankID)))
                    {
                        // Add a new card with suit s and rank r
                        cards.Add(new Card(s, r));
                    }
                }
            }

            // Cards are in a logical sequence, so randomize the collection
            Shuffle();
        }

        //arg is to tell the client if they need to empty the hand with this call
        private void UpdateAllClients(bool emptyHand)
        {
            //create data transpher pobject
            CallbackInfo info = new CallbackInfo(cards.Count - cardIdx, numDecks, emptyHand);

            //update all clients cia the callback contract

            foreach (KeyValuePair<int, ICallback> cb in clientCallbacks)
            {
                cb.Value.UpdateGui(info);
            }
        }

    }
}
