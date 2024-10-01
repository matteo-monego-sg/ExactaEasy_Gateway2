namespace HanselRecipeEditor {
    partial class frmRecipe {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecipe));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSpare = new System.Windows.Forms.Button();
            this.dgvConfig = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfig)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "16x16windowsIcon.png");
            this.imageList1.Images.SetKeyName(1, "Debug_16_bkgWhite.png");
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.Location = new System.Drawing.Point(8, 9);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(603, 36);
            this.lblStepDescription.TabIndex = 3;
            this.lblStepDescription.Text = "LAST STEP\r\n";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnBack, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSpare, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(37, 350);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(544, 96);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnBack
            // 
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(372, 10);
            this.btnBack.Margin = new System.Windows.Forms.Padding(10);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(162, 76);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "BACK";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(10, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(161, 76);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSpare
            // 
            this.btnSpare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSpare.Enabled = false;
            this.btnSpare.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnSpare.Location = new System.Drawing.Point(191, 10);
            this.btnSpare.Margin = new System.Windows.Forms.Padding(10);
            this.btnSpare.Name = "btnSpare";
            this.btnSpare.Size = new System.Drawing.Size(161, 76);
            this.btnSpare.TabIndex = 0;
            this.btnSpare.Text = "---";
            this.btnSpare.UseVisualStyleBackColor = true;
            // 
            // dgvConfig
            // 
            this.dgvConfig.AllowUserToAddRows = false;
            this.dgvConfig.AllowUserToDeleteRows = false;
            this.dgvConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConfig.Location = new System.Drawing.Point(14, 48);
            this.dgvConfig.Name = "dgvConfig";
            this.dgvConfig.Size = new System.Drawing.Size(593, 296);
            this.dgvConfig.TabIndex = 4;
            // 
            // frmRecipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 452);
            this.Controls.Add(this.dgvConfig);
            this.Controls.Add(this.lblStepDescription);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmRecipe";
            this.Text = "Hansel Recipe Editor";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfig)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSpare;
        private System.Windows.Forms.DataGridView dgvConfig;
    }
}