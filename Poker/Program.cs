using System;

namespace Poker
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.SetWindowSize(65, 40);
            // Console.BufferWidth = 65;
            //Console.BufferWidth = 100;
            //Console.BufferHeight = 40;
            //Console.BufferHeight = 1000;
            Console.Title = "Poker Game";
            DealCards dc = new DealCards();

            bool quit = false;

            //register players

            dc.Deal();

            //bet

            dc.DealFlop();

            //bet

            dc.DealTurn();

            //bet

            dc.DealRiver();

            //bet 
            Console.WriteLine();
            dc.evaluateHands();



            Console.ReadLine();

            //while (!quit)
            //{
            //    dc.Deal();

            //    char selection = ' ';
            //    while (!selection.Equals('Y') && !selection.Equals('N'))
            //    {
            //        //Console.WriteLine("Play again? Y-N");
            //        selection = Convert.ToChar(Console.ReadLine().ToUpper());

            //        if (selection.Equals('Y'))
            //            quit = false;
            //        else if (selection.Equals('N'))
            //            quit = true;
            //        else
            //            Console.WriteLine("Invalid Selection. Try again.");
            //    }
            //}
        }
    }
}
