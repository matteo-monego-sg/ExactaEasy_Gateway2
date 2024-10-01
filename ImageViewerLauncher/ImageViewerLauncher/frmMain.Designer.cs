namespace ImageViewerLauncher {
    partial class frmMain {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.imageViewer1 = new SPAMI.Util.EmguImageViewer.ImageViewer();
            this.SuspendLayout();
            // 
            // imageViewer1
            // 
            this.imageViewer1.AllowDrop = true;
            this.imageViewer1.BackColor = System.Drawing.Color.Transparent;
            this.imageViewer1.ClassName = "ImageViewer";
            this.imageViewer1.ContinuousRecycle = true;
            this.imageViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer1.Fps = 10D;
            this.imageViewer1.FullframeZoomMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageViewer1.Location = new System.Drawing.Point(0, 0);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.OfflineRecycleFolderPath = "";
            this.imageViewer1.Online = false;
            this.imageViewer1.PixInfoHeight = 5;
            this.imageViewer1.PixInfoWidth = 5;
            this.imageViewer1.QueueCapacity = 10;
            this.imageViewer1.ShowAdjacentPixel = true;
            this.imageViewer1.ShowBigHistogram = false;
            this.imageViewer1.ShowHistogram = false;
            this.imageViewer1.ShowHorizontalPixel = false;
            this.imageViewer1.ShowVerticalPixel = false;
            this.imageViewer1.Size = new System.Drawing.Size(784, 562);
            this.imageViewer1.TabIndex = 0;
            this.imageViewer1.ZoomActive = true;
            this.imageViewer1.ZoomStretch = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.imageViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Image Viewer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SPAMI.Util.EmguImageViewer.ImageViewer imageViewer1;
    }
}

