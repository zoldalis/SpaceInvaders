using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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
        public delegate void HitHandler(ConsoleKeyInfo key, Plane obj);
        public event HitHandler keywaspressed;

        int[,] coords = new int[3, 2];
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
            IndexCoords();
            var activeXLibType = Type.GetTypeFromProgID("ConsoleDrawing")
                ?? throw new ArgumentException("не удалось загрузить ActiveX object c ProgId ConsoleDrawing");
            dynamic CD = Activator.CreateInstance(activeXLibType);
            CD.PrintPlane(coords);
            CD.ErasePlane(topleft.x,topleft.y);
        }
        public void IndexCoords()
        {
            coords[0, 0] = topleft.x;
            coords[0, 1] = topleft.y;
            coords[1, 0] = topleft.x;
            coords[1, 1] = topleft.y + 1;
            coords[2, 0] = topleft.x;
            coords[2, 1] = topleft.y + 2;
        }
        public void CheckCondition()
        {
            for(int j = 0; j < 13; j++)
            {
                //if()
            }
        }
    }

}
