namespace ExactaEasy {
    partial class ExtStatusStrip {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtStatusStrip));
            this.ledImageList = new System.Windows.Forms.ImageList(this.components);
            this.intStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblActiveRecipeName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblActiveRecipeVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAppStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCameraInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMachineStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.intStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ledImageList
            // 
            this.ledImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ledImageList.ImageStream")));
            this.ledImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ledImageList.Images.SetKeyName(0, "white-off-16.png");
            this.ledImageList.Images.SetKeyName(1, "blink-white.gif");
            this.ledImageList.Images.SetKeyName(2, "green-on-16.png");
            this.ledImageList.Images.SetKeyName(3, "yellow-on-16.png");
            this.ledImageList.Images.SetKeyName(4, "red-on-16.png");
            this.ledImageList.Images.SetKeyName(5, "white-on-16.png");
            // 
            // intStatusStrip
            // 
            this.intStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.intStatusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.intStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblActiveRecipeName,
            this.lblActiveRecipeVersion,
            this.lblAppStatus,
            this.lblCameraInfo,
            this.lblMachineStatus});
            this.intStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.intStatusStrip.Name = "intStatusStrip";
            this.intStatusStrip.Size = new System.Drawing.Size(1029, 150);
            this.intStatusStrip.SizingGrip = false;
            this.intStatusStrip.TabIndex = 0;
            // 
            // lblActiveRecipeName
            // 
            this.lblActiveRecipeName.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblActiveRecipeName.Name = "lblActiveRecipeName";
            this.lblActiveRecipeName.Size = new System.Drawing.Size(107, 145);
            this.lblActiveRecipeName.Text = "ActiveRecipeName";
            // 
            // lblActiveRecipeVersion
            // 
            this.lblActiveRecipeVersion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblActiveRecipeVersion.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblActiveRecipeVersion.Name = "lblActiveRecipeVersion";
            this.lblActiveRecipeVersion.Size = new System.Drawing.Size(117, 145);
            this.lblActiveRecipeVersion.Text = "ActiveRecipeVersion";
            // 
            // lblAppStatus
            // 
            this.lblAppStatus.AutoSize = false;
            this.lblAppStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblAppStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblAppStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAppStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblAppStatus.Name = "lblAppStatus";
            this.lblAppStatus.Size = new System.Drawing.Size(409, 145);
            this.lblAppStatus.Spring = true;
            this.lblAppStatus.Text = "AppStatus";
            // 
            // lblCameraInfo
            // 
            this.lblCameraInfo.AutoSize = false;
            this.lblCameraInfo.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCameraInfo.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblCameraInfo.Name = "lblCameraInfo";
            this.lblCameraInfo.Size = new System.Drawing.Size(250, 145);
            this.lblCameraInfo.Text = "CameraInfo";
            // 
            // lblMachineStatus
            // 
            this.lblMachineStatus.AutoSize = false;
            this.lblMachineStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblMachineStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblMachineStatus.Name = "lblMachineStatus";
            this.lblMachineStatus.Size = new System.Drawing.Size(100, 145);
            this.lblMachineStatus.Text = "MachineStatus";
            // 
            // ExtStatusStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.Controls.Add(this.intStatusStrip);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "ExtStatusStrip";
            this.Size = new System.Drawing.Size(1029, 150);
            this.intStatusStrip.ResumeLayout(false);
            this.intStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList ledImageList;
        private System.Windows.Forms.ToolStripStatusLabel lblAppStatus;
        private System.Windows.Forms.StatusStrip intStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblMachineStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCameraInfo;
        private System.Windows.Forms.ToolStripStatusLabel lblActiveRecipeName;
        private System.Windows.Forms.ToolStripStatusLabel lblActiveRecipeVersion;
    }
}
