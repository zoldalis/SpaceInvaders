using System;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
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

            Tachanka.keywaspressed += Tachanka_keywaspressed;

            //tachanka.MoveLeft();
            
            //Console.ReadLine();





            


        }

        private static void Tachanka_keywaspressed(ConsoleKeyInfo key)
        {
            Tachanka.tmut.WaitOne();
            if (key.Key == ConsoleKey.LeftArrow)
            {
                Tachanka.MoveLeft();
            }
            if(key.Key == ConsoleKey.RightArrow)
            {
                Tachanka.MoveRight();
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                Tachanka.Erase();
            }
            bool h = Tachanka.keythread.IsAlive;
            Tachanka.tmut.ReleaseMutex();
            h = Tachanka.keythread.IsAlive;
        }
    }
}
