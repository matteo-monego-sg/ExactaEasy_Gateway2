namespace machineConfiguration
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewErrors = new System.Windows.Forms.DataGridView();
            this.subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.labelNErrors = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonSaveDumpImg = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.panelBtn = new System.Windows.Forms.Panel();
            this.buttonMenHistoricizedTools = new System.Windows.Forms.Button();
            this.buttonMenDisplay = new System.Windows.Forms.Button();
            this.buttonMenCamera = new System.Windows.Forms.Button();
            this.buttonMenStation = new System.Windows.Forms.Button();
            this.buttonMenNode = new System.Windows.Forms.Button();
            this.buttonMenGeneral = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panelComand = new System.Windows.Forms.Panel();
            this.buttonDeleteStationNamesToNodes = new System.Windows.Forms.Button();
            this.buttonAddStationNameToNode = new System.Windows.Forms.Button();
            this.buttonAutoCompleteId = new System.Windows.Forms.Button();
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.panelDysp = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCreateFromStation = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.mngDisplay1 = new machineConfiguration.MngDisplay();
            this.mngCamera1 = new machineConfiguration.MngCamera();
            this.mngGeneral1 = new machineConfiguration.MngGeneral();
            this.mngStation1 = new machineConfiguration.MngStation();
            this.mngNode1 = new machineConfiguration.MngNode();
            this.mngHistoricizedTools1 = new machineConfiguration.MngHistoricizedTools();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewErrors)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelBtn.SuspendLayout();
            this.panelComand.SuspendLayout();
            this.panelDysp.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridViewErrors);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1081, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 594);
            this.panel1.TabIndex = 6;
            // 
            // dataGridViewErrors
            // 
            this.dataGridViewErrors.AllowUserToAddRows = false;
            this.dataGridViewErrors.AllowUserToDeleteRows = false;
            this.dataGridViewErrors.AllowUserToResizeColumns = false;
            this.dataGridViewErrors.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Control;
            this.dataGridViewErrors.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewErrors.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridViewErrors.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.dataGridViewErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewErrors.ColumnHeadersVisible = false;
            this.dataGridViewErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.subject,
            this.message});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewErrors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewErrors.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewErrors.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridViewErrors.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewErrors.MultiSelect = false;
            this.dataGridViewErrors.Name = "dataGridViewErrors";
            this.dataGridViewErrors.ReadOnly = true;
            this.dataGridViewErrors.RowHeadersVisible = false;
            this.dataGridViewErrors.Size = new System.Drawing.Size(383, 385);
            this.dataGridViewErrors.TabIndex = 8;
            this.dataGridViewErrors.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewErrors_CellClick);
            // 
            // subject
            // 
            this.subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.subject.FillWeight = 30F;
            this.subject.HeaderText = "";
            this.subject.Name = "subject";
            this.subject.ReadOnly = true;
            // 
            // message
            // 
            this.message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.message.HeaderText = "";
            this.message.Name = "message";
            this.message.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.buttonCheck);
            this.panel2.Controls.Add(this.labelNErrors);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 385);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(383, 27);
            this.panel2.TabIndex = 7;
            // 
            // buttonCheck
            // 
            this.buttonCheck.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCheck.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonCheck.FlatAppearance.BorderSize = 2;
            this.buttonCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCheck.Location = new System.Drawing.Point(288, 0);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(93, 25);
            this.buttonCheck.TabIndex = 1;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // labelNErrors
            // 
            this.labelNErrors.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelNErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNErrors.Location = new System.Drawing.Point(0, 0);
            this.labelNErrors.Name = "labelNErrors";
            this.labelNErrors.Size = new System.Drawing.Size(101, 25);
            this.labelNErrors.TabIndex = 0;
            this.labelNErrors.Text = "Errors: 0";
            this.labelNErrors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.buttonSaveDumpImg);
            this.panel4.Controls.Add(this.buttonNew);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.buttonSave);
            this.panel4.Controls.Add(this.buttonOpen);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.ForeColor = System.Drawing.SystemColors.Control;
            this.panel4.Location = new System.Drawing.Point(0, 412);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(383, 182);
            this.panel4.TabIndex = 6;
            // 
            // buttonSaveDumpImg
            // 
            this.buttonSaveDumpImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveDumpImg.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonSaveDumpImg.FlatAppearance.BorderSize = 2;
            this.buttonSaveDumpImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveDumpImg.Location = new System.Drawing.Point(144, 143);
            this.buttonSaveDumpImg.Name = "buttonSaveDumpImg";
            this.buttonSaveDumpImg.Size = new System.Drawing.Size(138, 34);
            this.buttonSaveDumpImg.TabIndex = 10;
            this.buttonSaveDumpImg.Text = "Save DumpImages";
            this.buttonSaveDumpImg.UseVisualStyleBackColor = true;
            this.buttonSaveDumpImg.Click += new System.EventHandler(this.buttonSaveDumpImg_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonNew.FlatAppearance.BorderSize = 2;
            this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNew.Location = new System.Drawing.Point(94, 143);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(44, 34);
            this.buttonNew.TabIndex = 9;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::machineConfiguration.Properties.Resources.SG_OneEngineering;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(373, 132);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonSave.FlatAppearance.BorderSize = 2;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Location = new System.Drawing.Point(288, 143);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(90, 34);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOpen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonOpen.FlatAppearance.BorderSize = 2;
            this.buttonOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpen.Location = new System.Drawing.Point(5, 143);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(83, 34);
            this.buttonOpen.TabIndex = 6;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // panelBtn
            // 
            this.panelBtn.Controls.Add(this.buttonMenHistoricizedTools);
            this.panelBtn.Controls.Add(this.buttonMenDisplay);
            this.panelBtn.Controls.Add(this.buttonMenCamera);
            this.panelBtn.Controls.Add(this.buttonMenStation);
            this.panelBtn.Controls.Add(this.buttonMenNode);
            this.panelBtn.Controls.Add(this.buttonMenGeneral);
            this.panelBtn.Controls.Add(this.panel5);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBtn.Location = new System.Drawing.Point(0, 551);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(1081, 43);
            this.panelBtn.TabIndex = 7;
            // 
            // buttonMenHistoricizedTools
            // 
            this.buttonMenHistoricizedTools.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMenHistoricizedTools.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMenHistoricizedTools.FlatAppearance.BorderSize = 2;
            this.buttonMenHistoricizedTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenHistoricizedTools.Location = new System.Drawing.Point(685, 7);
            this.buttonMenHistoricizedTools.Name = "buttonMenHistoricizedTools";
            this.buttonMenHistoricizedTools.Size = new System.Drawing.Size(137, 36);
            this.buttonMenHistoricizedTools.TabIndex = 19;
            this.buttonMenHistoricizedTools.Text = "HISTORICIZED TOOLS";
            this.buttonMenHistoricizedTools.UseVisualStyleBackColor = true;
            this.buttonMenHistoricizedTools.Click += new System.EventHandler(this.buttonMenHistoricisedTools_Click);
            // 
            // buttonMenDisplay
            // 
            this.buttonMenDisplay.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMenDisplay.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMenDisplay.FlatAppearance.BorderSize = 2;
            this.buttonMenDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenDisplay.Location = new System.Drawing.Point(548, 7);
            this.buttonMenDisplay.Name = "buttonMenDisplay";
            this.buttonMenDisplay.Size = new System.Drawing.Size(137, 36);
            this.buttonMenDisplay.TabIndex = 12;
            this.buttonMenDisplay.Text = "DISPLAY";
            this.buttonMenDisplay.UseVisualStyleBackColor = true;
            this.buttonMenDisplay.Click += new System.EventHandler(this.buttonMenDisplay_Click);
            // 
            // buttonMenCamera
            // 
            this.buttonMenCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMenCamera.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMenCamera.FlatAppearance.BorderSize = 2;
            this.buttonMenCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenCamera.Location = new System.Drawing.Point(411, 7);
            this.buttonMenCamera.Name = "buttonMenCamera";
            this.buttonMenCamera.Size = new System.Drawing.Size(137, 36);
            this.buttonMenCamera.TabIndex = 16;
            this.buttonMenCamera.Text = "CAMERA";
            this.buttonMenCamera.UseVisualStyleBackColor = true;
            this.buttonMenCamera.Click += new System.EventHandler(this.buttonMenCamera_Click);
            // 
            // buttonMenStation
            // 
            this.buttonMenStation.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMenStation.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMenStation.FlatAppearance.BorderSize = 2;
            this.buttonMenStation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenStation.Location = new System.Drawing.Point(274, 7);
            this.buttonMenStation.Name = "buttonMenStation";
            this.buttonMenStation.Size = new System.Drawing.Size(137, 36);
            this.buttonMenStation.TabIndex = 15;
            this.buttonMenStation.Text = "STATION";
            this.buttonMenStation.UseVisualStyleBackColor = true;
            this.buttonMenStation.Click += new System.EventHandler(this.buttonMenStation_Click);
            // 
            // buttonMenNode
            // 
            this.buttonMenNode.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMenNode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMenNode.FlatAppearance.BorderSize = 2;
            this.buttonMenNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenNode.Location = new System.Drawing.Point(137, 7);
            this.buttonMenNode.Name = "buttonMenNode";
            this.buttonMenNode.Size = new System.Drawing.Size(137, 36);
            this.buttonMenNode.TabIndex = 14;
            this.buttonMenNode.Text = "NODE";
            this.buttonMenNode.UseVisualStyleBackColor = true;
            this.buttonMenNode.Click += new System.EventHandler(this.buttonMenNode_Click);
            // 
            // buttonMenGeneral
            // 
            this.buttonMenGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonMenGeneral.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMenGeneral.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMenGeneral.FlatAppearance.BorderSize = 2;
            this.buttonMenGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenGeneral.Location = new System.Drawing.Point(0, 7);
            this.buttonMenGeneral.Name = "buttonMenGeneral";
            this.buttonMenGeneral.Size = new System.Drawing.Size(137, 36);
            this.buttonMenGeneral.TabIndex = 13;
            this.buttonMenGeneral.Text = "GENERAL";
            this.buttonMenGeneral.UseVisualStyleBackColor = false;
            this.buttonMenGeneral.Click += new System.EventHandler(this.buttonMenGeneral_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1081, 7);
            this.panel5.TabIndex = 17;
            // 
            // panelComand
            // 
            this.panelComand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelComand.Controls.Add(this.buttonDeleteStationNamesToNodes);
            this.panelComand.Controls.Add(this.buttonAddStationNameToNode);
            this.panelComand.Controls.Add(this.buttonAutoCompleteId);
            this.panelComand.Controls.Add(this.buttonDeleteAll);
            this.panelComand.Controls.Add(this.panelDysp);
            this.panelComand.Controls.Add(this.buttonCreateFromStation);
            this.panelComand.Controls.Add(this.buttonMoveDown);
            this.panelComand.Controls.Add(this.buttonMoveUp);
            this.panelComand.Controls.Add(this.buttonDelete);
            this.panelComand.Controls.Add(this.buttonAdd);
            this.panelComand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelComand.Location = new System.Drawing.Point(0, 429);
            this.panelComand.Name = "panelComand";
            this.panelComand.Size = new System.Drawing.Size(1081, 122);
            this.panelComand.TabIndex = 8;
            // 
            // buttonDeleteStationNamesToNodes
            // 
            this.buttonDeleteStationNamesToNodes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonDeleteStationNamesToNodes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonDeleteStationNamesToNodes.FlatAppearance.BorderSize = 2;
            this.buttonDeleteStationNamesToNodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteStationNamesToNodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeleteStationNamesToNodes.Location = new System.Drawing.Point(197, 79);
            this.buttonDeleteStationNamesToNodes.Name = "buttonDeleteStationNamesToNodes";
            this.buttonDeleteStationNamesToNodes.Size = new System.Drawing.Size(180, 31);
            this.buttonDeleteStationNamesToNodes.TabIndex = 15;
            this.buttonDeleteStationNamesToNodes.Text = "Remove station names";
            this.buttonDeleteStationNamesToNodes.UseVisualStyleBackColor = false;
            this.buttonDeleteStationNamesToNodes.Click += new System.EventHandler(this.buttonDeleteStationNamesToNodes_Click);
            // 
            // buttonAddStationNameToNode
            // 
            this.buttonAddStationNameToNode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonAddStationNameToNode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonAddStationNameToNode.FlatAppearance.BorderSize = 2;
            this.buttonAddStationNameToNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddStationNameToNode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddStationNameToNode.Location = new System.Drawing.Point(197, 42);
            this.buttonAddStationNameToNode.Name = "buttonAddStationNameToNode";
            this.buttonAddStationNameToNode.Size = new System.Drawing.Size(180, 31);
            this.buttonAddStationNameToNode.TabIndex = 14;
            this.buttonAddStationNameToNode.Text = "Add/Set station names";
            this.buttonAddStationNameToNode.UseVisualStyleBackColor = false;
            this.buttonAddStationNameToNode.Click += new System.EventHandler(this.buttonAddStationNameToNode_Click);
            // 
            // buttonAutoCompleteId
            // 
            this.buttonAutoCompleteId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAutoCompleteId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonAutoCompleteId.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonAutoCompleteId.FlatAppearance.BorderSize = 2;
            this.buttonAutoCompleteId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAutoCompleteId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAutoCompleteId.Location = new System.Drawing.Point(894, 42);
            this.buttonAutoCompleteId.Name = "buttonAutoCompleteId";
            this.buttonAutoCompleteId.Size = new System.Drawing.Size(180, 31);
            this.buttonAutoCompleteId.TabIndex = 13;
            this.buttonAutoCompleteId.Text = "Auto Complete Id";
            this.buttonAutoCompleteId.UseVisualStyleBackColor = false;
            this.buttonAutoCompleteId.Click += new System.EventHandler(this.buttonAutoCompleteId_Click);
            // 
            // buttonDeleteAll
            // 
            this.buttonDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonDeleteAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonDeleteAll.FlatAppearance.BorderSize = 2;
            this.buttonDeleteAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeleteAll.Location = new System.Drawing.Point(894, 79);
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Size = new System.Drawing.Size(180, 31);
            this.buttonDeleteAll.TabIndex = 12;
            this.buttonDeleteAll.Text = "Delete All";
            this.buttonDeleteAll.UseVisualStyleBackColor = false;
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // panelDysp
            // 
            this.panelDysp.Controls.Add(this.tableLayoutPanel1);
            this.panelDysp.Controls.Add(this.tableLayoutPanel6);
            this.panelDysp.Controls.Add(this.tableLayoutPanel2);
            this.panelDysp.Controls.Add(this.tableLayoutPanel5);
            this.panelDysp.Controls.Add(this.tableLayoutPanel3);
            this.panelDysp.Controls.Add(this.tableLayoutPanel4);
            this.panelDysp.Location = new System.Drawing.Point(258, 5);
            this.panelDysp.Name = "panelDysp";
            this.panelDysp.Size = new System.Drawing.Size(397, 37);
            this.panelDysp.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(60, 30);
            this.tableLayoutPanel1.TabIndex = 5;
            this.tableLayoutPanel1.Click += new System.EventHandler(this.tableLayoutPanel1_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.Location = new System.Drawing.Point(333, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(60, 30);
            this.tableLayoutPanel6.TabIndex = 10;
            this.tableLayoutPanel6.Click += new System.EventHandler(this.tableLayoutPanel6_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(69, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(60, 30);
            this.tableLayoutPanel2.TabIndex = 6;
            this.tableLayoutPanel2.Click += new System.EventHandler(this.tableLayoutPanel2_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Location = new System.Drawing.Point(267, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(60, 30);
            this.tableLayoutPanel5.TabIndex = 9;
            this.tableLayoutPanel5.Click += new System.EventHandler(this.tableLayoutPanel5_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Location = new System.Drawing.Point(135, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(60, 30);
            this.tableLayoutPanel3.TabIndex = 7;
            this.tableLayoutPanel3.Click += new System.EventHandler(this.tableLayoutPanel3_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Location = new System.Drawing.Point(201, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(60, 30);
            this.tableLayoutPanel4.TabIndex = 8;
            this.tableLayoutPanel4.Click += new System.EventHandler(this.tableLayoutPanel4_Click);
            // 
            // buttonCreateFromStation
            // 
            this.buttonCreateFromStation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateFromStation.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonCreateFromStation.FlatAppearance.BorderSize = 2;
            this.buttonCreateFromStation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateFromStation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCreateFromStation.Location = new System.Drawing.Point(894, 5);
            this.buttonCreateFromStation.Name = "buttonCreateFromStation";
            this.buttonCreateFromStation.Size = new System.Drawing.Size(180, 31);
            this.buttonCreateFromStation.TabIndex = 4;
            this.buttonCreateFromStation.Text = "Create From Stations";
            this.buttonCreateFromStation.UseVisualStyleBackColor = true;
            this.buttonCreateFromStation.Click += new System.EventHandler(this.buttonCreateFromStation_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMoveDown.FlatAppearance.BorderSize = 2;
            this.buttonMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMoveDown.Location = new System.Drawing.Point(111, 79);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(80, 31);
            this.buttonMoveDown.TabIndex = 3;
            this.buttonMoveDown.Text = "Move Down";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonMoveUp.FlatAppearance.BorderSize = 2;
            this.buttonMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMoveUp.Location = new System.Drawing.Point(11, 79);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(80, 31);
            this.buttonMoveUp.TabIndex = 2;
            this.buttonMoveUp.Text = "Move Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonDelete.FlatAppearance.BorderSize = 2;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.Location = new System.Drawing.Point(11, 42);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(180, 31);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.buttonAdd.FlatAppearance.BorderSize = 2;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(11, 5);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(180, 31);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // mngDisplay1
            // 
            this.mngDisplay1.AutoScroll = true;
            this.mngDisplay1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.mngDisplay1.Location = new System.Drawing.Point(411, 201);
            this.mngDisplay1.Name = "mngDisplay1";
            this.mngDisplay1.Size = new System.Drawing.Size(222, 155);
            this.mngDisplay1.TabIndex = 13;
            this.mngDisplay1.TypeDisplaySelected = 0;
            // 
            // mngCamera1
            // 
            this.mngCamera1.Location = new System.Drawing.Point(94, 201);
            this.mngCamera1.Name = "mngCamera1";
            this.mngCamera1.Size = new System.Drawing.Size(243, 163);
            this.mngCamera1.TabIndex = 12;
            this.mngCamera1.TypeDisplaySelected = 0;
            // 
            // mngGeneral1
            // 
            this.mngGeneral1.Location = new System.Drawing.Point(598, 25);
            this.mngGeneral1.Name = "mngGeneral1";
            this.mngGeneral1.Size = new System.Drawing.Size(245, 141);
            this.mngGeneral1.TabIndex = 11;
            this.mngGeneral1.TypeDisplaySelected = 0;
            // 
            // mngStation1
            // 
            this.mngStation1.Location = new System.Drawing.Point(318, 25);
            this.mngStation1.Name = "mngStation1";
            this.mngStation1.Size = new System.Drawing.Size(243, 155);
            this.mngStation1.TabIndex = 10;
            this.mngStation1.TypeDisplaySelected = 0;
            // 
            // mngNode1
            // 
            this.mngNode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.mngNode1.Location = new System.Drawing.Point(94, 25);
            this.mngNode1.Name = "mngNode1";
            this.mngNode1.Size = new System.Drawing.Size(195, 155);
            this.mngNode1.TabIndex = 9;
            this.mngNode1.TypeDisplaySelected = 0;
            // 
            // mngHistoricizedTools1
            // 
            this.mngHistoricizedTools1.Location = new System.Drawing.Point(726, 216);
            this.mngHistoricizedTools1.Name = "mngHistoricizedTools1";
            this.mngHistoricizedTools1.Size = new System.Drawing.Size(316, 185);
            this.mngHistoricizedTools1.TabIndex = 14;
            this.mngHistoricizedTools1.TypeDisplaySelected = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(1464, 594);
            this.Controls.Add(this.mngHistoricizedTools1);
            this.Controls.Add(this.mngDisplay1);
            this.Controls.Add(this.mngCamera1);
            this.Controls.Add(this.mngGeneral1);
            this.Controls.Add(this.mngStation1);
            this.Controls.Add(this.mngNode1);
            this.Controls.Add(this.panelComand);
            this.Controls.Add(this.panelBtn);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Machine Config Editor";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewErrors)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelBtn.ResumeLayout(false);
            this.panelComand.ResumeLayout(false);
            this.panelDysp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Label labelNErrors;
        private System.Windows.Forms.Panel panelBtn;
        private System.Windows.Forms.Button buttonMenDisplay;
        private System.Windows.Forms.Button buttonMenCamera;
        private System.Windows.Forms.Button buttonMenStation;
        private System.Windows.Forms.Button buttonMenNode;
        private System.Windows.Forms.Button buttonMenGeneral;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panelComand;
        private System.Windows.Forms.Button buttonCreateFromStation;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private MngNode mngNode1;
        private MngStation mngStation1;
        private MngGeneral mngGeneral1;
        private MngCamera mngCamera1;
        private MngDisplay mngDisplay1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelDysp;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.DataGridView dataGridViewErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn message;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonAutoCompleteId;
        private System.Windows.Forms.Button buttonAddStationNameToNode;
        private System.Windows.Forms.Button buttonDeleteStationNamesToNodes;
        private System.Windows.Forms.Button buttonSaveDumpImg;
        private System.Windows.Forms.Button buttonMenHistoricizedTools;
        private MngHistoricizedTools mngHistoricizedTools1;
    }
}

