namespace RecipeEditorUI {
    partial class NewCamera {
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
            this.pnlNewCam = new System.Windows.Forms.Panel();
            this.lblId = new System.Windows.Forms.Label();
            this.nudId = new System.Windows.Forms.NumericUpDown();
            this.lblDevType = new System.Windows.Forms.Label();
            this.cbCamSelector = new System.Windows.Forms.ComboBox();
            this.nudStrobeCount = new System.Windows.Forms.NumericUpDown();
            this.lblStrobeCount = new System.Windows.Forms.Label();
            this.pnlNewCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrobeCount)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlNewCam
            // 
            this.pnlNewCam.Controls.Add(this.nudStrobeCount);
            this.pnlNewCam.Controls.Add(this.lblStrobeCount);
            this.pnlNewCam.Controls.Add(this.cbCamSelector);
            this.pnlNewCam.Controls.Add(this.lblDevType);
            this.pnlNewCam.Controls.Add(this.nudId);
            this.pnlNewCam.Controls.Add(this.lblId);
            this.pnlNewCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNewCam.Location = new System.Drawing.Point(0, 0);
            this.pnlNewCam.Name = "pnlNewCam";
            this.pnlNewCam.Size = new System.Drawing.Size(642, 31);
            this.pnlNewCam.TabIndex = 0;
            // 
            // lblId
            // 
            this.lblId.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblId.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblId.Location = new System.Drawing.Point(0, 0);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(32, 31);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            this.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudId
            // 
            this.nudId.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudId.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudId.Location = new System.Drawing.Point(32, 0);
            this.nudId.Name = "nudId";
            this.nudId.Size = new System.Drawing.Size(65, 29);
            this.nudId.TabIndex = 1;
            this.nudId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDevType
            // 
            this.lblDevType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDevType.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevType.Location = new System.Drawing.Point(97, 0);
            this.lblDevType.Name = "lblDevType";
            this.lblDevType.Size = new System.Drawing.Size(130, 31);
            this.lblDevType.TabIndex = 2;
            this.lblDevType.Text = "Device type";
            this.lblDevType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCamSelector
            // 
            this.cbCamSelector.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbCamSelector.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCamSelector.FormattingEnabled = true;
            this.cbCamSelector.Location = new System.Drawing.Point(227, 0);
            this.cbCamSelector.Name = "cbCamSelector";
            this.cbCamSelector.Size = new System.Drawing.Size(221, 29);
            this.cbCamSelector.TabIndex = 3;
            // 
            // nudStrobeCount
            // 
            this.nudStrobeCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudStrobeCount.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudStrobeCount.Location = new System.Drawing.Point(574, 0);
            this.nudStrobeCount.Name = "nudStrobeCount";
            this.nudStrobeCount.Size = new System.Drawing.Size(65, 29);
            this.nudStrobeCount.TabIndex = 5;
            this.nudStrobeCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStrobeCount
            // 
            this.lblStrobeCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStrobeCount.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStrobeCount.Location = new System.Drawing.Point(448, 0);
            this.lblStrobeCount.Name = "lblStrobeCount";
            this.lblStrobeCount.Size = new System.Drawing.Size(126, 31);
            this.lblStrobeCount.TabIndex = 4;
            this.lblStrobeCount.Text = "Strobe count";
            this.lblStrobeCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NewCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlNewCam);
            this.Name = "NewCamera";
            this.Size = new System.Drawing.Size(642, 31);
            this.pnlNewCam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrobeCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNewCam;
        private System.Windows.Forms.ComboBox cbCamSelector;
        private System.Windows.Forms.Label lblDevType;
        private System.Windows.Forms.NumericUpDown nudId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.NumericUpDown nudStrobeCount;
        private System.Windows.Forms.Label lblStrobeCount;
    }
}
