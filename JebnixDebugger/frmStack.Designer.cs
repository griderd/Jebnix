namespace kOSDebugger
{
    partial class frmStack
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstStack = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstStack
            // 
            this.lstStack.FormattingEnabled = true;
            this.lstStack.Location = new System.Drawing.Point(12, 12);
            this.lstStack.Name = "lstStack";
            this.lstStack.Size = new System.Drawing.Size(319, 407);
            this.lstStack.TabIndex = 0;
            this.lstStack.SelectedIndexChanged += new System.EventHandler(this.lstStack_SelectedIndexChanged);
            // 
            // frmStack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 437);
            this.Controls.Add(this.lstStack);
            this.Name = "frmStack";
            this.Text = "Stack Viewer";
            this.Load += new System.EventHandler(this.frmStack_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstStack;
    }
}