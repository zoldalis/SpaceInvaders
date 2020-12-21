using System;
using TachankaObj;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.Title = "SpaceInvaders";
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;
            Console.SetCursorPosition(5,5);
            Console.Write("#");
            Console.SetCursorPosition(6, 6);
            Console.Write("#");
            Console.MoveBufferArea(4, 4, 5, 5, 10, 5);

            Tachanka tachanka = new Tachanka();
            Console.ReadKey();







        }
    }
}
