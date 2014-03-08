using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using KerboScriptEngine;
using Jebnix;
using Jebnix.Graphics;
using Jebnix.stdlib;

namespace JebnixConsoleDebugger
{
    class Program
    {
        static KerboScriptEngine.Interpreter interpreter;
        static System.Timers.Timer clock;
        static int consoleProc;

        static int busyCount = 0;
        static int cycleCount = 0;
        static double percentUsage;
        static double peakPercent = 0;
        static int clockFreq = 100;

        static void Main(string[] args)
        {
            ExternalFunctionTest.Class1 obj = new ExternalFunctionTest.Class1();

            Console.Title = "Jebnix Debug Console";
            Console.ForegroundColor = ConsoleColor.Green;

            clock = new System.Timers.Timer(1000 / clockFreq);
            interpreter = new KerboScriptEngine.Interpreter(new System.IO.DirectoryInfo("Archive"));
            Graphics.Mode = Graphics.GraphicsMode.Text;
            clock.Elapsed += new System.Timers.ElapsedEventHandler(clock_Elapsed);
            stdio.PrintLine("Welcome to Jebnix");
            stdio.PrintLine(interpreter.GetInterpreterVersion());
            stdio.PrintLine(stdio.GetJebnixVersion());
            stdio.PrintLine();
            consoleProc = interpreter.CreateProcess(new string[0], "Console");

            Console.SetWindowSize(40, 21);

            //DebugMode();
            RealTimeMode();
        }

        static void RealTimeMode()
        {
            clock.Enabled = true;

            while (clock.Enabled)
            {
                Debug.Print("Waiting for user input.");
                string inp = Console.ReadLine();
                stdio.PrintLine(inp);
                Debug.Print("Sending input string \"" + inp + "\" to interpreter.");
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
            if (cycleCount == 1000)
            {
                busyCount--;
            }
            else if (cycleCount < 1000)
            {
                cycleCount++;
            }
#if DEBUG
            Debug.Print("CYCLE START");

            if (interpreter.Busy)
            {
                Debug.Print("Interpreter is busy.");
                busyCount++;
            }

            percentUsage = ((double)busyCount / (double)cycleCount);
            if (percentUsage > peakPercent)
                peakPercent = percentUsage;

            Debug.Print("Percent usage: {0:P2}", percentUsage);
            Debug.Print("Peak usage: {0:P2}", peakPercent);
#endif

            if (!interpreter.Busy) 
                interpreter.ExecuteProcess();

            if (Graphics.TextChanged)
            {
#if DEBUG
                Debug.Print("Updating screen.");
#endif
                Console.Clear();
                Console.Write(Graphics.Text);
                Console.SetCursorPosition(Graphics.TextColumn, Graphics.TextRow);
                Graphics.ClearTextChangedFlag();
            }

#if DEBUG
            Debug.Print("CYCLE END");
            Debug.Print("");
#endif
        }
    }
}
