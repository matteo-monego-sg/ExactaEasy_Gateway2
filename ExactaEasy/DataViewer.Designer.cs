namespace ExactaEasy {
    partial class DataViewer {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlUp = new System.Windows.Forms.Panel();
            this.calibrationTurbidimeter = new System.Windows.Forms.CheckBox();
            this.cbOfflineMode = new System.Windows.Forms.CheckBox();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlParameter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStationStatus = new System.Windows.Forms.Label();
            this.lblCameraDescription = new System.Windows.Forms.Label();
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
            this.pnlCameraCommand = new System.Windows.Forms.Panel();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cbViewResults = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.tlpCalibrationTurbidimeter = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.zgCalibration = new ZedGraph.ZedGraphControl();
            this.dgvCalibrationTurbidimeter = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEASURE_10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEDIA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblNumSamples = new System.Windows.Forms.Label();
            this.nudNumbersOfItems = new System.Windows.Forms.NumericUpDown();
            this.lblSuggestedLimits = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbHisto = new System.Windows.Forms.RadioButton();
            this.rbAllStdDev = new System.Windows.Forms.RadioButton();
            this.rbLow = new System.Windows.Forms.RadioButton();
            this.rbMedium = new System.Windows.Forms.RadioButton();
            this.rbHigh = new System.Windows.Forms.RadioButton();
            this.cbAcqCalibrationTurbidimeter = new System.Windows.Forms.CheckBox();
            this.tlpGraphs = new System.Windows.Forms.TableLayoutPanel();
            this.extGraphBox = new ZedGraph.ZedGraphControl();
            this.timerDevStatus = new System.Windows.Forms.Timer(this.components);
            this.timerValuesTurbidimeter = new System.Windows.Forms.Timer(this.components);
            this.yesNoPanel = new ExactaEasy.YesNoPanel();
            this.saveMenu = new ExactaEasy.SaveMenu();
            this.resGrid = new ExactaEasy.ResultsGrid();
            this.pgCameraParams = new ExactaEasy.PagedGrid();
            this.pnlControls.SuspendLayout();
            this.pnlUp.SuspendLayout();
            this.pnlParameter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.pnlCameraTables.SuspendLayout();
            this.pnlParamSelector.SuspendLayout();
            this.pnlCameraCommand.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.tlpCalibrationTurbidimeter.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalibrationTurbidimeter)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumbersOfItems)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tlpGraphs.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.pnlControls.Controls.Add(this.lblStatus);
            this.pnlControls.Controls.Add(this.yesNoPanel);
            this.pnlControls.Controls.Add(this.saveMenu);
            this.pnlControls.Controls.Add(this.pnlUp);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(480, 426);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(440, 146);
            this.pnlControls.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(6, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(90, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Working mode:";
            // 
            // pnlUp
            // 
            this.pnlUp.Controls.Add(this.calibrationTurbidimeter);
            this.pnlUp.Controls.Add(this.cbOfflineMode);
            this.pnlUp.Controls.Add(this.btnSaveData);
            this.pnlUp.Controls.Add(this.btnUpload);
            this.pnlUp.Controls.Add(this.btnReset);
            this.pnlUp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUp.Location = new System.Drawing.Point(0, 76);
            this.pnlUp.Name = "pnlUp";
            this.pnlUp.Size = new System.Drawing.Size(440, 70);
            this.pnlUp.TabIndex = 1;
            // 
            // calibrationTurbidimeter
            // 
            this.calibrationTurbidimeter.Appearance = System.Windows.Forms.Appearance.Button;
            this.calibrationTurbidimeter.Dock = System.Windows.Forms.DockStyle.Left;
            this.calibrationTurbidimeter.FlatAppearance.BorderSize = 0;
            this.calibrationTurbidimeter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.calibrationTurbidimeter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.calibrationTurbidimeter.Image = global::ExactaEasy.Properties.Resources.nepomuk;
            this.calibrationTurbidimeter.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.calibrationTurbidimeter.Location = new System.Drawing.Point(288, 0);
            this.calibrationTurbidimeter.Name = "calibrationTurbidimeter";
            this.calibrationTurbidimeter.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.calibrationTurbidimeter.Size = new System.Drawing.Size(77, 70);
            this.calibrationTurbidimeter.TabIndex = 24;
            this.calibrationTurbidimeter.Text = "Thresholds";
            this.calibrationTurbidimeter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.calibrationTurbidimeter.UseVisualStyleBackColor = true;
            this.calibrationTurbidimeter.Visible = false;
            this.calibrationTurbidimeter.CheckedChanged += new System.EventHandler(this.calibrationTurbidimeter_CheckedChanged);
            // 
            // cbOfflineMode
            // 
            this.cbOfflineMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbOfflineMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbOfflineMode.FlatAppearance.BorderSize = 0;
            this.cbOfflineMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbOfflineMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOfflineMode.Image = global::ExactaEasy.Properties.Resources.application_x_trash;
            this.cbOfflineMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbOfflineMode.Location = new System.Drawing.Point(216, 0);
            this.cbOfflineMode.Name = "cbOfflineMode";
            this.cbOfflineMode.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cbOfflineMode.Size = new System.Drawing.Size(72, 70);
            this.cbOfflineMode.TabIndex = 23;
            this.cbOfflineMode.Text = "Offline";
            this.cbOfflineMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbOfflineMode.UseVisualStyleBackColor = true;
            this.cbOfflineMode.CheckedChanged += new System.EventHandler(this.cbOfflineMode_CheckedChanged);
            // 
            // btnSaveData
            // 
            this.btnSaveData.AutoEllipsis = true;
            this.btnSaveData.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveData.FlatAppearance.BorderSize = 0;
            this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveData.Image = global::ExactaEasy.Properties.Resources.webcamreceive;
            this.btnSaveData.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveData.Location = new System.Drawing.Point(144, 0);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnSaveData.Size = new System.Drawing.Size(72, 70);
            this.btnSaveData.TabIndex = 22;
            this.btnSaveData.Text = "Save";
            this.btnSaveData.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.AutoEllipsis = true;
            this.btnUpload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Image = global::ExactaEasy.Properties.Resources.upload_recipe32x32;
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpload.Location = new System.Drawing.Point(72, 0);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnUpload.Size = new System.Drawing.Size(72, 70);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnReset
            // 
            this.btnReset.AutoEllipsis = true;
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Image = global::ExactaEasy.Properties.Resources.edit_clear;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReset.Location = new System.Drawing.Point(0, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnReset.Size = new System.Drawing.Size(72, 70);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            this.pnlStatus.Controls.Add(this.lblStationStatus);
            this.pnlStatus.Controls.Add(this.lblCameraDescription);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(3, 3);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(474, 47);
            this.pnlStatus.TabIndex = 4;
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
            // lblCameraDescription
            // 
            this.lblCameraDescription.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCameraDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCameraDescription.Font = new System.Drawing.Font("Nirmala UI", 12F);
            this.lblCameraDescription.ForeColor = System.Drawing.Color.White;
            this.lblCameraDescription.Location = new System.Drawing.Point(0, 0);
            this.lblCameraDescription.Name = "lblCameraDescription";
            this.lblCameraDescription.Size = new System.Drawing.Size(474, 47);
            this.lblCameraDescription.TabIndex = 0;
            this.lblCameraDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCameraTables
            // 
            this.pnlCameraTables.Controls.Add(this.pnlParams);
            this.pnlCameraTables.Controls.Add(this.resGrid);
            this.pnlCameraTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCameraTables.Location = new System.Drawing.Point(3, 535);
            this.pnlCameraTables.Name = "pnlCameraTables";
            this.pnlCameraTables.Size = new System.Drawing.Size(474, 14);
            this.pnlCameraTables.TabIndex = 3;
            // 
            // pnlParams
            // 
            this.pnlParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(474, 14);
            this.pnlParams.TabIndex = 4;
            this.pnlParams.Visible = false;
            // 
            // pnlParamSelector
            // 
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
            this.btnMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMachine.Image = global::ExactaEasy.Properties.Resources.system_run;
            this.btnMachine.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMachine.Location = new System.Drawing.Point(288, 0);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(80, 60);
            this.btnMachine.TabIndex = 6;
            this.btnMachine.Text = "Machine";
            this.btnMachine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            this.btnAdvanced.Location = new System.Drawing.Point(256, 0);
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
            this.btnROI.Image = global::ExactaEasy.Properties.Resources.transform_crop_and_resize;
            this.btnROI.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnROI.Location = new System.Drawing.Point(224, 0);
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
            this.btnStrobo.FlatAppearance.BorderSize = 0;
            this.btnStrobo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrobo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStrobo.Image = global::ExactaEasy.Properties.Resources.games_hint;
            this.btnStrobo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStrobo.Location = new System.Drawing.Point(192, 0);
            this.btnStrobo.Name = "btnStrobo";
            this.btnStrobo.Size = new System.Drawing.Size(32, 60);
            this.btnStrobo.TabIndex = 1;
            this.btnStrobo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStrobo.UseVisualStyleBackColor = true;
            this.btnStrobo.Click += new System.EventHandler(this.btnStrobo_Click);
            // 
            // btnElaboration
            // 
            this.btnElaboration.AutoEllipsis = true;
            this.btnElaboration.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnElaboration.FlatAppearance.BorderSize = 0;
            this.btnElaboration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElaboration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnElaboration.Image = global::ExactaEasy.Properties.Resources.text_x_c__src;
            this.btnElaboration.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnElaboration.Location = new System.Drawing.Point(112, 0);
            this.btnElaboration.Name = "btnElaboration";
            this.btnElaboration.Size = new System.Drawing.Size(80, 60);
            this.btnElaboration.TabIndex = 0;
            this.btnElaboration.Text = "Elaboration";
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
            this.btnDigitizer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDigitizer.Image = global::ExactaEasy.Properties.Resources.hwinfo;
            this.btnDigitizer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDigitizer.Location = new System.Drawing.Point(80, 0);
            this.btnDigitizer.Name = "btnDigitizer";
            this.btnDigitizer.Size = new System.Drawing.Size(32, 60);
            this.btnDigitizer.TabIndex = 4;
            this.btnDigitizer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDigitizer.UseVisualStyleBackColor = true;
            this.btnDigitizer.Click += new System.EventHandler(this.btnDigitizer_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.AutoEllipsis = true;
            this.btnCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCamera.FlatAppearance.BorderSize = 0;
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCamera.Image = global::ExactaEasy.Properties.Resources.camera_photo_big;
            this.btnCamera.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCamera.Location = new System.Drawing.Point(0, 0);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(80, 60);
            this.btnCamera.TabIndex = 3;
            this.btnCamera.Text = "Acquisition";
            this.btnCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // pnlCameraCommand
            // 
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
            // btnRestore
            // 
            this.btnRestore.AutoEllipsis = true;
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRestore.Image = global::ExactaEasy.Properties.Resources.edit_undo;
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
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::ExactaEasy.Properties.Resources.document_save;
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
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnEdit.Image = global::ExactaEasy.Properties.Resources.document_save;
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
            this.cbViewResults.Image = global::ExactaEasy.Properties.Resources.edit_find1;
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
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Image = global::ExactaEasy.Properties.Resources.checkmark;
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
            this.pnlCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.pnlCenter.Controls.Add(this.tlpCalibrationTurbidimeter);
            this.pnlCenter.Controls.Add(this.tlpGraphs);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(480, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCenter.Size = new System.Drawing.Size(440, 426);
            this.pnlCenter.TabIndex = 4;
            this.pnlCenter.Resize += new System.EventHandler(this.pnlCenter_Resize);
            // 
            // tlpCalibrationTurbidimeter
            // 
            this.tlpCalibrationTurbidimeter.ColumnCount = 2;
            this.tlpCalibrationTurbidimeter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlpCalibrationTurbidimeter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCalibrationTurbidimeter.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tlpCalibrationTurbidimeter.Controls.Add(this.panel1, 1, 0);
            this.tlpCalibrationTurbidimeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCalibrationTurbidimeter.Location = new System.Drawing.Point(5, 5);
            this.tlpCalibrationTurbidimeter.Name = "tlpCalibrationTurbidimeter";
            this.tlpCalibrationTurbidimeter.RowCount = 1;
            this.tlpCalibrationTurbidimeter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCalibrationTurbidimeter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416F));
            this.tlpCalibrationTurbidimeter.Size = new System.Drawing.Size(430, 416);
            this.tlpCalibrationTurbidimeter.TabIndex = 6;
            this.tlpCalibrationTurbidimeter.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.zgCalibration, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvCalibrationTurbidimeter, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(316, 410);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // zgCalibration
            // 
            this.zgCalibration.BackColor = System.Drawing.SystemColors.Control;
            this.zgCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgCalibration.EditButtons = System.Windows.Forms.MouseButtons.None;
            this.zgCalibration.EditModifierKeys = System.Windows.Forms.Keys.None;
            this.zgCalibration.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zgCalibration.IsEnableHPan = false;
            this.zgCalibration.IsEnableHZoom = false;
            this.zgCalibration.IsEnableVPan = false;
            this.zgCalibration.IsEnableVZoom = false;
            this.zgCalibration.IsEnableWheelZoom = false;
            this.zgCalibration.IsPrintFillPage = false;
            this.zgCalibration.IsPrintKeepAspectRatio = false;
            this.zgCalibration.IsShowContextMenu = false;
            this.zgCalibration.Location = new System.Drawing.Point(3, 208);
            this.zgCalibration.Name = "zgCalibration";
            this.zgCalibration.ScrollGrace = 0D;
            this.zgCalibration.ScrollMaxX = 0D;
            this.zgCalibration.ScrollMaxY = 0D;
            this.zgCalibration.ScrollMaxY2 = 0D;
            this.zgCalibration.ScrollMinX = 0D;
            this.zgCalibration.ScrollMinY = 0D;
            this.zgCalibration.ScrollMinY2 = 0D;
            this.zgCalibration.Size = new System.Drawing.Size(310, 199);
            this.zgCalibration.TabIndex = 25;
            this.zgCalibration.Tag = "";
            this.zgCalibration.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // dgvCalibrationTurbidimeter
            // 
            this.dgvCalibrationTurbidimeter.AllowUserToAddRows = false;
            this.dgvCalibrationTurbidimeter.AllowUserToDeleteRows = false;
            this.dgvCalibrationTurbidimeter.AllowUserToResizeRows = false;
            this.dgvCalibrationTurbidimeter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCalibrationTurbidimeter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCalibrationTurbidimeter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.MEASURE_1,
            this.MEASURE_2,
            this.MEASURE_3,
            this.MEASURE_4,
            this.MEASURE_5,
            this.MEASURE_6,
            this.MEASURE_7,
            this.MEASURE_8,
            this.MEASURE_9,
            this.MEASURE_10,
            this.MEDIA});
            this.dgvCalibrationTurbidimeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCalibrationTurbidimeter.Enabled = false;
            this.dgvCalibrationTurbidimeter.Location = new System.Drawing.Point(3, 3);
            this.dgvCalibrationTurbidimeter.MultiSelect = false;
            this.dgvCalibrationTurbidimeter.Name = "dgvCalibrationTurbidimeter";
            this.dgvCalibrationTurbidimeter.ReadOnly = true;
            this.dgvCalibrationTurbidimeter.RowHeadersVisible = false;
            this.dgvCalibrationTurbidimeter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCalibrationTurbidimeter.ShowCellErrors = false;
            this.dgvCalibrationTurbidimeter.ShowCellToolTips = false;
            this.dgvCalibrationTurbidimeter.ShowEditingIcon = false;
            this.dgvCalibrationTurbidimeter.ShowRowErrors = false;
            this.dgvCalibrationTurbidimeter.Size = new System.Drawing.Size(310, 199);
            this.dgvCalibrationTurbidimeter.TabIndex = 4;
            // 
            // ID
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ID.DefaultCellStyle = dataGridViewCellStyle1;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_1
            // 
            this.MEASURE_1.HeaderText = "1";
            this.MEASURE_1.Name = "MEASURE_1";
            this.MEASURE_1.ReadOnly = true;
            this.MEASURE_1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_2
            // 
            this.MEASURE_2.HeaderText = "2";
            this.MEASURE_2.Name = "MEASURE_2";
            this.MEASURE_2.ReadOnly = true;
            this.MEASURE_2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_3
            // 
            this.MEASURE_3.HeaderText = "3";
            this.MEASURE_3.Name = "MEASURE_3";
            this.MEASURE_3.ReadOnly = true;
            this.MEASURE_3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_4
            // 
            this.MEASURE_4.HeaderText = "4";
            this.MEASURE_4.Name = "MEASURE_4";
            this.MEASURE_4.ReadOnly = true;
            this.MEASURE_4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_5
            // 
            this.MEASURE_5.HeaderText = "5";
            this.MEASURE_5.Name = "MEASURE_5";
            this.MEASURE_5.ReadOnly = true;
            this.MEASURE_5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_6
            // 
            this.MEASURE_6.HeaderText = "6";
            this.MEASURE_6.Name = "MEASURE_6";
            this.MEASURE_6.ReadOnly = true;
            this.MEASURE_6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_7
            // 
            this.MEASURE_7.HeaderText = "7";
            this.MEASURE_7.Name = "MEASURE_7";
            this.MEASURE_7.ReadOnly = true;
            this.MEASURE_7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_8
            // 
            this.MEASURE_8.HeaderText = "8";
            this.MEASURE_8.Name = "MEASURE_8";
            this.MEASURE_8.ReadOnly = true;
            this.MEASURE_8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_9
            // 
            this.MEASURE_9.HeaderText = "9";
            this.MEASURE_9.Name = "MEASURE_9";
            this.MEASURE_9.ReadOnly = true;
            this.MEASURE_9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEASURE_10
            // 
            this.MEASURE_10.HeaderText = "10";
            this.MEASURE_10.Name = "MEASURE_10";
            this.MEASURE_10.ReadOnly = true;
            this.MEASURE_10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MEDIA
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MEDIA.DefaultCellStyle = dataGridViewCellStyle2;
            this.MEDIA.HeaderText = "AVG";
            this.MEDIA.Name = "MEDIA";
            this.MEDIA.ReadOnly = true;
            this.MEDIA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Controls.Add(this.lblSuggestedLimits);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.cbAcqCalibrationTurbidimeter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(325, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 410);
            this.panel1.TabIndex = 15;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(4, 371);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 36);
            this.btnExport.TabIndex = 13;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(4, 333);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(95, 36);
            this.btnImport.TabIndex = 12;
            this.btnImport.Text = "IMPORT";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.lblNumSamples, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.nudNumbersOfItems, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(96, 47);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // lblNumSamples
            // 
            this.lblNumSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumSamples.Location = new System.Drawing.Point(4, 4);
            this.lblNumSamples.Margin = new System.Windows.Forms.Padding(3);
            this.lblNumSamples.Name = "lblNumSamples";
            this.lblNumSamples.Size = new System.Drawing.Size(40, 39);
            this.lblNumSamples.TabIndex = 9;
            this.lblNumSamples.Text = "N° Elements by type";
            this.lblNumSamples.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudNumbersOfItems
            // 
            this.nudNumbersOfItems.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudNumbersOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudNumbersOfItems.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudNumbersOfItems.Location = new System.Drawing.Point(51, 13);
            this.nudNumbersOfItems.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudNumbersOfItems.MaximumSize = new System.Drawing.Size(70, 0);
            this.nudNumbersOfItems.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNumbersOfItems.MinimumSize = new System.Drawing.Size(40, 0);
            this.nudNumbersOfItems.Name = "nudNumbersOfItems";
            this.nudNumbersOfItems.Size = new System.Drawing.Size(41, 21);
            this.nudNumbersOfItems.TabIndex = 7;
            this.nudNumbersOfItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudNumbersOfItems.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblSuggestedLimits
            // 
            this.lblSuggestedLimits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSuggestedLimits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSuggestedLimits.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSuggestedLimits.Location = new System.Drawing.Point(3, 205);
            this.lblSuggestedLimits.Name = "lblSuggestedLimits";
            this.lblSuggestedLimits.Size = new System.Drawing.Size(96, 122);
            this.lblSuggestedLimits.TabIndex = 10;
            this.lblSuggestedLimits.Text = "-";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.rbHisto);
            this.flowLayoutPanel1.Controls.Add(this.rbAllStdDev);
            this.flowLayoutPanel1.Controls.Add(this.rbLow);
            this.flowLayoutPanel1.Controls.Add(this.rbMedium);
            this.flowLayoutPanel1.Controls.Add(this.rbHigh);
            this.flowLayoutPanel1.Enabled = false;
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 126);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(96, 76);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // rbHisto
            // 
            this.rbHisto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbHisto.Checked = true;
            this.rbHisto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHisto.Location = new System.Drawing.Point(3, 3);
            this.rbHisto.Name = "rbHisto";
            this.rbHisto.Size = new System.Drawing.Size(100, 19);
            this.rbHisto.TabIndex = 0;
            this.rbHisto.TabStop = true;
            this.rbHisto.Text = "Histogram";
            this.rbHisto.UseVisualStyleBackColor = true;
            this.rbHisto.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbAllStdDev
            // 
            this.rbAllStdDev.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbAllStdDev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAllStdDev.Location = new System.Drawing.Point(3, 28);
            this.rbAllStdDev.Name = "rbAllStdDev";
            this.rbAllStdDev.Size = new System.Drawing.Size(100, 19);
            this.rbAllStdDev.TabIndex = 1;
            this.rbAllStdDev.Text = "Std Dev";
            this.rbAllStdDev.UseVisualStyleBackColor = true;
            this.rbAllStdDev.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbLow
            // 
            this.rbLow.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbLow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLow.ForeColor = System.Drawing.Color.Red;
            this.rbLow.Location = new System.Drawing.Point(3, 53);
            this.rbLow.Name = "rbLow";
            this.rbLow.Size = new System.Drawing.Size(100, 19);
            this.rbLow.TabIndex = 2;
            this.rbLow.Text = "Light";
            this.rbLow.UseVisualStyleBackColor = true;
            this.rbLow.Visible = false;
            this.rbLow.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbMedium
            // 
            this.rbMedium.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbMedium.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMedium.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rbMedium.Location = new System.Drawing.Point(3, 78);
            this.rbMedium.Name = "rbMedium";
            this.rbMedium.Size = new System.Drawing.Size(100, 19);
            this.rbMedium.TabIndex = 3;
            this.rbMedium.Text = "Good";
            this.rbMedium.UseVisualStyleBackColor = true;
            this.rbMedium.Visible = false;
            this.rbMedium.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbHigh
            // 
            this.rbHigh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbHigh.ForeColor = System.Drawing.Color.Blue;
            this.rbHigh.Location = new System.Drawing.Point(3, 103);
            this.rbHigh.Name = "rbHigh";
            this.rbHigh.Size = new System.Drawing.Size(100, 19);
            this.rbHigh.TabIndex = 4;
            this.rbHigh.Text = "Dark";
            this.rbHigh.UseVisualStyleBackColor = true;
            this.rbHigh.Visible = false;
            this.rbHigh.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // cbAcqCalibrationTurbidimeter
            // 
            this.cbAcqCalibrationTurbidimeter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAcqCalibrationTurbidimeter.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAcqCalibrationTurbidimeter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAcqCalibrationTurbidimeter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAcqCalibrationTurbidimeter.Location = new System.Drawing.Point(3, 54);
            this.cbAcqCalibrationTurbidimeter.Name = "cbAcqCalibrationTurbidimeter";
            this.cbAcqCalibrationTurbidimeter.Size = new System.Drawing.Size(96, 66);
            this.cbAcqCalibrationTurbidimeter.TabIndex = 5;
            this.cbAcqCalibrationTurbidimeter.Text = "Start Acquisition";
            this.cbAcqCalibrationTurbidimeter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAcqCalibrationTurbidimeter.UseVisualStyleBackColor = true;
            this.cbAcqCalibrationTurbidimeter.CheckedChanged += new System.EventHandler(this.cbAcqCalibrationTurbidimeter_CheckedChanged);
            // 
            // tlpGraphs
            // 
            this.tlpGraphs.ColumnCount = 1;
            this.tlpGraphs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.Controls.Add(this.extGraphBox, 0, 0);
            this.tlpGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGraphs.Location = new System.Drawing.Point(5, 5);
            this.tlpGraphs.Name = "tlpGraphs";
            this.tlpGraphs.RowCount = 1;
            this.tlpGraphs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.Size = new System.Drawing.Size(430, 416);
            this.tlpGraphs.TabIndex = 3;
            // 
            // extGraphBox
            // 
            this.extGraphBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extGraphBox.IsShowPointValues = true;
            this.extGraphBox.Location = new System.Drawing.Point(6, 6);
            this.extGraphBox.Margin = new System.Windows.Forms.Padding(6);
            this.extGraphBox.Name = "extGraphBox";
            this.extGraphBox.ScrollGrace = 0D;
            this.extGraphBox.ScrollMaxX = 0D;
            this.extGraphBox.ScrollMaxY = 0D;
            this.extGraphBox.ScrollMaxY2 = 0D;
            this.extGraphBox.ScrollMinX = 0D;
            this.extGraphBox.ScrollMinY = 0D;
            this.extGraphBox.ScrollMinY2 = 0D;
            this.extGraphBox.Size = new System.Drawing.Size(418, 404);
            this.extGraphBox.TabIndex = 0;
            // 
            // timerDevStatus
            // 
            this.timerDevStatus.Interval = 300;
            this.timerDevStatus.Tick += new System.EventHandler(this.timerDevStatus_Tick);
            // 
            // timerValuesTurbidimeter
            // 
            this.timerValuesTurbidimeter.Interval = 200;
            this.timerValuesTurbidimeter.Tick += new System.EventHandler(this.timerValuesTurbidimeter_Tick);
            // 
            // yesNoPanel
            // 
            this.yesNoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.yesNoPanel.Location = new System.Drawing.Point(0, -97);
            this.yesNoPanel.Margin = new System.Windows.Forms.Padding(6);
            this.yesNoPanel.Name = "yesNoPanel";
            this.yesNoPanel.Size = new System.Drawing.Size(440, 93);
            this.yesNoPanel.TabIndex = 3;
            this.yesNoPanel.Visible = false;
            // 
            // saveMenu
            // 
            this.saveMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.saveMenu.Location = new System.Drawing.Point(0, -4);
            this.saveMenu.Margin = new System.Windows.Forms.Padding(6);
            this.saveMenu.Name = "saveMenu";
            this.saveMenu.Size = new System.Drawing.Size(440, 80);
            this.saveMenu.TabIndex = 5;
            this.saveMenu.Visible = false;
            // 
            // resGrid
            // 
            this.resGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resGrid.Location = new System.Drawing.Point(0, 0);
            this.resGrid.Margin = new System.Windows.Forms.Padding(6);
            this.resGrid.Name = "resGrid";
            this.resGrid.Size = new System.Drawing.Size(474, 14);
            this.resGrid.TabIndex = 0;
            // 
            // pgCameraParams
            // 
            this.pgCameraParams.ChildId = null;
            this.pgCameraParams.DataSource = null;
            this.pgCameraParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgCameraParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgCameraParams.Location = new System.Drawing.Point(7, 125);
            this.pgCameraParams.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pgCameraParams.MaxValueColumnName = "MaxValue";
            this.pgCameraParams.MinValueColumnName = "MinValue";
            this.pgCameraParams.Name = "pgCameraParams";
            this.pgCameraParams.ParentDataSource = null;
            this.pgCameraParams.ShowDataGridViewParent = true;
            this.pgCameraParams.ShowLogInfo = true;
            this.pgCameraParams.ShowMinMaxInfo = false;
            this.pgCameraParams.ShowTitleBar = false;
            this.pgCameraParams.Size = new System.Drawing.Size(466, 401);
            this.pgCameraParams.TabIndex = 3;
            this.pgCameraParams.Title = "";
            // 
            // DataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlParameter);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Name = "DataViewer";
            this.Size = new System.Drawing.Size(920, 572);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlUp.ResumeLayout(false);
            this.pnlParameter.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.pnlCameraTables.ResumeLayout(false);
            this.pnlParamSelector.ResumeLayout(false);
            this.pnlCameraCommand.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.tlpCalibrationTurbidimeter.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalibrationTurbidimeter)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumbersOfItems)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tlpGraphs.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnStrobo;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDigitizer;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Button btnROI;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStationStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnMachine;
        private System.Windows.Forms.CheckBox cbViewResults;
        private System.Windows.Forms.Panel pnlUp;
        private System.Windows.Forms.Panel pnlCameraTables;
        private ExactaEasy.ResultsGrid resGrid;
        private System.Windows.Forms.Panel pnlParams;
        private ZedGraph.ZedGraphControl extGraphBox;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timerDevStatus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnUpload;
        private YesNoPanel yesNoPanel;
        private System.Windows.Forms.TableLayoutPanel tlpGraphs;
        private System.Windows.Forms.CheckBox cbOfflineMode;
        private System.Windows.Forms.Button btnSaveData;
        private SaveMenu saveMenu;
        private System.Windows.Forms.CheckBox calibrationTurbidimeter;
        private System.Windows.Forms.DataGridView dgvCalibrationTurbidimeter;
        private System.Windows.Forms.TableLayoutPanel tlpCalibrationTurbidimeter;
        private System.Windows.Forms.CheckBox cbAcqCalibrationTurbidimeter;
        private System.Windows.Forms.Panel panel1;
        private ZedGraph.ZedGraphControl zgCalibration;
        private System.Windows.Forms.NumericUpDown nudNumbersOfItems;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rbHisto;
        private System.Windows.Forms.RadioButton rbAllStdDev;
        private System.Windows.Forms.RadioButton rbLow;
        private System.Windows.Forms.RadioButton rbMedium;
        private System.Windows.Forms.RadioButton rbHigh;
        private System.Windows.Forms.Label lblNumSamples;
        private System.Windows.Forms.Label lblSuggestedLimits;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_3;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_4;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_5;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_6;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_7;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_8;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_9;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEASURE_10;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEDIA;
        private System.Windows.Forms.Timer timerValuesTurbidimeter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
    }
}
