namespace EoptisClientTester {
    partial class frmMain {
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.ipAddressCtrl = new IPAddressControlLib.IPAddressControl();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.nudWarnHighTh = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudWarnLowTh = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudErrHighTh = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudErrLowTh = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbWorkMode = new System.Windows.Forms.ComboBox();
            this.tbWorkMode = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbSetBusy = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cbReady = new System.Windows.Forms.CheckBox();
            this.cbDataValid = new System.Windows.Forms.CheckBox();
            this.cbVialId0 = new System.Windows.Forms.CheckBox();
            this.cbVialId1 = new System.Windows.Forms.CheckBox();
            this.cbAcqReady = new System.Windows.Forms.CheckBox();
            this.cbReject0 = new System.Windows.Forms.CheckBox();
            this.cbReject1 = new System.Windows.Forms.CheckBox();
            this.cbReject2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWarnHighTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWarnLowTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudErrHighTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudErrLowTh)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(18, 59);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(150, 27);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(174, 59);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(150, 27);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "DISCONNECT";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "PORT";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(218, 20);
            this.nudPort.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(106, 23);
            this.nudPort.TabIndex = 4;
            this.nudPort.Value = new decimal(new int[] {
            23456,
            0,
            0,
            0});
            // 
            // ipAddressCtrl
            // 
            this.ipAddressCtrl.AllowInternalTab = false;
            this.ipAddressCtrl.AutoHeight = true;
            this.ipAddressCtrl.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressCtrl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressCtrl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressCtrl.Location = new System.Drawing.Point(39, 19);
            this.ipAddressCtrl.MinimumSize = new System.Drawing.Size(99, 23);
            this.ipAddressCtrl.Name = "ipAddressCtrl";
            this.ipAddressCtrl.ReadOnly = false;
            this.ipAddressCtrl.Size = new System.Drawing.Size(129, 23);
            this.ipAddressCtrl.TabIndex = 5;
            this.ipAddressCtrl.Text = "10.1.1.198";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(418, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "TEST";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(96, 288);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(72, 42);
            this.btnGet.TabIndex = 7;
            this.btnGet.Text = "GET";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(174, 288);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(72, 42);
            this.btnSet.TabIndex = 8;
            this.btnSet.Text = "SET";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // nudWarnHighTh
            // 
            this.nudWarnHighTh.Location = new System.Drawing.Point(174, 142);
            this.nudWarnHighTh.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudWarnHighTh.Name = "nudWarnHighTh";
            this.nudWarnHighTh.Size = new System.Drawing.Size(131, 23);
            this.nudWarnHighTh.TabIndex = 10;
            this.nudWarnHighTh.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "WARN HIGH TH";
            // 
            // nudWarnLowTh
            // 
            this.nudWarnLowTh.Location = new System.Drawing.Point(174, 175);
            this.nudWarnLowTh.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudWarnLowTh.Name = "nudWarnLowTh";
            this.nudWarnLowTh.Size = new System.Drawing.Size(131, 23);
            this.nudWarnLowTh.TabIndex = 12;
            this.nudWarnLowTh.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "WARN LOW TH";
            // 
            // nudErrHighTh
            // 
            this.nudErrHighTh.Location = new System.Drawing.Point(174, 209);
            this.nudErrHighTh.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudErrHighTh.Name = "nudErrHighTh";
            this.nudErrHighTh.Size = new System.Drawing.Size(131, 23);
            this.nudErrHighTh.TabIndex = 14;
            this.nudErrHighTh.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "ERR HIGH TH";
            // 
            // nudErrLowTh
            // 
            this.nudErrLowTh.Location = new System.Drawing.Point(174, 242);
            this.nudErrLowTh.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudErrLowTh.Name = "nudErrLowTh";
            this.nudErrLowTh.Size = new System.Drawing.Size(131, 23);
            this.nudErrLowTh.TabIndex = 16;
            this.nudErrLowTh.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "ERR LOW TH";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "WORKING MODE";
            // 
            // cbWorkMode
            // 
            this.cbWorkMode.FormattingEnabled = true;
            this.cbWorkMode.Location = new System.Drawing.Point(378, 72);
            this.cbWorkMode.Name = "cbWorkMode";
            this.cbWorkMode.Size = new System.Drawing.Size(131, 23);
            this.cbWorkMode.TabIndex = 18;
            this.cbWorkMode.Visible = false;
            // 
            // tbWorkMode
            // 
            this.tbWorkMode.Location = new System.Drawing.Point(174, 107);
            this.tbWorkMode.Name = "tbWorkMode";
            this.tbWorkMode.ReadOnly = true;
            this.tbWorkMode.Size = new System.Drawing.Size(131, 23);
            this.tbWorkMode.TabIndex = 20;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbSetBusy
            // 
            this.cbSetBusy.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbSetBusy.Location = new System.Drawing.Point(18, 288);
            this.cbSetBusy.Name = "cbSetBusy";
            this.cbSetBusy.Size = new System.Drawing.Size(72, 42);
            this.cbSetBusy.TabIndex = 21;
            this.cbSetBusy.Text = "SET BUSY";
            this.cbSetBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbSetBusy.UseVisualStyleBackColor = true;
            this.cbSetBusy.CheckedChanged += new System.EventHandler(this.cbSetBusy_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(252, 288);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 42);
            this.button2.TabIndex = 22;
            this.button2.Text = "LIVE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 348);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 15);
            this.label8.TabIndex = 23;
            this.label8.Text = "OUTPUTs";
            // 
            // cbReady
            // 
            this.cbReady.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbReady.Location = new System.Drawing.Point(18, 375);
            this.cbReady.Name = "cbReady";
            this.cbReady.Size = new System.Drawing.Size(56, 53);
            this.cbReady.TabIndex = 24;
            this.cbReady.Text = "READY";
            this.cbReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbReady.UseVisualStyleBackColor = true;
            this.cbReady.CheckedChanged += new System.EventHandler(this.cbReady_CheckedChanged);
            // 
            // cbDataValid
            // 
            this.cbDataValid.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbDataValid.Location = new System.Drawing.Point(258, 375);
            this.cbDataValid.Name = "cbDataValid";
            this.cbDataValid.Size = new System.Drawing.Size(56, 53);
            this.cbDataValid.TabIndex = 25;
            this.cbDataValid.Text = "DATA VALID";
            this.cbDataValid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDataValid.UseVisualStyleBackColor = true;
            this.cbDataValid.CheckedChanged += new System.EventHandler(this.cbDataValid_CheckedChanged);
            // 
            // cbVialId0
            // 
            this.cbVialId0.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbVialId0.Location = new System.Drawing.Point(138, 375);
            this.cbVialId0.Name = "cbVialId0";
            this.cbVialId0.Size = new System.Drawing.Size(56, 53);
            this.cbVialId0.TabIndex = 26;
            this.cbVialId0.Text = "VIAL ID 0";
            this.cbVialId0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbVialId0.UseVisualStyleBackColor = true;
            this.cbVialId0.CheckedChanged += new System.EventHandler(this.cbVialId0_CheckedChanged);
            // 
            // cbVialId1
            // 
            this.cbVialId1.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbVialId1.Location = new System.Drawing.Point(198, 375);
            this.cbVialId1.Name = "cbVialId1";
            this.cbVialId1.Size = new System.Drawing.Size(56, 53);
            this.cbVialId1.TabIndex = 24;
            this.cbVialId1.Text = "VIAL ID 1";
            this.cbVialId1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbVialId1.UseVisualStyleBackColor = true;
            this.cbVialId1.CheckedChanged += new System.EventHandler(this.cbVialId1_CheckedChanged);
            // 
            // cbAcqReady
            // 
            this.cbAcqReady.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAcqReady.Location = new System.Drawing.Point(78, 375);
            this.cbAcqReady.Name = "cbAcqReady";
            this.cbAcqReady.Size = new System.Drawing.Size(56, 53);
            this.cbAcqReady.TabIndex = 25;
            this.cbAcqReady.Text = "ACQ READY";
            this.cbAcqReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAcqReady.UseVisualStyleBackColor = true;
            this.cbAcqReady.CheckedChanged += new System.EventHandler(this.cbAcqReady_CheckedChanged);
            // 
            // cbReject0
            // 
            this.cbReject0.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbReject0.Location = new System.Drawing.Point(318, 375);
            this.cbReject0.Name = "cbReject0";
            this.cbReject0.Size = new System.Drawing.Size(56, 53);
            this.cbReject0.TabIndex = 27;
            this.cbReject0.Text = "REJECT 0";
            this.cbReject0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbReject0.UseVisualStyleBackColor = true;
            this.cbReject0.CheckedChanged += new System.EventHandler(this.cbReject0_CheckedChanged);
            // 
            // cbReject1
            // 
            this.cbReject1.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbReject1.Location = new System.Drawing.Point(378, 375);
            this.cbReject1.Name = "cbReject1";
            this.cbReject1.Size = new System.Drawing.Size(56, 53);
            this.cbReject1.TabIndex = 28;
            this.cbReject1.Text = "REJECT 1";
            this.cbReject1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbReject1.UseVisualStyleBackColor = true;
            this.cbReject1.CheckedChanged += new System.EventHandler(this.cbReject1_CheckedChanged);
            // 
            // cbReject2
            // 
            this.cbReject2.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbReject2.Location = new System.Drawing.Point(438, 375);
            this.cbReject2.Name = "cbReject2";
            this.cbReject2.Size = new System.Drawing.Size(56, 53);
            this.cbReject2.TabIndex = 29;
            this.cbReject2.Text = "REJECT 2";
            this.cbReject2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbReject2.UseVisualStyleBackColor = true;
            this.cbReject2.CheckedChanged += new System.EventHandler(this.cbReject2_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 440);
            this.Controls.Add(this.cbReject2);
            this.Controls.Add(this.cbReject1);
            this.Controls.Add(this.cbReject0);
            this.Controls.Add(this.cbVialId0);
            this.Controls.Add(this.cbAcqReady);
            this.Controls.Add(this.cbDataValid);
            this.Controls.Add(this.cbVialId1);
            this.Controls.Add(this.cbReady);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cbSetBusy);
            this.Controls.Add(this.tbWorkMode);
            this.Controls.Add(this.cbWorkMode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudErrLowTh);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudErrHighTh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudWarnLowTh);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudWarnHighTh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ipAddressCtrl);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.Text = "EOPTIS DEVICE TESTER";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWarnHighTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWarnLowTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudErrHighTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudErrLowTh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPort;
        private IPAddressControlLib.IPAddressControl ipAddressCtrl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.NumericUpDown nudWarnHighTh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudWarnLowTh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudErrHighTh;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudErrLowTh;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbWorkMode;
        private System.Windows.Forms.TextBox tbWorkMode;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbSetBusy;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbReady;
        private System.Windows.Forms.CheckBox cbDataValid;
        private System.Windows.Forms.CheckBox cbVialId0;
        private System.Windows.Forms.CheckBox cbVialId1;
        private System.Windows.Forms.CheckBox cbAcqReady;
        private System.Windows.Forms.CheckBox cbReject0;
        private System.Windows.Forms.CheckBox cbReject1;
        private System.Windows.Forms.CheckBox cbReject2;
    }
}

