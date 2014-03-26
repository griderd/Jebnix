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

        public static void RaiseOnKeyPress(object sender, KeyData key)
        {
            if (OnKeyPress != null)
                OnKeyPress(sender, new InterruptEventArgs<KeyData>(key));
            RaiseOnInterrupt(sender, key);
        }

        public static void RaiseOnInterrupt(object sender, object data)
        {
            if (OnInterrupt != null)
                OnInterrupt(sender, new InterruptEventArgs<object>(data));
        }
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
            ConsKey = 0;
            IsFormsKey = true;
            Character = c;
        }

        public KeyData(bool alt, bool ctrl, bool shift, bool capsLock, System.ConsoleKey keyCode, char c)
            : this()
        {
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
            CapsLock = capsLock;
            ConsKey = keyCode;
            KeyCode = 0;
            IsFormsKey = false;
            Character = c;
        }

        public bool Alt { get; private set; }
        public bool Ctrl { get; private set; }
        public bool Shift { get; private set; }
        public bool CapsLock { get; private set; }
        public bool IsFormsKey { get; private set; }
        public System.Windows.Forms.Keys KeyCode { get; private set; }
        public System.ConsoleKey ConsKey { get; private set; }
        public char Character { get; private set; }
    }
}
