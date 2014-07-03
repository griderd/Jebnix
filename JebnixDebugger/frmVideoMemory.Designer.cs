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
            this.txtHexView = new System.Windows.Forms.TextBox();
            this.chkHexDigits = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtMemory
            // 
            this.txtMemory.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemory.Location = new System.Drawing.Point(12, 12);
            this.txtMemory.Multiline = true;
            this.txtMemory.Name = "txtMemory";
            this.txtMemory.Size = new System.Drawing.Size(392, 428);
            this.txtMemory.TabIndex = 0;
            this.txtMemory.Text = "This isntance is being run outside of KSP.";
            // 
            // txtHexView
            // 
            this.txtHexView.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHexView.Location = new System.Drawing.Point(410, 12);
            this.txtHexView.Multiline = true;
            this.txtHexView.Name = "txtHexView";
            this.txtHexView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHexView.Size = new System.Drawing.Size(732, 428);
            this.txtHexView.TabIndex = 1;
            // 
            // chkHexDigits
            // 
            this.chkHexDigits.AutoSize = true;
            this.chkHexDigits.Location = new System.Drawing.Point(410, 446);
            this.chkHexDigits.Name = "chkHexDigits";
            this.chkHexDigits.Size = new System.Drawing.Size(74, 17);
            this.chkHexDigits.TabIndex = 2;
            this.chkHexDigits.Text = "Hex Digits";
            this.chkHexDigits.UseVisualStyleBackColor = true;
            this.chkHexDigits.CheckedChanged += new System.EventHandler(this.chkHexDigits_CheckedChanged);
            // 
            // frmVideoMemory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 496);
            this.Controls.Add(this.chkHexDigits);
            this.Controls.Add(this.txtHexView);
            this.Controls.Add(this.txtMemory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmVideoMemory";
            this.Text = "Video Memory";
            this.Load += new System.EventHandler(this.frmVideoMemory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtMemory;
        internal System.Windows.Forms.TextBox txtHexView;
        private System.Windows.Forms.CheckBox chkHexDigits;


    }
}