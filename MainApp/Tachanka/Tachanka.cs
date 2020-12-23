using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;


namespace TachankaObj
{

    [Guid("12848582-cb59-429c-b9d2-12ac62d58de0"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMyEvents
    {
    }


    [Guid("ad3a48b6-0e4d-45dd-90c0-c5343173c041")]
    interface Itachanka
    {
        
        void SetBodyXY(int x, int y);
        void MoveLeft();
        void MoveRight();
        void TransportTank(int x, int y);
        void Erase();
        void PrintBody(char item);
        void IndexingCoords();
    }

    [Guid("91abff50-0361-4681-bf72-f3ec51a1570b"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IMyEvents))]
    public class Tachanka 
    {
        public static Mutex tmut = new Mutex();

        public delegate void MoveHandler(ConsoleKeyInfo key);
        static public event MoveHandler keywaspressed;
        static private int[,] bodycoords = new int[20, 2];
        public struct Topleft
        {
            public int x;
            public int y;
        }
        static Topleft topleft;
        //private int[] topleft = new int[2];
        const int CURR_KEY_ID = 1;
        public static ThreadStart st1 = new ThreadStart(CheckKeyEvent);
        public static Thread keythread = new Thread(st1);
        
        public Tachanka()
        {
            SetBodyXY(Console.BufferWidth / 2, Console.BufferHeight - 4);

            IndexingCoords();

            PrintBody('#');
            keythread.Start();


        }
        static public void SetBodyXY(int x, int y)
        {
            topleft.x = x;
            topleft.y = y;
        }
        static public void MoveLeft()
        {
            //topleft.x -= 1;
            TransportTank(topleft.x-1,topleft.y);
        }
        static public void MoveRight()
        {
            //topleft.x += 1;
            TransportTank(topleft.x+1, topleft.y);
        }

        static public void CheckKeyEvent()
        {
            while (true)
            {
                tmut.WaitOne();
                ConsoleKeyInfo kk = Console.ReadKey();
                tmut.ReleaseMutex();
                keywaspressed?.Invoke(kk);
                Thread.Sleep(10);
            }
        }
        static public void TransportTank(int x,int y)
        {
            Erase();
            SetBodyXY(x,y);
            PrintBody('#');
        }
        static public void Erase()
        {
            PrintBody(' ');
        }
        static public void PrintBody(char item)
        {
            IndexingCoords();
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(bodycoords[i,0], bodycoords[i,1]);
                Console.Write(item);
            }
        }

        static public void IndexingCoords()
        {
            bodycoords[0, 0] = topleft.x + 3;
            bodycoords[0, 1] = topleft.y;
            bodycoords[1, 0] = bodycoords[0, 0] + 1;
            bodycoords[1, 1] = bodycoords[0, 1];

            bodycoords[2, 0] = topleft.x + 3;
            bodycoords[2, 1] = topleft.y + 1;

            bodycoords[3, 0] = topleft.x + 4;
            bodycoords[3, 1] = topleft.y + 1;

            bodycoords[4, 0] = topleft.x;
            bodycoords[4, 1] = topleft.y + 2;

            bodycoords[5, 0] = topleft.x + 1;
            bodycoords[5, 1] = topleft.y + 2;

            bodycoords[6, 0] = topleft.x + 2;
            bodycoords[6, 1] = topleft.y + 2;

            bodycoords[7, 0] = topleft.x + 3;
            bodycoords[7, 1] = topleft.y + 2;

            bodycoords[8, 0] = topleft.x + 4;
            bodycoords[8, 1] = topleft.y + 2;

            bodycoords[9, 0] = topleft.x + 5;
            bodycoords[9, 1] = topleft.y + 2;

            bodycoords[10, 0] = topleft.x + 6;
            bodycoords[10, 1] = topleft.y + 2;

            bodycoords[11, 0] = topleft.x + 7;
            bodycoords[11, 1] = topleft.y + 2;

            bodycoords[12, 0] = topleft.x;
            bodycoords[12, 1] = topleft.y + 3;

            bodycoords[13, 0] = topleft.x + 1;
            bodycoords[13, 1] = topleft.y + 3;

            bodycoords[14, 0] = topleft.x + 2;
            bodycoords[14, 1] = topleft.y + 3;

            bodycoords[15, 0] = topleft.x + 3;
            bodycoords[15, 1] = topleft.y + 3;

            bodycoords[16, 0] = topleft.x + 4;
            bodycoords[16, 1] = topleft.y + 3;

            bodycoords[17, 0] = topleft.x + 5;
            bodycoords[17, 1] = topleft.y + 3;

            bodycoords[18, 0] = topleft.x + 6;
            bodycoords[18, 1] = topleft.y + 3;

            bodycoords[19, 0] = topleft.x + 7;
            bodycoords[19, 1] = topleft.y + 3;
        }
    }
}
