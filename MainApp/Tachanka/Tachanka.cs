using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using ShellObj;


namespace TachankaObj
{

    [Guid("12848582-cb59-429c-b9d2-12ac62d58de0"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMyEvents
    {
        //delegate void MoveHandler(ConsoleKeyInfo key, Tachanka obj);
        event EventHandler keywaspressed;
    }


    [Guid("ad3a48b6-0e4d-45dd-90c0-c5343173c041")]
    interface Itachanka
    {
        [DispId(1)]
        void SetBodyXY(int x, int y);
        [DispId(2)]
        void MoveLeft();
        [DispId(3)]
        void MoveRight();
        [DispId(4)]
        void CheckKeyEvent();
        [DispId(5)]
        void TransportTank(int x, int y);
        [DispId(6)]
        void Erase();
        [DispId(7)]
        void PrintBody(char item);
        [DispId(8)]
        void IndexingCoords();

    }

    [Guid("91abff50-0361-4681-bf72-f3ec51a1570b"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IMyEvents))]
    public class Tachanka : Itachanka
    {
        public  Mutex tmut = new Mutex();
        public delegate void MoveHandler(ConsoleKeyInfo key, Tachanka obj);
        public event MoveHandler keywaspressed;
        private int[,] bodycoords = new int[20, 2];
        public struct Topleft
        {
            public int x;
            public int y;
        }
        Topleft topleft;
        //private int[] topleft = new int[2];
        const int CURR_KEY_ID = 1;
        public  ThreadStart st1;
        public  Thread keythread;
        
        public Tachanka()
        {
            SetBodyXY(Console.BufferWidth / 2, Console.BufferHeight - 4);

            IndexingCoords();

            PrintBody('#');
            st1 = new ThreadStart(CheckKeyEvent);
            keythread = new Thread(st1);
            keythread.Start();
            keywaspressed += Tachanka_keywaspressed;

        }
       public void SetBodyXY(int x, int y)
        {
            topleft.x = x;
            topleft.y = y;
        }
        public void MoveLeft()
        {
            try
            {
                TransportTank(topleft.x - 1, topleft.y);
            }
            catch (Exception)
            {
                return;
            }
            
        }
        public void MoveRight()
        {
            try
            {
                TransportTank(topleft.x + 1, topleft.y);
            }
            catch (Exception)
            {
                return;
            }
            
        }

        public void Shoot()
        {
            int timerinf = 1;
            TimerCallback tm;
            Timer timer;
            if (timerinf == 0)
            {
                Shell shell = new Shell(topleft.x + 3);
                timerinf = 1;
                tm = new TimerCallback(Count);
                timer = new Timer(tm, timerinf, 1000, 1000000);
            }

        }

        public static void Count(object obj)
        {
            int x = (int)obj;
            x = 1;
        }

        public void CheckKeyEvent()
        {
            while (true)
            {
                tmut.WaitOne();
                ConsoleKeyInfo kk = Console.ReadKey();
                tmut.ReleaseMutex();
                keywaspressed?.Invoke(kk,this);
                Thread.Sleep(5);
            }
        }
        public void TransportTank(int x,int y)
        {
            Erase();
            SetBodyXY(x,y);
            PrintBody('#');
        }
        public void Erase()
        {
            PrintBody(' ');
        }
        public void PrintBody(char item)
        {
            try
            {
                IndexingCoords();
                for (int i = 0; i < 20; i++)
                {
                    Console.SetCursorPosition(bodycoords[i, 0], bodycoords[i, 1]);
                    Console.Write(item);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void IndexingCoords()
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

        private static void Tachanka_keywaspressed(ConsoleKeyInfo key, Tachanka obj)
        {
            obj.tmut.WaitOne();
            if (key.Key == ConsoleKey.LeftArrow)
            {
                obj.MoveLeft();
            }
            if (key.Key == ConsoleKey.RightArrow)
            {
                obj.MoveRight();
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                obj.Shoot();
            }
            bool h = obj.keythread.IsAlive;
            obj.tmut.ReleaseMutex();
            h = obj.keythread.IsAlive;
        }
    }
}
