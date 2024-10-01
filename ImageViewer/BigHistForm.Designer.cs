namespace SPAMI.Util.EmguImageViewer
{
    partial class BigHistForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.histogramBoxLarge = new Emgu.CV.UI.HistogramBox();
            this.SuspendLayout();
            // 
            // histogramBoxLarge
            // 
            this.histogramBoxLarge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.histogramBoxLarge.Location = new System.Drawing.Point(0, 0);
            this.histogramBoxLarge.Name = "histogramBoxLarge";
            this.histogramBoxLarge.Size = new System.Drawing.Size(284, 262);
            this.histogramBoxLarge.TabIndex = 0;
            // 
            // BigHistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ControlBox = false;
            this.Controls.Add(this.histogramBoxLarge);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "BigHistForm";
            this.Text = "#";
            this.DoubleClick += new System.EventHandler(this.BigHistForm_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.HistogramBox histogramBoxLarge;

    }
}