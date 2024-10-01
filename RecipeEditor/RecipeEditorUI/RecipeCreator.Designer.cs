namespace RecipeEditorUI
{
    partial class RecipeCreator
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
            this.pnlDevCreator = new System.Windows.Forms.Panel();
            this.nudDevCount = new System.Windows.Forms.NumericUpDown();
            this.lblDevCount = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlDevCustomization = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.pnlDevCreator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDevCount)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDevCreator
            // 
            this.pnlDevCreator.Controls.Add(this.btnCreate);
            this.pnlDevCreator.Controls.Add(this.btnStart);
            this.pnlDevCreator.Controls.Add(this.nudDevCount);
            this.pnlDevCreator.Controls.Add(this.lblDevCount);
            this.pnlDevCreator.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDevCreator.Location = new System.Drawing.Point(0, 0);
            this.pnlDevCreator.Name = "pnlDevCreator";
            this.pnlDevCreator.Size = new System.Drawing.Size(679, 59);
            this.pnlDevCreator.TabIndex = 0;
            // 
            // nudDevCount
            // 
            this.nudDevCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudDevCount.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDevCount.Location = new System.Drawing.Point(121, 0);
            this.nudDevCount.Name = "nudDevCount";
            this.nudDevCount.Size = new System.Drawing.Size(65, 29);
            this.nudDevCount.TabIndex = 3;
            // 
            // lblDevCount
            // 
            this.lblDevCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDevCount.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevCount.Location = new System.Drawing.Point(0, 0);
            this.lblDevCount.Name = "lblDevCount";
            this.lblDevCount.Size = new System.Drawing.Size(121, 59);
            this.lblDevCount.TabIndex = 2;
            this.lblDevCount.Text = "Devices count";
            this.lblDevCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStart.Location = new System.Drawing.Point(186, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 59);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pnlDevCustomization
            // 
            this.pnlDevCustomization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDevCustomization.Location = new System.Drawing.Point(0, 59);
            this.pnlDevCustomization.Name = "pnlDevCustomization";
            this.pnlDevCustomization.Size = new System.Drawing.Size(679, 393);
            this.pnlDevCustomization.TabIndex = 1;
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCreate.Location = new System.Drawing.Point(261, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 59);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "Create";
            this.btnCreate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // RecipeCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDevCustomization);
            this.Controls.Add(this.pnlDevCreator);
            this.Name = "RecipeCreator";
            this.Size = new System.Drawing.Size(679, 452);
            this.pnlDevCreator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDevCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDevCreator;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NumericUpDown nudDevCount;
        private System.Windows.Forms.Label lblDevCount;
        private System.Windows.Forms.Panel pnlDevCustomization;
        private System.Windows.Forms.Button btnCreate;
    }
}
