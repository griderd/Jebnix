namespace kOSDebugger
{
    partial class frmVideoMemory
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
            this.txtMemory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMemory
            // 
            this.txtMemory.Location = new System.Drawing.Point(17, 15);
            this.txtMemory.Multiline = true;
            this.txtMemory.Name = "txtMemory";
            this.txtMemory.Size = new System.Drawing.Size(550, 452);
            this.txtMemory.TabIndex = 0;
            // 
            // frmVideoMemory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 492);
            this.Controls.Add(this.txtMemory);
            this.Name = "frmVideoMemory";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmVideoMemory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtMemory;


    }
}