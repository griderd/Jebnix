using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIOS.stdlib
{
    public class stdio
    {
        static BIOS.Graphics.TextMemory screen = BIOS.Graphics.Graphics.TextMode;

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
    }
}
