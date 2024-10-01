namespace ExactaEasy {
    partial class VialAxisMenu {
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
            this.btnExitVialAxisMenu = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
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
            this.label2.Size = new System.Drawing.Size(126, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "BACKUP / RESTORE";
            // 
            // btnExitVialAxisMenu
            // 
            this.btnExitVialAxisMenu.AutoEllipsis = true;
            this.btnExitVialAxisMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExitVialAxisMenu.FlatAppearance.BorderSize = 0;
            this.btnExitVialAxisMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitVialAxisMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExitVialAxisMenu.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExitVialAxisMenu.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExitVialAxisMenu.Location = new System.Drawing.Point(664, 23);
            this.btnExitVialAxisMenu.Name = "btnExitVialAxisMenu";
            this.btnExitVialAxisMenu.Size = new System.Drawing.Size(72, 57);
            this.btnExitVialAxisMenu.TabIndex = 22;
            this.btnExitVialAxisMenu.Text = "Exit";
            this.btnExitVialAxisMenu.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitVialAxisMenu.UseVisualStyleBackColor = true;
            this.btnExitVialAxisMenu.Click += new System.EventHandler(this.btnExitVialAxisMenu_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.AutoEllipsis = true;
            this.btnBackup.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBackup.FlatAppearance.BorderSize = 0;
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnBackup.Image = global::ExactaEasy.Properties.Resources.document_save;
            this.btnBackup.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBackup.Location = new System.Drawing.Point(0, 23);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(72, 57);
            this.btnBackup.TabIndex = 23;
            this.btnBackup.Text = "Backup";
            this.btnBackup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.AutoEllipsis = true;
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRestore.Image = global::ExactaEasy.Properties.Resources.document_export1;
            this.btnRestore.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRestore.Location = new System.Drawing.Point(72, 23);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(72, 57);
            this.btnRestore.TabIndex = 24;
            this.btnRestore.Text = "Restore";
            this.btnRestore.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // VialAxisMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnExitVialAxisMenu);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "VialAxisMenu";
            this.Size = new System.Drawing.Size(736, 80);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExitVialAxisMenu;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnRestore;
    }
}
