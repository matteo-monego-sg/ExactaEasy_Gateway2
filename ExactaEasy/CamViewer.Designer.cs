namespace ExactaEasy {
    partial class CamViewer {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CamViewer));
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlUp = new System.Windows.Forms.Panel();
            this.lblPixInfo = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblStatusInfo = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.pnlCamControls = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnVialAxis = new System.Windows.Forms.Button();
            this.btnRemoteDesktop = new System.Windows.Forms.Button();
            this.bttDownloadImages = new System.Windows.Forms.Button();
            this.bttStopAnalysis = new System.Windows.Forms.Button();
            this.bttAnalysis = new System.Windows.Forms.Button();
            this.btnStopOnCondition = new System.Windows.Forms.Button();
            this.pnlParameter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.lblStationStatus = new System.Windows.Forms.Label();
            this.lblCameraDescription = new System.Windows.Forms.Label();
            this.pnlParamSelector = new System.Windows.Forms.Panel();
            this.btnMachine = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.btnROI = new System.Windows.Forms.Button();
            this.btnStrobo = new System.Windows.Forms.Button();
            this.btnElaboration = new System.Windows.Forms.Button();
            this.btnDigitizer = new System.Windows.Forms.Button();
            this.btnCamera = new System.Windows.Forms.Button();
            this.pgCameraParams = new ExactaEasy.PagedGrid();
            this.pnlLiveMenu = new System.Windows.Forms.Panel();
            this.btnExitLiveMenu = new System.Windows.Forms.Button();
            this.rbtROI = new System.Windows.Forms.RadioButton();
            this.rbtFullFrame = new System.Windows.Forms.RadioButton();
            this.pnlCameraCommand = new System.Windows.Forms.Panel();
            this.cbZoom = new System.Windows.Forms.CheckBox();
            this.btnLiveStartFF = new System.Windows.Forms.Button();
            this.btnLiveStart = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pcbLive = new ExactaEasy.ExtPictureBox();
            this.pnlControls.SuspendLayout();
            this.pnlUp.SuspendLayout();
            this.pnlCamControls.SuspendLayout();
            this.pnlParameter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlParamSelector.SuspendLayout();
            this.pnlLiveMenu.SuspendLayout();
            this.pnlCameraCommand.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLive)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlControls.Controls.Add(this.pnlUp);
            this.pnlControls.Controls.Add(this.pnlCamControls);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(424, 426);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(496, 146);
            this.pnlControls.TabIndex = 1;
            // 
            // pnlUp
            // 
            this.pnlUp.Controls.Add(this.lblPixInfo);
            this.pnlUp.Controls.Add(this.lblInfo);
            this.pnlUp.Controls.Add(this.lblStatusInfo);
            this.pnlUp.Controls.Add(this.btnPrev);
            this.pnlUp.Controls.Add(this.btnLast);
            this.pnlUp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUp.Location = new System.Drawing.Point(0, 0);
            this.pnlUp.Name = "pnlUp";
            this.pnlUp.Size = new System.Drawing.Size(496, 70);
            this.pnlUp.TabIndex = 1;
            // 
            // lblPixInfo
            // 
            this.lblPixInfo.AutoSize = true;
            this.lblPixInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPixInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPixInfo.Location = new System.Drawing.Point(295, 0);
            this.lblPixInfo.Name = "lblPixInfo";
            this.lblPixInfo.Size = new System.Drawing.Size(57, 15);
            this.lblPixInfo.TabIndex = 10;
            this.lblPixInfo.Text = "Pixel info";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(3, 7);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(130, 15);
            this.lblInfo.TabIndex = 9;
            this.lblInfo.Text = "Stop on Condition Info:";
            // 
            // lblStatusInfo
            // 
            this.lblStatusInfo.AutoSize = true;
            this.lblStatusInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusInfo.Location = new System.Drawing.Point(3, 29);
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new System.Drawing.Size(91, 15);
            this.lblStatusInfo.TabIndex = 7;
            this.lblStatusInfo.Text = "Status Camera:";
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnPrev.Image = global::ExactaEasy.Properties.Resources.media_seek_backward;
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.Location = new System.Drawing.Point(352, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(72, 70);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "Prev";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnLast
            // 
            this.btnLast.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLast.FlatAppearance.BorderSize = 0;
            this.btnLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLast.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLast.Image = global::ExactaEasy.Properties.Resources.media_skip_forward;
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLast.Location = new System.Drawing.Point(424, 0);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(72, 70);
            this.btnLast.TabIndex = 1;
            this.btnLast.Text = "Last";
            this.btnLast.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // pnlCamControls
            // 
            this.pnlCamControls.Controls.Add(this.btnReset);
            this.pnlCamControls.Controls.Add(this.btnVialAxis);
            this.pnlCamControls.Controls.Add(this.btnRemoteDesktop);
            this.pnlCamControls.Controls.Add(this.bttDownloadImages);
            this.pnlCamControls.Controls.Add(this.bttStopAnalysis);
            this.pnlCamControls.Controls.Add(this.bttAnalysis);
            this.pnlCamControls.Controls.Add(this.btnStopOnCondition);
            this.pnlCamControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCamControls.Location = new System.Drawing.Point(0, 70);
            this.pnlCamControls.Name = "pnlCamControls";
            this.pnlCamControls.Size = new System.Drawing.Size(496, 76);
            this.pnlCamControls.TabIndex = 14;
            // 
            // btnReset
            // 
            this.btnReset.AutoEllipsis = true;
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.Navy;
            this.btnReset.Image = global::ExactaEasy.Properties.Resources.system_shutdown;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReset.Location = new System.Drawing.Point(424, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(72, 76);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnVialAxis
            // 
            this.btnVialAxis.AutoEllipsis = true;
            this.btnVialAxis.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnVialAxis.FlatAppearance.BorderSize = 0;
            this.btnVialAxis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVialAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnVialAxis.ForeColor = System.Drawing.Color.Navy;
            this.btnVialAxis.Image = global::ExactaEasy.Properties.Resources.music_amarok;
            this.btnVialAxis.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnVialAxis.Location = new System.Drawing.Point(380, 0);
            this.btnVialAxis.Name = "btnVialAxis";
            this.btnVialAxis.Size = new System.Drawing.Size(72, 76);
            this.btnVialAxis.TabIndex = 14;
            this.btnVialAxis.Text = "Vial axis";
            this.btnVialAxis.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVialAxis.UseVisualStyleBackColor = true;
            this.btnVialAxis.Click += new System.EventHandler(this.btnVialAxis_Click);
            // 
            // btnRemoteDesktop
            // 
            this.btnRemoteDesktop.AutoEllipsis = true;
            this.btnRemoteDesktop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemoteDesktop.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRemoteDesktop.Enabled = false;
            this.btnRemoteDesktop.FlatAppearance.BorderSize = 0;
            this.btnRemoteDesktop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoteDesktop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRemoteDesktop.Image = global::ExactaEasy.Properties.Resources.configure;
            this.btnRemoteDesktop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRemoteDesktop.Location = new System.Drawing.Point(308, 0);
            this.btnRemoteDesktop.Name = "btnRemoteDesktop";
            this.btnRemoteDesktop.Size = new System.Drawing.Size(72, 76);
            this.btnRemoteDesktop.TabIndex = 13;
            this.btnRemoteDesktop.Text = "Config";
            this.btnRemoteDesktop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRemoteDesktop.UseVisualStyleBackColor = true;
            this.btnRemoteDesktop.Click += new System.EventHandler(this.btnRemoteDesktop_Click);
            // 
            // bttDownloadImages
            // 
            this.bttDownloadImages.AutoEllipsis = true;
            this.bttDownloadImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttDownloadImages.FlatAppearance.BorderSize = 0;
            this.bttDownloadImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttDownloadImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.bttDownloadImages.ForeColor = System.Drawing.Color.Navy;
            this.bttDownloadImages.Image = global::ExactaEasy.Properties.Resources.webcamreceive;
            this.bttDownloadImages.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bttDownloadImages.Location = new System.Drawing.Point(236, 0);
            this.bttDownloadImages.Name = "bttDownloadImages";
            this.bttDownloadImages.Size = new System.Drawing.Size(72, 76);
            this.bttDownloadImages.TabIndex = 6;
            this.bttDownloadImages.Text = "Download images";
            this.bttDownloadImages.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bttDownloadImages.UseVisualStyleBackColor = true;
            this.bttDownloadImages.Click += new System.EventHandler(this.bttDownloadImages_Click);
            // 
            // bttStopAnalysis
            // 
            this.bttStopAnalysis.AutoEllipsis = true;
            this.bttStopAnalysis.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttStopAnalysis.FlatAppearance.BorderSize = 0;
            this.bttStopAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttStopAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.bttStopAnalysis.Image = global::ExactaEasy.Properties.Resources.video_x_generic;
            this.bttStopAnalysis.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bttStopAnalysis.Location = new System.Drawing.Point(154, 0);
            this.bttStopAnalysis.Name = "bttStopAnalysis";
            this.bttStopAnalysis.Size = new System.Drawing.Size(82, 76);
            this.bttStopAnalysis.TabIndex = 12;
            this.bttStopAnalysis.Text = "Stop off. analysis";
            this.bttStopAnalysis.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bttStopAnalysis.UseVisualStyleBackColor = true;
            this.bttStopAnalysis.Click += new System.EventHandler(this.bttStopAnalysis_Click);
            // 
            // bttAnalysis
            // 
            this.bttAnalysis.AutoEllipsis = true;
            this.bttAnalysis.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttAnalysis.FlatAppearance.BorderSize = 0;
            this.bttAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.bttAnalysis.Image = global::ExactaEasy.Properties.Resources.winff;
            this.bttAnalysis.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bttAnalysis.Location = new System.Drawing.Point(72, 0);
            this.bttAnalysis.Name = "bttAnalysis";
            this.bttAnalysis.Size = new System.Drawing.Size(82, 76);
            this.bttAnalysis.TabIndex = 10;
            this.bttAnalysis.Text = "Offline analysis";
            this.bttAnalysis.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bttAnalysis.UseVisualStyleBackColor = true;
            this.bttAnalysis.Click += new System.EventHandler(this.bttAnalysis_Click);
            // 
            // btnStopOnCondition
            // 
            this.btnStopOnCondition.AutoEllipsis = true;
            this.btnStopOnCondition.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStopOnCondition.FlatAppearance.BorderSize = 0;
            this.btnStopOnCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopOnCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStopOnCondition.ForeColor = System.Drawing.Color.Navy;
            this.btnStopOnCondition.Image = global::ExactaEasy.Properties.Resources.music_amarok;
            this.btnStopOnCondition.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStopOnCondition.Location = new System.Drawing.Point(0, 0);
            this.btnStopOnCondition.Name = "btnStopOnCondition";
            this.btnStopOnCondition.Size = new System.Drawing.Size(72, 76);
            this.btnStopOnCondition.TabIndex = 5;
            this.btnStopOnCondition.Text = "Stop on condition";
            this.btnStopOnCondition.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStopOnCondition.UseVisualStyleBackColor = true;
            this.btnStopOnCondition.Click += new System.EventHandler(this.btnStopOnCondition_Click);
            // 
            // pnlParameter
            // 
            this.pnlParameter.Controls.Add(this.tableLayoutPanel1);
            this.pnlParameter.Controls.Add(this.pnlLiveMenu);
            this.pnlParameter.Controls.Add(this.pnlCameraCommand);
            this.pnlParameter.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlParameter.Location = new System.Drawing.Point(0, 0);
            this.pnlParameter.Name = "pnlParameter";
            this.pnlParameter.Size = new System.Drawing.Size(424, 572);
            this.pnlParameter.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(424, 432);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCameraStatus);
            this.panel1.Controls.Add(this.lblStationStatus);
            this.panel1.Controls.Add(this.lblCameraDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 47);
            this.panel1.TabIndex = 4;
            // 
            // lblCameraStatus
            // 
            this.lblCameraStatus.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCameraStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.white_off_32;
            this.lblCameraStatus.Location = new System.Drawing.Point(333, 0);
            this.lblCameraStatus.Name = "lblCameraStatus";
            this.lblCameraStatus.Size = new System.Drawing.Size(46, 47);
            this.lblCameraStatus.TabIndex = 2;
            this.lblCameraStatus.Click += new System.EventHandler(this.lblCameraStatus_Click);
            // 
            // lblStationStatus
            // 
            this.lblStationStatus.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStationStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStationStatus.Image = global::ExactaEasy.Properties.Resources.white_off_32;
            this.lblStationStatus.Location = new System.Drawing.Point(379, 0);
            this.lblStationStatus.Name = "lblStationStatus";
            this.lblStationStatus.Size = new System.Drawing.Size(39, 47);
            this.lblStationStatus.TabIndex = 1;
            // 
            // lblCameraDescription
            // 
            this.lblCameraDescription.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCameraDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCameraDescription.ForeColor = System.Drawing.Color.White;
            this.lblCameraDescription.Location = new System.Drawing.Point(0, 0);
            this.lblCameraDescription.Name = "lblCameraDescription";
            this.lblCameraDescription.Size = new System.Drawing.Size(418, 47);
            this.lblCameraDescription.TabIndex = 0;
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
            this.pnlParamSelector.Size = new System.Drawing.Size(418, 54);
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
            this.btnMachine.Location = new System.Drawing.Point(360, 0);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(60, 54);
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
            this.btnAdvanced.Location = new System.Drawing.Point(300, 0);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(60, 54);
            this.btnAdvanced.TabIndex = 2;
            this.btnAdvanced.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
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
            this.btnROI.Image = ((System.Drawing.Image)(resources.GetObject("btnROI.Image")));
            this.btnROI.Location = new System.Drawing.Point(240, 0);
            this.btnROI.Name = "btnROI";
            this.btnROI.Size = new System.Drawing.Size(60, 54);
            this.btnROI.TabIndex = 5;
            this.btnROI.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
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
            this.btnStrobo.Location = new System.Drawing.Point(180, 0);
            this.btnStrobo.Name = "btnStrobo";
            this.btnStrobo.Size = new System.Drawing.Size(60, 54);
            this.btnStrobo.TabIndex = 1;
            this.btnStrobo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStrobo.UseVisualStyleBackColor = true;
            this.btnStrobo.Click += new System.EventHandler(this.btnStrobo_Click);
            // 
            // btnElaboration
            // 
            this.btnElaboration.AutoEllipsis = true;
            this.btnElaboration.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnElaboration.FlatAppearance.BorderSize = 0;
            this.btnElaboration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElaboration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElaboration.Image = global::ExactaEasy.Properties.Resources.run_build;
            this.btnElaboration.Location = new System.Drawing.Point(120, 0);
            this.btnElaboration.Name = "btnElaboration";
            this.btnElaboration.Size = new System.Drawing.Size(60, 54);
            this.btnElaboration.TabIndex = 0;
            this.btnElaboration.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
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
            this.btnDigitizer.Image = ((System.Drawing.Image)(resources.GetObject("btnDigitizer.Image")));
            this.btnDigitizer.Location = new System.Drawing.Point(60, 0);
            this.btnDigitizer.Name = "btnDigitizer";
            this.btnDigitizer.Size = new System.Drawing.Size(60, 54);
            this.btnDigitizer.TabIndex = 4;
            this.btnDigitizer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDigitizer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDigitizer.UseVisualStyleBackColor = true;
            this.btnDigitizer.Click += new System.EventHandler(this.btnDigitizer_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.AutoEllipsis = true;
            this.btnCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCamera.FlatAppearance.BorderSize = 0;
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.Image = global::ExactaEasy.Properties.Resources.camera_photo_big;
            this.btnCamera.Location = new System.Drawing.Point(0, 0);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(60, 54);
            this.btnCamera.TabIndex = 3;
            this.btnCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCamera.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // pgCameraParams
            // 
            this.pgCameraParams.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pgCameraParams.ChildId = null;
            this.pgCameraParams.DataSource = null;
            this.pgCameraParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgCameraParams.ForeColor = System.Drawing.SystemColors.Control;
            this.pgCameraParams.Location = new System.Drawing.Point(3, 116);
            this.pgCameraParams.MaxValueColumnName = "MaxValue";
            this.pgCameraParams.MinValueColumnName = "MinValue";
            this.pgCameraParams.Name = "pgCameraParams";
            this.pgCameraParams.ParentDataSource = null;
            this.pgCameraParams.ShowDataGridViewParent = true;
            this.pgCameraParams.ShowLogInfo = true;
            this.pgCameraParams.ShowMinMaxInfo = true;
            this.pgCameraParams.ShowTitleBar = false;
            this.pgCameraParams.Size = new System.Drawing.Size(418, 401);
            this.pgCameraParams.TabIndex = 3;
            this.pgCameraParams.Title = "";
            // 
            // pnlLiveMenu
            // 
            this.pnlLiveMenu.Controls.Add(this.btnExitLiveMenu);
            this.pnlLiveMenu.Controls.Add(this.rbtROI);
            this.pnlLiveMenu.Controls.Add(this.rbtFullFrame);
            this.pnlLiveMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLiveMenu.Location = new System.Drawing.Point(0, 432);
            this.pnlLiveMenu.Name = "pnlLiveMenu";
            this.pnlLiveMenu.Size = new System.Drawing.Size(424, 70);
            this.pnlLiveMenu.TabIndex = 14;
            this.pnlLiveMenu.Visible = false;
            // 
            // btnExitLiveMenu
            // 
            this.btnExitLiveMenu.AutoEllipsis = true;
            this.btnExitLiveMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExitLiveMenu.FlatAppearance.BorderSize = 0;
            this.btnExitLiveMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitLiveMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExitLiveMenu.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExitLiveMenu.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExitLiveMenu.Location = new System.Drawing.Point(352, 0);
            this.btnExitLiveMenu.Name = "btnExitLiveMenu";
            this.btnExitLiveMenu.Size = new System.Drawing.Size(72, 70);
            this.btnExitLiveMenu.TabIndex = 12;
            this.btnExitLiveMenu.Text = "Exit";
            this.btnExitLiveMenu.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitLiveMenu.UseVisualStyleBackColor = true;
            // 
            // rbtROI
            // 
            this.rbtROI.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtROI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtROI.FlatAppearance.BorderSize = 0;
            this.rbtROI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtROI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtROI.Image = global::ExactaEasy.Properties.Resources.mail_folder_sent;
            this.rbtROI.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbtROI.Location = new System.Drawing.Point(2, 0);
            this.rbtROI.Name = "rbtROI";
            this.rbtROI.Size = new System.Drawing.Size(72, 70);
            this.rbtROI.TabIndex = 11;
            this.rbtROI.Text = "A.O.I.";
            this.rbtROI.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtROI.UseVisualStyleBackColor = true;
            // 
            // rbtFullFrame
            // 
            this.rbtFullFrame.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtFullFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtFullFrame.Enabled = false;
            this.rbtFullFrame.FlatAppearance.BorderSize = 0;
            this.rbtFullFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtFullFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtFullFrame.Image = global::ExactaEasy.Properties.Resources.teamviewer;
            this.rbtFullFrame.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbtFullFrame.Location = new System.Drawing.Point(72, 0);
            this.rbtFullFrame.Name = "rbtFullFrame";
            this.rbtFullFrame.Size = new System.Drawing.Size(72, 70);
            this.rbtFullFrame.TabIndex = 10;
            this.rbtFullFrame.Text = "Fullframe";
            this.rbtFullFrame.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtFullFrame.UseVisualStyleBackColor = true;
            // 
            // pnlCameraCommand
            // 
            this.pnlCameraCommand.Controls.Add(this.cbZoom);
            this.pnlCameraCommand.Controls.Add(this.btnLiveStartFF);
            this.pnlCameraCommand.Controls.Add(this.btnLiveStart);
            this.pnlCameraCommand.Controls.Add(this.btnRestore);
            this.pnlCameraCommand.Controls.Add(this.btnSave);
            this.pnlCameraCommand.Controls.Add(this.btnEdit);
            this.pnlCameraCommand.Controls.Add(this.btnApply);
            this.pnlCameraCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCameraCommand.Location = new System.Drawing.Point(0, 502);
            this.pnlCameraCommand.Name = "pnlCameraCommand";
            this.pnlCameraCommand.Size = new System.Drawing.Size(424, 70);
            this.pnlCameraCommand.TabIndex = 3;
            // 
            // cbZoom
            // 
            this.cbZoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbZoom.AutoEllipsis = true;
            this.cbZoom.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbZoom.FlatAppearance.BorderSize = 0;
            this.cbZoom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cbZoom.Image = global::ExactaEasy.Properties.Resources.zoom_in;
            this.cbZoom.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbZoom.Location = new System.Drawing.Point(432, 0);
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cbZoom.Size = new System.Drawing.Size(72, 70);
            this.cbZoom.TabIndex = 7;
            this.cbZoom.Text = "Zoom";
            this.cbZoom.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbZoom.UseVisualStyleBackColor = true;
            this.cbZoom.CheckedChanged += new System.EventHandler(this.cbZoom_CheckedChanged);
            // 
            // btnLiveStartFF
            // 
            this.btnLiveStartFF.AutoEllipsis = true;
            this.btnLiveStartFF.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLiveStartFF.FlatAppearance.BorderSize = 0;
            this.btnLiveStartFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLiveStartFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLiveStartFF.Image = global::ExactaEasy.Properties.Resources.media_playback_start;
            this.btnLiveStartFF.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLiveStartFF.Location = new System.Drawing.Point(360, 0);
            this.btnLiveStartFF.Name = "btnLiveStartFF";
            this.btnLiveStartFF.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnLiveStartFF.Size = new System.Drawing.Size(72, 70);
            this.btnLiveStartFF.TabIndex = 10;
            this.btnLiveStartFF.Text = "Start live";
            this.btnLiveStartFF.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLiveStartFF.UseVisualStyleBackColor = true;
            this.btnLiveStartFF.Click += new System.EventHandler(this.btnLiveStartFF_Click);
            // 
            // btnLiveStart
            // 
            this.btnLiveStart.AutoEllipsis = true;
            this.btnLiveStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLiveStart.FlatAppearance.BorderSize = 0;
            this.btnLiveStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLiveStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_start;
            this.btnLiveStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLiveStart.Location = new System.Drawing.Point(288, 0);
            this.btnLiveStart.Name = "btnLiveStart";
            this.btnLiveStart.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnLiveStart.Size = new System.Drawing.Size(72, 70);
            this.btnLiveStart.TabIndex = 8;
            this.btnLiveStart.Text = "Start live";
            this.btnLiveStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLiveStart.UseVisualStyleBackColor = true;
            this.btnLiveStart.Click += new System.EventHandler(this.btnLive_Click);
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
            this.btnRestore.Location = new System.Drawing.Point(216, 0);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnRestore.Size = new System.Drawing.Size(72, 70);
            this.btnRestore.TabIndex = 6;
            this.btnRestore.Text = "Undo";
            this.btnRestore.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRestore.UseVisualStyleBackColor = true;
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
            this.btnSave.Location = new System.Drawing.Point(144, 0);
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
            this.btnEdit.Location = new System.Drawing.Point(72, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnEdit.Size = new System.Drawing.Size(72, 70);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
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
            this.pnlCenter.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCenter.Controls.Add(this.pcbLive);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(424, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCenter.Size = new System.Drawing.Size(496, 426);
            this.pnlCenter.TabIndex = 4;
            this.pnlCenter.Resize += new System.EventHandler(this.pnlCenter_Resize);
            // 
            // pcbLive
            // 
            this.pcbLive.BackColor = System.Drawing.Color.Black;
            this.pcbLive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pcbLive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbLive.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.pcbLive.Location = new System.Drawing.Point(5, 5);
            this.pcbLive.Name = "pcbLive";
            this.pcbLive.Size = new System.Drawing.Size(486, 416);
            this.pcbLive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbLive.TabIndex = 0;
            this.pcbLive.TabStop = false;
            this.pcbLive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbLive_MouseDown);
            this.pcbLive.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pcbLive_MouseMove);
            // 
            // CamViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlParameter);
            this.Name = "CamViewer";
            this.Size = new System.Drawing.Size(920, 572);
            this.Load += new System.EventHandler(this.CamViewer_Load);
            this.pnlControls.ResumeLayout(false);
            this.pnlUp.ResumeLayout(false);
            this.pnlUp.PerformLayout();
            this.pnlCamControls.ResumeLayout(false);
            this.pnlParameter.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlParamSelector.ResumeLayout(false);
            this.pnlLiveMenu.ResumeLayout(false);
            this.pnlCameraCommand.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbLive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExactaEasy.ExtPictureBox pcbLive;
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
        private System.Windows.Forms.CheckBox cbZoom;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDigitizer;
        private System.Windows.Forms.Button btnLiveStart;
        //private System.Windows.Forms.Button btnLiveStop;
        private System.Windows.Forms.Button btnStopOnCondition;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Label lblStatusInfo;
        private System.Windows.Forms.Button bttDownloadImages;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnROI;
        private System.Windows.Forms.Button bttAnalysis;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStationStatus;
        private System.Windows.Forms.Label lblCameraStatus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnMachine;
        private System.Windows.Forms.Panel pnlLiveMenu;
        private System.Windows.Forms.Button btnExitLiveMenu;
        private System.Windows.Forms.RadioButton rbtROI;
        private System.Windows.Forms.RadioButton rbtFullFrame;
        private System.Windows.Forms.Panel pnlCamControls;
        private System.Windows.Forms.Button btnLiveStartFF;
        private System.Windows.Forms.Button bttStopAnalysis;
        private System.Windows.Forms.Button btnRemoteDesktop;
        private System.Windows.Forms.Panel pnlUp;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnVialAxis;
        private System.Windows.Forms.Label lblPixInfo;
    }
}
