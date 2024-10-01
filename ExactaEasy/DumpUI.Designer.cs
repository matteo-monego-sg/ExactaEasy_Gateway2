namespace ExactaEasy {
    partial class DumpUI {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DumpUI));
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlDumpTable = new System.Windows.Forms.Panel();
            this.pnlDumpSettings = new System.Windows.Forms.Panel();
            this.pnlInspectionSettings = new System.Windows.Forms.Panel();
            this.dgvDumpInspectionSettings = new System.Windows.Forms.DataGridView();
            this.pnlUserSettings = new System.Windows.Forms.Panel();
            this.cbUserSettings = new System.Windows.Forms.ComboBox();
            this.lblUserSettings = new System.Windows.Forms.Label();
            this.pnlToolSettings = new System.Windows.Forms.Panel();
            this.dgvToolSettings = new System.Windows.Forms.DataGridView();
            this.pnlToolHeader = new System.Windows.Forms.Panel();
            this.lblToolSettings = new System.Windows.Forms.Label();
            this.pnlDumpBar = new System.Windows.Forms.Panel();
            this.btnCaronteDisable = new System.Windows.Forms.Button();
            this.btnCaronteEnable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImportRecipeDTwin = new System.Windows.Forms.Button();
            this.btnExportImagesDigTwin = new System.Windows.Forms.Button();
            this.lblStatusDump = new System.Windows.Forms.Label();
            this.progressBar_dump = new System.Windows.Forms.ProgressBar();
            this.btnExportImages = new System.Windows.Forms.Button();
            this.btnStopDump = new System.Windows.Forms.Button();
            this.btnStartDump = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.pnlContainer.SuspendLayout();
            this.pnlDumpTable.SuspendLayout();
            this.pnlDumpSettings.SuspendLayout();
            this.pnlInspectionSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDumpInspectionSettings)).BeginInit();
            this.pnlUserSettings.SuspendLayout();
            this.pnlToolSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolSettings)).BeginInit();
            this.pnlToolHeader.SuspendLayout();
            this.pnlDumpBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.pnlDumpTable);
            this.pnlContainer.Controls.Add(this.pnlDumpBar);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(3);
            this.pnlContainer.Size = new System.Drawing.Size(1142, 620);
            this.pnlContainer.TabIndex = 4;
            // 
            // pnlDumpTable
            // 
            this.pnlDumpTable.Controls.Add(this.pnlDumpSettings);
            this.pnlDumpTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDumpTable.Location = new System.Drawing.Point(3, 3);
            this.pnlDumpTable.Name = "pnlDumpTable";
            this.pnlDumpTable.Size = new System.Drawing.Size(1136, 544);
            this.pnlDumpTable.TabIndex = 2;
            // 
            // pnlDumpSettings
            // 
            this.pnlDumpSettings.Controls.Add(this.pnlInspectionSettings);
            this.pnlDumpSettings.Controls.Add(this.pnlToolSettings);
            this.pnlDumpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDumpSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlDumpSettings.Name = "pnlDumpSettings";
            this.pnlDumpSettings.Size = new System.Drawing.Size(1136, 544);
            this.pnlDumpSettings.TabIndex = 3;
            // 
            // pnlInspectionSettings
            // 
            this.pnlInspectionSettings.Controls.Add(this.dgvDumpInspectionSettings);
            this.pnlInspectionSettings.Controls.Add(this.pnlUserSettings);
            this.pnlInspectionSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInspectionSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlInspectionSettings.Name = "pnlInspectionSettings";
            this.pnlInspectionSettings.Size = new System.Drawing.Size(1136, 296);
            this.pnlInspectionSettings.TabIndex = 24;
            // 
            // dgvDumpInspectionSettings
            // 
            this.dgvDumpInspectionSettings.AllowUserToAddRows = false;
            this.dgvDumpInspectionSettings.AllowUserToDeleteRows = false;
            this.dgvDumpInspectionSettings.AllowUserToResizeColumns = false;
            this.dgvDumpInspectionSettings.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.dgvDumpInspectionSettings.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDumpInspectionSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDumpInspectionSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDumpInspectionSettings.EnableHeadersVisualStyles = false;
            this.dgvDumpInspectionSettings.Location = new System.Drawing.Point(0, 44);
            this.dgvDumpInspectionSettings.Name = "dgvDumpInspectionSettings";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.dgvDumpInspectionSettings.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDumpInspectionSettings.RowTemplate.Height = 30;
            this.dgvDumpInspectionSettings.Size = new System.Drawing.Size(1136, 252);
            this.dgvDumpInspectionSettings.TabIndex = 26;
            this.dgvDumpInspectionSettings.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDumpInspectionSettings_CellClick);
            this.dgvDumpInspectionSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDumpInspectionSettings_CellValueChanged);
            this.dgvDumpInspectionSettings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDumpInspectionSettings_DataError);
            this.dgvDumpInspectionSettings.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvDumpInspectionSettings_EditingControlShowing);
            this.dgvDumpInspectionSettings.SelectionChanged += new System.EventHandler(this.dgvDumpInspectionSettings_SelectionChanged);
            // 
            // pnlUserSettings
            // 
            this.pnlUserSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlUserSettings.Controls.Add(this.cbUserSettings);
            this.pnlUserSettings.Controls.Add(this.lblUserSettings);
            this.pnlUserSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlUserSettings.Name = "pnlUserSettings";
            this.pnlUserSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnlUserSettings.Size = new System.Drawing.Size(1136, 44);
            this.pnlUserSettings.TabIndex = 27;
            // 
            // cbUserSettings
            // 
            this.cbUserSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbUserSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUserSettings.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUserSettings.FormattingEnabled = true;
            this.cbUserSettings.Location = new System.Drawing.Point(159, 5);
            this.cbUserSettings.Name = "cbUserSettings";
            this.cbUserSettings.Size = new System.Drawing.Size(198, 29);
            this.cbUserSettings.TabIndex = 1;
            this.cbUserSettings.SelectedIndexChanged += new System.EventHandler(this.cbUserSettings_SelectedIndexChanged);
            // 
            // lblUserSettings
            // 
            this.lblUserSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblUserSettings.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserSettings.Location = new System.Drawing.Point(5, 5);
            this.lblUserSettings.Name = "lblUserSettings";
            this.lblUserSettings.Padding = new System.Windows.Forms.Padding(3);
            this.lblUserSettings.Size = new System.Drawing.Size(154, 34);
            this.lblUserSettings.TabIndex = 0;
            this.lblUserSettings.Text = "USER SETTINGS";
            // 
            // pnlToolSettings
            // 
            this.pnlToolSettings.Controls.Add(this.dgvToolSettings);
            this.pnlToolSettings.Controls.Add(this.pnlToolHeader);
            this.pnlToolSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlToolSettings.Location = new System.Drawing.Point(0, 296);
            this.pnlToolSettings.Name = "pnlToolSettings";
            this.pnlToolSettings.Size = new System.Drawing.Size(1136, 248);
            this.pnlToolSettings.TabIndex = 25;
            // 
            // dgvToolSettings
            // 
            this.dgvToolSettings.AllowUserToAddRows = false;
            this.dgvToolSettings.AllowUserToDeleteRows = false;
            this.dgvToolSettings.AllowUserToResizeColumns = false;
            this.dgvToolSettings.AllowUserToResizeRows = false;
            this.dgvToolSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvToolSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvToolSettings.Location = new System.Drawing.Point(0, 32);
            this.dgvToolSettings.Name = "dgvToolSettings";
            this.dgvToolSettings.Size = new System.Drawing.Size(1136, 216);
            this.dgvToolSettings.TabIndex = 26;
            this.dgvToolSettings.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvToolSettings_CellContentClick);
            this.dgvToolSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvToolSettings_CellValueChanged);
            // 
            // pnlToolHeader
            // 
            this.pnlToolHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlToolHeader.Controls.Add(this.lblToolSettings);
            this.pnlToolHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlToolHeader.Name = "pnlToolHeader";
            this.pnlToolHeader.Padding = new System.Windows.Forms.Padding(5);
            this.pnlToolHeader.Size = new System.Drawing.Size(1136, 32);
            this.pnlToolHeader.TabIndex = 25;
            // 
            // lblToolSettings
            // 
            this.lblToolSettings.AutoSize = true;
            this.lblToolSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblToolSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolSettings.Location = new System.Drawing.Point(5, 5);
            this.lblToolSettings.Name = "lblToolSettings";
            this.lblToolSettings.Padding = new System.Windows.Forms.Padding(3);
            this.lblToolSettings.Size = new System.Drawing.Size(112, 19);
            this.lblToolSettings.TabIndex = 0;
            this.lblToolSettings.Text = "TOOL SETTINGS";
            // 
            // pnlDumpBar
            // 
            this.pnlDumpBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlDumpBar.Controls.Add(this.btnCaronteDisable);
            this.pnlDumpBar.Controls.Add(this.btnCaronteEnable);
            this.pnlDumpBar.Controls.Add(this.label1);
            this.pnlDumpBar.Controls.Add(this.btnImportRecipeDTwin);
            this.pnlDumpBar.Controls.Add(this.btnExportImagesDigTwin);
            this.pnlDumpBar.Controls.Add(this.lblStatusDump);
            this.pnlDumpBar.Controls.Add(this.progressBar_dump);
            this.pnlDumpBar.Controls.Add(this.btnExportImages);
            this.pnlDumpBar.Controls.Add(this.btnStopDump);
            this.pnlDumpBar.Controls.Add(this.btnStartDump);
            this.pnlDumpBar.Controls.Add(this.btnSaveSettings);
            this.pnlDumpBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDumpBar.Location = new System.Drawing.Point(3, 547);
            this.pnlDumpBar.Name = "pnlDumpBar";
            this.pnlDumpBar.Size = new System.Drawing.Size(1136, 70);
            this.pnlDumpBar.TabIndex = 3;
            // 
            // btnCaronteDisable
            // 
            this.btnCaronteDisable.AutoEllipsis = true;
            this.btnCaronteDisable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCaronteDisable.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCaronteDisable.FlatAppearance.BorderSize = 0;
            this.btnCaronteDisable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaronteDisable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCaronteDisable.Image = global::ExactaEasy.ResourcesArtic.delete;
            this.btnCaronteDisable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCaronteDisable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCaronteDisable.Location = new System.Drawing.Point(560, 0);
            this.btnCaronteDisable.Margin = new System.Windows.Forms.Padding(0);
            this.btnCaronteDisable.Name = "btnCaronteDisable";
            this.btnCaronteDisable.Size = new System.Drawing.Size(75, 70);
            this.btnCaronteDisable.TabIndex = 28;
            this.btnCaronteDisable.Text = "Caronte disable";
            this.btnCaronteDisable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCaronteDisable.UseVisualStyleBackColor = false;
            this.btnCaronteDisable.Click += new System.EventHandler(this.btnCaronteDisable_Click);
            // 
            // btnCaronteEnable
            // 
            this.btnCaronteEnable.AutoEllipsis = true;
            this.btnCaronteEnable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCaronteEnable.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCaronteEnable.FlatAppearance.BorderSize = 0;
            this.btnCaronteEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaronteEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCaronteEnable.Image = global::ExactaEasy.ResourcesArtic.checkmark2;
            this.btnCaronteEnable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCaronteEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCaronteEnable.Location = new System.Drawing.Point(485, 0);
            this.btnCaronteEnable.Margin = new System.Windows.Forms.Padding(0);
            this.btnCaronteEnable.Name = "btnCaronteEnable";
            this.btnCaronteEnable.Size = new System.Drawing.Size(75, 70);
            this.btnCaronteEnable.TabIndex = 27;
            this.btnCaronteEnable.Text = "Caronte Enable";
            this.btnCaronteEnable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCaronteEnable.UseVisualStyleBackColor = false;
            this.btnCaronteEnable.Click += new System.EventHandler(this.btnCaronteEnable_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(450, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 70);
            this.label1.TabIndex = 26;
            // 
            // btnImportRecipeDTwin
            // 
            this.btnImportRecipeDTwin.AutoEllipsis = true;
            this.btnImportRecipeDTwin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnImportRecipeDTwin.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnImportRecipeDTwin.FlatAppearance.BorderSize = 0;
            this.btnImportRecipeDTwin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportRecipeDTwin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnImportRecipeDTwin.Image = global::ExactaEasy.ResourcesArtic.down_config;
            this.btnImportRecipeDTwin.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnImportRecipeDTwin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnImportRecipeDTwin.Location = new System.Drawing.Point(375, 0);
            this.btnImportRecipeDTwin.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportRecipeDTwin.Name = "btnImportRecipeDTwin";
            this.btnImportRecipeDTwin.Size = new System.Drawing.Size(75, 70);
            this.btnImportRecipeDTwin.TabIndex = 25;
            this.btnImportRecipeDTwin.Text = "Import Recipe D. Twin";
            this.btnImportRecipeDTwin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnImportRecipeDTwin.UseVisualStyleBackColor = false;
            this.btnImportRecipeDTwin.Click += new System.EventHandler(this.btnImportRecipeDTwin_Click);
            // 
            // btnExportImagesDigTwin
            // 
            this.btnExportImagesDigTwin.AutoEllipsis = true;
            this.btnExportImagesDigTwin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnExportImagesDigTwin.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExportImagesDigTwin.FlatAppearance.BorderSize = 0;
            this.btnExportImagesDigTwin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportImagesDigTwin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExportImagesDigTwin.Image = ((System.Drawing.Image)(resources.GetObject("btnExportImagesDigTwin.Image")));
            this.btnExportImagesDigTwin.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExportImagesDigTwin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExportImagesDigTwin.Location = new System.Drawing.Point(300, 0);
            this.btnExportImagesDigTwin.Margin = new System.Windows.Forms.Padding(0);
            this.btnExportImagesDigTwin.Name = "btnExportImagesDigTwin";
            this.btnExportImagesDigTwin.Size = new System.Drawing.Size(75, 70);
            this.btnExportImagesDigTwin.TabIndex = 24;
            this.btnExportImagesDigTwin.Text = "Export D. Twin";
            this.btnExportImagesDigTwin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportImagesDigTwin.UseVisualStyleBackColor = false;
            this.btnExportImagesDigTwin.Click += new System.EventHandler(this.btnExportImagesDigTwin_Click);
            // 
            // lblStatusDump
            // 
            this.lblStatusDump.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusDump.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusDump.Location = new System.Drawing.Point(378, 54);
            this.lblStatusDump.Name = "lblStatusDump";
            this.lblStatusDump.Size = new System.Drawing.Size(755, 16);
            this.lblStatusDump.TabIndex = 23;
            this.lblStatusDump.Text = "-------------------------";
            this.lblStatusDump.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar_dump
            // 
            this.progressBar_dump.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar_dump.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar_dump.Location = new System.Drawing.Point(638, 6);
            this.progressBar_dump.Name = "progressBar_dump";
            this.progressBar_dump.Size = new System.Drawing.Size(495, 45);
            this.progressBar_dump.TabIndex = 22;
            // 
            // btnExportImages
            // 
            this.btnExportImages.AutoEllipsis = true;
            this.btnExportImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnExportImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExportImages.FlatAppearance.BorderSize = 0;
            this.btnExportImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExportImages.Image = ((System.Drawing.Image)(resources.GetObject("btnExportImages.Image")));
            this.btnExportImages.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExportImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExportImages.Location = new System.Drawing.Point(225, 0);
            this.btnExportImages.Margin = new System.Windows.Forms.Padding(0);
            this.btnExportImages.Name = "btnExportImages";
            this.btnExportImages.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnExportImages.Size = new System.Drawing.Size(75, 70);
            this.btnExportImages.TabIndex = 21;
            this.btnExportImages.Text = "Export";
            this.btnExportImages.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportImages.UseVisualStyleBackColor = false;
            this.btnExportImages.Click += new System.EventHandler(this.btnExportImages_Click);
            // 
            // btnStopDump
            // 
            this.btnStopDump.AutoEllipsis = true;
            this.btnStopDump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnStopDump.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStopDump.FlatAppearance.BorderSize = 0;
            this.btnStopDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopDump.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStopDump.Image = global::ExactaEasy.ResourcesArtic.stop;
            this.btnStopDump.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStopDump.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStopDump.Location = new System.Drawing.Point(150, 0);
            this.btnStopDump.Margin = new System.Windows.Forms.Padding(0);
            this.btnStopDump.Name = "btnStopDump";
            this.btnStopDump.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnStopDump.Size = new System.Drawing.Size(75, 70);
            this.btnStopDump.TabIndex = 19;
            this.btnStopDump.Text = "Stop";
            this.btnStopDump.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStopDump.UseVisualStyleBackColor = false;
            this.btnStopDump.Click += new System.EventHandler(this.btnStopDump_Click);
            // 
            // btnStartDump
            // 
            this.btnStartDump.AutoEllipsis = true;
            this.btnStartDump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnStartDump.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStartDump.FlatAppearance.BorderSize = 0;
            this.btnStartDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartDump.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStartDump.Image = global::ExactaEasy.ResourcesArtic.play;
            this.btnStartDump.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStartDump.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartDump.Location = new System.Drawing.Point(75, 0);
            this.btnStartDump.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartDump.Name = "btnStartDump";
            this.btnStartDump.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnStartDump.Size = new System.Drawing.Size(75, 70);
            this.btnStartDump.TabIndex = 18;
            this.btnStartDump.Text = "Start";
            this.btnStartDump.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStartDump.UseVisualStyleBackColor = false;
            this.btnStartDump.Click += new System.EventHandler(this.btnStartDump_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.AutoEllipsis = true;
            this.btnSaveSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnSaveSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveSettings.FlatAppearance.BorderSize = 0;
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSaveSettings.Image = global::ExactaEasy.ResourcesArtic.save;
            this.btnSaveSettings.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveSettings.Location = new System.Drawing.Point(0, 0);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(0);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 70);
            this.btnSaveSettings.TabIndex = 20;
            this.btnSaveSettings.Text = "Save settings";
            this.btnSaveSettings.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // DumpUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.pnlContainer);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "DumpUI";
            this.Size = new System.Drawing.Size(1142, 620);
            this.Load += new System.EventHandler(this.DumpUI_Load);
            this.pnlContainer.ResumeLayout(false);
            this.pnlDumpTable.ResumeLayout(false);
            this.pnlDumpSettings.ResumeLayout(false);
            this.pnlInspectionSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDumpInspectionSettings)).EndInit();
            this.pnlUserSettings.ResumeLayout(false);
            this.pnlToolSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolSettings)).EndInit();
            this.pnlToolHeader.ResumeLayout(false);
            this.pnlToolHeader.PerformLayout();
            this.pnlDumpBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Panel pnlDumpTable;
        private System.Windows.Forms.Panel pnlDumpSettings;
        private System.Windows.Forms.Panel pnlInspectionSettings;
        private System.Windows.Forms.DataGridView dgvDumpInspectionSettings;
        private System.Windows.Forms.Panel pnlDumpBar;
        private System.Windows.Forms.Button btnStopDump;
        private System.Windows.Forms.Button btnStartDump;
        private System.Windows.Forms.Panel pnlToolSettings;
        private System.Windows.Forms.DataGridView dgvToolSettings;
        private System.Windows.Forms.Panel pnlUserSettings;
        private System.Windows.Forms.ComboBox cbUserSettings;
        private System.Windows.Forms.Label lblUserSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Panel pnlToolHeader;
        private System.Windows.Forms.Label lblToolSettings;
        private System.Windows.Forms.Button btnExportImages;
        private System.Windows.Forms.ProgressBar progressBar_dump;
        private System.Windows.Forms.Label lblStatusDump;
        private System.Windows.Forms.Button btnExportImagesDigTwin;
        private System.Windows.Forms.Button btnImportRecipeDTwin;
        private System.Windows.Forms.Button btnCaronteDisable;
        private System.Windows.Forms.Button btnCaronteEnable;
        private System.Windows.Forms.Label label1;
    }
}
