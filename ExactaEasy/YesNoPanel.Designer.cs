namespace ExactaEasy {
    partial class YesNoPanel {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 80);
            this.panel1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 80);
            this.label2.TabIndex = 27;
            this.label2.Text = "ARE YOU SURE?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExit
            // 
            this.btnExit.AutoEllipsis = true;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExit.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(16, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 80);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExitStopCondMenu_Click);
            // 
            // btnYes
            // 
            this.btnYes.AutoEllipsis = true;
            this.btnYes.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.Image = global::ExactaEasy.ResourcesArtic.checkmark2;
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnYes.Location = new System.Drawing.Point(124, 0);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(72, 80);
            this.btnYes.TabIndex = 26;
            this.btnYes.Text = "Yes";
            this.btnYes.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.AutoEllipsis = true;
            this.btnNo.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Image = global::ExactaEasy.ResourcesArtic.delete;
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNo.Location = new System.Drawing.Point(196, 0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(72, 80);
            this.btnNo.TabIndex = 25;
            this.btnNo.Text = "No";
            this.btnNo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // YesNoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "YesNoPanel";
            this.Size = new System.Drawing.Size(268, 80);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnExit;
    }
}
