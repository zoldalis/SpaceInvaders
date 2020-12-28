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
        private static string ProgId => "TachankaObj";
        private static string ProgId2 => "ShellObj";

        static void Main(string[] args)
        {
            

            Console.Title = "SpaceInvaders";
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;
            Console.SetWindowPosition(0, 0);

            var activeXLibType = Type.GetTypeFromProgID(ProgId)
                ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId {ProgId}");
            dynamic activeXobj = Activator.CreateInstance(activeXLibType);

            var activeXLibType1 = Type.GetTypeFromProgID("PlaneObj")
                ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId PlaneObj");
            dynamic plane = Activator.CreateInstance(activeXLibType1);



            //var ob1 = Type.GetTypeFromProgID(ProgId2)
            //    ?? throw new ArgumentException($"не удалось загрузить ActiveX object c ProgId {ProgId2}");
            //dynamic obj2 = Activator.CreateInstance(ob1, 15);











            //Tachanka tachanka = new Tachanka(); 



        }

       
    }
}
