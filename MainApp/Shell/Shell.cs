using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace ShellObj
{
    [Guid("9c3c8f24-8f52-47b7-a6c2-d158c25a3505")]
    public interface IShell
    {
        [DispId(1)]
        void MoveForward();
        void PrintBody();
    }

    [Guid("6cdbc8dc-ee7e-4a51-b561-792ce00dc06e"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ShellIvents
    {
    }

    [Guid("3cb5c493-a995-460a-af06-87d7b34eda6c"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(ShellIvents))]
    public class Shell : IShell
    {
        public struct coords
        {
            public int x;
            public int y;
        }
        coords coos;
        public ThreadStart st2;
        public Thread shellthread;
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
                if (shellthread.IsAlive == false)
                    break;
                Console.CursorVisible = false;
                Erase();
                coos.y -= 1;
                PrintBody();
                Thread.Sleep(100);
            }
            
        }
        public void PrintBody()
        {
            try
            {
                Console.SetCursorPosition(coos.x, coos.y);
                Console.Write("/\\");
            }
            catch (Exception)
            {
                shellthread.Abort();
            }

        }
        public void Erase()
        {
            Console.SetCursorPosition(coos.x,coos.y);
            Console.Write(' ');
            Console.SetCursorPosition(coos.x +1 , coos.y);
            Console.Write(' ');
        }
    }
}

