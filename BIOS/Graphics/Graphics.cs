using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIOS.Graphics
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

        public static TextMemory TextMode { get; private set; }
    }
}
