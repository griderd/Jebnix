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

namespace kOSDebugger
{
    public partial class frmMain : Form
    {
        Rectangle[,] cells;
        VirtualMachine vm;
        Graphics graphics;

        const int CellHeight = 14;
        const int CellWidth = 10;

        /// <summary>
        /// Gets a value that determines if the form is currently being drawn.
        /// </summary>
        public bool IsDrawing { get; private set; }

        public frmMain()
        {
            InitializeComponent();

            stdio.Initialize(TextMonitor.Mode.Window);
            DebugLogger.Initialize();
            vm = new VirtualMachine(10);

            Settings.ShowPath = false;
            //Settings.UsePrompts = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            vm.ScreenRefresh += new EventHandler(vm_ScreenRefresh);
            vm.CursorChanged += new EventHandler(vm_CursorChanged);

            graphics = CreateGraphics();
            BuildCellArray();

            this.ClientSize = new Size(CellWidth * TextMonitor.WIDTH, CellHeight * TextMonitor.HEIGHT);

            vm.ShowCursor = true;
        }

        void vm_CursorChanged(object sender, EventArgs e)
        {
            this.Invalidate(cells[stdio.CursorX, stdio.CursorY]);
            //InvokePaint(this, new PaintEventArgs(graphics, this.DisplayRectangle));
        }

        void vm_ScreenRefresh(object sender, EventArgs e)
        {
            if (stdio.OutputReady)
            {
                for (int y = 0; y < TextMonitor.HEIGHT; y++)
                {
                    for (int x = 0; x < TextMonitor.WIDTH; x++)
                    {
                        if (!stdio.CellValidity[x, y]) Invalidate(cells[x, y]);
                    }
                }
                //InvokePaint(this, new PaintEventArgs(graphics, this.DisplayRectangle));
            }
        }

        private void BuildCellArray()
        {   
            cells = new Rectangle[TextMonitor.WIDTH, TextMonitor.HEIGHT];
            for (int x = 0; x < TextMonitor.WIDTH; x++)
            {
                for (int y = 0; y < TextMonitor.HEIGHT; y++)
                {
                    cells[x,y] = new Rectangle(x * CellWidth, y * CellHeight, CellWidth, CellHeight);
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            vm.Start();
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

                    if (vm.ShowingCursor)
                        e.Graphics.DrawString("_", new Font("Lucida Console", 10), Brushes.Lime, cells[stdio.CursorX, stdio.CursorY]);
                    else
                        e.Graphics.DrawString(" ", new Font("Lucida Console", 10), Brushes.Lime, cells[stdio.CursorX, stdio.CursorY]);
                    e.Graphics.DrawRectangle(rectPen, cells[stdio.CursorX, stdio.CursorY]);

                    for (int y = 0; y < TextMonitor.HEIGHT; y++)
                    {
                        for (int x = 0; x < TextMonitor.WIDTH; x++)
                        {
                            if (!stdio.CellValidity[x, y])
                            {
                                string s = char.ToString(stdio.ScreenCells[x, y]);
                                e.Graphics.DrawString(s, new Font("Lucida Console", 10), Brushes.Lime, cells[x, y]);
                                e.Graphics.DrawRectangle(rectPen, cells[x, y]);
                                stdio.Validate(x, y);
                            }
                        }

                        stdio.ScreenDrawn();
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
            char c;

            switch (e.KeyChar)
            {
                case ((char)Keys.Return):
                    c = '\n'; break;
                    
                case ((char)Keys.Back):
                    c = '\b'; break;

                default:
                    c = e.KeyChar; break;
            }
            
            stdio.Print(c);
            stdio.Input(c);
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
            vm.ShowCursor = false;
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
            vm.Stop();
            vm.ShowCursor = true;
            vm.Start();
        }

        private void RedrawScreen()
        {
            stdio.ForceRedraw();
            Invalidate();
        }
    }
}
