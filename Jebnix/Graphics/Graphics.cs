using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.Graphics
{
    public class Graphics
    {   
        public enum GraphicsMode
        {
            Text,
            Graphical
        }

        static GraphicsMode mode = GraphicsMode.Text;
        public static GraphicsMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode == GraphicsMode.Text)
                    TextMode = new TextMemory(40, 20);
            }
        }

        public static string Text
        {
            get
            {
                return TextMode.ToString();
            }
        }

        public static char[][] Cells
        {
            get
            {
                return TextMode.Cells;
            }
        }

        public static bool TextChanged
        {
            get
            {
                return TextMode.ScreenChanged;
            }
        }

        public static void ClearTextChangedFlag()
        {
            TextMode.ClearFlag();
        }

        public static int TextColumn
        {
            get
            {
                return TextMode.Column;
            }
        }

        public static int TextRow
        {
            get
            {
                return TextMode.Row;
            }
        }

        public static int TextWidth
        {
            get
            {
                return TextMode.Width;
            }
        }

        public static int TextHeight
        {
            get
            {
                return TextMode.Height;
            }
        }

        internal static TextMemory TextMode { get; private set; }
    }
}
