using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix
{
    public class TextMonitor
    {
        public enum Mode
        {
            Console,
            Window
        }

        public Mode MonitorMode { get; private set; }

        TextLine[] vram;

        int cursorX;
        int cursorY;

        /// <summary>
        /// Gets or sets a value determining whether to show the cursor.
        /// </summary>
        public bool ShowCursor { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether to lock a line on new line.
        /// </summary>
        public bool ProtectOnNewLine { get; set; }

        public int CursorX
        {
            get
            {
                return cursorX;
            }
        }

        public int CursorY
        {
            get
            {
                return cursorY;
            }
        }

        public char[,] VideoMemory
        {
            get
            {
                char[,] mem = new char[WIDTH, HEIGHT];
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        mem[x, y] = vram[y][x];
                    }
                }

                return mem;
            }
        }

        public bool[,] CellValidity
        {
            get
            {
                bool[,] v = new bool[WIDTH, HEIGHT];
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        v[x, y] = vram[y].Validity[x];
                    }
                }

                return v;
            }
        }

        /// <summary>
        /// Defines the width of the screen in characters.
        /// </summary>
        public const int WIDTH = 40;
        public const int HEIGHT = 25;
        /// <summary>
        /// Defines the length of a horizontal tab.
        /// </summary>
        public const int H_TAB_WIDTH = 5;

        public enum CursorDirection
        {
            Left = -1,
            Right = 1
        }

        public TextMonitor(Mode mode)
        {
            MonitorMode = mode;
            vram = new TextLine[HEIGHT];
            for (int i = 0; i < HEIGHT; i++) { vram[i] = new TextLine(); }
            ClearScreen();
        }

        /// <summary>
        /// Writes a single character to the next location in video memory.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="readOnly"></param>
        public void Put(char c, bool readOnly = false)
        {
            if (MonitorMode == Mode.Console) 
            {
                Console.Write(c); 
            }

            switch (c)
            {
                case '\n':
                    NewLine();
                    break;

                case '\t':
                    HorizontalTab();
                    break;

                case '\b':
                    Backspace();
                    break;

                default:
                    vram[cursorY][cursorX] = c;
                    if (readOnly) vram[cursorY].Protected = cursorX;
                    MoveCursor(CursorDirection.Right);
                    break;
            }
        }

        public void Put(char c, int x, int y, bool readOnly = false)
        {
            if (MonitorMode == Mode.Console)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(c);
            }

            if (InRange(x, y))
            {
                switch (c)
                {
                    case '\n':
                        NewLine(x, y);
                        break;

                    case '\t':
                        HorizontalTab(x, y);
                        break;

                    case '\b':
                        Backspace(x, y);
                        break;

                    default:
                        vram[y][x] = c;
                        if (readOnly) vram[y].Protected = x;
                        break;
                }
            }

        }

        public void Puts(string s, int x, int y, bool readOnly = false)
        {
            if (InRange(x, y))
            {
                int row = y;
                int column = x;
                for (int i = 0; i < s.Length; i++)
                {
                    column++;
                    if (column >= WIDTH)
                    {
                        column = 0;
                        row++;
                    }
                    Put(s[i], column, row, readOnly);
                }
            }
        }

        public void Puts(string s, bool readOnly = false)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Put(s[i], readOnly);
            }
        }

        public bool InRange(int x, int y)
        {
            return ((x >= 0) & (x < WIDTH) & (y >= 0) & (y < HEIGHT));
        }

        public void Scroll()
        {
            vram[0].Invalidate();
            for (int i = 1; i < HEIGHT; i++)
            {
                vram[i - 1] = vram[i];
                vram[i].Invalidate();
            }
            ClearLine(HEIGHT - 1);
        }

        public void ClearLine(int lineNumber)
        {
            if (MonitorMode == Mode.Console)
            {
                Console.SetCursorPosition(0, lineNumber);
                Console.Write(new string(' ', WIDTH));
            }

            for (int i = 0; i < WIDTH; i++)
            {
                vram[lineNumber][i] = ' ';
            }
            vram[lineNumber].LineBreak = -1;
            vram[lineNumber].Protected = -1;
        }

        public void ClearScreen()
        {
            if (MonitorMode == Mode.Console)
            {
                Console.Clear();
            }

            for (int i = 0; i < HEIGHT; i++)
            {
                ClearLine(i);
            }
            cursorX = 0;
            cursorY = 0;
        }

        public void ClearCell(int x, int y)
        {
            if (MonitorMode == Mode.Console)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(' ');
            }

            vram[y][x] = ' ';
        }

        public void PutCursor(int x, int y)
        {
            if (MonitorMode == Mode.Console)
            {
                Console.SetCursorPosition(x, y);
            }

            if (InRange(x, y))
            {
                cursorX = x;
                cursorY = y;
            }
        }

        public void MoveCursor(CursorDirection direction)
        {
            vram[cursorY].Invalidate(cursorX);

            cursorX = cursorX + (int)direction;
            if (cursorX >= WIDTH)
            {
                cursorX = 0;
                if (cursorY + 1 < HEIGHT) cursorY++;
                else Scroll();
            }
            else if (cursorX < 0)
            {
                if (cursorY - 1 > 0)
                {
                    cursorY--;
                    if (vram[cursorY].LineBreak >= 0) cursorX = vram[cursorY].LineBreak;
                    else cursorX = WIDTH - 1;
                }
            }

            if (MonitorMode == Mode.Console)
                Console.SetCursorPosition(cursorX, cursorY);
        }

        /// <summary>
        /// Applies a linebreak where the cursor is.
        /// </summary>
        public void NewLine()
        {
            vram[cursorY].LineBreak = cursorX;
            if (ProtectOnNewLine) vram[cursorY].Protected = cursorX;
            cursorX = 0;
            if (cursorY + 1 >= HEIGHT)
                Scroll();
            else
                cursorY++;
        }

        public void NewLine(int x, int y)
        {
            if (ProtectOnNewLine) vram[cursorY].Protected = cursorX;
            vram[y].LineBreak = x;
        }

        public void HorizontalTab(int x, int y)
        {
            int spaceCount = H_TAB_WIDTH - ((x + 1) % H_TAB_WIDTH);
            Puts(new string(' ', spaceCount), x, y);
        }

        public void HorizontalTab()
        {
            int spaceCount = H_TAB_WIDTH - ((cursorX + 1) % H_TAB_WIDTH);
            Puts(new string(' ', spaceCount));
        }

        private void Backspace()
        {
            if (((cursorX > 0) && (vram[cursorY].Protected < cursorX - 1)) | ((cursorX == 0) & (cursorY > 0) && (vram[cursorY - 1].Protected >= 0)))
            {
                MoveCursor(CursorDirection.Left);
                ClearCell(cursorX, cursorY);
            }
        }

        private void Backspace(int x, int y)
        {
            if ((x - 1 < 0) && (y - 1 >= 0))
            {
                y--;
                x = WIDTH - 1;
            }
            if (((x > 0) & (vram[y].Protected < x - 1)) | ((x == 0) & (y > 0) && (vram[y - 1].Protected >= 0))) ClearCell(x - 1, y);
        }

        internal void Validate()
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                vram[y].Validate();
            }
        }

        internal void Invalidate()
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                vram[y].Invalidate();
            }
        }

        internal void ValidateCell(int x, int y)
        {
            vram[y].Validate(x);
        }

    }
}
