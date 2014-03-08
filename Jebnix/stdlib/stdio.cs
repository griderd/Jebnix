using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Jebnix.Types;

namespace Jebnix.stdlib
{
    /// <summary>
    /// Handles I/O related operations
    /// </summary>
    public class stdio
    {
        static Jebnix.Graphics.TextMemory screen = Jebnix.Graphics.Graphics.TextMode;

        public static void Print(string s)
        {
            Debug.Print("stdio.Print(\"" + s + "\");");
            foreach (char c in s)
            {
                screen.Put(c);
            }
        }

        public static void Print(Value s)
        {
            Print(s.StringValue);
        }

        public static void PrintLine(string s)
        {
            Debug.Print("stdio.PrintLine(\"" + s + "\");");
            foreach (char c in s)
            {
                screen.Put(c);
            }
            screen.NewLine();
        }

        public static void PrintLine(Value s)
        {
            PrintLine(s.StringValue);
        }

        public static void PrintLine()
        {
            screen.NewLine();
        }

        public static void PrintAt(string s, int column, int row)
        {
            int c = screen.Column;
            int r = screen.Row;

            if ((column < 0) | (column > screen.Width) |
                (row < 0) | (row > screen.Height))
                throw new ArgumentOutOfRangeException();

            screen.Column = column;
            screen.Row = row;

            Print(s);

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
    }
}
