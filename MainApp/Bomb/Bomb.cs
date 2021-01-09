using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;


namespace Bomb
{
    [Guid("9c6105b4-4bbc-4399-bd94-e0a19ecee518")]
    interface IBomb
    {

    }
    [ProgId("BombObj")]
    [Guid("23ebd3c2-8b62-46de-a072-40374fb5e6d8"), ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Bomb : IBomb
    {
        static Type activeXLibType = Type.GetTypeFromProgID("ConsoleDrawing");
        dynamic CD = Activator.CreateInstance(activeXLibType);
        public struct Сoords
        {
            public int x;
            public int y;
        }
        Сoords coos;
        public Mutex tmut = new Mutex();
        public ThreadStart st;
        public Thread bombthread;
        public Bomb()
        {

        }

        public Bomb(int x,int y)
        {
            coos.x = x;
            coos.y = y;
            tmut.WaitOne();
            Console.SetCursorPosition(x,y);
            Console.Write("<>");
            tmut.ReleaseMutex();
            st = new ThreadStart(GoDown);
            bombthread = new Thread(st);
            bombthread.Start();


        }

        void GoDown()
        {
            while (true)
            {
                tmut.WaitOne();
                if (coos.y + 2 > Console.BufferHeight - 1)
                {
                    Erase();
                    tmut.ReleaseMutex();
                    break;
                }
                //Erase();
                //coos.y -= 1;
                string line = CD.ReadString((short)(coos.x-2), (short)(coos.y + 2), 4);
                if (line != "")
                {
                    if(coos.y > Console.BufferHeight - 7)

                    Console.MoveBufferArea(coos.x, coos.y, 2, 1, coos.x, coos.y + 6);
                    coos.y += 6;

                    
                }
                else
                {
                    Console.MoveBufferArea(coos.x, coos.y, 2, 1, coos.x, coos.y + 2);
                    coos.y += 2;
                }
                Thread.Sleep(40);
                tmut.ReleaseMutex();
            }
        }
        void Erase()
        {
            tmut.WaitOne();
            Console.SetCursorPosition(coos.x, coos.y);
            Console.Write("  ");
            tmut.ReleaseMutex();
        }
    }
}
