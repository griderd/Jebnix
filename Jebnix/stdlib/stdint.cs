using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jebnix.stdlib
{
    /// <summary>
    /// Standard Interrupt Library. This library is designed to allow interrupts to occur.
    /// </summary>
    public class stdint
    {
        public enum InterruptType : int
        {
            Generic,
            KeyPress,
            Halt,
            Resume,
            Error
        }

        /// <summary>
        /// Software interrupt. Used for/by processes.
        /// </summary>
        public static event EventHandler<InterruptEventArgs> OnInterrupt;

        /// <summary>
        /// Hardware interrupt, raised when a keyboard key is pressed.
        /// </summary>
        public static event EventHandler<InterruptEventArgs<KeyData>> OnKeyPress;

        public static void RaiseOnKeyPress(object sender, KeyData key)
        {
            if (OnKeyPress != null)
                OnKeyPress(sender, new InterruptEventArgs<KeyData>(InterruptType.KeyPress, key));
            RaiseOnInterrupt(sender, InterruptType.KeyPress, key);
        }

        public static void RaiseOnInterrupt(object sender, InterruptType type, object data)
        {
            if (OnInterrupt != null)
                OnInterrupt(sender, new InterruptEventArgs(type, data));
        }

        public static void RaiseOnInterrupt(object sender, InterruptType type)
        {
            RaiseOnInterrupt(sender, type, null);
        }

        public static void RaiseOnInterrupt(object sender, object data)
        {
            RaiseOnInterrupt(sender, InterruptType.Generic, data);
        }

        public static void RaiseOnResume(object sender)
        {
            if (OnInterrupt != null)
                OnInterrupt(sender, new InterruptEventArgs(InterruptType.Resume));
        }

        public static void RaiseOnHalt(object sender)
        {
            if (OnInterrupt != null)
                OnInterrupt(sender, new InterruptEventArgs(InterruptType.Halt));
        }
    }

    public class InterruptEventArgs : EventArgs 
    {
        public stdint.InterruptType Type { get; private set; }
        public virtual object Data { get; private set; }

        public InterruptEventArgs(stdint.InterruptType type)
        {
            Type = type;
            Data = null;
        }

        public InterruptEventArgs(stdint.InterruptType type, object data)
        {
            Type = type;
            Data = data;
        }
    }

    public class InterruptEventArgs<T> : InterruptEventArgs
    {
        public InterruptEventArgs(stdint.InterruptType type, T data)
            : base(type)
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
        public KeyData(bool alt, bool ctrl, bool shift, bool capsLock, char c)
            : this()
        {
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
            CapsLock = capsLock;
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
            IsFormsKey = false;
            Character = c;
        }

        public bool Alt { get; private set; }
        public bool Ctrl { get; private set; }
        public bool Shift { get; private set; }
        public bool CapsLock { get; private set; }
        public bool IsFormsKey { get; private set; }
        public char Character { get; private set; }
    }
}
