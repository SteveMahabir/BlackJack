﻿/*
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
using BlackJackContracts;
using System.ServiceModel;  // WCF namespace

namespace CardsLibrary
{

    public class Shoe
    {
        // ---------------- Private member variables ----------------

        private List<Card> cards = new List<Card>();    // A container of all cards in the shoe
        private int cardIdx;                            // The index of the next card to be drawn
        private int numDecks = 1;                       // Number of decks in the shoe
        private Dictionary<int, ICallback> clientCallbacks = new Dictionary<int, ICallback>();
        private int nextCallbackId = 1;

        // ---------------------- Constructor -----------------------

        public Shoe(int _numDecks)
        {
            this.numDecks = _numDecks;
            cardIdx = 0;
            repopulate();
        }

        // --------------- Public methods / properties --------------
        
        // Draws a new card from the deck!
        public Card Draw()
        {
            if (cardIdx == cards.Count)
                throw new System.IndexOutOfRangeException("The Shoe is empty. Please reset.");
            //TODO : Check cardIdx , If 0 then throw Empty Deck Excption
            Console.WriteLine("Dealing: " + cards[cardIdx].Name);
            Card card = cards[cardIdx++];

            return card;
        }

        //Shuffling
        public void Shuffle()
        {
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

    }
}
