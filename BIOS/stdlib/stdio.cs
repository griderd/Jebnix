using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
            foreach (char c in s)
            {
                screen.Put(c);
            }
        }

        public static void PrintLine(string s)
        {
            foreach (char c in s)
            {
                screen.Put(c);
            }
            screen.NewLine();
        }

        public static void PrintLine()
        {
            screen.NewLine();
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
