using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jebnix;
using Jebnix.stdlib;
using KerboScriptEngine;
using KerboScriptEngine.UtilityModules;

namespace kOSDebugger
{
    public partial class frmMain : Form
    {
        Rectangle[,] cells;
        Graphics graphics;
        Processor interpreter;

        frmVideoMemory vmem;

        const int CellHeight = 14;
        const int CellWidth = 10;

        int clockFreq = 100;

        int HEIGHT, WIDTH;

        StringBuilder input;

        enum ConsoleMode
        {
            Intermediate,
            Edit
        }

        ConsoleMode Mode { get; set; }

        /// <summary>
        /// Gets a value that determines if the form is currently being drawn.
        /// </summary>
        public bool IsDrawing { get; private set; }

        public frmMain()
        {
            InitializeComponent();
            
            // Set up window title
            this.Text = "Jebnix Debug";
            
            // Initialize graphic driver
            Jebnix.Graphics.Graphics.Mode = Jebnix.Graphics.Graphics.GraphicsMode.Text;
            HEIGHT = Jebnix.Graphics.Graphics.TextHeight;
            WIDTH = Jebnix.Graphics.Graphics.TextWidth;
            
            // Initialize clock
            clock.Interval = 1000 / clockFreq;
            clock.Tick += new EventHandler(clock_Tick);

            // Initialize interpreter
            interpreter = new Processor(new System.IO.DirectoryInfo("Archive"));

            // Set up console
            Mode = ConsoleMode.Intermediate;
            stdint.OnKeyPress += new EventHandler<InterruptEventArgs<KeyData>>(stdint_OnKeyPress);
            input = new StringBuilder();
            clock.Enabled = true;
            vmem = new frmVideoMemory();
        }

        void clock_Tick(object sender, EventArgs e)
        {
            interpreter.ExecuteProcess();

            if (Jebnix.Graphics.Graphics.TextChanged)
            {
                vmem.RefreshText();
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        Invalidate(cells[x, y]);
                    }
                }
                Jebnix.Graphics.Graphics.ClearTextChangedFlag();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphics = CreateGraphics();
            BuildCellArray();

            this.ClientSize = new Size(CellWidth * WIDTH, CellHeight * HEIGHT);
        }

        private void BuildCellArray()
        {
            cells = new Rectangle[WIDTH, HEIGHT];
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    cells[x,y] = new Rectangle(x * CellWidth, y * CellHeight, CellWidth, CellHeight);
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            vmem.Show();

            // Print welcome message
            stdio.PrintLine("Welcome to Jebnix");
            stdio.PrintLine(interpreter.GetInterpreterVersion());
            stdio.PrintLine(stdio.GetJebnixVersion());
            stdio.PrintLine();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!IsDrawing)
                {
                    IsDrawing = true;

                    Pen rectPen = Pens.Transparent;
                    if (showGridToolStripMenuItem.Checked) rectPen = Pens.Gray;

                    e.Graphics.DrawString("_", new Font("Lucida Console", 10), Brushes.Lime, cells[Jebnix.Graphics.Graphics.TextColumn, Jebnix.Graphics.Graphics.TextRow]);
                    //if (vm.ShowingCursor)
                    //    e.Graphics.DrawString("_", new Font("Lucida Console", 10), Brushes.Lime, cells[stdio.CursorX, stdio.CursorY]);
                    //else
                    //    e.Graphics.DrawString(" ", new Font("Lucida Console", 10), Brushes.Lime, cells[stdio.CursorX, stdio.CursorY]);
                    e.Graphics.DrawRectangle(rectPen, cells[Jebnix.Graphics.Graphics.TextColumn, Jebnix.Graphics.Graphics.TextRow]);

                    for (int y = 0; y < HEIGHT; y++)
                    {
                        for (int x = 0; x < WIDTH; x++)
                        {
                            string s = char.ToString(Jebnix.Graphics.Graphics.Cells[y][x]);
                            e.Graphics.DrawString(s, new Font("Lucida Console", 10), Brushes.Lime, cells[x, y]);
                            e.Graphics.DrawRectangle(rectPen, cells[x, y]);
                        }
                    }

                    IsDrawing = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace, ex.GetType().Name);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //char c;

            //switch (e.KeyChar)
            //{
            //    case ((char)Keys.Return):
            //        c = '\n'; break;
                    
            //    case ((char)Keys.Back):
            //        c = '\b'; break;

            //    default:
            //        c = e.KeyChar; break;
            //}
            
            //stdio.Print(c);
            ////stdio.Input(c);
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGridToolStripMenuItem.Checked = !showGridToolStripMenuItem.Checked;
            RedrawScreen();
        }

        private void characterMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stdio.ClearScreen();
            stdio.Print(" !\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", true);
            RedrawScreen();
        }

        private void redrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedrawScreen();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stdio.ClearScreen();
            RedrawScreen();
            clock.Stop();
            interpreter = new Processor(new System.IO.DirectoryInfo("Archive"));
        }

        private void RedrawScreen()
        {
            //stdio.ForceRedraw();
            Invalidate();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            bool alt = e.Alt;
            bool shift = e.Shift;
            bool ctrl = e.Control;
            bool capsLock = (e.Modifiers & Keys.CapsLock) != 0;
            //char c = (new KeysConverter()).ConvertToString(e.KeyCode)[0];
            char c = char.ToLower((char)e.KeyValue);
            if (shift ^ capsLock)
                c = char.ToUpper(c);

            KeyData data = new KeyData(alt, ctrl, shift, capsLock, e.KeyCode, c);
            stdint.RaiseOnKeyPress(this, data);
        }

        void stdint_OnKeyPress(object sender, InterruptEventArgs<KeyData> e)
        {
            if (!e.Data.IsFormsKey) return;

            switch (e.Data.KeyCode)
            {
                case Keys.Enter:
                    stdio.ProcessChar('\n');
                    if (Mode == ConsoleMode.Intermediate)
                    {
                        //interpreter.AddCodeToProcess(consoleProc, new string[] { input.ToString() }, "Console");
                        InterpretCommand(input.ToString());
                        input.Clear();
                    }
                    break;

                case Keys.Back:
                    stdio.ProcessChar('\b');
                    if (input.Length > 0)
                        input.Remove(input.Length - 1, 1);
                    break;

                case Keys.Tab:
                    stdio.ProcessChar('\t');
                    input.Append('\t');
                    break;

                default:
                    stdio.ProcessChar(e.Data.Character);
                    input.Append(e.Data.Character);
                    break;
            }
        }

        void InterpretCommand(string command)
        {
            string[] parts = command.ToLower().Trim().Split(' ');

            if (parts.Length == 0)
                return;

            try
            {
                switch (parts[0])
                {
                    case "cls":
                        if (parts.Length == 1)
                            stdio.ClearScreen();
                        else if ((parts[1] == "?") | (parts[1] == "-help"))
                            ShowMan("cls");
                        break;


                    case "help":
                    case "man":
                        if (parts.Length == 1)
                            ShowMan();
                        else
                            ShowMan(parts[1]);
                        break;

                    case "run":
                        if (parts.Length == 2)
                        {
                            if ((parts[1] == "?") | (parts[1] == "-help"))
                                ShowMan("run");
                            else
                                interpreter.CreateProcess(parts[1] + ".txt");
                        }
                        else
                        {
                            stdio.PrintLine("Expected script name.");
                        }
                        break;

                    case "cd":
                        if (parts.Length == 2)
                        {

                        }
                        else
                        {
                            stdio.PrintLine("Expected directory name.");
                        }
                        break;

                    case "copy":
                        if (parts.Length == 3)
                        {
                            SystemAPI.CopyTo(parts[1], parts[2]);
                        }
                        else if (parts.Length == 2)
                        {
                            if ((parts[1] == "?") | (parts[1] == "-help"))
                                ShowMan("run");
                            else
                                stdio.PrintLine("Expected destination.");
                        }
                        else
                        {
                            stdio.PrintLine("Expected source and destination.");
                        }
                        break;

                    case "del":
                        if (parts.Length == 2)
                        {
                            if ((parts[1] == "?") | (parts[1] == "-help"))
                                ShowMan("run");
                            else
                                SystemAPI.Delete(parts[1]);
                        }
                        else
                        {
                            stdio.PrintLine("Expected filename.");
                        }
                        break;

                    default:
                        stdio.PrintLine("Unknown command. Enter MAN for help.");
                        break;
                }
            }
            catch (KerboScriptEngine.KSRuntimeException ex)
            {
                stdio.PrintLine(ex.Message);
            }
        }

        void ShowMan()
        {
            stdio.PrintLine("MANUAL");
            stdio.PrintLine("Shell Manual"); 
            stdio.PrintLine(); 
            stdio.PrintLine("RUN - Compiles and executes a script."); 
            stdio.PrintLine("CD - Changes directories."); 
            stdio.Print("COPY - Copies a file between directories");
            stdio.PrintLine("DEL - Deletes a file."); 
            stdio.PrintLine("EDIT - Starts the editor.");
            stdio.PrintLine("DEBUG - Starts the debugger."); 
            stdio.PrintLine("CLS - Clears the screen.");
            stdio.PrintLine();
            stdio.Print("You can get more information by entering");
            stdio.PrintLine("the name of the command followed by a");
            stdio.PrintLine("question mark, or MAN followed by the");
            stdio.PrintLine("command.");
            stdio.PrintLine("For example:");
            stdio.PrintLine("run ?"); 
            stdio.PrintLine("run -help");
            stdio.PrintLine("man run"); 
        }

        void ShowMan(string topic)
        {
            switch (topic)
            {
                case "cls":
                    stdio.PrintLine("CLS - Clears the screen.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("cls");
                    stdio.PrintLine();
                    break;

                case "run":
                    stdio.PrintLine("RUN - Compiles and executes a script.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("run <scriptname>");
                    stdio.PrintLine();
                    break;

                case "cd":
                    stdio.PrintLine("CD - Changes directories.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("cd <directory>");
                    stdio.PrintLine();
                    break;

                case "copy":
                    stdio.PrintLine("copy - Copies the source file to the destination directory.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("copy <sourcefile> <destination>");
                    stdio.PrintLine();
                    break;

                case "del":
                    stdio.PrintLine("DEL - Deletes a file.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("del <filename>");
                    stdio.PrintLine();
                    break;

                case "edit":
                    stdio.PrintLine("EDIT - Starts the editor.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("edit");
                    stdio.PrintLine("edit <filename>");
                    stdio.PrintLine();
                    stdio.PrintLine("The first version opens the editor with a new file.");
                    stdio.PrintLine("The second version opens the editor with the given file open.");
                    stdio.PrintLine("If the file doesn't exist, it is created.");
                    stdio.PrintLine();
                    break;

                case "debug":
                    stdio.PrintLine("DEBUG - Starts the debugger.");
                    stdio.PrintLine();
                    stdio.PrintLine("Syntax:");
                    stdio.PrintLine("debug <script>");
                    stdio.PrintLine();
                    stdio.PrintLine("The script is compiled and executed with debugger support.");
                    stdio.PrintLine();
                    break;

                default:
                    stdio.PrintLine("Command not recognized.");
                    ShowMan();
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
