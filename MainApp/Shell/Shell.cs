using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;



namespace ShellObj
{
    
    [Guid("9c3c8f24-8f52-47b7-a6c2-d158c25a3505")]
    public interface IShell
    {
        [DispId(1)]
        void MoveForward();
        [DispId(2)]
        void PrintBody();
        [DispId(3)]
        void Erase();
    }
 
    [ProgId("ShellObj")]
    [Guid("16d3b300-e44c-4673-a3b9-dbe89efeab47"),ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Shell : IShell
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        //const int STD_OUTPUT_HANDLE = -11;
        //const int STD_INPUT_HANDLE = -10;
        //[DllImport("kernel32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool AllocConsole();

        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern IntPtr GetStdHandle(int nStdHandle);
        //IntPtr hStdout;

        //[DllImport("kernel32.dll")]
        //static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput,
        // string lpCharacter, uint nLength, COORD dwWriteCoord,
        //out uint lpNumberOfCharsWritten);

        MSScriptControl.ScriptControl ScriptObj;

        public Mutex tmut = new Mutex();
        
        COORD coos;
        public ThreadStart st2;
        public Thread shellthread;
        public Shell()
        {

        }
        public Shell(int x)
        {
            ScriptObj = new MSScriptControl.ScriptControl();
            ScriptObj.Language = "VBScript";
            ScriptObj.AddCode("Function Check(y) If (y < 2) Then Check = true Else Check = false End If End Function");
            //hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
            st2 = new ThreadStart(MoveForward);
            shellthread = new Thread(st2);
            shellthread.Start();
            coos.X = (short)x;
            coos.Y = (short)(Console.BufferHeight-6);
            PrintBody();
        }
        public void MoveForward()
        {
            while (true)
            {
                tmut.WaitOne();

                bool YCond = Convert.ToBoolean(ScriptObj.Run("Check", coos.Y));

                if (YCond)
                {
                    Erase();
                    tmut.ReleaseMutex();
                    break;
                }
                //Erase();
                //coos.y -= 1;
                Console.MoveBufferArea(coos.X, coos.Y, 2, 1, coos.X , coos.Y-1);
                coos.Y-= 1;
                Thread.Sleep(10);
                tmut.ReleaseMutex();
            }
            
        }
        public void PrintBody()
        {
            tmut.WaitOne();
            try
            {
                Console.SetCursorPosition(coos.X, coos.Y);
                Console.Write("/\\");
            }
            catch (Exception)
            {
                Erase();
                shellthread.Abort();
            }
            tmut.ReleaseMutex();
        }
        public void Erase()
        {
            tmut.WaitOne();
            Console.SetCursorPosition(coos.X,coos.Y);
            Console.Write("  ");
            tmut.ReleaseMutex();
        }



    }
}

