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


        }

        public bool IsIntact()
        {
            string line = CD.ReadString((short)topleft.x, (short)topleft.y,13);
            if (line.Contains("/\\"))
                return false;
            else
                return true;
        }
        void checkCondition(Object source, ElapsedEventArgs e)
        {
            if (IsIntact() == false)
            {
                DestuctPlane();
                CD.ErasePlane(topleft.x, topleft.y);
            }
        }
        void moving(Object source, ElapsedEventArgs e)
        {
            tmut.WaitOne();
            if (IsIntact() == false)
            {
                DestuctPlane();
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
            string j = "down";
            if (topleft.y + 6 < Console.BufferHeight)
            { 
                CD.PlaneMove(ref j, ref topleft.x, ref topleft.y);
                
            }
            else
            {
                DestuctPlane();
                CD.ErasePlane(topleft.x, topleft.y);
                Console.Clear();
                Console.SetCursorPosition(Console.BufferWidth / 2 - 5, Console.BufferHeight / 2);
                Console.Write("            " + "GAME OVER");
            }
            tmut.ReleaseMutex();
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
            moveTimer = new Timer(200);
            moveTimer.Elapsed += moving;
            moveTimer.AutoReset = true;
            moveTimer.Enabled = true;
        }
        void InitCheckTimer()
        {
            checkTimer = new Timer(25);
            checkTimer.Elapsed += checkCondition;
            checkTimer.AutoReset = true;
            checkTimer.Enabled = true;
        }
        void InitGoDownTimer()
        {
            godowntmr = new Timer(931);
            godowntmr.Elapsed += GoDown;
            godowntmr.AutoReset = true;
            godowntmr.Enabled = true;
        }
        void DestuctPlane()
        {
            dirTimer.Dispose();
            moveTimer.Dispose();
            checkTimer.Dispose();
            godowntmr.Dispose();
        }

    }

}
