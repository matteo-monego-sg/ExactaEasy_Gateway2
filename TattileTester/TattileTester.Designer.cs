namespace TattileTesterUI {
    partial class TattileTester {
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
            this.tbIP = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.cbScan = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbExtLib = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(58, 32);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(98, 20);
            this.tbIP.TabIndex = 3;
            this.tbIP.Text = "10.200.10.70";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(440, 29);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(359, 85);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(359, 57);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(58, 6);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(98, 20);
            this.tbID.TabIndex = 4;
            this.tbID.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Buffer";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(218, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(98, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "20";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Test",
            "M9",
            "M12"});
            this.cbType.Location = new System.Drawing.Point(58, 57);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(98, 21);
            this.cbType.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Head";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(218, 6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(98, 20);
            this.textBox2.TabIndex = 11;
            this.textBox2.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(172, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Station";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(218, 32);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(98, 20);
            this.textBox3.TabIndex = 13;
            this.textBox3.Text = "0";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(359, 29);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 15;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // cbScan
            // 
            this.cbScan.AutoSize = true;
            this.cbScan.Location = new System.Drawing.Point(359, 8);
            this.cbScan.Name = "cbScan";
            this.cbScan.Size = new System.Drawing.Size(51, 17);
            this.cbScan.TabIndex = 16;
            this.cbScan.Text = "Scan";
            this.cbScan.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "External lib dependency";
            // 
            // tbExtLib
            // 
            this.tbExtLib.Location = new System.Drawing.Point(143, 88);
            this.tbExtLib.Name = "tbExtLib";
            this.tbExtLib.Size = new System.Drawing.Size(173, 20);
            this.tbExtLib.TabIndex = 17;
            this.tbExtLib.Text = "TagFilterLib.dll";
            // 
            // TattileTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbExtLib);
            this.Controls.Add(this.cbScan);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tbIP);
            this.Name = "TattileTester";
            this.Size = new System.Drawing.Size(529, 119);
            this.Load += new System.EventHandler(this.TattileTester_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.CheckBox cbScan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbExtLib;
    }
}
