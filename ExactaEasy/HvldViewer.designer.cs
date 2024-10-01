namespace ExactaEasy {
    partial class HvldViewer
    {
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
            this.pnlControls = new System.Windows.Forms.Panel();
            this.rsRecImages = new ExactaEasy.RecordStatus();
            this.cbSaveResults = new System.Windows.Forms.CheckBox();
            this.btnRDP = new System.Windows.Forms.Button();
            this.cbRecordImages = new System.Windows.Forms.CheckBox();
            this.pnlParameter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblCameraDescription = new System.Windows.Forms.Label();
            this.lblStationStatus = new System.Windows.Forms.Label();
            this.pnlCameraTables = new System.Windows.Forms.Panel();
            this.pnlParams = new System.Windows.Forms.Panel();
            this.pnlParamSelector = new System.Windows.Forms.Panel();
            this.btnMachine = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.btnROI = new System.Windows.Forms.Button();
            this.btnStrobo = new System.Windows.Forms.Button();
            this.btnElaboration = new System.Windows.Forms.Button();
            this.btnDigitizer = new System.Windows.Forms.Button();
            this.btnCamera = new System.Windows.Forms.Button();
            this.pgCameraParams = new ExactaEasy.PagedGrid();
            this.pnlCameraCommand = new System.Windows.Forms.Panel();
            this.btnZoom = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cbViewResults = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.SignalStacker = new Hvld.Controls.HvldDisplayControl();
            this.resGrid = new ExactaEasy.ResultsGrid();
            this.resGrid = new ExactaEasy.ResultsGrid();
            this.pnlControls.SuspendLayout();
            this.pnlParameter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.pnlCameraTables.SuspendLayout();
            this.pnlParamSelector.SuspendLayout();
            this.pnlCameraCommand.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlControls.Controls.Add(this.rsRecImages);
            this.pnlControls.Controls.Add(this.cbSaveResults);
            this.pnlControls.Controls.Add(this.btnRDP);
            this.pnlControls.Controls.Add(this.cbRecordImages);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(480, 502);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(440, 70);
            this.pnlControls.TabIndex = 1;
            // 
            // rsRecImages
            // 
            this.rsRecImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.rsRecImages.Dock = System.Windows.Forms.DockStyle.Right;
            this.rsRecImages.ForeColor = System.Drawing.SystemColors.Control;
            this.rsRecImages.Location = new System.Drawing.Point(240, 0);
            this.rsRecImages.Name = "rsRecImages";
            this.rsRecImages.Size = new System.Drawing.Size(200, 70);
            this.rsRecImages.TabIndex = 14;
            this.rsRecImages.Visible = false;
            // 
            // cbSaveResults
            // 
            this.cbSaveResults.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbSaveResults.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbSaveResults.Enabled = false;
            this.cbSaveResults.FlatAppearance.BorderSize = 0;
            this.cbSaveResults.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbSaveResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.cbSaveResults.Image = global::ExactaEasy.ResourcesArtic.page;
            this.cbSaveResults.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbSaveResults.Location = new System.Drawing.Point(144, 0);
            this.cbSaveResults.Name = "cbSaveResults";
            this.cbSaveResults.Size = new System.Drawing.Size(72, 70);
            this.cbSaveResults.TabIndex = 13;
            this.cbSaveResults.Text = "Save results";
            this.cbSaveResults.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbSaveResults.UseVisualStyleBackColor = true;
            this.cbSaveResults.Visible = false;
            // 
            // btnRDP
            // 
            this.btnRDP.AutoEllipsis = true;
            this.btnRDP.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRDP.Enabled = false;
            this.btnRDP.FlatAppearance.BorderSize = 0;
            this.btnRDP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRDP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRDP.Image = global::ExactaEasy.ResourcesArtic.tools;
            this.btnRDP.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRDP.Location = new System.Drawing.Point(72, 0);
            this.btnRDP.Name = "btnRDP";
            this.btnRDP.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnRDP.Size = new System.Drawing.Size(72, 70);
            this.btnRDP.TabIndex = 12;
            this.btnRDP.Text = "Config";
            this.btnRDP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRDP.UseVisualStyleBackColor = true;
            this.btnRDP.Click += new System.EventHandler(this.btnRDP_Click);
            // 
            // cbRecordImages
            // 
            this.cbRecordImages.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbRecordImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbRecordImages.FlatAppearance.BorderSize = 0;
            this.cbRecordImages.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbRecordImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.cbRecordImages.Image = global::ExactaEasy.ResourcesArtic.record;
            this.cbRecordImages.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbRecordImages.Location = new System.Drawing.Point(0, 0);
            this.cbRecordImages.Name = "cbRecordImages";
            this.cbRecordImages.Size = new System.Drawing.Size(72, 70);
            this.cbRecordImages.TabIndex = 6;
            this.cbRecordImages.Text = "Record images";
            this.cbRecordImages.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbRecordImages.UseVisualStyleBackColor = true;
            this.cbRecordImages.Visible = false;
            this.cbRecordImages.CheckedChanged += new System.EventHandler(this.cbRecordImages_CheckedChanged);
            // 
            // pnlParameter
            // 
            this.pnlParameter.Controls.Add(this.tableLayoutPanel1);
            this.pnlParameter.Controls.Add(this.pnlCameraCommand);
            this.pnlParameter.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlParameter.Location = new System.Drawing.Point(0, 0);
            this.pnlParameter.Name = "pnlParameter";
            this.pnlParameter.Size = new System.Drawing.Size(480, 572);
            this.pnlParameter.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlStatus, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlCameraTables, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pnlParamSelector, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pgCameraParams, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(480, 502);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblCameraDescription);
            this.pnlStatus.Controls.Add(this.lblStationStatus);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(3, 3);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(474, 47);
            this.pnlStatus.TabIndex = 4;
            // 
            // lblCameraDescription
            // 
            this.lblCameraDescription.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCameraDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCameraDescription.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraDescription.ForeColor = System.Drawing.Color.White;
            this.lblCameraDescription.Location = new System.Drawing.Point(0, 0);
            this.lblCameraDescription.Name = "lblCameraDescription";
            this.lblCameraDescription.Size = new System.Drawing.Size(435, 47);
            this.lblCameraDescription.TabIndex = 0;
            this.lblCameraDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStationStatus
            // 
            this.lblStationStatus.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStationStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStationStatus.Image = global::ExactaEasy.Properties.Resources.white_off_32;
            this.lblStationStatus.Location = new System.Drawing.Point(435, 0);
            this.lblStationStatus.Name = "lblStationStatus";
            this.lblStationStatus.Size = new System.Drawing.Size(39, 47);
            this.lblStationStatus.TabIndex = 1;
            // 
            // pnlCameraTables
            // 
            this.pnlCameraTables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlCameraTables.Controls.Add(this.resGrid);
            this.pnlCameraTables.Controls.Add(this.pnlParams);
            this.pnlCameraTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCameraTables.Location = new System.Drawing.Point(3, 517);
            this.pnlCameraTables.Name = "pnlCameraTables";
            this.pnlCameraTables.Size = new System.Drawing.Size(474, 14);
            this.pnlCameraTables.TabIndex = 3;
            // 
            // pnlParams
            // 
            this.pnlParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(474, 14);
            this.pnlParams.TabIndex = 4;
            // 
            // pnlParamSelector
            // 
            this.pnlParamSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlParamSelector.Controls.Add(this.btnMachine);
            this.pnlParamSelector.Controls.Add(this.btnAdvanced);
            this.pnlParamSelector.Controls.Add(this.btnROI);
            this.pnlParamSelector.Controls.Add(this.btnStrobo);
            this.pnlParamSelector.Controls.Add(this.btnElaboration);
            this.pnlParamSelector.Controls.Add(this.btnDigitizer);
            this.pnlParamSelector.Controls.Add(this.btnCamera);
            this.pnlParamSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParamSelector.Location = new System.Drawing.Point(3, 56);
            this.pnlParamSelector.Name = "pnlParamSelector";
            this.pnlParamSelector.Size = new System.Drawing.Size(474, 60);
            this.pnlParamSelector.TabIndex = 2;
            // 
            // btnMachine
            // 
            this.btnMachine.AutoEllipsis = true;
            this.btnMachine.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMachine.FlatAppearance.BorderSize = 0;
            this.btnMachine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachine.Image = global::ExactaEasy.Properties.Resources.system_run;
            this.btnMachine.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMachine.Location = new System.Drawing.Point(384, 0);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(32, 60);
            this.btnMachine.TabIndex = 6;
            this.btnMachine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMachine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMachine.UseVisualStyleBackColor = true;
            this.btnMachine.Click += new System.EventHandler(this.btnMachine_Click);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.AutoEllipsis = true;
            this.btnAdvanced.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAdvanced.FlatAppearance.BorderSize = 0;
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvanced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdvanced.Image = global::ExactaEasy.Properties.Resources.run_build_configure;
            this.btnAdvanced.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdvanced.Location = new System.Drawing.Point(352, 0);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(32, 60);
            this.btnAdvanced.TabIndex = 2;
            this.btnAdvanced.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // btnROI
            // 
            this.btnROI.AutoEllipsis = true;
            this.btnROI.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnROI.FlatAppearance.BorderSize = 0;
            this.btnROI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnROI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnROI.Image = global::ExactaEasy.Properties.Resources.transform_crop_and_resize1;
            this.btnROI.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnROI.Location = new System.Drawing.Point(320, 0);
            this.btnROI.Name = "btnROI";
            this.btnROI.Size = new System.Drawing.Size(32, 60);
            this.btnROI.TabIndex = 5;
            this.btnROI.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnROI.UseVisualStyleBackColor = true;
            this.btnROI.Click += new System.EventHandler(this.btnROI_Click);
            // 
            // btnStrobo
            // 
            this.btnStrobo.AutoEllipsis = true;
            this.btnStrobo.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStrobo.Enabled = false;
            this.btnStrobo.FlatAppearance.BorderSize = 0;
            this.btnStrobo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrobo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStrobo.Image = global::ExactaEasy.ResourcesArtic.light_bulb;
            this.btnStrobo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStrobo.Location = new System.Drawing.Point(240, 0);
            this.btnStrobo.Name = "btnStrobo";
            this.btnStrobo.Size = new System.Drawing.Size(80, 60);
            this.btnStrobo.TabIndex = 1;
            this.btnStrobo.Text = "Strobo";
            this.btnStrobo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStrobo.UseVisualStyleBackColor = true;
            this.btnStrobo.Visible = false;
            this.btnStrobo.Click += new System.EventHandler(this.btnStrobo_Click);
            // 
            // btnElaboration
            // 
            this.btnElaboration.AutoEllipsis = true;
            this.btnElaboration.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnElaboration.FlatAppearance.BorderSize = 0;
            this.btnElaboration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElaboration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnElaboration.Image = global::ExactaEasy.ResourcesArtic.tools_c__;
            this.btnElaboration.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnElaboration.Location = new System.Drawing.Point(160, 0);
            this.btnElaboration.Name = "btnElaboration";
            this.btnElaboration.Size = new System.Drawing.Size(80, 60);
            this.btnElaboration.TabIndex = 0;
            this.btnElaboration.Text = "Tools";
            this.btnElaboration.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnElaboration.UseVisualStyleBackColor = true;
            this.btnElaboration.Click += new System.EventHandler(this.btnElaboration_Click);
            // 
            // btnDigitizer
            // 
            this.btnDigitizer.AutoEllipsis = true;
            this.btnDigitizer.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDigitizer.FlatAppearance.BorderSize = 0;
            this.btnDigitizer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigitizer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDigitizer.Image = global::ExactaEasy.ResourcesArtic.digitizer;
            this.btnDigitizer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDigitizer.Location = new System.Drawing.Point(80, 0);
            this.btnDigitizer.Name = "btnDigitizer";
            this.btnDigitizer.Size = new System.Drawing.Size(80, 60);
            this.btnDigitizer.TabIndex = 4;
            this.btnDigitizer.Text = "Digitizer";
            this.btnDigitizer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDigitizer.UseVisualStyleBackColor = true;
            this.btnDigitizer.Click += new System.EventHandler(this.btnDigitizer_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.AutoEllipsis = true;
            this.btnCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCamera.Enabled = false;
            this.btnCamera.FlatAppearance.BorderSize = 0;
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCamera.Image = global::ExactaEasy.ResourcesArtic.photo_camera;
            this.btnCamera.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCamera.Location = new System.Drawing.Point(0, 0);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(80, 60);
            this.btnCamera.TabIndex = 3;
            this.btnCamera.Text = "Camera";
            this.btnCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // pgCameraParams
            // 
            this.pgCameraParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pgCameraParams.ChildId = null;
            this.pgCameraParams.DataSource = null;
            this.pgCameraParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgCameraParams.ForeColor = System.Drawing.SystemColors.Control;
            this.pgCameraParams.Location = new System.Drawing.Point(3, 122);
            this.pgCameraParams.MaxValueColumnName = "MaxValue";
            this.pgCameraParams.MinValueColumnName = "MinValue";
            this.pgCameraParams.Name = "pgCameraParams";
            this.pgCameraParams.ParentDataSource = null;
            this.pgCameraParams.ShowDataGridViewParent = false;
            this.pgCameraParams.ShowLogInfo = true;
            this.pgCameraParams.ShowMinMaxInfo = false;
            this.pgCameraParams.ShowTitleBar = false;
            this.pgCameraParams.Size = new System.Drawing.Size(474, 389);
            this.pgCameraParams.TabIndex = 3;
            this.pgCameraParams.Title = "";
            // 
            // pnlCameraCommand
            // 
            this.pnlCameraCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlCameraCommand.Controls.Add(this.btnZoom);
            this.pnlCameraCommand.Controls.Add(this.btnRestore);
            this.pnlCameraCommand.Controls.Add(this.btnSave);
            this.pnlCameraCommand.Controls.Add(this.btnEdit);
            this.pnlCameraCommand.Controls.Add(this.cbViewResults);
            this.pnlCameraCommand.Controls.Add(this.btnApply);
            this.pnlCameraCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCameraCommand.Location = new System.Drawing.Point(0, 502);
            this.pnlCameraCommand.Name = "pnlCameraCommand";
            this.pnlCameraCommand.Size = new System.Drawing.Size(480, 70);
            this.pnlCameraCommand.TabIndex = 3;
            // 
            // btnZoom
            // 
            this.btnZoom.AutoEllipsis = true;
            this.btnZoom.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnZoom.Enabled = false;
            this.btnZoom.FlatAppearance.BorderSize = 0;
            this.btnZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnZoom.Image = global::ExactaEasy.ResourcesArtic.search;
            this.btnZoom.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnZoom.Location = new System.Drawing.Point(360, 0);
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnZoom.Size = new System.Drawing.Size(72, 70);
            this.btnZoom.TabIndex = 7;
            this.btnZoom.Text = "Zoom";
            this.btnZoom.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnZoom.UseVisualStyleBackColor = true;
            this.btnZoom.Visible = false;
            // 
            // btnRestore
            // 
            this.btnRestore.AutoEllipsis = true;
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRestore.Enabled = false;
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRestore.Image = global::ExactaEasy.ResourcesArtic.skip_backward;
            this.btnRestore.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRestore.Location = new System.Drawing.Point(288, 0);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnRestore.Size = new System.Drawing.Size(72, 70);
            this.btnRestore.TabIndex = 6;
            this.btnRestore.Text = "Undo";
            this.btnRestore.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Visible = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoEllipsis = true;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::ExactaEasy.ResourcesArtic.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(216, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnSave.Size = new System.Drawing.Size(72, 70);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoEllipsis = true;
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnEdit.Enabled = false;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnEdit.Image = global::ExactaEasy.ResourcesArtic.save;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEdit.Location = new System.Drawing.Point(144, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnEdit.Size = new System.Drawing.Size(72, 70);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cbViewResults
            // 
            this.cbViewResults.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbViewResults.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbViewResults.FlatAppearance.BorderSize = 0;
            this.cbViewResults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbViewResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cbViewResults.Image = global::ExactaEasy.ResourcesArtic.search;
            this.cbViewResults.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbViewResults.Location = new System.Drawing.Point(72, 0);
            this.cbViewResults.Name = "cbViewResults";
            this.cbViewResults.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cbViewResults.Size = new System.Drawing.Size(72, 70);
            this.cbViewResults.TabIndex = 14;
            this.cbViewResults.Text = "Results";
            this.cbViewResults.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbViewResults.UseVisualStyleBackColor = true;
            this.cbViewResults.CheckedChanged += new System.EventHandler(this.cbViewResults_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.AutoEllipsis = true;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnApply.Image = global::ExactaEasy.ResourcesArtic.checkmark2;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApply.Location = new System.Drawing.Point(0, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnApply.Size = new System.Drawing.Size(72, 70);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pnlCenter.Controls.Add(this.SignalStacker);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(480, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCenter.Size = new System.Drawing.Size(440, 502);
            this.pnlCenter.TabIndex = 4;
            this.pnlCenter.Resize += new System.EventHandler(this.pnlCenter_Resize);
            // 
            // SignalStacker
            // 
            this.SignalStacker.BackColor = System.Drawing.SystemColors.Control;
            this.SignalStacker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SignalStacker.EnableGlobalAntialising = true;
            this.SignalStacker.Location = new System.Drawing.Point(5, 5);
            this.SignalStacker.Name = "SignalStacker";
            this.SignalStacker.Size = new System.Drawing.Size(430, 492);
            this.SignalStacker.TabIndex = 0;
            // 
            // resGrid
            // 
            this.resGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.resGrid.ForeColor = System.Drawing.SystemColors.Control;
            this.resGrid.Location = new System.Drawing.Point(0, 0);
            this.resGrid.Name = "resGrid";
            this.resGrid.Size = new System.Drawing.Size(333, 336);
            this.resGrid.TabIndex = 0;
            // 
            // resultsGrid1
            // 
            this.resGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.resGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resGrid.ForeColor = System.Drawing.SystemColors.Control;
            this.resGrid.Location = new System.Drawing.Point(0, 0);
            this.resGrid.Name = "resultsGrid1";
            this.resGrid.Size = new System.Drawing.Size(474, 14);
            this.resGrid.TabIndex = 5;
            // 
            // HvldViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlParameter);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "HvldViewer";
            this.Size = new System.Drawing.Size(920, 572);
            this.Load += new System.EventHandler(this.CamViewer_Load);
            this.pnlControls.ResumeLayout(false);
            this.pnlParameter.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.pnlCameraTables.ResumeLayout(false);
            this.pnlParamSelector.ResumeLayout(false);
            this.pnlCameraCommand.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Panel pnlParameter;
        private System.Windows.Forms.Panel pnlParamSelector;
        private System.Windows.Forms.Label lblCameraDescription;
        private System.Windows.Forms.Panel pnlCameraCommand;
        private PagedGrid pgCameraParams;
        private System.Windows.Forms.Button btnElaboration;
        private System.Windows.Forms.Button btnZoom;
        private System.Windows.Forms.Button btnStrobo;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDigitizer;
        private System.Windows.Forms.Panel pnlCenter;
		private System.Windows.Forms.CheckBox cbRecordImages;
        private System.Windows.Forms.Button btnROI;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStationStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnMachine;
        private System.Windows.Forms.Button btnRDP;
        private System.Windows.Forms.CheckBox cbViewResults;
        private System.Windows.Forms.Panel pnlCameraTables;
        private System.Windows.Forms.Panel pnlParams;
        private System.Windows.Forms.CheckBox cbSaveResults;
        private RecordStatus rsRecImages;
        private Hvld.Controls.HvldDisplayControl SignalStacker;
        private ResultsGrid resGrid;
    }
}
