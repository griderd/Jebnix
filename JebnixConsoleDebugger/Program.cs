using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KerboScriptEngine;
using Jebnix;
using Jebnix.Graphics;
using Jebnix.stdlib;

namespace JebnixConsoleDebugger
{
    class Program
    {
        //static VirtualMachine vm;
        static KerboScriptEngine.Interpreter interpreter;
        static System.Timers.Timer clock;
        static int consoleProc;

        static void Main(string[] args)
        {
            ExternalFunctionTest.Class1 obj = new ExternalFunctionTest.Class1();

            Console.Title = "Jebnix Debug Console";
            Console.ForegroundColor = ConsoleColor.Green;

            clock = new System.Timers.Timer(50);
            interpreter = new KerboScriptEngine.Interpreter(new System.IO.DirectoryInfo("Archive"));
            Graphics.Mode = Graphics.GraphicsMode.Text;
            clock.Elapsed += new System.Timers.ElapsedEventHandler(clock_Elapsed);
            stdio.PrintLine("Welcome to Jebnix");
            stdio.PrintLine(interpreter.GetInterpreterVersion());
            stdio.PrintLine(stdio.GetJebnixVersion());
            stdio.PrintLine();
            consoleProc = interpreter.CreateProcess(new string[0], "Console");

            Console.SetWindowSize(40, 21);

            DebugMode();
            //RealTimeMode();
        }

        static void RealTimeMode()
        {
            clock.Enabled = true;

            while (clock.Enabled)
            {
                string inp = Console.ReadLine();
                stdio.PrintLine(inp);
                interpreter.AddCodeToProcess(consoleProc, new string[] { inp }, "Console");
            }
        }

        static void DebugMode()
        {
            clock_Elapsed(null, null);

            string inp = Console.ReadLine();
            stdio.PrintLine(inp);
            interpreter.AddCodeToProcess(consoleProc, new string[] { inp }, "Console");

            clock_Elapsed(null, null);
            clock_Elapsed(null, null);
            Console.ReadLine();
        }

        static void clock_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            interpreter.ExecuteProcess();
            if (Graphics.TextChanged)
            {
                Console.Clear();
                Console.Write(Graphics.Text);
                Console.SetCursorPosition(Graphics.TextColumn, Graphics.TextRow);
                Graphics.ClearTextChangedFlag();
            }
        }
    }
}
