using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ConsoleDrawing
{
    [Guid("0ac7e180-039a-4b93-bf32-f8cbe648a286")]
    interface IPrinting
    {
        [DispId(1)]
        void PrintPlane(int x, int y);
        [DispId(2)]
        void ErasePlane(int x, int y);
    }
    [ProgId("ConsoleDrawing")]
    [Guid("1f49722f-65c6-42eb-a7c6-3aaf48037ee8"), ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Printing : IPrinting
    {
        public Mutex tmut = new Mutex();
        //public string PlaneBody1 = "  /######\\  ";
        //public string PlaneBody2 = " //  __###\\ ";
        //public string PlaneBody3 = "/####__####\\";
        public string[] PlaneBody = new string[3] {"  /######\\ "," //  __###\\","/####__####\\"};
        public Printing()
        {

        }
        public void ErasePlane(int x, int y)
        {
            tmut.WaitOne();
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(x,y+i);
                Console.Write("             ");
            }
            tmut.ReleaseMutex();
        }
        public void PrintPlane(int x,int y)
        {
            tmut.WaitOne();
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(x,y + i);
                Console.Write(PlaneBody[i]);
            }
            tmut.ReleaseMutex();
        }
        public string ReadString(short x,short y,short width)
        {
            string output = "";
            foreach (string line in ConsoleReader.ReadFromBuffer(x, y, width, 3))
            {
                if (line.Contains("/\\"))
                    output =  line;
            }
            return output;
        }
        public void PlaneMove(ref string direction,ref int x, ref int y)
        {
            if (direction == "right")
            {
                if (x + 2 < Console.BufferWidth - 14)
                {
                    Console.MoveBufferArea(x, y, 13, 3, x + 2, y);
                    x += 2;
                }
                else
                {
                    direction = "left";
                    PlaneMove(ref direction, ref x, ref y);
                }
            }
            
            if (direction == "left")
            {
                if (x - 2 > 0)
                {
                    Console.MoveBufferArea(x, y, 13, 3, x - 2, y);
                    x -= 2;
                }
                else
                {
                    direction = "right";
                    PlaneMove(ref direction, ref x, ref y);
                }
            }
            if (direction == "down")
            {
                if (y + 6 < Console.BufferHeight)
                {
                    Console.MoveBufferArea(x, y, 13, 3, x, y + 1);
                    y += 1;
                }
                else
                {
                    ErasePlane(x,y);
                }
            }
        }
    }
}
