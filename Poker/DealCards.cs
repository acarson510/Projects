using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Poker
{
    public class DealCards : Deck
    {
        private Card[] playerHand;
        private Card[] computerHand;
        private Card[] sortedPlayerHand;
        private Card[] sortedComputerHand;

        private Card[] flop;
        private Card turn;
        private Card river;
        public DealCards()
        {
            playerHand = new Card[2];
            sortedPlayerHand = new Card[2];
            computerHand = new Card[2];
            sortedComputerHand = new Card[2];

            flop = new Card[3];            
        }

        public void Deal()
        {
            setUpDeck();
            getHand();
            sortCards();
            displayCards();
            //evaluateHands();

        }

        public void DealFlop()
        {
            getFlop();
            displayFlop();
        }

        public void DealTurn()
        {
            getTurn();
            displayTurn();
        }

        public void DealRiver()
        {
            getRiver();
            displayRiver();
        }

        public void getHand()
        {
            
            //var a = getDeck;
            //5 cards for player
            for (int i = 0; i < 2; i++)
                playerHand[i] = getDeck[i];

            //5 cards for computer
            for (int i = 2; i < 4; i++)
                computerHand[i - 2] = getDeck[i];

        }

        public void getFlop()
        {
            //3 cards for flop
            for (int i = 4; i < 7; i++)
                flop[i - 4] = getDeck[i];
        }

        public void getTurn()
        {
            //3 cards for flop
            for (int i = 7; i < 8; i++)
                turn = getDeck[i];
        }

        public void getRiver()
        {
            //3 cards for flop
            for (int i = 8; i < 9; i++)
                river = getDeck[i];
        }

        public void sortCards()
        {
            var queryPlayer = from hand in playerHand
                              orderby hand.MyValue
                              select hand;

            var queryComputer = from hand in computerHand
                                orderby hand.MyValue
                                select hand;

            var index = 0;
            foreach(var element in queryPlayer.ToList())
            {
                sortedPlayerHand[index] = element;
                index++;
            }

            index = 0;
            foreach (var element in queryComputer.ToList())
            {
                sortedComputerHand[index] = element;
                index++;
            }
        }

        public void displayCards()
        {
            Console.Clear();
            
            int y = 1;
            int x = 0;

            //display player hand
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Player's hand:");

            for(int i = 0; i < 2; i++)
            {
                DrawCards.DrawCardOutline(x, y);
                DrawCards.DrawCardSuitValue(sortedPlayerHand[i], x, y);
                x++;
            }

            y = 15;
            x = 0;

            Console.SetCursorPosition(x, 14);

            //display computer hand            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Computer's hand:");
                                   
            for (int i = 2; i < 4; i++)
            {
                DrawCards.DrawCardOutline(x, y);
                DrawCards.DrawCardSuitValue(sortedComputerHand[i - 2], x, y);
                x++;
            }

        }

        public void displayFlop()
        {
            //Console.Clear();
            
            int y = 30;
            int x = 0;

            
            Console.SetCursorPosition(x, 29);

            //display flop
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Flop:");

            for (int i = 4; i < 7; i++)
            {
                DrawCards.DrawCardOutline(x, y);
                DrawCards.DrawCardSuitValue(flop[i - 4], x, y);
                x++;
            }
        }

        public void displayTurn()
        {
            //Console.Clear();

            int y = 30;
            int x = 36;

            Console.SetCursorPosition(x, 29);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Turn:");

            DrawCards.DrawCardOutline(3, y);
            DrawCards.DrawCardSuitValue(turn, 3, y);
        }

        public void displayRiver()
        {
            //Console.Clear();

            int y = 30;
            int x = 48;

            Console.SetCursorPosition(x, 29);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("River:");

            DrawCards.DrawCardOutline(4, y);
            DrawCards.DrawCardSuitValue(river, 4, y);
        }

        public bool evaluateHands()
        {
            List<Card> cards = new List<Card>();            

            cards.AddRange(sortedComputerHand);
            cards.AddRange(flop);
            cards.Add(turn);
            cards.Add(river);

            bool hits = false;
            var a = cards.GroupBy(c => c.MyValue).Where(grp => grp.Count() == 2).Select(grp => grp.Key);

            if (a.Count() == 1)
            {
                hits = true;
            }

            return hits;
        }
    }
}
