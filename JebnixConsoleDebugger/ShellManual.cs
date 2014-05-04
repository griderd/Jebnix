using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix.stdlib;

namespace JebnixConsoleDebugger
{
    partial class Program
    {
        static void ShowMan()
        {
            stdio.PrintLine("MANUAL");
            stdio.PrintLine("Shell Manual");
            stdio.PrintLine();
            stdio.PrintLine("RUN - Compiles and executes a script.");
            stdio.PrintLine("CD - Changes directories.");
            stdio.PrintLine("COPY - Copies a file between directories");
            stdio.PrintLine("DEL - Deletes a file.");
            stdio.PrintLine("EDIT - Starts the editor.");
            stdio.PrintLine("DEBUG - Starts the debugger.");
            stdio.PrintLine("CLS - Clears the screen.");
            stdio.PrintLine();
            stdio.PrintLine("You can get more information by entering");
            stdio.PrintLine("the name of the command followed by a");
            stdio.PrintLine("question mark, or MAN followed by the");
            stdio.PrintLine("command.");
            stdio.PrintLine("For example:");
            stdio.PrintLine("run ?");
            stdio.PrintLine("run -help");
            stdio.PrintLine("man run");
        }

        static void ShowMan(string topic)
        {
            switch (topic)
            {
                case "cls":
                    stdio.PrintLine("CLS - Clears the screen.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("cls");
                    stdio.PrintLine();
                    break;

                case "run":
                    stdio.PrintLine("RUN - Compiles and executes a script.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("run <scriptname>");
                    stdio.PrintLine();
                    break;

                case "cd":
                    stdio.PrintLine("CD - Changes directories.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("cd <directory>");
                    stdio.PrintLine();
                    break;

                case "copy":
                    stdio.PrintLine("copy - Copies the source file to the destination directory.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("copy <sourcefile> <destination>");
                    stdio.PrintLine();
                    break;

                case "del":
                    stdio.PrintLine("DEL - Deletes a file.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("del <filename>");
                    stdio.PrintLine();
                    break;

                case "edit":
                    stdio.PrintLine("EDIT - Starts the editor.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("edit");
                    stdio.PrintLine("edit <filename>");
                    stdio.PrintLine();
                    stdio.PrintLine("The first version opens the editor with a new file.");
                    stdio.PrintLine("The second version opens the editor with the given file open.");
                    stdio.PrintLine("If the file doesn't exist, it is created.");
                    stdio.PrintLine();
                    break;

                case "debug":
                    stdio.PrintLine("DEBUG - Starts the debugger.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("debug <script>");
                    stdio.PrintLine();
                    stdio.PrintLine("The script is compiled and executed with debugger support.");
                    stdio.PrintLine();
                    break;

                default:
                    stdio.PrintLine("Command not recognized.");
                    ShowMan();
                    break;
            }
        }
    }
}
