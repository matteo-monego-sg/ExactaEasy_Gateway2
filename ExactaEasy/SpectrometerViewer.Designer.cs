namespace ExactaEasy {
    partial class SpectrometerViewer {
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
            this.pnlControls = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.loadSaveMenu = new ExactaEasy.LoadSaveMenu();
            this.yesNoPanel = new ExactaEasy.YesNoPanel();
            this.saveMenu = new ExactaEasy.SaveMenu();
            this.pnlUp = new System.Windows.Forms.Panel();
            this.btnLoadSave = new System.Windows.Forms.Button();
            this.cbOfflineMode = new System.Windows.Forms.CheckBox();
            this.btnSaveSpectra = new System.Windows.Forms.Button();
            this.cbViewInfo = new System.Windows.Forms.CheckBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.cbLive = new System.Windows.Forms.CheckBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlParameter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStationStatus = new System.Windows.Forms.Label();
            this.lblCameraDescription = new System.Windows.Forms.Label();
            this.pnlCameraTables = new System.Windows.Forms.Panel();
            this.pnlParams = new System.Windows.Forms.Panel();
            this.resGrid = new ExactaEasy.ResultsGrid();
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
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cbViewResults = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.tlpGraphs = new System.Windows.Forms.TableLayoutPanel();
            this.zgcSpectra = new ZedGraph.ZedGraphControl();
            this.zgcElaboration = new ZedGraph.ZedGraphControl();
            this.timerDevStatus = new System.Windows.Forms.Timer(this.components);
            this.timerLive = new System.Windows.Forms.Timer(this.components);
            this.pnlControls.SuspendLayout();
            this.pnlUp.SuspendLayout();
            this.pnlParameter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.pnlCameraTables.SuspendLayout();
            this.pnlParamSelector.SuspendLayout();
            this.pnlCameraCommand.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.tlpGraphs.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.pnlControls.Controls.Add(this.lblStatus);
            this.pnlControls.Controls.Add(this.loadSaveMenu);
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
            // loadSaveMenu
            // 
            this.loadSaveMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadSaveMenu.Location = new System.Drawing.Point(0, -177);
            this.loadSaveMenu.Name = "loadSaveMenu";
            this.loadSaveMenu.Size = new System.Drawing.Size(440, 80);
            this.loadSaveMenu.TabIndex = 5;
            this.loadSaveMenu.Visible = false;
            // 
            // yesNoPanel
            // 
            this.yesNoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.yesNoPanel.Location = new System.Drawing.Point(0, -97);
            this.yesNoPanel.Name = "yesNoPanel";
            this.yesNoPanel.Size = new System.Drawing.Size(440, 93);
            this.yesNoPanel.TabIndex = 3;
            this.yesNoPanel.Visible = false;
            // 
            // saveMenu
            // 
            this.saveMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.saveMenu.Location = new System.Drawing.Point(0, -4);
            this.saveMenu.Name = "saveMenu";
            this.saveMenu.Size = new System.Drawing.Size(440, 80);
            this.saveMenu.TabIndex = 4;
            this.saveMenu.Visible = false;
            // 
            // pnlUp
            // 
            this.pnlUp.Controls.Add(this.btnLoadSave);
            this.pnlUp.Controls.Add(this.cbOfflineMode);
            this.pnlUp.Controls.Add(this.btnSaveSpectra);
            this.pnlUp.Controls.Add(this.cbViewInfo);
            this.pnlUp.Controls.Add(this.btnRun);
            this.pnlUp.Controls.Add(this.cbLive);
            this.pnlUp.Controls.Add(this.btnUpload);
            this.pnlUp.Controls.Add(this.btnReset);
            this.pnlUp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUp.Location = new System.Drawing.Point(0, 76);
            this.pnlUp.Name = "pnlUp";
            this.pnlUp.Size = new System.Drawing.Size(440, 70);
            this.pnlUp.TabIndex = 1;
            // 
            // btnLoadSave
            // 
            this.btnLoadSave.AutoEllipsis = true;
            this.btnLoadSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLoadSave.FlatAppearance.BorderSize = 0;
            this.btnLoadSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSave.Image = global::ExactaEasy.Properties.Resources.accessories_text_editor;
            this.btnLoadSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadSave.Location = new System.Drawing.Point(504, 0);
            this.btnLoadSave.Name = "btnLoadSave";
            this.btnLoadSave.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnLoadSave.Size = new System.Drawing.Size(72, 70);
            this.btnLoadSave.TabIndex = 22;
            this.btnLoadSave.Text = "Recipe";
            this.btnLoadSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadSave.UseVisualStyleBackColor = true;
            this.btnLoadSave.Click += new System.EventHandler(this.btnLoadSave_Click);
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
            this.cbOfflineMode.Location = new System.Drawing.Point(432, 0);
            this.cbOfflineMode.Name = "cbOfflineMode";
            this.cbOfflineMode.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cbOfflineMode.Size = new System.Drawing.Size(72, 70);
            this.cbOfflineMode.TabIndex = 20;
            this.cbOfflineMode.Text = "Offline";
            this.cbOfflineMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbOfflineMode.UseVisualStyleBackColor = true;
            this.cbOfflineMode.CheckedChanged += new System.EventHandler(this.cbOfflineMode_CheckedChanged);
            // 
            // btnSaveSpectra
            // 
            this.btnSaveSpectra.AutoEllipsis = true;
            this.btnSaveSpectra.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveSpectra.FlatAppearance.BorderSize = 0;
            this.btnSaveSpectra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSpectra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSpectra.Image = global::ExactaEasy.Properties.Resources.webcamreceive;
            this.btnSaveSpectra.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveSpectra.Location = new System.Drawing.Point(360, 0);
            this.btnSaveSpectra.Name = "btnSaveSpectra";
            this.btnSaveSpectra.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnSaveSpectra.Size = new System.Drawing.Size(72, 70);
            this.btnSaveSpectra.TabIndex = 19;
            this.btnSaveSpectra.Text = "Save";
            this.btnSaveSpectra.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveSpectra.UseVisualStyleBackColor = true;
            this.btnSaveSpectra.Click += new System.EventHandler(this.btnSaveSpectra_Click);
            // 
            // cbViewInfo
            // 
            this.cbViewInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbViewInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbViewInfo.FlatAppearance.BorderSize = 0;
            this.cbViewInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbViewInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbViewInfo.Image = global::ExactaEasy.Properties.Resources.dialog_information;
            this.cbViewInfo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbViewInfo.Location = new System.Drawing.Point(288, 0);
            this.cbViewInfo.Name = "cbViewInfo";
            this.cbViewInfo.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cbViewInfo.Size = new System.Drawing.Size(72, 70);
            this.cbViewInfo.TabIndex = 15;
            this.cbViewInfo.Text = "Info";
            this.cbViewInfo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbViewInfo.UseVisualStyleBackColor = true;
            this.cbViewInfo.CheckedChanged += new System.EventHandler(this.cbViewInfo_CheckedChanged);
            // 
            // btnRun
            // 
            this.btnRun.AutoEllipsis = true;
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRun.FlatAppearance.BorderSize = 0;
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Image = global::ExactaEasy.Properties.Resources.debug_run32x32;
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRun.Location = new System.Drawing.Point(216, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnRun.Size = new System.Drawing.Size(72, 70);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "Run";
            this.btnRun.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // cbLive
            // 
            this.cbLive.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbLive.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbLive.FlatAppearance.BorderSize = 0;
            this.cbLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLive.Image = global::ExactaEasy.Properties.Resources.media_playback_start;
            this.cbLive.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbLive.Location = new System.Drawing.Point(144, 0);
            this.cbLive.Name = "cbLive";
            this.cbLive.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cbLive.Size = new System.Drawing.Size(72, 70);
            this.cbLive.TabIndex = 21;
            this.cbLive.Text = "Live";
            this.cbLive.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbLive.UseVisualStyleBackColor = true;
            this.cbLive.CheckedChanged += new System.EventHandler(this.cbLive_CheckedChanged);
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
            this.pnlCameraTables.Location = new System.Drawing.Point(3, 529);
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
            // resGrid
            // 
            this.resGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resGrid.Location = new System.Drawing.Point(0, 0);
            this.resGrid.Name = "resGrid";
            this.resGrid.Size = new System.Drawing.Size(474, 14);
            this.resGrid.TabIndex = 0;
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
            // pgCameraParams
            // 
            this.pgCameraParams.ChildId = null;
            this.pgCameraParams.DataSource = null;
            this.pgCameraParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgCameraParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgCameraParams.Location = new System.Drawing.Point(3, 122);
            this.pgCameraParams.MaxValueColumnName = "MaxValue";
            this.pgCameraParams.MinValueColumnName = "MinValue";
            this.pgCameraParams.Name = "pgCameraParams";
            this.pgCameraParams.ParentDataSource = null;
            this.pgCameraParams.ShowDataGridViewParent = true;
            this.pgCameraParams.ShowLogInfo = true;
            this.pgCameraParams.ShowMinMaxInfo = false;
            this.pgCameraParams.ShowTitleBar = false;
            this.pgCameraParams.Size = new System.Drawing.Size(474, 401);
            this.pgCameraParams.TabIndex = 3;
            this.pgCameraParams.Title = "";
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
            this.pnlCenter.Controls.Add(this.tlpGraphs);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(480, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCenter.Size = new System.Drawing.Size(440, 426);
            this.pnlCenter.TabIndex = 4;
            this.pnlCenter.Resize += new System.EventHandler(this.pnlCenter_Resize);
            // 
            // tlpGraphs
            // 
            this.tlpGraphs.ColumnCount = 1;
            this.tlpGraphs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.Controls.Add(this.zgcSpectra, 0, 0);
            this.tlpGraphs.Controls.Add(this.zgcElaboration, 0, 1);
            this.tlpGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGraphs.Location = new System.Drawing.Point(5, 5);
            this.tlpGraphs.Name = "tlpGraphs";
            this.tlpGraphs.RowCount = 2;
            this.tlpGraphs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpGraphs.Size = new System.Drawing.Size(430, 416);
            this.tlpGraphs.TabIndex = 3;
            // 
            // zgcSpectra
            // 
            this.zgcSpectra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgcSpectra.IsShowPointValues = true;
            this.zgcSpectra.Location = new System.Drawing.Point(3, 3);
            this.zgcSpectra.Name = "zgcSpectra";
            this.zgcSpectra.ScrollGrace = 0D;
            this.zgcSpectra.ScrollMaxX = 0D;
            this.zgcSpectra.ScrollMaxY = 0D;
            this.zgcSpectra.ScrollMaxY2 = 0D;
            this.zgcSpectra.ScrollMinX = 0D;
            this.zgcSpectra.ScrollMinY = 0D;
            this.zgcSpectra.ScrollMinY2 = 0D;
            this.zgcSpectra.Size = new System.Drawing.Size(424, 202);
            this.zgcSpectra.TabIndex = 0;
            // 
            // zgcElaboration
            // 
            this.zgcElaboration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgcElaboration.IsShowPointValues = true;
            this.zgcElaboration.Location = new System.Drawing.Point(3, 211);
            this.zgcElaboration.Name = "zgcElaboration";
            this.zgcElaboration.ScrollGrace = 0D;
            this.zgcElaboration.ScrollMaxX = 0D;
            this.zgcElaboration.ScrollMaxY = 0D;
            this.zgcElaboration.ScrollMaxY2 = 0D;
            this.zgcElaboration.ScrollMinX = 0D;
            this.zgcElaboration.ScrollMinY = 0D;
            this.zgcElaboration.ScrollMinY2 = 0D;
            this.zgcElaboration.Size = new System.Drawing.Size(424, 202);
            this.zgcElaboration.TabIndex = 1;
            // 
            // timerDevStatus
            // 
            this.timerDevStatus.Interval = 300;
            this.timerDevStatus.Tick += new System.EventHandler(this.timerDevStatus_Tick);
            // 
            // timerLive
            // 
            this.timerLive.Interval = 500;
            this.timerLive.Tick += new System.EventHandler(this.timerLive_Tick);
            // 
            // SpectrometerViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlParameter);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "SpectrometerViewer";
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
        private ZedGraph.ZedGraphControl zgcSpectra;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timerDevStatus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnUpload;
        private ZedGraph.ZedGraphControl zgcElaboration;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.CheckBox cbViewInfo;
        private YesNoPanel yesNoPanel;
        private System.Windows.Forms.TableLayoutPanel tlpGraphs;
        private System.Windows.Forms.Button btnSaveSpectra;
        private SaveMenu saveMenu;
        private System.Windows.Forms.CheckBox cbOfflineMode;
        private System.Windows.Forms.CheckBox cbLive;
        private System.Windows.Forms.Timer timerLive;
        private System.Windows.Forms.Button btnLoadSave;
        private LoadSaveMenu loadSaveMenu;
    }
}
