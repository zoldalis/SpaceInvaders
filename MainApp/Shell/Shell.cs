using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace ShellObj
{
    //[Guid("6cdbc8dc-ee7e-4a51-b561-792ce00dc06e"),
    //    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    //public interface ShellIvents
    //{
    //}
    
    
    
    
    [Guid("9c3c8f24-8f52-47b7-a6c2-d158c25a3505")]
    public interface IShell
    {
        [DispId(1)]
        void MoveForward();
        [DispId(2)]
        void PrintBody();
        [DispId(3)]
        void Erase();
    }

    

    [ProgId("ShellObj")]
    [Guid("16d3b300-e44c-4673-a3b9-dbe89efeab47"),ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Shell : IShell
    {
        public Mutex tmut = new Mutex();
        public struct Сoords
        {
            public int x;
            public int y;
        }
        Сoords coos;
        public ThreadStart st2;
        public Thread shellthread;
        public Shell()
        {

        }
        public Shell(int x)
        {
            st2 = new ThreadStart(MoveForward);
            shellthread = new Thread(st2);
            shellthread.Start();
            coos.x = x;
            coos.y = Console.BufferHeight-6;
            PrintBody();
        }
        public void MoveForward()
        {
            while (true)
            {
                tmut.WaitOne();
                if (coos.y < 2)
                {
                    Erase();
                    tmut.ReleaseMutex();
                    break;
                }
                //Erase();
                //coos.y -= 1;
                Console.MoveBufferArea(coos.x, coos.y, 2, 1, coos.x , coos.y-2);
                coos.y-= 2;
                Thread.Sleep(20);
                tmut.ReleaseMutex();
            }
            
        }
        public void PrintBody()
        {
            tmut.WaitOne();
            try
            {
                Console.SetCursorPosition(coos.x, coos.y);
                Console.Write("/\\");
            }
            catch (Exception)
            {
                shellthread.Abort();
            }
            tmut.ReleaseMutex();
        }
        public void Erase()
        {
            tmut.WaitOne();
            Console.SetCursorPosition(coos.x,coos.y);
            Console.Write("  ");
            tmut.ReleaseMutex();
        }
    }
}

