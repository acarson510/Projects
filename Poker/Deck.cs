﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    public class Deck : Card
    {
        const int NUM_OF_CARDS = 52;
        private Card[] deck;

        public Deck()
        {
            deck = new Card[NUM_OF_CARDS];
        }
        public Card[] getDeck { get { return deck; } }

        public void setUpDeck()
        {
            int i = 0;
            foreach(SUIT s in Enum.GetValues(typeof(SUIT)))
            {
                foreach(VALUE v in Enum.GetValues(typeof(VALUE)))
                {
                    deck[i] = new Card { MySuit = s, MyValue = v };
                    i++;
                }                
            }

            ShuffleCards();
        }

        public void ShuffleCards()
        {
            Random rand = new Random();
            Card temp;

            for(int shuffleTimes = 0; shuffleTimes < NUM_OF_CARDS; shuffleTimes++)
            {
                for (int i = 0; i < NUM_OF_CARDS; i++)
                {
                    int secondCardIndex = rand.Next(13);
                    temp = deck[i];
                    deck[i] = deck[secondCardIndex];
                    deck[secondCardIndex] = temp;
                }                
            }
        }
    }
}
