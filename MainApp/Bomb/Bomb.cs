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
        [DispId(1)]
        void Erase();
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
                string line = CD.ReadString((short)(coos.x-1), (short)(coos.y + 2), 4,1);
                
                if (line != "    " & !(line.Contains("/") | line.Contains("\\")))
                {
                    if(coos.y > Console.BufferHeight - 7 & line.Contains("#"))
                    {
                        Erase();
                        Console.Clear();
                        Console.SetCursorPosition(Console.BufferWidth / 2 - 5, Console.BufferHeight / 2);
                        Console.Write("            " + "GAME OVER" + "  " + $"Your Score:{Convert.ToInt32(Console.Title.Split(':')[1])}, Great job!");
                    }
                    else 
                    {
                        Console.MoveBufferArea(coos.x, coos.y, 2, 1, coos.x, coos.y + 6);
                        coos.y += 6;
                    }
                    
                    
                }
                else
                {
                    if(line.Contains("/") | line.Contains("\\"))
                    {
                        Console.MoveBufferArea(coos.x, coos.y, 2, 1, coos.x, coos.y + 3);
                        coos.y += 3;
                    }
                    else
                    {
                        Console.MoveBufferArea(coos.x, coos.y, 2, 1, coos.x, coos.y + 1);
                        coos.y += 1;
                    }
                    
                }
                Thread.Sleep(20);
                tmut.ReleaseMutex();
            }
        }
        public void Erase()
        {
            tmut.WaitOne();
            Console.SetCursorPosition(coos.x, coos.y);
            Console.Write("  ");
            tmut.ReleaseMutex();
        }
    }
}
