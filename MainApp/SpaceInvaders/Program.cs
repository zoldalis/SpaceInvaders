using System;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
//using TachankaObj;
using System.Threading;
using System.Runtime.InteropServices;


namespace SpaceInvaders
{
    class Program
    {
        private static System.Timers.Timer planecreator;
        private static string ProgId => "TachankaObj";
        private static string ProgId2 => "ShellObj";

        static void Main(string[] args)
        {


            Console.Title = "SpaceInvaders";
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;

            var activeXLibType = Type.GetTypeFromProgID(ProgId)
                ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId {ProgId}");
            dynamic activeXobj = Activator.CreateInstance(activeXLibType);

            //CreatePlane();
            InitPlaneCreator();


            void InitPlaneCreator()
            {
                CreatePlane();
                planecreator = new System.Timers.Timer(4000);
                planecreator.Elapsed += createPlane;
                planecreator.AutoReset = true;
                planecreator.Enabled = true;
            }

            void createPlane(Object source, System.Timers.ElapsedEventArgs e)
            {
                var activeXLibType1 = Type.GetTypeFromProgID("PlaneObj")
                    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId PlaneObj");
                dynamic plane = Activator.CreateInstance(activeXLibType1);
            }

            void CreatePlane()
            {
                var activeXLibType1 = Type.GetTypeFromProgID("PlaneObj")
                    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId PlaneObj");
                dynamic plane = Activator.CreateInstance(activeXLibType1);
            }

            //Type elib = Type.GetTypeFromProgID("ConsoleDrawing");
            //dynamic CD = Activator.CreateInstance(activeXLibType);


            //char[,] con = new ;




            //var ob1 = Type.GetTypeFromProgID(ProgId2)
            //    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId {ProgId2}");
            //dynamic obj2 = Activator.CreateInstance(ob1, 15);




            //Tachanka tachanka = new Tachanka(); 



        }
        


    }
}
