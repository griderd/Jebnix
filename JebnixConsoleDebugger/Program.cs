using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix;
using Jebnix.stdlib;

namespace JebnixConsoleDebugger
{
    class Program
    {
        static VirtualMachine vm;
        static void Main(string[] args)
        {
            vm = new VirtualMachine(10000);
            stdio.outs = Console.Out;
            stdio.ins = Console.In;
            stdio.Initialize(TextMonitor.Mode.Console);
            DebugLogger.Initialize();
            Settings.ShowPath = false;
            Console.SetWindowSize(TextMonitor.WIDTH, TextMonitor.HEIGHT);
            Console.ForegroundColor = ConsoleColor.Green;
            vm.ShowCursor = false;
            stdio.ClearScreen();
            Console.SetCursorPosition(0, 0);
            vm.ScreenRefresh += new EventHandler(vm_ScreenRefresh);

            vm.Start();
            while (vm.IsRunning)
            {

            }
        }

        static void vm_ScreenRefresh(object sender, EventArgs e)
        {
            Console.Clear();
            for (int y = 0; y < TextMonitor.HEIGHT; y++)
            {   
                for (int x = 0; x < TextMonitor.WIDTH; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(stdio.ScreenCells[x, y]);
                }
            }
        }
        
    }
}
