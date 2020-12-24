using System;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
using TachankaObj;
using System.Threading;

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

            Tachanka tachanka = new Tachanka();
            


        }

       
    }
}
