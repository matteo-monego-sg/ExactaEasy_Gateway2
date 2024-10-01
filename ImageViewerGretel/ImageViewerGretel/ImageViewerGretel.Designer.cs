namespace ImageViewerGretel
{
    partial class ImageViewerGretel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlControlsMenu = new System.Windows.Forms.Panel();
            this.btnSaveImagesMenu = new System.Windows.Forms.Button();
            this.tbRate = new System.Windows.Forms.TrackBar();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.rbPause = new System.Windows.Forms.RadioButton();
            this.rbPlay = new System.Windows.Forms.RadioButton();
            this.pnlImageInfo = new System.Windows.Forms.Panel();
            this.lblZoom = new System.Windows.Forms.Label();
            this.lblMini = new System.Windows.Forms.Label();
            this.splitContImage = new System.Windows.Forms.SplitContainer();
            this.imageBoxMini = new Emgu.CV.UI.ImageBox();
            this.imageBoxResult = new Emgu.CV.UI.ImageBox();
            this.imageBoxZoom = new Emgu.CV.UI.ImageBox();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.pnlSaveImagesMenu = new System.Windows.Forms.Panel();
            this.btnSaveImagesMenuExit = new System.Windows.Forms.Button();
            this.btnSaveAllImages = new System.Windows.Forms.Button();
            this.btnSaveSingleImage = new System.Windows.Forms.Button();
            this.timerRefresh = new System.Windows.Forms.Timer();
            this.pnlControlsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbRate)).BeginInit();
            this.pnlImageInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContImage)).BeginInit();
            this.splitContImage.Panel1.SuspendLayout();
            this.splitContImage.Panel2.SuspendLayout();
            this.splitContImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxMini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxZoom)).BeginInit();
            this.pnlSaveImagesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControlsMenu
            // 
            this.pnlControlsMenu.Controls.Add(this.btnSaveImagesMenu);
            this.pnlControlsMenu.Controls.Add(this.tbRate);
            this.pnlControlsMenu.Controls.Add(this.btnNext);
            this.pnlControlsMenu.Controls.Add(this.btnPrev);
            this.pnlControlsMenu.Controls.Add(this.rbPause);
            this.pnlControlsMenu.Controls.Add(this.rbPlay);
            this.pnlControlsMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControlsMenu.ForeColor = System.Drawing.Color.White;
            this.pnlControlsMenu.Location = new System.Drawing.Point(0, 652);
            this.pnlControlsMenu.Name = "pnlControlsMenu";
            this.pnlControlsMenu.Size = new System.Drawing.Size(1024, 58);
            this.pnlControlsMenu.TabIndex = 0;
            // 
            // btnSaveImagesMenu
            // 
            this.btnSaveImagesMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSaveImagesMenu.FlatAppearance.BorderSize = 0;
            this.btnSaveImagesMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSaveImagesMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSaveImagesMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveImagesMenu.Image = global::ImageViewerGretel.Properties.Resources.save_image;
            this.btnSaveImagesMenu.Location = new System.Drawing.Point(966, 0);
            this.btnSaveImagesMenu.Name = "btnSaveImagesMenu";
            this.btnSaveImagesMenu.Size = new System.Drawing.Size(58, 58);
            this.btnSaveImagesMenu.TabIndex = 5;
            this.btnSaveImagesMenu.UseVisualStyleBackColor = true;
            this.btnSaveImagesMenu.Click += new System.EventHandler(this.btnSaveImagesMenu_Click);
            // 
            // tbRate
            // 
            this.tbRate.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbRate.Location = new System.Drawing.Point(232, 0);
            this.tbRate.Margin = new System.Windows.Forms.Padding(0);
            this.tbRate.Maximum = 333;
            this.tbRate.Minimum = 10;
            this.tbRate.Name = "tbRate";
            this.tbRate.Size = new System.Drawing.Size(160, 58);
            this.tbRate.TabIndex = 1;
            this.tbRate.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbRate.Value = 333;
            this.tbRate.Scroll += new System.EventHandler(this.tbRate_Scroll);
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Image = global::ImageViewerGretel.Properties.Resources.next;
            this.btnNext.Location = new System.Drawing.Point(174, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(58, 58);
            this.btnNext.TabIndex = 4;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Image = global::ImageViewerGretel.Properties.Resources.prev;
            this.btnPrev.Location = new System.Drawing.Point(116, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(58, 58);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // rbPause
            // 
            this.rbPause.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbPause.BackColor = System.Drawing.Color.Black;
            this.rbPause.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbPause.FlatAppearance.BorderSize = 0;
            this.rbPause.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.rbPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.rbPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.rbPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPause.ForeColor = System.Drawing.Color.Silver;
            this.rbPause.Image = global::ImageViewerGretel.Properties.Resources.media_playback_pause;
            this.rbPause.Location = new System.Drawing.Point(58, 0);
            this.rbPause.Name = "rbPause";
            this.rbPause.Size = new System.Drawing.Size(58, 58);
            this.rbPause.TabIndex = 7;
            this.rbPause.TabStop = true;
            this.rbPause.UseVisualStyleBackColor = false;
            this.rbPause.CheckedChanged += new System.EventHandler(this.rbPause_CheckedChanged);
            // 
            // rbPlay
            // 
            this.rbPlay.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbPlay.BackColor = System.Drawing.Color.Black;
            this.rbPlay.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbPlay.FlatAppearance.BorderSize = 0;
            this.rbPlay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.rbPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.rbPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.rbPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbPlay.ForeColor = System.Drawing.Color.Silver;
            this.rbPlay.Image = global::ImageViewerGretel.Properties.Resources.media_playback_start;
            this.rbPlay.Location = new System.Drawing.Point(0, 0);
            this.rbPlay.Name = "rbPlay";
            this.rbPlay.Size = new System.Drawing.Size(58, 58);
            this.rbPlay.TabIndex = 6;
            this.rbPlay.TabStop = true;
            this.rbPlay.UseVisualStyleBackColor = false;
            this.rbPlay.CheckedChanged += new System.EventHandler(this.rbPlay_CheckedChanged);
            // 
            // pnlImageInfo
            // 
            this.pnlImageInfo.Controls.Add(this.lblZoom);
            this.pnlImageInfo.Controls.Add(this.lblMini);
            this.pnlImageInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlImageInfo.Location = new System.Drawing.Point(0, 632);
            this.pnlImageInfo.Name = "pnlImageInfo";
            this.pnlImageInfo.Size = new System.Drawing.Size(1024, 20);
            this.pnlImageInfo.TabIndex = 0;
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblZoom.Location = new System.Drawing.Point(265, 0);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(0, 13);
            this.lblZoom.TabIndex = 1;
            // 
            // lblMini
            // 
            this.lblMini.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMini.Location = new System.Drawing.Point(0, 0);
            this.lblMini.Name = "lblMini";
            this.lblMini.Size = new System.Drawing.Size(265, 20);
            this.lblMini.TabIndex = 0;
            // 
            // splitContImage
            // 
            this.splitContImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContImage.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContImage.Location = new System.Drawing.Point(0, 0);
            this.splitContImage.Name = "splitContImage";
            // 
            // splitContImage.Panel1
            // 
            this.splitContImage.Panel1.Controls.Add(this.imageBoxMini);
            this.splitContImage.Panel1.Controls.Add(this.imageBoxResult);
            // 
            // splitContImage.Panel2
            // 
            this.splitContImage.Panel2.Controls.Add(this.imageBoxZoom);
            this.splitContImage.Panel2.Controls.Add(this.btnCollapse);
            this.splitContImage.Panel2.SizeChanged += new System.EventHandler(this.splitContImage_Panel2_SizeChanged);
            this.splitContImage.Size = new System.Drawing.Size(1024, 632);
            this.splitContImage.SplitterDistance = 266;
            this.splitContImage.TabIndex = 1;
            // 
            // imageBoxMini
            // 
            this.imageBoxMini.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxMini.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBoxMini.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imageBoxMini.Location = new System.Drawing.Point(0, 268);
            this.imageBoxMini.Name = "imageBoxMini";
            this.imageBoxMini.Size = new System.Drawing.Size(264, 362);
            this.imageBoxMini.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBoxMini.TabIndex = 2;
            this.imageBoxMini.TabStop = false;
            // 
            // imageBoxResult
            // 
            this.imageBoxResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.imageBoxResult.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBoxResult.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imageBoxResult.Location = new System.Drawing.Point(0, 0);
            this.imageBoxResult.Name = "imageBoxResult";
            this.imageBoxResult.Size = new System.Drawing.Size(264, 268);
            this.imageBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBoxResult.TabIndex = 2;
            this.imageBoxResult.TabStop = false;
            // 
            // imageBoxZoom
            // 
            this.imageBoxZoom.Cursor = System.Windows.Forms.Cursors.Cross;
            this.imageBoxZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxZoom.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBoxZoom.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imageBoxZoom.Location = new System.Drawing.Point(33, 0);
            this.imageBoxZoom.Name = "imageBoxZoom";
            this.imageBoxZoom.Size = new System.Drawing.Size(719, 630);
            this.imageBoxZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageBoxZoom.TabIndex = 2;
            this.imageBoxZoom.TabStop = false;
            this.imageBoxZoom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBoxZoom_MouseMove);
            // 
            // btnCollapse
            // 
            this.btnCollapse.BackColor = System.Drawing.Color.Black;
            this.btnCollapse.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCollapse.FlatAppearance.BorderSize = 0;
            this.btnCollapse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCollapse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollapse.Location = new System.Drawing.Point(0, 0);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(33, 630);
            this.btnCollapse.TabIndex = 3;
            this.btnCollapse.UseVisualStyleBackColor = false;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // pnlSaveImagesMenu
            // 
            this.pnlSaveImagesMenu.Controls.Add(this.btnSaveImagesMenuExit);
            this.pnlSaveImagesMenu.Controls.Add(this.btnSaveAllImages);
            this.pnlSaveImagesMenu.Controls.Add(this.btnSaveSingleImage);
            this.pnlSaveImagesMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSaveImagesMenu.ForeColor = System.Drawing.Color.Transparent;
            this.pnlSaveImagesMenu.Location = new System.Drawing.Point(0, 710);
            this.pnlSaveImagesMenu.Name = "pnlSaveImagesMenu";
            this.pnlSaveImagesMenu.Size = new System.Drawing.Size(1024, 58);
            this.pnlSaveImagesMenu.TabIndex = 0;
            this.pnlSaveImagesMenu.Visible = false;
            // 
            // btnSaveImagesMenuExit
            // 
            this.btnSaveImagesMenuExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSaveImagesMenuExit.FlatAppearance.BorderSize = 0;
            this.btnSaveImagesMenuExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSaveImagesMenuExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSaveImagesMenuExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveImagesMenuExit.Image = global::ImageViewerGretel.Properties.Resources.edit_undo;
            this.btnSaveImagesMenuExit.Location = new System.Drawing.Point(966, 0);
            this.btnSaveImagesMenuExit.Name = "btnSaveImagesMenuExit";
            this.btnSaveImagesMenuExit.Size = new System.Drawing.Size(58, 58);
            this.btnSaveImagesMenuExit.TabIndex = 8;
            this.btnSaveImagesMenuExit.UseVisualStyleBackColor = true;
            this.btnSaveImagesMenuExit.Click += new System.EventHandler(this.btnSaveImagesMenuExit_Click);
            // 
            // btnSaveAllImages
            // 
            this.btnSaveAllImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveAllImages.FlatAppearance.BorderSize = 0;
            this.btnSaveAllImages.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSaveAllImages.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSaveAllImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAllImages.Image = global::ImageViewerGretel.Properties.Resources.save_images1;
            this.btnSaveAllImages.Location = new System.Drawing.Point(58, 0);
            this.btnSaveAllImages.Name = "btnSaveAllImages";
            this.btnSaveAllImages.Size = new System.Drawing.Size(58, 58);
            this.btnSaveAllImages.TabIndex = 7;
            this.btnSaveAllImages.UseVisualStyleBackColor = true;
            this.btnSaveAllImages.Click += new System.EventHandler(this.btnSaveAllImages_Click);
            // 
            // btnSaveSingleImage
            // 
            this.btnSaveSingleImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveSingleImage.FlatAppearance.BorderSize = 0;
            this.btnSaveSingleImage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSaveSingleImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSaveSingleImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSingleImage.Image = global::ImageViewerGretel.Properties.Resources.save_image;
            this.btnSaveSingleImage.Location = new System.Drawing.Point(0, 0);
            this.btnSaveSingleImage.Name = "btnSaveSingleImage";
            this.btnSaveSingleImage.Size = new System.Drawing.Size(58, 58);
            this.btnSaveSingleImage.TabIndex = 6;
            this.btnSaveSingleImage.UseVisualStyleBackColor = true;
            this.btnSaveSingleImage.Click += new System.EventHandler(this.btnSaveSingleImage_Click);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 30;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // ImageViewerGretel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.splitContImage);
            this.Controls.Add(this.pnlImageInfo);
            this.Controls.Add(this.pnlControlsMenu);
            this.Controls.Add(this.pnlSaveImagesMenu);
            this.ForeColor = System.Drawing.Color.Silver;
            this.Name = "ImageViewerGretel";
            this.Size = new System.Drawing.Size(1024, 768);
            this.pnlControlsMenu.ResumeLayout(false);
            this.pnlControlsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbRate)).EndInit();
            this.pnlImageInfo.ResumeLayout(false);
            this.pnlImageInfo.PerformLayout();
            this.splitContImage.Panel1.ResumeLayout(false);
            this.splitContImage.Panel2.ResumeLayout(false);
            this.splitContImage.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContImage)).EndInit();
            this.splitContImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxMini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxZoom)).EndInit();
            this.pnlSaveImagesMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControlsMenu;
        private System.Windows.Forms.Panel pnlImageInfo;
        private System.Windows.Forms.TrackBar tbRate;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSaveImagesMenu;
        private System.Windows.Forms.SplitContainer splitContImage;
        private System.Windows.Forms.Panel pnlSaveImagesMenu;
        private System.Windows.Forms.Button btnSaveAllImages;
        private System.Windows.Forms.Button btnSaveSingleImage;
        private System.Windows.Forms.Button btnSaveImagesMenuExit;
        private Emgu.CV.UI.ImageBox imageBoxMini;
        private System.Windows.Forms.RadioButton rbPlay;
        private System.Windows.Forms.RadioButton rbPause;
        private Emgu.CV.UI.ImageBox imageBoxZoom;
        private System.Windows.Forms.Label lblMini;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.Button btnCollapse;
        private System.Windows.Forms.Timer timerRefresh;
        private Emgu.CV.UI.ImageBox imageBoxResult;
    }
}
