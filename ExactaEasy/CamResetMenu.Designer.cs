namespace ExactaEasy {
    partial class CamResetMenu {
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
            this.btnExitResetMenu = new System.Windows.Forms.Button();
            this.btnSoftReset = new System.Windows.Forms.Button();
            this.btnHardReset = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 23);
            this.panel1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "RESET";
            // 
            // btnExitResetMenu
            // 
            this.btnExitResetMenu.AutoEllipsis = true;
            this.btnExitResetMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExitResetMenu.FlatAppearance.BorderSize = 0;
            this.btnExitResetMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitResetMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExitResetMenu.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExitResetMenu.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExitResetMenu.Location = new System.Drawing.Point(664, 23);
            this.btnExitResetMenu.Name = "btnExitResetMenu";
            this.btnExitResetMenu.Size = new System.Drawing.Size(72, 57);
            this.btnExitResetMenu.TabIndex = 22;
            this.btnExitResetMenu.Text = "Exit";
            this.btnExitResetMenu.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitResetMenu.UseVisualStyleBackColor = true;
            this.btnExitResetMenu.Click += new System.EventHandler(this.btnExitResetMenu_Click);
            // 
            // btnSoftReset
            // 
            this.btnSoftReset.AutoEllipsis = true;
            this.btnSoftReset.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSoftReset.FlatAppearance.BorderSize = 0;
            this.btnSoftReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSoftReset.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSoftReset.Image = global::ExactaEasy.Properties.Resources.run_build_prune;
            this.btnSoftReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSoftReset.Location = new System.Drawing.Point(0, 23);
            this.btnSoftReset.Name = "btnSoftReset";
            this.btnSoftReset.Size = new System.Drawing.Size(72, 57);
            this.btnSoftReset.TabIndex = 23;
            this.btnSoftReset.Text = "Soft Reset";
            this.btnSoftReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSoftReset.UseVisualStyleBackColor = true;
            this.btnSoftReset.Click += new System.EventHandler(this.btnSoftReset_Click);
            // 
            // btnHardReset
            // 
            this.btnHardReset.AutoEllipsis = true;
            this.btnHardReset.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnHardReset.FlatAppearance.BorderSize = 0;
            this.btnHardReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHardReset.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnHardReset.Image = global::ExactaEasy.Properties.Resources.dialog_error_ruotato1;
            this.btnHardReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHardReset.Location = new System.Drawing.Point(72, 23);
            this.btnHardReset.Name = "btnHardReset";
            this.btnHardReset.Size = new System.Drawing.Size(72, 57);
            this.btnHardReset.TabIndex = 24;
            this.btnHardReset.Text = "Hard Reset";
            this.btnHardReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHardReset.UseVisualStyleBackColor = true;
            this.btnHardReset.Click += new System.EventHandler(this.btnHardReset_Click);
            // 
            // CamResetMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnHardReset);
            this.Controls.Add(this.btnSoftReset);
            this.Controls.Add(this.btnExitResetMenu);
            this.Controls.Add(this.panel1);
            this.Name = "CamResetMenu";
            this.Size = new System.Drawing.Size(736, 80);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExitResetMenu;
        private System.Windows.Forms.Button btnSoftReset;
        private System.Windows.Forms.Button btnHardReset;
    }
}
