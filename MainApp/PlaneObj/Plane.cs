using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        static Type activeXLibType = Type.GetTypeFromProgID("ConsoleDrawing");
        dynamic CD = Activator.CreateInstance(activeXLibType);
        private string direction = "left";
        private Timer dirTimer;
        private Timer moveTimer;
        private Timer checkTimer;
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
                dirTimer.Dispose();
                moveTimer.Dispose();
                checkTimer.Dispose();
                CD.ErasePlane(topleft.x, topleft.y);
            }
        }
        void moving(Object source, ElapsedEventArgs e)
        {
            if (IsIntact() == false)
            {
                dirTimer.Dispose();
                moveTimer.Dispose();
                checkTimer.Dispose();
                CD.ErasePlane(topleft.x, topleft.y);
            }
            else
                CD.PlaneMove(ref direction, ref topleft.x,ref topleft.y);
        }
        void cngdirection(Object source, ElapsedEventArgs e)
        {
            string j = "down";
            if (topleft.y + 6 < Console.BufferHeight)
            {
                CD.PlaneMove(ref j, ref topleft.x, ref topleft.y);
            }
            else
            {
                dirTimer.Dispose();
                moveTimer.Dispose();
                checkTimer.Dispose();
                Console.Clear();
                Console.SetCursorPosition(Console.BufferWidth/2-5, Console.BufferHeight/2);
                Console.Write("            " + "GAME OVER");
            }
            Random rnd = new Random();
            int val = rnd.Next(0,2);
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
        void InitDirTimer()
        {
            dirTimer = new Timer(913);
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
    }

}
