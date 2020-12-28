using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ConsoleDrawing
{
    [Guid("0ac7e180-039a-4b93-bf32-f8cbe648a286")]
    interface IPrinting
    {
        [DispId(1)]
        void PrintPlane(int[,] coords);
        [DispId(2)]
        void ErasePlane(int x, int y);
    }
    [ProgId("ConsoleDrawing")]
    [Guid("1f49722f-65c6-42eb-a7c6-3aaf48037ee8"), ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Printing : IPrinting
    {
        //public string PlaneBody1 = "  /######\\  ";
        //public string PlaneBody2 = " //  __###\\ ";
        //public string PlaneBody3 = "/####__####\\";
        public string[] PlaneBody = new string[3] {"  /######\\ "," //  __###\\","/####__####\\"};
        public Printing()
        {

        }
        public void ErasePlane(int x, int y)
        {
            for(int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(x,y+i);
                Console.Write("             ");
            }
        }
        public void PrintPlane(int[,] coords)
        {
            for(int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(coords[i,0],coords[i,1]);
                Console.Write(PlaneBody[i]);
            }
        }
    }
}
