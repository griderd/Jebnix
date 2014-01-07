using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix
{
    class TextLine
    {
        char[] characters;
        bool[] valid;
        int lineBreak;

        public char this[int index]
        {
            get
            {
                if ((index < 0) | (index >= TextMonitor.WIDTH)) throw new IndexOutOfRangeException();
                return characters[index];
            }
            set
            {
                if ((index < 0) | (index >= TextMonitor.WIDTH)) throw new IndexOutOfRangeException();
                Invalidate(index);
                characters[index] = value;
            }
        }

        public int Protected { get; set; }

        public bool[] Validity
        {
            get
            {
                return valid;
            }
        }

        public void Validate(int index)
        {
            if ((index < 0) | (index >= TextMonitor.WIDTH)) throw new IndexOutOfRangeException();
            valid[index] = true;
        }

        public void Validate()
        {
            for (int i = 0; i < TextMonitor.WIDTH; i++)
            {
                valid[i] = true;
            }
        }

        public void Invalidate()
        {
            for (int i = 0; i < TextMonitor.WIDTH; i++)
            {
                valid[i] = false;
            }
        }

        public void Invalidate(int index)
        {
            if ((index < 0) | (index >= TextMonitor.WIDTH)) throw new IndexOutOfRangeException();
            valid[index] = false;
        }

        public int LineBreak
        {
            get
            {
                return lineBreak;
            }
            set
            {
                if ((value < -1) | (value >= TextMonitor.WIDTH)) throw new IndexOutOfRangeException();
                lineBreak = value;
            }
        }

        public TextLine()
        {
            characters = new char[TextMonitor.WIDTH];
            valid = new bool[TextMonitor.WIDTH];
            Protected = -1;
            lineBreak = -1;
        }
    }
}
