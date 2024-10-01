namespace SPAMI.Util.EmguImageViewer
{
    partial class ImageViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBoxGenInfo = new System.Windows.Forms.RichTextBox();
            this.customTabControlMenu = new System.Windows.Forms.CustomTabControl();
            this.tabPageFile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRecycle = new System.Windows.Forms.Panel();
            this.buttonFile = new System.Windows.Forms.Button();
            this.buttonFolder = new System.Windows.Forms.Button();
            this.checkBoxContinuousRecycle = new System.Windows.Forms.CheckBox();
            this.tabPagePlay = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMediaCmds = new System.Windows.Forms.Panel();
            this.pnlFps = new System.Windows.Forms.Panel();
            this.buttonFpsDec = new System.Windows.Forms.Button();
            this.buttonFpsInc = new System.Windows.Forms.Button();
            this.labelFps = new System.Windows.Forms.Label();
            this.ntbFps = new System.Windows.Forms.NumericTextBox(this.components);
            this.radioButtonRec = new System.Windows.Forms.RadioButton();
            this.radioButtonNext = new System.Windows.Forms.RadioButton();
            this.radioButtonPrev = new System.Windows.Forms.RadioButton();
            this.radioButtonStop = new System.Windows.Forms.RadioButton();
            this.radioButtonPause = new System.Windows.Forms.RadioButton();
            this.radioButtonPlay = new System.Windows.Forms.RadioButton();
            this.tabPageZoom = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlZoomMode = new System.Windows.Forms.Panel();
            this.radioButtonZoomOff = new System.Windows.Forms.RadioButton();
            this.radioButtonZoomOrig = new System.Windows.Forms.RadioButton();
            this.radioButtonZoomStretch = new System.Windows.Forms.RadioButton();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxHist = new System.Windows.Forms.GroupBox();
            this.checkBoxHexDisplay = new System.Windows.Forms.CheckBox();
            this.checkBoxBigHist = new System.Windows.Forms.CheckBox();
            this.checkBoxVerticalPixel = new System.Windows.Forms.CheckBox();
            this.checkBoxHorizontalPixel = new System.Windows.Forms.CheckBox();
            this.checkBoxViewInfoPixel = new System.Windows.Forms.CheckBox();
            this.radioButtonInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonHist = new System.Windows.Forms.RadioButton();
            this.imageListTabMenu = new System.Windows.Forms.ImageList(this.components);
            this.timerBlink = new System.Windows.Forms.Timer(this.components);
            this.toolTipCmd = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.customTabControlMenu.SuspendLayout();
            this.tabPageFile.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlRecycle.SuspendLayout();
            this.tabPagePlay.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.pnlMediaCmds.SuspendLayout();
            this.pnlFps.SuspendLayout();
            this.tabPageZoom.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.pnlZoomMode.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBoxHist.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.customTabControlMenu, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(595, 551);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel1_DragDrop);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AllowDrop = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.70588F));
            this.tableLayoutPanel2.Controls.Add(this.imageBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.imageBox2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(589, 338);
            this.tableLayoutPanel2.TabIndex = 2;
            this.tableLayoutPanel2.DragDrop += new System.Windows.Forms.DragEventHandler(this.tableLayoutPanel2_DragDrop);
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.Color.Black;
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox1.Location = new System.Drawing.Point(3, 3);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(201, 332);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            this.imageBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.imageBox1_DragDrop);
            // 
            // imageBox2
            // 
            this.imageBox2.BackColor = System.Drawing.Color.Black;
            this.imageBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox2.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox2.Location = new System.Drawing.Point(210, 3);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(376, 332);
            this.imageBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox2.TabIndex = 2;
            this.imageBox2.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.363F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.637F));
            this.tableLayoutPanel3.Controls.Add(this.richTextBoxGenInfo, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 347);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(589, 80);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // richTextBoxGenInfo
            // 
            this.richTextBoxGenInfo.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBoxGenInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxGenInfo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxGenInfo.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxGenInfo.Name = "richTextBoxGenInfo";
            this.richTextBoxGenInfo.ReadOnly = true;
            this.richTextBoxGenInfo.Size = new System.Drawing.Size(202, 74);
            this.richTextBoxGenInfo.TabIndex = 0;
            this.richTextBoxGenInfo.Text = "";
            this.richTextBoxGenInfo.WordWrap = false;
            // 
            // customTabControlMenu
            // 
            this.customTabControlMenu.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.customTabControlMenu.Controls.Add(this.tabPageFile);
            this.customTabControlMenu.Controls.Add(this.tabPagePlay);
            this.customTabControlMenu.Controls.Add(this.tabPageZoom);
            this.customTabControlMenu.Controls.Add(this.tabPageOptions);
            this.customTabControlMenu.DisplayStyle = System.Windows.Forms.TabStyle.Angled;
            // 
            // 
            // 
            this.customTabControlMenu.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.customTabControlMenu.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.customTabControlMenu.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.customTabControlMenu.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.customTabControlMenu.DisplayStyleProvider.FocusTrack = false;
            this.customTabControlMenu.DisplayStyleProvider.HotTrack = true;
            this.customTabControlMenu.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.customTabControlMenu.DisplayStyleProvider.Opacity = 1F;
            this.customTabControlMenu.DisplayStyleProvider.Overlap = 7;
            this.customTabControlMenu.DisplayStyleProvider.Padding = new System.Drawing.Point(10, 3);
            this.customTabControlMenu.DisplayStyleProvider.Radius = 10;
            this.customTabControlMenu.DisplayStyleProvider.ShowTabCloser = false;
            this.customTabControlMenu.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.customTabControlMenu.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.customTabControlMenu.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.customTabControlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControlMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabControlMenu.HotTrack = true;
            this.customTabControlMenu.ImageList = this.imageListTabMenu;
            this.customTabControlMenu.ItemSize = new System.Drawing.Size(100, 30);
            this.customTabControlMenu.Location = new System.Drawing.Point(3, 433);
            this.customTabControlMenu.Name = "customTabControlMenu";
            this.customTabControlMenu.SelectedIndex = 0;
            this.customTabControlMenu.Size = new System.Drawing.Size(589, 115);
            this.customTabControlMenu.TabIndex = 4;
            // 
            // tabPageFile
            // 
            this.tabPageFile.Controls.Add(this.tableLayoutPanel4);
            this.tabPageFile.ImageKey = "drive-harddisk.png";
            this.tabPageFile.Location = new System.Drawing.Point(4, 4);
            this.tabPageFile.Name = "tabPageFile";
            this.tabPageFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFile.Size = new System.Drawing.Size(581, 76);
            this.tabPageFile.TabIndex = 0;
            this.tabPageFile.Text = "FILE";
            this.tabPageFile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.pnlRecycle, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(575, 70);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // pnlRecycle
            // 
            this.pnlRecycle.Controls.Add(this.checkBoxContinuousRecycle);
            this.pnlRecycle.Controls.Add(this.buttonFolder);
            this.pnlRecycle.Controls.Add(this.buttonFile);
            this.pnlRecycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlRecycle.Location = new System.Drawing.Point(3, 3);
            this.pnlRecycle.Name = "pnlRecycle";
            this.pnlRecycle.Size = new System.Drawing.Size(569, 64);
            this.pnlRecycle.TabIndex = 3;
            this.pnlRecycle.Text = "OFFLINE RECYCLE OPTIONS";
            // 
            // buttonFile
            // 
            this.buttonFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonFile.FlatAppearance.BorderSize = 0;
            this.buttonFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFile.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.cameraPhoto;
            this.buttonFile.Location = new System.Drawing.Point(106, 0);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(53, 64);
            this.buttonFile.TabIndex = 1;
            this.toolTipCmd.SetToolTip(this.buttonFile, "Select a single file");
            this.buttonFile.UseVisualStyleBackColor = true;
            // 
            // buttonFolder
            // 
            this.buttonFolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonFolder.FlatAppearance.BorderSize = 0;
            this.buttonFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFolder.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.folderImage;
            this.buttonFolder.Location = new System.Drawing.Point(53, 0);
            this.buttonFolder.Name = "buttonFolder";
            this.buttonFolder.Size = new System.Drawing.Size(53, 64);
            this.buttonFolder.TabIndex = 1;
            this.toolTipCmd.SetToolTip(this.buttonFolder, "Select a folder");
            this.buttonFolder.UseVisualStyleBackColor = true;
            // 
            // checkBoxContinuousRecycle
            // 
            this.checkBoxContinuousRecycle.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxContinuousRecycle.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxContinuousRecycle.FlatAppearance.BorderSize = 0;
            this.checkBoxContinuousRecycle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxContinuousRecycle.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mail_mark_junk;
            this.checkBoxContinuousRecycle.Location = new System.Drawing.Point(0, 0);
            this.checkBoxContinuousRecycle.Name = "checkBoxContinuousRecycle";
            this.checkBoxContinuousRecycle.Size = new System.Drawing.Size(53, 64);
            this.checkBoxContinuousRecycle.TabIndex = 2;
            this.toolTipCmd.SetToolTip(this.checkBoxContinuousRecycle, "Set if you want continuous recycle");
            this.checkBoxContinuousRecycle.UseVisualStyleBackColor = true;
            // 
            // tabPagePlay
            // 
            this.tabPagePlay.Controls.Add(this.tableLayoutPanel6);
            this.tabPagePlay.ImageKey = "applications-multimedia.png";
            this.tabPagePlay.Location = new System.Drawing.Point(4, 4);
            this.tabPagePlay.Name = "tabPagePlay";
            this.tabPagePlay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlay.Size = new System.Drawing.Size(581, 76);
            this.tabPagePlay.TabIndex = 2;
            this.tabPagePlay.Text = "PLAY";
            this.tabPagePlay.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel6.Controls.Add(this.pnlMediaCmds, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(575, 70);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // pnlMediaCmds
            // 
            this.pnlMediaCmds.Controls.Add(this.pnlFps);
            this.pnlMediaCmds.Controls.Add(this.radioButtonRec);
            this.pnlMediaCmds.Controls.Add(this.radioButtonNext);
            this.pnlMediaCmds.Controls.Add(this.radioButtonPrev);
            this.pnlMediaCmds.Controls.Add(this.radioButtonStop);
            this.pnlMediaCmds.Controls.Add(this.radioButtonPause);
            this.pnlMediaCmds.Controls.Add(this.radioButtonPlay);
            this.pnlMediaCmds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMediaCmds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlMediaCmds.Location = new System.Drawing.Point(3, 3);
            this.pnlMediaCmds.Name = "pnlMediaCmds";
            this.pnlMediaCmds.Size = new System.Drawing.Size(569, 64);
            this.pnlMediaCmds.TabIndex = 0;
            this.pnlMediaCmds.Text = "MEDIA COMMANDS";
            // 
            // pnlFps
            // 
            this.pnlFps.Controls.Add(this.buttonFpsDec);
            this.pnlFps.Controls.Add(this.buttonFpsInc);
            this.pnlFps.Controls.Add(this.labelFps);
            this.pnlFps.Controls.Add(this.ntbFps);
            this.pnlFps.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFps.Location = new System.Drawing.Point(318, 0);
            this.pnlFps.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.pnlFps.Name = "pnlFps";
            this.pnlFps.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.pnlFps.Size = new System.Drawing.Size(236, 64);
            this.pnlFps.TabIndex = 15;
            // 
            // buttonFpsDec
            // 
            this.buttonFpsDec.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonFpsDec.BackgroundImage")));
            this.buttonFpsDec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonFpsDec.FlatAppearance.BorderSize = 0;
            this.buttonFpsDec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFpsDec.Location = new System.Drawing.Point(12, 18);
            this.buttonFpsDec.Name = "buttonFpsDec";
            this.buttonFpsDec.Size = new System.Drawing.Size(53, 46);
            this.buttonFpsDec.TabIndex = 13;
            this.buttonFpsDec.UseVisualStyleBackColor = true;
            // 
            // buttonFpsInc
            // 
            this.buttonFpsInc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonFpsInc.BackgroundImage")));
            this.buttonFpsInc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonFpsInc.FlatAppearance.BorderSize = 0;
            this.buttonFpsInc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFpsInc.Location = new System.Drawing.Point(163, 15);
            this.buttonFpsInc.Name = "buttonFpsInc";
            this.buttonFpsInc.Size = new System.Drawing.Size(53, 46);
            this.buttonFpsInc.TabIndex = 12;
            this.buttonFpsInc.UseVisualStyleBackColor = true;
            // 
            // labelFps
            // 
            this.labelFps.AutoSize = true;
            this.labelFps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFps.Location = new System.Drawing.Point(81, 6);
            this.labelFps.Name = "labelFps";
            this.labelFps.Size = new System.Drawing.Size(34, 16);
            this.labelFps.TabIndex = 11;
            this.labelFps.Text = "Fps";
            // 
            // ntbFps
            // 
            this.ntbFps.AutoValidate = true;
            this.ntbFps.AutoValidationTime = 5000;
            this.ntbFps.BackColor = System.Drawing.Color.White;
            this.ntbFps.DecimalPlaces = 0;
            this.ntbFps.EnableErrorValue = false;
            this.ntbFps.EnableWarningValue = false;
            this.ntbFps.ErrorColor = System.Drawing.Color.OrangeRed;
            this.ntbFps.ErrorValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbFps.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ntbFps.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbFps.InterceptArrowKeys = true;
            this.ntbFps.Location = new System.Drawing.Point(72, 24);
            this.ntbFps.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ntbFps.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbFps.Name = "ntbFps";
            this.ntbFps.ReadOnly = true;
            this.ntbFps.Size = new System.Drawing.Size(84, 33);
            this.ntbFps.TabIndex = 14;
            this.ntbFps.Text = "100";
            this.ntbFps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbFps.ThousandsSeparator = false;
            this.ntbFps.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ntbFps.WarningColor = System.Drawing.Color.Gold;
            this.ntbFps.WarningValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // radioButtonRec
            // 
            this.radioButtonRec.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonRec.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonRec.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonRec.Enabled = false;
            this.radioButtonRec.FlatAppearance.BorderSize = 0;
            this.radioButtonRec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonRec.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaRecord;
            this.radioButtonRec.Location = new System.Drawing.Point(265, 0);
            this.radioButtonRec.Name = "radioButtonRec";
            this.radioButtonRec.Padding = new System.Windows.Forms.Padding(5);
            this.radioButtonRec.Size = new System.Drawing.Size(53, 64);
            this.radioButtonRec.TabIndex = 5;
            this.radioButtonRec.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonRec, "Rec");
            this.radioButtonRec.UseVisualStyleBackColor = false;
            this.radioButtonRec.Visible = false;
            // 
            // radioButtonNext
            // 
            this.radioButtonNext.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonNext.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonNext.FlatAppearance.BorderSize = 0;
            this.radioButtonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonNext.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaSkipForward;
            this.radioButtonNext.Location = new System.Drawing.Point(212, 0);
            this.radioButtonNext.Name = "radioButtonNext";
            this.radioButtonNext.Padding = new System.Windows.Forms.Padding(5);
            this.radioButtonNext.Size = new System.Drawing.Size(53, 64);
            this.radioButtonNext.TabIndex = 10;
            this.radioButtonNext.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonNext, "Next");
            this.radioButtonNext.UseVisualStyleBackColor = true;
            // 
            // radioButtonPrev
            // 
            this.radioButtonPrev.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonPrev.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonPrev.FlatAppearance.BorderSize = 0;
            this.radioButtonPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPrev.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaSkipBackward;
            this.radioButtonPrev.Location = new System.Drawing.Point(159, 0);
            this.radioButtonPrev.Name = "radioButtonPrev";
            this.radioButtonPrev.Padding = new System.Windows.Forms.Padding(5);
            this.radioButtonPrev.Size = new System.Drawing.Size(53, 64);
            this.radioButtonPrev.TabIndex = 9;
            this.radioButtonPrev.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonPrev, "Prev");
            this.radioButtonPrev.UseVisualStyleBackColor = true;
            // 
            // radioButtonStop
            // 
            this.radioButtonStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonStop.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonStop.FlatAppearance.BorderSize = 0;
            this.radioButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonStop.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaPlaybackStop;
            this.radioButtonStop.Location = new System.Drawing.Point(106, 0);
            this.radioButtonStop.Name = "radioButtonStop";
            this.radioButtonStop.Padding = new System.Windows.Forms.Padding(5);
            this.radioButtonStop.Size = new System.Drawing.Size(53, 64);
            this.radioButtonStop.TabIndex = 8;
            this.radioButtonStop.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonStop, "Stop");
            this.radioButtonStop.UseVisualStyleBackColor = true;
            // 
            // radioButtonPause
            // 
            this.radioButtonPause.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonPause.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonPause.FlatAppearance.BorderSize = 0;
            this.radioButtonPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPause.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaPlaybackPause;
            this.radioButtonPause.Location = new System.Drawing.Point(53, 0);
            this.radioButtonPause.Name = "radioButtonPause";
            this.radioButtonPause.Padding = new System.Windows.Forms.Padding(5);
            this.radioButtonPause.Size = new System.Drawing.Size(53, 64);
            this.radioButtonPause.TabIndex = 7;
            this.radioButtonPause.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonPause, "Pause");
            this.radioButtonPause.UseVisualStyleBackColor = true;
            // 
            // radioButtonPlay
            // 
            this.radioButtonPlay.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonPlay.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonPlay.FlatAppearance.BorderSize = 0;
            this.radioButtonPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPlay.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaPlaybackStart;
            this.radioButtonPlay.Location = new System.Drawing.Point(0, 0);
            this.radioButtonPlay.Name = "radioButtonPlay";
            this.radioButtonPlay.Padding = new System.Windows.Forms.Padding(5);
            this.radioButtonPlay.Size = new System.Drawing.Size(53, 64);
            this.radioButtonPlay.TabIndex = 6;
            this.radioButtonPlay.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonPlay, "Play");
            this.radioButtonPlay.UseVisualStyleBackColor = true;
            // 
            // tabPageZoom
            // 
            this.tabPageZoom.Controls.Add(this.tableLayoutPanel5);
            this.tabPageZoom.ImageKey = "zoom-fit-best.png";
            this.tabPageZoom.Location = new System.Drawing.Point(4, 4);
            this.tabPageZoom.Name = "tabPageZoom";
            this.tabPageZoom.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageZoom.Size = new System.Drawing.Size(581, 76);
            this.tabPageZoom.TabIndex = 4;
            this.tabPageZoom.Text = "ZOOM";
            this.tabPageZoom.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.pnlZoomMode, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(575, 70);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // pnlZoomMode
            // 
            this.pnlZoomMode.Controls.Add(this.radioButtonZoomOff);
            this.pnlZoomMode.Controls.Add(this.radioButtonZoomOrig);
            this.pnlZoomMode.Controls.Add(this.radioButtonZoomStretch);
            this.pnlZoomMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlZoomMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlZoomMode.Location = new System.Drawing.Point(3, 3);
            this.pnlZoomMode.Name = "pnlZoomMode";
            this.pnlZoomMode.Size = new System.Drawing.Size(569, 64);
            this.pnlZoomMode.TabIndex = 1;
            this.pnlZoomMode.Text = "ZOOM MODE";
            // 
            // radioButtonZoomOff
            // 
            this.radioButtonZoomOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonZoomOff.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonZoomOff.FlatAppearance.BorderSize = 0;
            this.radioButtonZoomOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonZoomOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonZoomOff.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.button_fewer;
            this.radioButtonZoomOff.Location = new System.Drawing.Point(106, 0);
            this.radioButtonZoomOff.Name = "radioButtonZoomOff";
            this.radioButtonZoomOff.Size = new System.Drawing.Size(53, 64);
            this.radioButtonZoomOff.TabIndex = 2;
            this.radioButtonZoomOff.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonZoomOff, "No zoom");
            this.radioButtonZoomOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonZoomOrig
            // 
            this.radioButtonZoomOrig.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonZoomOrig.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonZoomOrig.FlatAppearance.BorderSize = 0;
            this.radioButtonZoomOrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonZoomOrig.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonZoomOrig.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.zoomOriginal;
            this.radioButtonZoomOrig.Location = new System.Drawing.Point(53, 0);
            this.radioButtonZoomOrig.Name = "radioButtonZoomOrig";
            this.radioButtonZoomOrig.Size = new System.Drawing.Size(53, 64);
            this.radioButtonZoomOrig.TabIndex = 1;
            this.radioButtonZoomOrig.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonZoomOrig, "Keep width/height ratio");
            this.radioButtonZoomOrig.UseVisualStyleBackColor = true;
            // 
            // radioButtonZoomStretch
            // 
            this.radioButtonZoomStretch.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonZoomStretch.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonZoomStretch.FlatAppearance.BorderSize = 0;
            this.radioButtonZoomStretch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonZoomStretch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonZoomStretch.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.zoomFitBest;
            this.radioButtonZoomStretch.Location = new System.Drawing.Point(0, 0);
            this.radioButtonZoomStretch.Name = "radioButtonZoomStretch";
            this.radioButtonZoomStretch.Size = new System.Drawing.Size(53, 64);
            this.radioButtonZoomStretch.TabIndex = 0;
            this.radioButtonZoomStretch.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonZoomStretch, "Stretch");
            this.radioButtonZoomStretch.UseVisualStyleBackColor = true;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.tableLayoutPanel7);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 4);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(581, 76);
            this.tabPageOptions.TabIndex = 1;
            this.tabPageOptions.Text = "OPTIONS";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.72737F));
            this.tableLayoutPanel7.Controls.Add(this.groupBoxHist, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(575, 70);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // groupBoxHist
            // 
            this.groupBoxHist.Controls.Add(this.checkBoxHexDisplay);
            this.groupBoxHist.Controls.Add(this.checkBoxBigHist);
            this.groupBoxHist.Controls.Add(this.checkBoxVerticalPixel);
            this.groupBoxHist.Controls.Add(this.checkBoxHorizontalPixel);
            this.groupBoxHist.Controls.Add(this.checkBoxViewInfoPixel);
            this.groupBoxHist.Controls.Add(this.radioButtonInfo);
            this.groupBoxHist.Controls.Add(this.radioButtonHist);
            this.groupBoxHist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxHist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxHist.Location = new System.Drawing.Point(3, 3);
            this.groupBoxHist.Name = "groupBoxHist";
            this.groupBoxHist.Size = new System.Drawing.Size(569, 64);
            this.groupBoxHist.TabIndex = 1;
            this.groupBoxHist.TabStop = false;
            this.groupBoxHist.Text = "HISTOGRAM AND PIXEL INFO";
            // 
            // checkBoxHexDisplay
            // 
            this.checkBoxHexDisplay.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxHexDisplay.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.text_x_hex;
            this.checkBoxHexDisplay.Location = new System.Drawing.Point(243, 19);
            this.checkBoxHexDisplay.Name = "checkBoxHexDisplay";
            this.checkBoxHexDisplay.Size = new System.Drawing.Size(53, 44);
            this.checkBoxHexDisplay.TabIndex = 8;
            this.toolTipCmd.SetToolTip(this.checkBoxHexDisplay, "Show hex values");
            this.checkBoxHexDisplay.UseVisualStyleBackColor = true;
            this.checkBoxHexDisplay.CheckedChanged += new System.EventHandler(this.checkBoxHexDisplay_CheckedChanged);
            // 
            // checkBoxBigHist
            // 
            this.checkBoxBigHist.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxBigHist.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.xloadZoom;
            this.checkBoxBigHist.Location = new System.Drawing.Point(123, 19);
            this.checkBoxBigHist.Name = "checkBoxBigHist";
            this.checkBoxBigHist.Size = new System.Drawing.Size(53, 44);
            this.checkBoxBigHist.TabIndex = 7;
            this.toolTipCmd.SetToolTip(this.checkBoxBigHist, "Show pixel value matrix");
            this.checkBoxBigHist.UseVisualStyleBackColor = true;
            // 
            // checkBoxVerticalPixel
            // 
            this.checkBoxVerticalPixel.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxVerticalPixel.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.insertVerticalRule;
            this.checkBoxVerticalPixel.Location = new System.Drawing.Point(303, 19);
            this.checkBoxVerticalPixel.Name = "checkBoxVerticalPixel";
            this.checkBoxVerticalPixel.Size = new System.Drawing.Size(53, 44);
            this.checkBoxVerticalPixel.TabIndex = 6;
            this.toolTipCmd.SetToolTip(this.checkBoxVerticalPixel, "Show vertical values (same X pixel selected)");
            this.checkBoxVerticalPixel.UseVisualStyleBackColor = true;
            // 
            // checkBoxHorizontalPixel
            // 
            this.checkBoxHorizontalPixel.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxHorizontalPixel.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.insertHorizontalRule;
            this.checkBoxHorizontalPixel.Location = new System.Drawing.Point(363, 19);
            this.checkBoxHorizontalPixel.Name = "checkBoxHorizontalPixel";
            this.checkBoxHorizontalPixel.Size = new System.Drawing.Size(53, 44);
            this.checkBoxHorizontalPixel.TabIndex = 5;
            this.toolTipCmd.SetToolTip(this.checkBoxHorizontalPixel, "Show horizontal values (same Y pixel selected)");
            this.checkBoxHorizontalPixel.UseVisualStyleBackColor = true;
            // 
            // checkBoxViewInfoPixel
            // 
            this.checkBoxViewInfoPixel.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxViewInfoPixel.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.insertTable;
            this.checkBoxViewInfoPixel.Location = new System.Drawing.Point(183, 19);
            this.checkBoxViewInfoPixel.Name = "checkBoxViewInfoPixel";
            this.checkBoxViewInfoPixel.Size = new System.Drawing.Size(53, 44);
            this.checkBoxViewInfoPixel.TabIndex = 4;
            this.toolTipCmd.SetToolTip(this.checkBoxViewInfoPixel, "Show pixel values matrix");
            this.checkBoxViewInfoPixel.UseVisualStyleBackColor = true;
            // 
            // radioButtonInfo
            // 
            this.radioButtonInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonInfo.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.text;
            this.radioButtonInfo.Location = new System.Drawing.Point(63, 19);
            this.radioButtonInfo.Name = "radioButtonInfo";
            this.radioButtonInfo.Size = new System.Drawing.Size(53, 44);
            this.radioButtonInfo.TabIndex = 2;
            this.radioButtonInfo.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonInfo, "Show info about zoom ROI");
            this.radioButtonInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonHist
            // 
            this.radioButtonHist.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonHist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonHist.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.xload;
            this.radioButtonHist.Location = new System.Drawing.Point(3, 19);
            this.radioButtonHist.Name = "radioButtonHist";
            this.radioButtonHist.Size = new System.Drawing.Size(53, 44);
            this.radioButtonHist.TabIndex = 1;
            this.radioButtonHist.TabStop = true;
            this.toolTipCmd.SetToolTip(this.radioButtonHist, "Show embedded histogram");
            this.radioButtonHist.UseVisualStyleBackColor = true;
            // 
            // imageListTabMenu
            // 
            this.imageListTabMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabMenu.ImageStream")));
            this.imageListTabMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabMenu.Images.SetKeyName(0, "drive-harddisk.png");
            this.imageListTabMenu.Images.SetKeyName(1, "camera-web.png");
            this.imageListTabMenu.Images.SetKeyName(2, "applications-multimedia.png");
            this.imageListTabMenu.Images.SetKeyName(3, "zoom-fit-best.png");
            // 
            // timerBlink
            // 
            this.timerBlink.Interval = 500;
            // 
            // ImageViewer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(595, 551);
            this.VisibleChanged += new System.EventHandler(this.ImageViewer_VisibleChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImageViewer_DragDrop);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.customTabControlMenu.ResumeLayout(false);
            this.tabPageFile.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlRecycle.ResumeLayout(false);
            this.tabPagePlay.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlMediaCmds.ResumeLayout(false);
            this.pnlFps.ResumeLayout(false);
            this.pnlFps.PerformLayout();
            this.tabPageZoom.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlZoomMode.ResumeLayout(false);
            this.tabPageOptions.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.groupBoxHist.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Emgu.CV.UI.ImageBox imageBox1;
        private Emgu.CV.UI.ImageBox imageBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TabPage tabPageFile;
        private System.Windows.Forms.Button buttonFolder;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.TabPage tabPagePlay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton radioButtonNext;
        private System.Windows.Forms.RadioButton radioButtonPrev;
        private System.Windows.Forms.RadioButton radioButtonStop;
        private System.Windows.Forms.RadioButton radioButtonPause;
        private System.Windows.Forms.RadioButton radioButtonPlay;
        private System.Windows.Forms.RadioButton radioButtonRec;
        private System.Windows.Forms.Timer timerBlink;
        private System.Windows.Forms.ImageList imageListTabMenu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.CheckBox checkBoxContinuousRecycle;
        private System.Windows.Forms.GroupBox groupBoxHist;
        private System.Windows.Forms.RadioButton radioButtonHist;
        private System.Windows.Forms.RadioButton radioButtonInfo;
        private System.Windows.Forms.RichTextBox richTextBoxGenInfo;
        private System.Windows.Forms.CheckBox checkBoxViewInfoPixel;
        private System.Windows.Forms.CheckBox checkBoxHorizontalPixel;
        private System.Windows.Forms.CheckBox checkBoxVerticalPixel;
        private System.Windows.Forms.TabPage tabPageZoom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Panel pnlZoomMode;
        private System.Windows.Forms.RadioButton radioButtonZoomOff;
        private System.Windows.Forms.RadioButton radioButtonZoomOrig;
        private System.Windows.Forms.RadioButton radioButtonZoomStretch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel pnlRecycle;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Panel pnlMediaCmds;
        private System.Windows.Forms.ToolTip toolTipCmd;
        private System.Windows.Forms.CheckBox checkBoxBigHist;
        private System.Windows.Forms.CheckBox checkBoxHexDisplay;
        private System.Windows.Forms.Button buttonFpsDec;
        private System.Windows.Forms.Button buttonFpsInc;
        private System.Windows.Forms.Label labelFps;
        private System.Windows.Forms.CustomTabControl customTabControlMenu;
        private System.Windows.Forms.Panel pnlFps;
        private System.Windows.Forms.NumericTextBox ntbFps;
    }
}
