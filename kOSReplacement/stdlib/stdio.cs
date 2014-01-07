using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Jebnix.stdlib
{
    public class stdio
    {
        /// <summary>
        /// Output buffer
        /// </summary>
        static Queue<Tuple<char, bool>> outp = new Queue<Tuple<char, bool>>();
        public static TextWriter outs = null;

        /// <summary>
        /// Input buffer
        /// </summary>
        static QueueStack<char> inp = new QueueStack<char>();
        public static TextReader ins = null;

        static internal TextMonitor screen;

        public const char ENDL = '\n';

        /// <summary>
        /// Gets a value that determines if there is data on the input stream ready for processing.
        /// </summary>
        public static bool InputReady { get; private set; }

        /// <summary>
        /// Gets a value that determines if there has been a change to the VRAM and the monitor needs to redraw.
        /// </summary>
        public static bool OutputReady
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets a value that determines if the screen is currently initialized.
        /// </summary>
        public static bool Initialized { get; private set; }

        /// <summary>
        /// Gets a 2-D array representing the characters in video memory.
        /// </summary>
        public static char[,] ScreenCells
        {
            get
            {
                CheckInit();
                return screen.VideoMemory;
            }
        }

        public static bool[,] CellValidity
        {
            get
            {
                CheckInit();
                return screen.CellValidity;
            }
        }

        /// <summary>
        /// Gets the position of the cursor on the X axis.
        /// </summary>
        public static int CursorX
        {
            get
            {
                CheckInit();
                return screen.CursorX;
            }
        }

        /// <summary>
        /// Gets the position of the cursor on the Y axis.
        /// </summary>
        public static int CursorY
        {
            get
            {
                CheckInit();
                return screen.CursorY;
            }
        }

        /// <summary>
        /// Gets a value that determines whether the cursor should be displayed.
        /// </summary>
        public static bool ShowCursor
        {
            get
            {
                CheckInit();
                return screen.ShowCursor;
            }
            internal set
            {
                CheckInit();
                screen.ShowCursor = value;
            }
        }

        public static void Initialize(TextMonitor.Mode mode)
        {
            screen = new TextMonitor(mode);
            Initialized = true;
        }

        private static void CheckInit()
        {
            if (!Initialized) throw new Exceptions.JebnixClassNotInitializedException("stdio");
        }

        /// <summary>
        /// Prints a string to standard output with a line terminator.
        /// </summary>
        /// <param name="s"></param>
        public static void PrintLine(string s, bool readOnly = false)
        {
            Print(s + ENDL, readOnly);
        }

        public static void PrintLine(int value, bool readOnly = false)
        {
            Print(value.ToString() + ENDL, readOnly);
        }

        public static void PrintLine(double value, bool readOnly = false)
        {
            Print(value.ToString() + ENDL, readOnly);
        }

        /// <summary>
        /// Prints a string to standard output.
        /// </summary>
        /// <param name="s"></param>
        public static void Print(string s, bool readOnly = false)
        {
            foreach (char c in s)
            {
                Put(c, readOnly);
            }
            UpdateScreen();
        }

        /// <summary>
        /// Prints a character to standard output.
        /// </summary>
        /// <param name="c"></param>
        public static void Print(char c, bool readOnly = false)
        {
            Put(c, readOnly);
            UpdateScreen();
        }

        /// <summary>
        /// Prints an integer to standard output.
        /// </summary>
        /// <param name="value"></param>
        public static void Print(int value, bool readOnly = false)
        {
            Print(value.ToString(), readOnly);
        }

        /// <summary>
        /// Prints a double to standard output.
        /// </summary>
        /// <param name="value"></param>
        public static void Print(double value, bool readOnly = false)
        {
            Print(value.ToString(), readOnly);
        }

        /// <summary>
        /// Sends a single character to the standard output stream.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="readOnly"></param>
        internal static void Put(char c, bool readOnly = false)
        {
            outp.Enqueue(new Tuple<char, bool>(c, readOnly));
            //outs.Write(c);
        }

        /// <summary>
        /// Prints an integer to standard output using the given format string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        public static void Printf(int value, string format, bool readOnly = false)
        {
            Print(value.ToString(format), readOnly);
        }

        /// <summary>
        /// Prints a double to standard output using the given format string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        public static void Printf(double value, string format, bool readOnly = false)
        {
            Print(value.ToString(format), readOnly);
        }


        public static string Scan()
        {
            if (screen.MonitorMode == TextMonitor.Mode.Console)
            {
                InputReady = true;
                return Console.ReadLine();
            }
            StringBuilder s = new StringBuilder();
            char c = '\0';
            while ((inp.Count > 0) && (c != '\n'))
            {
                c = inp.Dequeue();
                if (c != '\n')
                    s.Append(c);
            }
            InputReady = inp.Count > 0;

            return s.ToString();
        }

        internal static void UpdateScreen()
        {
            CheckInit();
            while (outp.Count > 0)
            {
                Tuple<char, bool> t = outp.Dequeue();
                screen.Put(t.Item1, t.Item2);
            }
            OutputReady = true;
        }

        public static void Input(char c)
        {
            if (c == '\n')
            {
                inp.Enqueue(c);
                InputReady = true;
            }
            else if (c == '\b')
            {
                if (inp.Count > 0) inp.Dequeue();
            }
            else
            {
                inp.Enqueue(c);
            }
        }

        /// <summary>
        /// Notifies the computer that the screen has been drawn, clearing the OutputReady flag.
        /// </summary>
        public static void ScreenDrawn()
        {
            OutputReady = false;
        }

        /// <summary>
        /// Forces the entire screen to redraw.
        /// </summary>
        public static void ForceRedraw()
        {
            CheckInit();
            screen.Invalidate();
        }

        /// <summary>
        /// Validates the given cell.
        /// </summary>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        public static void Validate(int x, int y)
        {
            CheckInit();
            screen.ValidateCell(x, y);
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        public static void ClearScreen()
        {
            CheckInit();
            screen.ClearScreen();
            OutputReady = true;
        }
    }
}
