using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Reflection;
using Jebnix.stdlib;

namespace Jebnix
{
    public class VirtualMachine
    {
        public enum Mode
        {
            /// <summary>
            /// Immediate mode interprets input as instructions and executes them immediately.
            /// </summary>
            Immediate,
            /// <summary>
            /// Edit mode interprets input as data to write to a filestream.
            /// </summary>
            Edit
        }

        Timer clock;
        Timer cursorBlink;

        public event EventHandler ScreenRefresh;
        public event EventHandler CursorChanged;

        /// <summary>
        /// Gets a value determining whether the cursor is currently being displayed.
        /// </summary>
        public bool ShowingCursor { get; private set; }

        /// <summary>
        /// Gets a value determining whether the virtual machine is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        bool waitingForInput;

        Mode mode;

        /// <summary>
        /// Gets or sets a value determining whether to display a cursor.
        /// </summary>
        public bool ShowCursor
        {
            get
            {
                return stdio.ShowCursor;
            }
            set
            {
                stdio.ShowCursor = value;
                cursorBlink.Enabled = value;
                ShowingCursor = value;
            }
        }

        /// <summary>
        /// Instantiates a virtual machine with the given interval between clock cycles.
        /// </summary>
        /// <param name="clockInterval">Interval between clock cycles in milliseconds.</param>
        public VirtualMachine(double clockInterval)
        {
            clock = new Timer(clockInterval);
            
            cursorBlink = new Timer(500);
            ShowingCursor = false;

            clock.Elapsed += new ElapsedEventHandler(clock_Elapsed);
            cursorBlink.Elapsed += new ElapsedEventHandler(cursorBlink_Elapsed);

            Settings.Initialize();
        }

        void cursorBlink_Elapsed(object sender, ElapsedEventArgs e)
        {
            ShowingCursor = !ShowingCursor;
            if (CursorChanged != null) CursorChanged(this, new EventArgs());
        }

        void clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ScreenRefresh != null) ScreenRefresh(this, new EventArgs());
            if (!waitingForInput)
            {
                Prompt();
            }
            else
            {
                clock.Stop();
                string input = stdio.Scan();
                Processor.ProcessInput(input, this);
            }
        }

        /// <summary>
        /// Starts the virtual machine.
        /// </summary>
        public void Start()
        {
            clock.Enabled = true;
            
            IsRunning = true;
            System.Version v =  Assembly.GetExecutingAssembly().GetName().Version;
            stdio.PrintLine("Jebnix v" + v.ToString(3) + " Build " + v.Revision, true);
#if DEBUG
            stdio.PrintLine("DEBUG VERSION\n", true);
#endif
            waitingForInput = false;
            mode = Mode.Immediate;
        }

        /// <summary>
        /// Stops the virtual machine.
        /// </summary>
        public void Stop()
        {
            clock.Enabled = false;
            cursorBlink.Enabled = false;
            IsRunning = false;

            stdio.ClearScreen();
            ShowingCursor = false;
            ScreenRefresh(this, new EventArgs());
        }

        public void Prompt()
        {
            if (!waitingForInput)
            {
                if (Settings.UsePrompts)
                {
                    StringBuilder s = new StringBuilder();
                    if (Settings.ShowPath) { /* Do something */ }
                    s.Append(Settings.PromptCharacter);
                    s.Append(" ");
                    stdio.Print(s.ToString(), true);
                }
                if (stdio.screen.MonitorMode == TextMonitor.Mode.Console)
                {
                    Console.ReadLine();
                }
                waitingForInput = true;
            }
        }
    }
}
