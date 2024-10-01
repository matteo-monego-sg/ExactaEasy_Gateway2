namespace ExactaEasy {
    partial class frmWait {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblWaitTxt = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWaitTxt
            // 
            this.lblWaitTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaitTxt.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblWaitTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(77)))));
            this.lblWaitTxt.Location = new System.Drawing.Point(10, 10);
            this.lblWaitTxt.Name = "lblWaitTxt";
            this.lblWaitTxt.Size = new System.Drawing.Size(640, 460);
            this.lblWaitTxt.TabIndex = 0;
            this.lblWaitTxt.Text = "PLEASE WAIT ...";
            this.lblWaitTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lblWaitTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(660, 480);
            this.panel1.TabIndex = 1;
            // 
            // frmWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(680, 500);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmWait";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmClosingForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWaitTxt;
        private System.Windows.Forms.Panel panel1;
    }
}