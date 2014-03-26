using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jebnix.stdlib
{
    /// <summary>
    /// Standard Interrupt Library. This library is designed to allow interrupts to occur.
    /// </summary>
    public class stdint
    {
        /// <summary>
        /// Generic interrupt.
        /// </summary>
        public static event EventHandler<InterruptEventArgs<object>> OnInterrupt;

        /// <summary>
        /// Raised when a keyboard key is pressed.
        /// </summary>
        public static event EventHandler<InterruptEventArgs<KeyData>> OnKeyPress;
    }

    public class InterruptEventArgs<T> : EventArgs
    {
        public InterruptEventArgs(T data)
        {
            Data = data;
        }

        public T Data
        {
            get;
            private set;
        }
    }

    public struct KeyData
    {
        public KeyData(bool alt, bool ctrl, bool shift, bool capsLock, System.Windows.Forms.Keys keyCode, char c)
            : this()
        {
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
            CapsLock = capsLock;
            KeyCode = keyCode;
            Character = c;
        }

        public bool Alt { get; private set; }
        public bool Ctrl { get; private set; }
        public bool Shift { get; private set; }
        public bool CapsLock { get; private set; }
        public System.Windows.Forms.Keys KeyCode { get; private set; }
        public char Character { get; private set; }
    }
}
