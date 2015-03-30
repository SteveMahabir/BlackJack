using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsLibrary
{
    class Player
    {
        int money;
        List<Card> hand;

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
