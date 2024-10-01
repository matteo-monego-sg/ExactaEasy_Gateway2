namespace ExactaEasy {
    partial class DownloadImagesMenu {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFramesUpload = new System.Windows.Forms.Button();
            this.btnRejectsBufferDownload = new System.Windows.Forms.Button();
            this.btnCurrentResDownload = new System.Windows.Forms.Button();
            this.btnFramesDownload = new System.Windows.Forms.Button();
            this.btnExitImagesDownloadsMenu = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 23);
            this.panel1.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(228, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "IMAGES UPLOAD";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "IMAGES DOWNLOADS";
            // 
            // btnFramesUpload
            // 
            this.btnFramesUpload.AutoEllipsis = true;
            this.btnFramesUpload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnFramesUpload.Enabled = false;
            this.btnFramesUpload.FlatAppearance.BorderSize = 0;
            this.btnFramesUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFramesUpload.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFramesUpload.Image = global::ExactaEasy.Properties.Resources.webcamsend;
            this.btnFramesUpload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFramesUpload.Location = new System.Drawing.Point(216, 23);
            this.btnFramesUpload.Name = "btnFramesUpload";
            this.btnFramesUpload.Size = new System.Drawing.Size(72, 57);
            this.btnFramesUpload.TabIndex = 26;
            this.btnFramesUpload.Text = "Upload";
            this.btnFramesUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFramesUpload.UseVisualStyleBackColor = true;
            this.btnFramesUpload.Click += new System.EventHandler(this.btnFramesUpload_Click);
            // 
            // btnRejectsBufferDownload
            // 
            this.btnRejectsBufferDownload.AutoEllipsis = true;
            this.btnRejectsBufferDownload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRejectsBufferDownload.FlatAppearance.BorderSize = 0;
            this.btnRejectsBufferDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRejectsBufferDownload.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRejectsBufferDownload.Image = global::ExactaEasy.Properties.Resources.folder_book;
            this.btnRejectsBufferDownload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRejectsBufferDownload.Location = new System.Drawing.Point(144, 23);
            this.btnRejectsBufferDownload.Name = "btnRejectsBufferDownload";
            this.btnRejectsBufferDownload.Size = new System.Drawing.Size(72, 57);
            this.btnRejectsBufferDownload.TabIndex = 25;
            this.btnRejectsBufferDownload.Text = "Rejects buffer";
            this.btnRejectsBufferDownload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRejectsBufferDownload.UseVisualStyleBackColor = true;
            this.btnRejectsBufferDownload.Click += new System.EventHandler(this.btnRejectsBufferDownload_Click);
            // 
            // btnCurrentResDownload
            // 
            this.btnCurrentResDownload.AutoEllipsis = true;
            this.btnCurrentResDownload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCurrentResDownload.FlatAppearance.BorderSize = 0;
            this.btnCurrentResDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurrentResDownload.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCurrentResDownload.Image = global::ExactaEasy.Properties.Resources.camera_photo_big;
            this.btnCurrentResDownload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCurrentResDownload.Location = new System.Drawing.Point(72, 23);
            this.btnCurrentResDownload.Name = "btnCurrentResDownload";
            this.btnCurrentResDownload.Size = new System.Drawing.Size(72, 57);
            this.btnCurrentResDownload.TabIndex = 24;
            this.btnCurrentResDownload.Text = "Current";
            this.btnCurrentResDownload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCurrentResDownload.UseVisualStyleBackColor = true;
            this.btnCurrentResDownload.Click += new System.EventHandler(this.btnCurrentResDownload_Click);
            // 
            // btnFramesDownload
            // 
            this.btnFramesDownload.AutoEllipsis = true;
            this.btnFramesDownload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnFramesDownload.FlatAppearance.BorderSize = 0;
            this.btnFramesDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFramesDownload.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFramesDownload.Image = global::ExactaEasy.Properties.Resources.webcamreceive;
            this.btnFramesDownload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFramesDownload.Location = new System.Drawing.Point(0, 23);
            this.btnFramesDownload.Name = "btnFramesDownload";
            this.btnFramesDownload.Size = new System.Drawing.Size(72, 57);
            this.btnFramesDownload.TabIndex = 23;
            this.btnFramesDownload.Text = "Frames";
            this.btnFramesDownload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFramesDownload.UseVisualStyleBackColor = true;
            this.btnFramesDownload.Click += new System.EventHandler(this.btnFramesDownload_Click);
            // 
            // btnExitImagesDownloadsMenu
            // 
            this.btnExitImagesDownloadsMenu.AutoEllipsis = true;
            this.btnExitImagesDownloadsMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExitImagesDownloadsMenu.FlatAppearance.BorderSize = 0;
            this.btnExitImagesDownloadsMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitImagesDownloadsMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExitImagesDownloadsMenu.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExitImagesDownloadsMenu.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExitImagesDownloadsMenu.Location = new System.Drawing.Point(664, 23);
            this.btnExitImagesDownloadsMenu.Name = "btnExitImagesDownloadsMenu";
            this.btnExitImagesDownloadsMenu.Size = new System.Drawing.Size(72, 57);
            this.btnExitImagesDownloadsMenu.TabIndex = 22;
            this.btnExitImagesDownloadsMenu.Text = "Exit";
            this.btnExitImagesDownloadsMenu.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitImagesDownloadsMenu.UseVisualStyleBackColor = true;
            this.btnExitImagesDownloadsMenu.Click += new System.EventHandler(this.btnExitImagesDownloadsMenu_Click);
            // 
            // DownloadImagesMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.btnFramesUpload);
            this.Controls.Add(this.btnRejectsBufferDownload);
            this.Controls.Add(this.btnCurrentResDownload);
            this.Controls.Add(this.btnFramesDownload);
            this.Controls.Add(this.btnExitImagesDownloadsMenu);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "DownloadImagesMenu";
            this.Size = new System.Drawing.Size(736, 80);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExitImagesDownloadsMenu;
        private System.Windows.Forms.Button btnFramesDownload;
        private System.Windows.Forms.Button btnCurrentResDownload;
        private System.Windows.Forms.Button btnRejectsBufferDownload;
        private System.Windows.Forms.Button btnFramesUpload;
        private System.Windows.Forms.Label label1;
    }
}
