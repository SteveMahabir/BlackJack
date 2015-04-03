using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel; // Needed for .Net
using System.Runtime.Serialization;

namespace CardsLibrary
{
    [DataContract]
    class Player
    {
        /* Data Members Passed Back to the Client */
        [DataMember]
        public int money;

        [DataMember]
        public List<Card> hand;

        [DataMember]
        public int bet;

        [DataMember]
        public int handScore;


        Player()
        {
            money = 100;
            hand = new List<Card>();
            bet = 5;
            handScore = 0;
        }

        //TODO: Put this 
        public int calcScore(){
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
            if ( aceFound & ret_score > 21)
            {
                while (ret_score > 21)
                {
                    if (aceCount == 0)
                    {
                        break;
                    }
                    ret_score = -10;
                    aceCount--;

                }
            }

            return ret_score;
        }


    }
}
