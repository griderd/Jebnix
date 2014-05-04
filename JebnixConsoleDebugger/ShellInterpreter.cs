using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.stdlib;
using KerboScriptEngine.UtilityModules;

namespace JebnixConsoleDebugger
{
    partial class Program
    {
        static void InterpretCommand(string command)
        {
            string[] parts = command.Trim().Split(' ');

            if (parts.Length == 0)
                return;

            switch (parts[0])
            {
                case "cls":
                    if (parts.Length == 1)
                        stdio.ClearScreen();
                    else if ((parts[1] == "?") | (parts[1] == "-help"))
                        ShowMan("cls");
                    break;


                case "man":
                    if (parts.Length == 1)
                        ShowMan();
                    else
                        ShowMan(parts[1]);
                    break;

                case "run":
                    if (parts.Length == 2)
                    {
                        if ((parts[1] == "?") | (parts[1] == "-help"))
                            ShowMan("run");
                        else
                            interpreter.CreateProcess(parts[1] + ".txt");
                    }
                    else
                    {
                        stdio.PrintLine("Expected script name.");
                    }
                    break;

                case "cd":
                    if (parts.Length == 2)
                    {
                        
                    }
                    else
                    {
                        stdio.PrintLine("Expected directory name.");
                    }
                    break;

                case "copy":
                    if (parts.Length == 3)
                    {
                        SystemAPI.CopyTo(parts[1], parts[2]);
                    }
                    else if (parts.Length == 2)
                    {
                        if ((parts[1] == "?") | (parts[1] == "-help"))
                            ShowMan("run");
                        else
                            stdio.PrintLine("Expected destination.");
                    }
                    else
                    {
                        stdio.PrintLine("Expected source and destination.");
                    }
                    break;
            }
        }
    }
}
