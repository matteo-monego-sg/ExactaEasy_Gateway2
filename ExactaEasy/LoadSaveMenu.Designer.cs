namespace ExactaEasy {
    partial class LoadSaveMenu {
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
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
            this.label2.Size = new System.Drawing.Size(321, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "LOAD / SAVE SPECTROPHOTOMETER PARAMETERS";
            // 
            // btnSave
            // 
            this.btnSave.AutoEllipsis = true;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::ExactaEasy.Properties.Resources.document_save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(72, 23);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 57);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.AutoEllipsis = true;
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Image = global::ExactaEasy.Properties.Resources.folder_photo1;
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoad.Location = new System.Drawing.Point(0, 23);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(72, 57);
            this.btnLoad.TabIndex = 24;
            this.btnLoad.Text = "Load";
            this.btnLoad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExit
            // 
            this.btnExit.AutoEllipsis = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExit.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(664, 23);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 57);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // LoadSaveMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "LoadSaveMenu";
            this.Size = new System.Drawing.Size(736, 80);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
    }
}
