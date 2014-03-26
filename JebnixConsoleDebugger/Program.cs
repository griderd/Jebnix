﻿using System;
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

        static StringBuilder input;

        enum ConsoleMode
        {
            Intermediate,
            Edit
        }

        static ConsoleMode Mode { get; set; }

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

            stdint.OnKeyPress += new EventHandler<InterruptEventArgs<KeyData>>(stdint_OnKeyPress);
            input = new StringBuilder();

            Mode = ConsoleMode.Intermediate;

            //DebugMode(25);
            RealTimeMode();
        }

        static void stdint_OnKeyPress(object sender, InterruptEventArgs<KeyData> e)
        {
            
        }

        static void RealTimeMode()
        {
            clock.Enabled = true;

            while (clock.Enabled)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            stdio.ProcessChar('\n');
                            if (Mode == ConsoleMode.Intermediate)
                            {
                                interpreter.AddCodeToProcess(consoleProc, new string[] { input.ToString() }, "Console");
                                input.Clear();
                            }
                            break;

                        case ConsoleKey.Backspace:
                            stdio.ProcessChar('\b');
                            if (input.Length > 0) 
                                input.Remove(input.Length - 1, 1);
                            break;

                        case ConsoleKey.Tab:
                            stdio.ProcessChar('\t');
                            input.Append('\t');
                            break;

                        default:
                            stdio.ProcessChar(key.KeyChar);
                            input.Append(key.KeyChar);
                            break;
                    }
                }

                //Debug.Print("Waiting for user input.");
                //string inp = Console.ReadLine();
                //stdio.PrintLine(inp);
                //Debug.Print("Sending input string \"" + inp + "\" to interpreter.");
                //interpreter.AddCodeToProcess(consoleProc, new string[] { inp }, "Console");
            }   
        }

        static void DebugMode(int count)
        {
            clock_Elapsed(null, null);

            string inp = Console.ReadLine();
            stdio.PrintLine(inp);
            interpreter.AddCodeToProcess(consoleProc, new string[] { inp }, "Console");

            for (int i = 0; i < count; i++)
            {
                clock_Elapsed(null, null);
            }
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
            if (interpreter.Busy) busyCount++;

            if (!interpreter.Busy)
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
