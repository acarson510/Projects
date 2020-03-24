using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    public enum Hand
    {
        Nothing,
        OnePair,
        TwoPairs,
        ThreeKind,
        Straight,
        Flush,
        FullHouse,
        FourKind
    }
    public struct HandValue
    {
        public int Total { get; set; }
        public int HighCard { get; set; }
    }
    public class HandEvaluator : Card
    {
        private int heartsSum;
        private int diamondSum;
        private int clubSum;
        private int spadesSum;
        private Card[] cards;
        private HandValue handValue;

        public HandEvaluator(Card[] sortedHand)
        {
            heartsSum = 0;
            diamondSum = 0;
            clubSum = 0;
            spadesSum = 0;
            cards = new Card[5];
            handValue = new HandValue();        
        }
        public HandValue HandValues
        {
            get { return handValue; }
            set { handValue = value; }
        }

        public Card [] Cards
        {
            get { return cards; }
            set
            {
                cards[0] = value[0];
                cards[1] = value[1];
                cards[2] = value[2];
                cards[3] = value[3];
                cards[4] = value[4];
            }
        }

        //public Hand EvaluateHand()
        //{
        //    getNumberOfSuit();
        //}

        //private void getNumberOfSuit()
        //{
        //    foreach (var element in Card)
        //    {
        //        if (element.MySuit == Card.SUIT.HEARTS)
        //            heartsSum++;
        //        else if (element.MySuit == Card.SUIT.DIAMONDS)
        //            diamondSum++;
        //        else if (element.MySuit == Card.SUIT.CLUBS)
        //            clubSum++;
        //        else if (element.MySuit == Card.SUIT.SPADES)
        //            spadesSum++;
        //    }
        //}



    }
}
