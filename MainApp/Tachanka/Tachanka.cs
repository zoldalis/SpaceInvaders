using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace TachankaObj
{

    [Guid("12848582-cb59-429c-b9d2-12ac62d58de0"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMyEvents
    {
    }


    [Guid("ad3a48b6-0e4d-45dd-90c0-c5343173c041")]
    interface Itachanka
    {
        void MoveOnCell();
    }

    [Guid("91abff50-0361-4681-bf72-f3ec51a1570b"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IMyEvents))]
    public class Tachanka : Itachanka
    {
        private int[,] bodycoords = new int[20, 2];
        private int[] topleft = new int[2];
        public Tachanka()
        {
            topleft[0] =  Console.BufferWidth / 2;
            topleft[1] = Console.BufferHeight - 4;
            
            bodycoords[0, 0] = topleft[0]+2;
            bodycoords[0, 1] = topleft[1];
            bodycoords[1, 0] = bodycoords[0, 0] + 1;
            bodycoords[1, 1] = bodycoords[0, 1];

            bodycoords[2, 0] = topleft[0] + 2;
            bodycoords[2, 1] = topleft[1] + 1;

            bodycoords[3, 0] = topleft[0] + 3;
            bodycoords[3, 1] = topleft[1] + 1;

            bodycoords[4, 0] = topleft[0];
            bodycoords[4, 1] = topleft[1] + 2;

            bodycoords[5, 0] = topleft[0] + 1;
            bodycoords[5, 1] = topleft[1]+2;

            bodycoords[6, 0] = topleft[0] + 2;
            bodycoords[6, 1] = topleft[1]+2;

            bodycoords[7, 0] = topleft[0] + 3;
            bodycoords[7, 1] = topleft[1]+2;

            bodycoords[8, 0] = topleft[0] + 4;
            bodycoords[8, 1] = topleft[1]+2;

            bodycoords[9, 0] = topleft[0] + 5;
            bodycoords[9, 1] = topleft[1]+2;

            bodycoords[10, 0] = topleft[0] + 6;
            bodycoords[10, 1] = topleft[1]+2;

            bodycoords[11, 0] = topleft[0] + 7;
            bodycoords[11, 1] = topleft[1]+2;

            bodycoords[12, 0] = topleft[0];
            bodycoords[12, 1] = topleft[1]+3;

            bodycoords[13, 0] = topleft[0] + 1;
            bodycoords[13, 1] = topleft[1]+3;

            bodycoords[14, 0] = topleft[0] + 2;
            bodycoords[14, 1] = topleft[1]+3;

            bodycoords[15, 0] = topleft[0] + 3;
            bodycoords[15, 1] = topleft[1]+3;

            bodycoords[16, 0] = topleft[0] + 4;
            bodycoords[16, 1] = topleft[1]+3;

            bodycoords[17, 0] = topleft[0] + 5;
            bodycoords[17, 1] = topleft[1]+3;

            bodycoords[18, 0] = topleft[0] + 6;
            bodycoords[18, 1] = topleft[1]+3;

            bodycoords[19, 0] = topleft[0] + 7;
            bodycoords[19, 1] = topleft[1]+3;

            PrintBody('#');

        }
        public void PrintBody(char item)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(bodycoords[i,0], bodycoords[i,1]);
                Console.Write(item);
            }
        }
        public void MoveOnCell()
        {

        }
    }
}
