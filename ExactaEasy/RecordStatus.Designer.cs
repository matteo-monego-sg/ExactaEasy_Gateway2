namespace ExactaEasy {
    partial class RecordStatus {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.pnlRec = new System.Windows.Forms.Panel();
            this.tbRecBuffer = new System.Windows.Forms.TextBox();
            this.tbRecBufferValue = new System.Windows.Forms.TextBox();
            this.pbRec = new System.Windows.Forms.ProgressBar();
            this.pnlRec.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRec
            // 
            this.pnlRec.Controls.Add(this.tbRecBuffer);
            this.pnlRec.Controls.Add(this.tbRecBufferValue);
            this.pnlRec.Controls.Add(this.pbRec);
            this.pnlRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRec.Location = new System.Drawing.Point(0, 0);
            this.pnlRec.Name = "pnlRec";
            this.pnlRec.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlRec.Size = new System.Drawing.Size(150, 150);
            this.pnlRec.TabIndex = 16;
            // 
            // tbRecBuffer
            // 
            this.tbRecBuffer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbRecBuffer.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbRecBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.tbRecBuffer.Location = new System.Drawing.Point(0, 0);
            this.tbRecBuffer.Name = "tbRecBuffer";
            this.tbRecBuffer.Size = new System.Drawing.Size(100, 18);
            this.tbRecBuffer.TabIndex = 15;
            this.tbRecBuffer.Text = "Record buffer";
            // 
            // tbRecBufferValue
            // 
            this.tbRecBufferValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbRecBufferValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.tbRecBufferValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.tbRecBufferValue.Location = new System.Drawing.Point(100, 0);
            this.tbRecBufferValue.Name = "tbRecBufferValue";
            this.tbRecBufferValue.Size = new System.Drawing.Size(50, 18);
            this.tbRecBufferValue.TabIndex = 16;
            this.tbRecBufferValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pbRec
            // 
            this.pbRec.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRec.Location = new System.Drawing.Point(0, 0);
            this.pbRec.Name = "pbRec";
            this.pbRec.Size = new System.Drawing.Size(150, 140);
            this.pbRec.Step = 1;
            this.pbRec.TabIndex = 14;
            // 
            // RecordStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.pnlRec);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "RecordStatus";
            this.pnlRec.ResumeLayout(false);
            this.pnlRec.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlRec;
        private System.Windows.Forms.TextBox tbRecBuffer;
        private System.Windows.Forms.TextBox tbRecBufferValue;
        private System.Windows.Forms.ProgressBar pbRec;
    }
}
