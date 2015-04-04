using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.Runtime.Serialization;

namespace BlackJackContracts
{
    [DataContract]
    public class Card
    {
        // ---------- Enums for the various suits and ranks ---------
        public enum SuitID { Clubs, Diamonds, Hearts, Spades };
        public enum RankID { Ace, King, Queen, Jack, Ten, Nine, Eight, Seven, Six, Five, Four, Three, Two };

        // --------------- Public methods / properties --------------

        #region Getters and Setters
        [DataMember]
        public SuitID Suit { get; private set; }

        [DataMember]
        public RankID Rank { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        public int score { get; private set; }

        #endregion

        // ---------------------- Constructor -----------------------

        public Card(SuitID s, RankID r)
        {
            Suit = s;
            Rank = r;
            Name = Rank.ToString() + " of " + Suit.ToString();

            switch (r)
            {
                case RankID.Ace:
                    score = 11;
                    break;
                case RankID.King:
                    score = 10;
                    break;
                case RankID.Queen:
                    score = 10;
                    break;
                case RankID.Jack:
                    score = 10;
                    break;
                case RankID.Ten:
                    score = 10;
                    break;
                case RankID.Nine:
                    score = 9;
                    break;
                case RankID.Eight:
                    score = 8;
                    break;
                case RankID.Seven:
                    score = 7;
                    break;
                case RankID.Six:
                    score = 6;
                    break;
                case RankID.Five:
                    score = 5;
                    break;
                case RankID.Four:
                    score = 4;
                    break;
                case RankID.Three:
                    score = 3;
                    break;
                case RankID.Two:
                    score = 2;
                    break;
                default:
                    break;
            }

        }
    }

    [DataContract]
    public class Player
    {
        /* Data Members Passed Back to the Client */
        #region Data Members
        [DataMember]
        public int money;

        [DataMember]
        public List<Card> hand;

        [DataMember]
        public int bet;

        [DataMember]
        public int handScore;

        [DataMember]
        public bool isReady;

        [DataMember]
        public bool stay;

        [DataMember]
        public string message;

        #endregion

        // Constructor
        public Player()
        {
            money = 100;
            hand = new List<Card>();
            bet = 5;
            handScore = 0;
            isReady = false;
            stay = false;
        }

    }

    [DataContract]
    public class CallbackInfo
    {
        [DataMember]
        public Dictionary<int, Player> Players { get; private set; }
        
        public CallbackInfo(Dictionary<int, Player> _players)
        {
            Players = _players;
        }

        [DataMember]
        public int NumCards { get; private set; }
        [DataMember]
        public int NumDecks { get; private set; }
        [DataMember]
        public bool EmptyHand { get; private set; }

        public CallbackInfo(int c, int d, bool e)
        {
            NumCards = c;
            NumDecks = d;
            EmptyHand = e;
        }
    }
}
