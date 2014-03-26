using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Graphics
{
    internal class TextMemory
    {
        // TODO: Add additional rows to make scrollable screens. Change output to display applicable values.

        char[][] cells;
        bool[][] locked;
        int[] lastChar;

        public bool ScreenChanged { get; private set; }

        // TODO: Change these so that they can only be modified within the correct range.
        public int Row { get; set; }
        public int Column { get; set; }

        public int Height { get; private set; }
        public int Width { get; private set; }

        public int LastColumn
        {
            get
            {
                return lastChar[Row];
            }
        }

        public bool CellLocked
        {
            get
            {
                return locked[Row][Column];
            }
        }

        public TextMemory(int width, int height)
        {
            cells = new char[height][];
            locked = new bool[height][];
            lastChar = new int[height];
            for (int i = 0; i < height; i++)
            {
                cells[i] = new char[width];
                locked[i] = new bool[width];
                lastChar[i] = -1;
                for (int j = 0; j < width; j++)
                {
                    cells[i][j] = ' ';
                    locked[i][j] = false;
                }
            }

            Height = height;
            Width = width;

            ScreenChanged = true;
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                ClearLine(y);
            }

            Column = 0;
            Row = 0;
        }

        public void ClearLine(int lineNumber)
        {
            if (lineNumber >= Height)
                throw new ArgumentOutOfRangeException();

            for (int x = 0; x < Width; x++)
            {
                cells[lineNumber][x] = ' ';
                locked[lineNumber][x] = false;
                lastChar[lineNumber] = -1;
            }

            ScreenChanged = true;
        }

        public void Put(char c, int x, int y, bool lockCell = false)
        {
            if ((x >= Width) | (y >= Height))
                throw new ArgumentOutOfRangeException();

            cells[y][x] = c;
            locked[y][x] = lockCell;
            lastChar[y] = x;

            ScreenChanged = true;
        }

        public void Put(char c, bool lockCell = false)
        {
            Put(c, Column, Row, lockCell);
            Column++;
            if (Column == Width)
            {
                Row++;
                Column = 0;
            }

            if (Row == Height)
            {
                ScrollDown();
            }
        }

        public void NewLine(bool lockCell = false)
        {
            locked[Row][Column] = lockCell;
            lastChar[Row] = Column;
            Row++;
            Column = 0;
            if (Row == Height)
            {
                ScrollDown();
            }

            ScreenChanged = true;
        }

        public void ScrollDown()
        {
            for (int y = 0; y < Height - 1; y++)
            {
                cells[y] = cells[y + 1];
                locked[y] = locked[y + 1];
                lastChar[y] = lastChar[y + 1];
            }
            Row--;
            ClearLine(Row);

            ScreenChanged = true;
        }

        public void ClearFlag()
        {
            ScreenChanged = false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    sb.Append(cells[y][x]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
