using System;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
//using TachankaObj;
using System.Timers;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.CSharp;

namespace SpaceInvaders
{
    public class Program
    {
        public System.Threading.Mutex tmut = new System.Threading.Mutex();

        static List<dynamic> Instances = new List<dynamic>();

        static Timer planecreator;
        static Timer checkonstate;
        private static string ProgId => "TachankaObj";
        private static string ProgId2 => "ShellObj";

        public static void Main(string[] args)
        {
            
            
            Console.Title = "SpaceInvaders! Your Score :0";
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;

            var activeXLibType = Type.GetTypeFromProgID(ProgId)
                ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId {ProgId}");
            dynamic tachanka = Activator.CreateInstance(activeXLibType);



            InitPlaneCreator();
            //CreatePlane();
            InitCheckOnState();

            //var libtype = Type.GetTypeFromProgID("BombObj");
           // dynamic bomb = Activator.CreateInstance(libtype, Console.BufferWidth/2, 1);

            void checkstate(Object source, ElapsedEventArgs e)
            {
                Type CDlibtype = Type.GetTypeFromProgID("ConsoleDrawing");
                dynamic CD = Activator.CreateInstance(CDlibtype);
                string line = CD.ReadString((short)(Console.BufferWidth / 2 - 5), (short)(Console.BufferHeight / 2), 21, 1);
                if (line == "            GAME OVER")
                {
                    ClearObjs();
                }
            }

            void InitCheckOnState()
            {
                checkonstate = new Timer(2);
                checkonstate.Elapsed += checkstate;
                checkonstate.AutoReset = true;
                checkonstate.Enabled = true;
            }

            void InitPlaneCreator()
            {
                CreatePlane();
                planecreator = new System.Timers.Timer(3000);
                planecreator.Elapsed += createPlane;
                planecreator.AutoReset = true;
                planecreator.Enabled = true;
            }

            void createPlane(Object source, System.Timers.ElapsedEventArgs e)
            {
                var activeXLibType1 = Type.GetTypeFromProgID("PlaneObj")
                    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId PlaneObj");
                dynamic plane = Activator.CreateInstance(activeXLibType1);
                Instances.Add(plane);
                Random createinterval = new Random();
                if (planecreator.Interval > 1000)
                {
                    planecreator.Interval -= 25;
                }
            }

            void CreatePlane()
            {
                var activeXLibType1 = Type.GetTypeFromProgID("PlaneObj")
                    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId PlaneObj");
                dynamic plane = Activator.CreateInstance(activeXLibType1);
                Instances.Add(plane);
            }

            void ClearObjs()
            {
                try
                {
                    planecreator.Dispose();
                    checkonstate.Dispose();
                }
                catch (Exception)
                {
                }
                
                foreach (var item in Instances)
                {
                    if (item != null)
                        item.Destruct();
                }
                tachanka.Destruct();
            }

        }



        



    }
}
