using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kOSDebugger
{
    public partial class frmVideoMemory : Form
    {
        public frmVideoMemory()
        {
            InitializeComponent();
        }

        private void frmVideoMemory_Load(object sender, EventArgs e)
        {
        }


        public void RefreshText()
        {
            txtMemory.Text = Jebnix.Graphics.Graphics.Text;
            txtMemory.Invalidate();
        }
    }
}
