namespace ExactaEasy {
    partial class CameraConfigUI {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblSection = new System.Windows.Forms.Label();
            this.dgvCameras = new System.Windows.Forms.DataGridView();
            this.CameraID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArchLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CameraIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CameraDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GretelVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CameraHWStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.HMIEnable = new System.Windows.Forms.DataGridViewImageColumn();
            this.RecipeEnable = new System.Windows.Forms.DataGridViewImageColumn();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.pnlGridContainer = new System.Windows.Forms.Panel();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCameras)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlGridContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSection
            // 
            this.lblSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSection.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSection.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSection.Location = new System.Drawing.Point(10, 2);
            this.lblSection.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.lblSection.Name = "lblSection";
            this.lblSection.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.lblSection.Size = new System.Drawing.Size(436, 71);
            this.lblSection.TabIndex = 0;
            this.lblSection.Text = "---";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvCameras
            // 
            this.dgvCameras.AllowUserToAddRows = false;
            this.dgvCameras.AllowUserToDeleteRows = false;
            this.dgvCameras.AllowUserToResizeColumns = false;
            this.dgvCameras.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.dgvCameras.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCameras.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvCameras.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCameras.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCameras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCameras.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CameraID,
            this.ArchLevel,
            this.TaskID,
            this.CameraIP,
            this.CameraDescription,
            this.GretelVersion,
            this.CameraHWStatus,
            this.HMIEnable,
            this.RecipeEnable});
            this.dgvCameras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCameras.EnableHeadersVisualStyles = false;
            this.dgvCameras.Location = new System.Drawing.Point(10, 10);
            this.dgvCameras.Name = "dgvCameras";
            this.dgvCameras.ReadOnly = true;
            this.dgvCameras.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCameras.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.dgvCameras.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCameras.RowTemplate.Height = 40;
            this.dgvCameras.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCameras.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCameras.Size = new System.Drawing.Size(620, 341);
            this.dgvCameras.TabIndex = 1;
            this.dgvCameras.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCameras_CellClick);
            this.dgvCameras.SelectionChanged += new System.EventHandler(this.dgvCameras_SelectionChanged);
            // 
            // CameraID
            // 
            this.CameraID.HeaderText = "CameraID";
            this.CameraID.Name = "CameraID";
            this.CameraID.ReadOnly = true;
            this.CameraID.Visible = false;
            // 
            // ArchLevel
            // 
            this.ArchLevel.HeaderText = "ArchitectureLevel";
            this.ArchLevel.Name = "ArchLevel";
            this.ArchLevel.ReadOnly = true;
            this.ArchLevel.Visible = false;
            // 
            // TaskID
            // 
            this.TaskID.HeaderText = "TaskID";
            this.TaskID.Name = "TaskID";
            this.TaskID.ReadOnly = true;
            this.TaskID.Visible = false;
            // 
            // CameraIP
            // 
            this.CameraIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CameraIP.FillWeight = 200F;
            this.CameraIP.HeaderText = "IP";
            this.CameraIP.Name = "CameraIP";
            this.CameraIP.ReadOnly = true;
            this.CameraIP.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // CameraDescription
            // 
            this.CameraDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CameraDescription.FillWeight = 167.5127F;
            this.CameraDescription.HeaderText = "Device";
            this.CameraDescription.MinimumWidth = 50;
            this.CameraDescription.Name = "CameraDescription";
            this.CameraDescription.ReadOnly = true;
            this.CameraDescription.Visible = false;
            // 
            // GretelVersion
            // 
            this.GretelVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.GretelVersion.FillWeight = 200F;
            this.GretelVersion.HeaderText = "Version";
            this.GretelVersion.Name = "GretelVersion";
            this.GretelVersion.ReadOnly = true;
            this.GretelVersion.Width = 200;
            // 
            // CameraHWStatus
            // 
            this.CameraHWStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CameraHWStatus.HeaderText = "Status";
            this.CameraHWStatus.MinimumWidth = 100;
            this.CameraHWStatus.Name = "CameraHWStatus";
            this.CameraHWStatus.ReadOnly = true;
            this.CameraHWStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CameraHWStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // HMIEnable
            // 
            this.HMIEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HMIEnable.HeaderText = "HMI Enable";
            this.HMIEnable.MinimumWidth = 100;
            this.HMIEnable.Name = "HMIEnable";
            this.HMIEnable.ReadOnly = true;
            this.HMIEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.HMIEnable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // RecipeEnable
            // 
            this.RecipeEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RecipeEnable.HeaderText = "Recipe Enable";
            this.RecipeEnable.MinimumWidth = 100;
            this.RecipeEnable.Name = "RecipeEnable";
            this.RecipeEnable.ReadOnly = true;
            this.RecipeEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RecipeEnable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RecipeEnable.Visible = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnRecipe);
            this.pnlHeader.Controls.Add(this.btnRescan);
            this.pnlHeader.Controls.Add(this.lblSection);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(20);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.pnlHeader.Size = new System.Drawing.Size(640, 75);
            this.pnlHeader.TabIndex = 2;
            // 
            // btnRecipe
            // 
            this.btnRecipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnRecipe.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRecipe.FlatAppearance.BorderSize = 0;
            this.btnRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRecipe.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRecipe.Image = global::ExactaEasy.ResourcesArtic.down_config;
            this.btnRecipe.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRecipe.Location = new System.Drawing.Point(480, 2);
            this.btnRecipe.Margin = new System.Windows.Forms.Padding(0);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(75, 71);
            this.btnRecipe.TabIndex = 2;
            this.btnRecipe.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRecipe.UseVisualStyleBackColor = false;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnRescan.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRescan.FlatAppearance.BorderSize = 0;
            this.btnRescan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRescan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRescan.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRescan.Image = global::ExactaEasy.ResourcesArtic.refresh;
            this.btnRescan.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRescan.Location = new System.Drawing.Point(555, 2);
            this.btnRescan.Margin = new System.Windows.Forms.Padding(0);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(75, 71);
            this.btnRescan.TabIndex = 1;
            this.btnRescan.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRescan.UseVisualStyleBackColor = false;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // pnlGridContainer
            // 
            this.pnlGridContainer.Controls.Add(this.dgvCameras);
            this.pnlGridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridContainer.Location = new System.Drawing.Point(0, 75);
            this.pnlGridContainer.Name = "pnlGridContainer";
            this.pnlGridContainer.Padding = new System.Windows.Forms.Padding(10);
            this.pnlGridContainer.Size = new System.Drawing.Size(640, 361);
            this.pnlGridContainer.TabIndex = 3;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.HeaderText = "Status";
            this.dataGridViewImageColumn1.MinimumWidth = 100;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn2.HeaderText = "HMI Enable";
            this.dataGridViewImageColumn2.MinimumWidth = 100;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn3.HeaderText = "Recipe Enable";
            this.dataGridViewImageColumn3.MinimumWidth = 100;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.ReadOnly = true;
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewImageColumn3.Visible = false;
            // 
            // CameraConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.pnlGridContainer);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CameraConfigUI";
            this.Size = new System.Drawing.Size(640, 436);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCameras)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlGridContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.DataGridView dgvCameras;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Panel pnlGridContainer;
        private System.Windows.Forms.DataGridViewTextBoxColumn CameraID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArchLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CameraIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn CameraDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn GretelVersion;
        private System.Windows.Forms.DataGridViewImageColumn CameraHWStatus;
        private System.Windows.Forms.DataGridViewImageColumn HMIEnable;
        private System.Windows.Forms.DataGridViewImageColumn RecipeEnable;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.Button btnRecipe;
    }
}
