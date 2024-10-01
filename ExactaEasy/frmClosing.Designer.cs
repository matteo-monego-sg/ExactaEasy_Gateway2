namespace ExactaEasy {
    partial class frmClosing {
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
            this.components = new System.ComponentModel.Container();
            this.lblClosingTxt = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClosingTxt
            // 
            this.lblClosingTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClosingTxt.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblClosingTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(77)))));
            this.lblClosingTxt.Location = new System.Drawing.Point(10, 10);
            this.lblClosingTxt.Name = "lblClosingTxt";
            this.lblClosingTxt.Size = new System.Drawing.Size(640, 460);
            this.lblClosingTxt.TabIndex = 0;
            this.lblClosingTxt.Text = "VISION SPV IS GOING TO SLEEP ...";
            this.lblClosingTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblClosingTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(660, 480);
            this.panel1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ExactaEasy.Properties.Resources.SG_Optrel_New;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(205, 393);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 74);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // frmClosing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(680, 500);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmClosing";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmClosingForm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblClosingTxt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}