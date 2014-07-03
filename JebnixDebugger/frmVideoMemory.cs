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
            this.Text = "Video Memory (" + Jebnix.Graphics.Graphics.TextWidth.ToString() + "x" + Jebnix.Graphics.Graphics.TextHeight.ToString() + ")";
            txtMemory.Text = Jebnix.Graphics.Graphics.Text;

            StringBuilder hex = new StringBuilder();
            for (int i = 0; i < txtMemory.Text.Length; i++)
            {
                if (i % 16 == 0)
                    hex.Append(i.ToString("X3"));
                hex.Append("  ");
                if (chkHexDigits.Checked)
                    hex.Append(((int)txtMemory.Text[i]).ToString("X2"));
                else
                {
                    hex.Append(' ');
                    hex.Append(txtMemory.Text[i]);
                }

                if ((i + 1) % 16 == 0)
                    hex.AppendLine();
            }
            txtHexView.Text = hex.ToString();

            txtMemory.Invalidate();
        }

        private void chkHexDigits_CheckedChanged(object sender, EventArgs e)
        {
            RefreshText();
        }
    }
}
