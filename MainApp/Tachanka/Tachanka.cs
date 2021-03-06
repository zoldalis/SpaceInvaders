﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;

//using ShellObj;
using System.Reflection;

namespace TachankaObj
{
    [Guid("12848582-cb59-429c-b9d2-12ac62d58de0"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface IMyEvents
    {
        //delegate void MoveHandler(ConsoleKeyInfo key, Tachanka obj);
       // event EventHandler keywaspressed;
        void Tachanka_keywaspressed(ConsoleKeyInfo key, Tachanka obj);
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
        void Erase();

    }

    [ProgId("TachankaObj")]
    [Guid("91abff50-0361-4681-bf72-f3ec51a1570b"), ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(IMyEvents))]
    [ComVisible(true)]
    public class Tachanka : Itachanka
    {
        private System.Timers.Timer reltimer;
        public static string ProgId2 = "ShellObj";
        string bodyline1;
        string bodyline2;
        string bodyline3;
        string bodyline4;

        public  Mutex tmut = new Mutex();
        public delegate void MoveHandler(ConsoleKeyInfo key, Tachanka obj);
        public event MoveHandler keywaspressed;
        public struct Topleft
        {
            public int x;
            public int y;
        }
        Topleft topleft;
        //private int[] topleft = new int[2];
        public  ThreadStart st1;
        public  Thread keythread;

        bool possibletoshoot;
        
        public Tachanka()
        {


            using (var mmFile = MemoryMappedFile.CreateFromFile(@"tachanka.txt", System.IO.FileMode.Open, "fileHandle"))
            {
                var myAccessor = mmFile.CreateViewAccessor();
                var readOut = new byte[38];
                myAccessor.ReadArray(0, readOut, 0, readOut.Length);
                var finalValue = Encoding.ASCII.GetString(readOut);
                bodyline1 = finalValue.Substring(0,8);
                bodyline2 = finalValue.Substring(10, 8);
                bodyline3 = finalValue.Substring(20, 8);
                bodyline4 = finalValue.Substring(30, 8);
            }



            SetBodyXY(Console.BufferWidth / 2, Console.BufferHeight - 4);

            //PrintBody('#');
            st1 = new ThreadStart(CheckKeyEvent);
            keythread = new Thread(st1);
            keythread.Start();
            keywaspressed += Tachanka_keywaspressed;
            tmut.WaitOne();
            Console.SetCursorPosition(topleft.x, topleft.y);
            Console.Write(bodyline1);
            Console.SetCursorPosition(topleft.x, topleft.y + 1);
            Console.Write(bodyline2);
            Console.SetCursorPosition(topleft.x, topleft.y + 2);
            Console.Write(bodyline3);
            Console.SetCursorPosition(topleft.x, topleft.y + 3);
            Console.Write(bodyline4);
            tmut.ReleaseMutex();
            InitRelTimer();
        }
       public void SetBodyXY(int x, int y)
        {
            topleft.x = x;
            topleft.y = y;
        }
        public void MoveLeft()
        {
            tmut.WaitOne();
            if(topleft.x == 0)
            {
                Console.MoveBufferArea(topleft.x, topleft.y, 8, 4, Console.BufferWidth-8, topleft.y);
                topleft.x = Console.BufferWidth - 8;
            }
            else
            {
                Console.MoveBufferArea(topleft.x, topleft.y, 8, 4, topleft.x - 2, topleft.y);
                topleft.x-=2;
            }
            tmut.ReleaseMutex();
        }
        public void MoveRight()
        {
            tmut.WaitOne();
            if (topleft.x + 10 > Console.BufferWidth)
            {
                Console.MoveBufferArea(topleft.x, topleft.y, 8, 4, 0, topleft.y);
                topleft.x = 0;
            }
            else
            {
                Console.MoveBufferArea(topleft.x, topleft.y, 8, 4, topleft.x + 2, topleft.y);
                topleft.x += 2;
            }
            tmut.ReleaseMutex();
        }

        public void Shoot()
        {
            if (possibletoshoot == true)
            {
                var ob1 = Type.GetTypeFromProgID(ProgId2)
                    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId {ProgId2}");
                dynamic obj2 = Activator.CreateInstance(ob1, topleft.x + 3);
                possibletoshoot = false;
                InitRelTimer();
            }
        }
      
        public void CheckKeyEvent()
        {
            while (true)
            {
                tmut.WaitOne();
                ConsoleKeyInfo kk = Console.ReadKey();
                tmut.ReleaseMutex();
                keywaspressed?.Invoke(kk,this);
                Thread.Sleep(1);
            }
        }
        public void Erase()
        {
            tmut.WaitOne();
            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(topleft.x,topleft.y + i);
                Console.Write("        ");
            }
            tmut.ReleaseMutex();
        }

        void reload(Object source, System.Timers.ElapsedEventArgs e)
        {
            possibletoshoot = true;
            reltimer.Dispose();
        }

        void InitRelTimer()
        {
            reltimer = new System.Timers.Timer(500);
            reltimer.Elapsed += reload;
            reltimer.AutoReset = true;
            reltimer.Enabled = true;
        }

        private void Tachanka_keywaspressed(ConsoleKeyInfo key, Tachanka obj)
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
            obj.tmut.ReleaseMutex();
        }

        public void Destruct()
        {
            Erase();
            keywaspressed -= Tachanka_keywaspressed;
            Thread.CurrentThread.Abort();
        }
    }
}
