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

        public enum SuitID
        {
            Clubs, Diamonds, Hearts, Spades
        };

        public enum RankID
        {
            Ace, King, Queen, Jack, Ten, Nine, Eight, Seven, Six,
            Five, Four, Three, Two
        };



        // --------------- Public methods / properties --------------

        [DataMember]
        public SuitID Suit { get; private set; }

        [DataMember]
        public RankID Rank { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        public int score { get; private set; }

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
}
