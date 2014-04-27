using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Jebnix.Types.BasicTypes;

namespace Jebnix.stdlib
{
    /// <summary>
    /// Handles I/O related operations
    /// </summary>
    public class stdio
    {
        static Jebnix.Graphics.TextMemory screen = Jebnix.Graphics.Graphics.TextMode;

        static int tabLength = 5;
        public static int TabLength { get; private set; }

        public static void SetTabLength(int length)
        {
            if (length > 0)
                tabLength = length;
        }

        public static void Print(string s)
        {
            Debug.Print("stdio.Print(\"" + s + "\");");
            foreach (char c in s)
            {
                screen.Put(c);
            }
        }

        /// <summary>
        /// Processes the character and updates the screen appropriately. 
        /// If the character is a printable character, prints it to the screen.
        /// </summary>
        /// <param name="c">Character to process</param>
        /// <param name="userEntered">Determines if the user entered the character. True by default.</param>
        public static void ProcessChar(char c, bool userEntered = true)
        {
            switch (c)
            {
                case '\a':  // BELL character
                    // TODO: Make a noise?
                    break;

                case '\b':  // BACKSPACE
                    if (screen.Row >= 0 && screen.Column > 0)
                    {
                        screen.Column--;
                        if (!screen.CellLocked)
                        {
                            screen.Put(' ', !userEntered);
                            screen.Column--;
                        }
                        else
                            screen.Column++;
                    }
                    else if (screen.Row > 0)
                    {
                        int column = screen.Column;
                        screen.Row--;
                        screen.Column = screen.LastColumn;
                        if (!screen.CellLocked)
                            screen.Put(' ', !userEntered);
                        else
                        {
                            screen.Row++;
                            screen.Column = column;
                        }
                    }
                    break;

                case '\f':  // FORM FEED
                    // TODO: decide what to do with this
                    break;

                case '\n':  // NEW LINE
                    PrintLine(userEntered);
                    break;

                case '\r':  // CARRIAGE RETURN
                    screen.Column = 0;
                    break; 

                case '\t':  // HORIZONTAL TAB
                    Print(new string(' ', tabLength), userEntered);
                    break;

                case '\v':  // VERTICAL TAB
                    // TODO: decide what to do with this
                    break;

                default:
                    Print(c, userEntered);
                    break;
            }
        }

        public static void Print(JString s, bool userEntered = false)
        {
            //Print(s, !userEntered);
        }

        public static void Print(char c, bool userEntered = false)
        {
            screen.Put(c, !userEntered);
        }

        public static void PrintLine(string s, bool userEntered = false)
        {
            Debug.Print("stdio.PrintLine(\"" + s + "\");");
            foreach (char c in s)
            {
                screen.Put(c, !userEntered);
            }
            screen.NewLine();
        }

        public static void PrintLine(JString s, bool userEntered = false)
        {
            PrintLine(s, !userEntered);
        }

        public static void PrintLine(bool userEntered = false)
        {
            screen.NewLine();
        }

        public static void PrintAt(string s, int column, int row, bool userEntered = false)
        {
            int c = screen.Column;
            int r = screen.Row;

            if ((column < 0) | (column > screen.Width) |
                (row < 0) | (row > screen.Height))
                throw new ArgumentOutOfRangeException();

            screen.Column = column;
            screen.Row = row;

            Print(s, userEntered);

            screen.Column = c;
            screen.Row = r;
        }

        public static void ClearScreen()
        {
            screen.Clear();
        }

        public static string GetJebnixVersion()
        {
            System.Version v = Assembly.GetExecutingAssembly().GetName().Version;
            return "Jebnix " + v.Major + "." + v.Minor + " Build " + v.Revision;
        }

        //public static string Input(string prompt)
        //{
        //    Print(prompt);
        //}

        //public static Value Input(Value prompt)
        //{
        //    Print(prompt);
        //}
    }
}
