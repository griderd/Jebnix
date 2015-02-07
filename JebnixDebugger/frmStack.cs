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
    public partial class frmStack : Form
    {
        public frmStack()
        {
            InitializeComponent();
        }

        private void frmStack_Load(object sender, EventArgs e)
        {

        }

        private void lstStack_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void RefreshStack(KerboScriptEngine.Processor processor)
        {
            lstStack.Items.Clear();

            if (processor.CurrentProcess != null)
            {
                Jebnix.Types.JObject[] values = processor.CurrentProcess.DataStack;
                foreach (Jebnix.Types.JObject value in values)
                {
                    lstStack.Items.Add(value.ToString());
                }
            }
        }
    }
}
