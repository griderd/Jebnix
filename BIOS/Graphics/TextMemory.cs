using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Graphics
{
    internal class TextMemory
    {
        char[][] cells;

        public bool ScreenChanged { get; private set; }

        public int Row { get; set; }
        public int Column { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public TextMemory(int width, int height)
        {
            cells = new char[height][];
            for (int i = 0; i < height; i++)
            {
                cells[i] = new char[width];
                for (int j = 0; j < width; j++)
                {
                    cells[i][j] = ' ';
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
            }

            ScreenChanged = true;
        }

        public void Put(char c, int x, int y)
        {
            if ((x >= Width) | (y >= Height))
                throw new ArgumentOutOfRangeException();

            cells[y][x] = c;

            ScreenChanged = true;
        }

        public void Put(char c)
        {
            Put(c, Column, Row);
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

        public void NewLine()
        {
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
