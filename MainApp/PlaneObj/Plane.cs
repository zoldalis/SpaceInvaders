//#include <windows.h>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Timers;

namespace PlaneObj
{
    [Guid("04cbda1a-8cd1-44ce-8316-7ad353dfbd16")]
    interface IPlane
    {
        [DispId(1)]
        void Destruct();
    }
  
    [ProgId("PlaneObj")]
    [Guid("86c1e0ca-89ac-4c34-b4f4-2054eef019c6"), ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Plane
    {
        public System.Threading.Mutex tmut = new System.Threading.Mutex();
        static Type activeXLibType = Type.GetTypeFromProgID("ConsoleDrawing");
        dynamic CD = Activator.CreateInstance(activeXLibType);
        private string direction = "left";
        private Timer dirTimer;
        private Timer moveTimer;
        private Timer checkTimer;
        private Timer godowntmr;
        private Timer dropbombrtmr;

        public System.Threading.ThreadStart st1;
        public System.Threading.Thread keythread;

        //public delegate void HitHandler(ConsoleKeyInfo key, Plane obj);
        //public event HitHandler TargetWasHit;

        public struct Topleft
        {
            public int x;
            public int y;
        }
        Topleft topleft;
        public Plane()
        {
            Random rand = new Random();
            topleft.x = rand.Next(0,Console.BufferWidth - 9);
            topleft.y = 1;


            //Type activeXLibType = Type.GetTypeFromProgID("ConsoleDrawing")
               // ?? throw new ArgumentException("не удалось загрузить ActiveX object c ProgId ConsoleDrawing");
            //dynamic CD = Activator.CreateInstance(activeXLibType);
            CD.PrintPlane(topleft.x,topleft.y);
            //CD.ErasePlane(topleft.x,topleft.y);
            InitDirTimer();
            InitMoveTimer();
            InitCheckTimer();
            InitGoDownTimer();
            InitDropBombTimer();


        }
        public bool IsIntact()
        {
            tmut.WaitOne();
            string line = CD.ReadString((short)topleft.x, (short)topleft.y,13);
            tmut.ReleaseMutex();
            if (line.Contains("/") | line.Contains("\\"))
                return false;
            else
                return true;
        }
        void checkCondition(Object source, ElapsedEventArgs e)
        {
            tmut.WaitOne();
            if (IsIntact() == false)
            {
                int score = Convert.ToInt32(Console.Title.Split(':')[1]);
                dynamic wsc = (dynamic)Microsoft.VisualBasic.Interaction.GetObject(@"script:C:\SpaceInvaders\MainApp\CurrentInterval.wsc", null);
                //var libtype = Type.GetTypeFromProgID("CurrentScore");
                //dynamic CI = Activator.CreateInstance(libtype);
                score = wsc.Increase(score);

                //score ++;
                Destruct();
                Console.Title = $"SpaceInvaders! Your Score:{score}";
                CD.ErasePlane(topleft.x, topleft.y);
            }
            tmut.ReleaseMutex();
        }
        void moving(Object source, ElapsedEventArgs e)
        {
            tmut.WaitOne();
            if (IsIntact() == false)
            {
                int score = Convert.ToInt32(Console.Title.Split(':')[1]);
                score += 1;
                Destruct();
                Console.Title = $"SpaceInvaders! Your Score:{score}";
                CD.ErasePlane(topleft.x, topleft.y);
            }
            else
                CD.PlaneMove(ref direction, ref topleft.x,ref topleft.y);
            tmut.ReleaseMutex();
        }
        void cngdirection(Object source, ElapsedEventArgs e)
        {
            Random rnd = new Random();
            int val = rnd.Next(0,3);
            if (val == 1)
            {
                direction = "right";
                return;
            }
            else
            {
                direction = "left";
                return;
            }
        }
        void GoDown(Object source, ElapsedEventArgs e)
        {
            tmut.WaitOne();
            dynamic wsc = (dynamic)Microsoft.VisualBasic.Interaction.GetObject(@"script:C:\SpaceInvaders\MainApp\CurrentInterval.wsc", null);
            string j = "down";
            if (wsc.IfLess(topleft.y + 6, Console.BufferHeight))
            { 
                CD.PlaneMove(ref j, ref topleft.x, ref topleft.y);
            }
            else
            {
                Destruct();
                CD.ErasePlane(topleft.x, topleft.y);
                Console.Clear();
                Console.SetCursorPosition(Console.BufferWidth / 2 - 5, Console.BufferHeight / 2);
                Console.Write("            " + "GAME OVER" + "  " + $"Your Score:{Convert.ToInt32(Console.Title.Split(':')[1])}, Great job!" );
            }
            tmut.ReleaseMutex();
        }

        void DropBomb(Object source, ElapsedEventArgs e)
        {
            var libtype = Type.GetTypeFromProgID("BombObj");
            dynamic bomb = Activator.CreateInstance(libtype,topleft.x + 3, topleft.y + 5);
            Random createinterval = new Random();
            dropbombrtmr.Interval = createinterval.Next(1500, 3000);
        }

        void InitDirTimer()
        {
            dirTimer = new Timer(2531);
            dirTimer.Elapsed += cngdirection;
            dirTimer.AutoReset = true;
            dirTimer.Enabled = true;
        }
        void InitMoveTimer()
        {
            moveTimer = new Timer(100);
            moveTimer.Elapsed += moving;
            moveTimer.AutoReset = true;
            moveTimer.Enabled = true;
        }
        void InitCheckTimer()
        {
            checkTimer = new Timer(1);
            checkTimer.Elapsed += checkCondition;
            checkTimer.AutoReset = true;
            checkTimer.Enabled = true;
        }
        void InitGoDownTimer()
        {
            godowntmr = new Timer(200);
            godowntmr.Elapsed += GoDown;
            godowntmr.AutoReset = true;
            godowntmr.Enabled = true;
        }
        public void Destruct()
        {
            tmut.WaitOne();
            CD.ErasePlane(topleft.x, topleft.y);
            dirTimer.Dispose();
            moveTimer.Dispose();
            checkTimer.Dispose();
            godowntmr.Dispose();
            dropbombrtmr.Dispose();
            tmut.ReleaseMutex();
        }

        void InitDropBombTimer()
        {
            dropbombrtmr = new Timer(1500);
            dropbombrtmr.Elapsed += DropBomb;
            dropbombrtmr.AutoReset = true;
            dropbombrtmr.Enabled = true;
        }

    }

}
