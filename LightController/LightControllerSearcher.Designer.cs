namespace SPAMI.LightControllers
{
    partial class LightControllerSearcher
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
            this.groupBoxFindControllers = new System.Windows.Forms.GroupBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listBoxDevice = new System.Windows.Forms.ListBox();
            this.labelIPaddr = new System.Windows.Forms.Label();
            this.ipAddressControl = new IPAddressControlLib.IPAddressControl();
            this.textBoxSendCmd = new System.Windows.Forms.TextBox();
            this.buttonSendCmd = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.labelSTATUS = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.groupBoxConnection = new System.Windows.Forms.GroupBox();
            this.groupBoxCmd = new System.Windows.Forms.GroupBox();
            this.groupBoxFindControllers.SuspendLayout();
            this.groupBoxConnection.SuspendLayout();
            this.groupBoxCmd.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFindControllers
            // 
            this.groupBoxFindControllers.Controls.Add(this.buttonSearch);
            this.groupBoxFindControllers.Controls.Add(this.listBoxDevice);
            this.groupBoxFindControllers.Location = new System.Drawing.Point(3, 3);
            this.groupBoxFindControllers.Name = "groupBoxFindControllers";
            this.groupBoxFindControllers.Size = new System.Drawing.Size(252, 154);
            this.groupBoxFindControllers.TabIndex = 1;
            this.groupBoxFindControllers.TabStop = false;
            this.groupBoxFindControllers.Text = "Find Controllers";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(6, 120);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(79, 24);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // listBoxDevice
            // 
            this.listBoxDevice.FormattingEnabled = true;
            this.listBoxDevice.HorizontalScrollbar = true;
            this.listBoxDevice.Location = new System.Drawing.Point(3, 19);
            this.listBoxDevice.Name = "listBoxDevice";
            this.listBoxDevice.Size = new System.Drawing.Size(243, 95);
            this.listBoxDevice.TabIndex = 0;
            this.listBoxDevice.SelectedIndexChanged += new System.EventHandler(this.listBoxDevice_SelectedIndexChanged);
            // 
            // labelIPaddr
            // 
            this.labelIPaddr.AutoSize = true;
            this.labelIPaddr.Location = new System.Drawing.Point(6, 23);
            this.labelIPaddr.Name = "labelIPaddr";
            this.labelIPaddr.Size = new System.Drawing.Size(58, 13);
            this.labelIPaddr.TabIndex = 4;
            this.labelIPaddr.Text = "IP Address";
            // 
            // ipAddressControl
            // 
            this.ipAddressControl.AllowInternalTab = false;
            this.ipAddressControl.AutoHeight = true;
            this.ipAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressControl.Location = new System.Drawing.Point(84, 19);
            this.ipAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipAddressControl.Name = "ipAddressControl";
            this.ipAddressControl.ReadOnly = false;
            this.ipAddressControl.Size = new System.Drawing.Size(162, 20);
            this.ipAddressControl.TabIndex = 3;
            this.ipAddressControl.Text = "...";
            // 
            // textBoxSendCmd
            // 
            this.textBoxSendCmd.Location = new System.Drawing.Point(6, 19);
            this.textBoxSendCmd.Name = "textBoxSendCmd";
            this.textBoxSendCmd.Size = new System.Drawing.Size(184, 20);
            this.textBoxSendCmd.TabIndex = 14;
            // 
            // buttonSendCmd
            // 
            this.buttonSendCmd.Location = new System.Drawing.Point(196, 19);
            this.buttonSendCmd.Name = "buttonSendCmd";
            this.buttonSendCmd.Size = new System.Drawing.Size(50, 22);
            this.buttonSendCmd.TabIndex = 13;
            this.buttonSendCmd.Text = "Send";
            this.buttonSendCmd.UseVisualStyleBackColor = true;
            this.buttonSendCmd.Click += new System.EventHandler(this.buttonSendCmd_Click);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxStatus.Location = new System.Drawing.Point(84, 97);
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.Size = new System.Drawing.Size(162, 13);
            this.textBoxStatus.TabIndex = 12;
            // 
            // labelSTATUS
            // 
            this.labelSTATUS.AutoSize = true;
            this.labelSTATUS.Location = new System.Drawing.Point(6, 97);
            this.labelSTATUS.Name = "labelSTATUS";
            this.labelSTATUS.Size = new System.Drawing.Size(50, 13);
            this.labelSTATUS.TabIndex = 11;
            this.labelSTATUS.Text = "STATUS";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(137, 49);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(91, 42);
            this.buttonDisconnect.TabIndex = 10;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(20, 49);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(91, 42);
            this.buttonConnect.TabIndex = 9;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // groupBoxConnection
            // 
            this.groupBoxConnection.Controls.Add(this.ipAddressControl);
            this.groupBoxConnection.Controls.Add(this.labelIPaddr);
            this.groupBoxConnection.Controls.Add(this.buttonConnect);
            this.groupBoxConnection.Controls.Add(this.textBoxStatus);
            this.groupBoxConnection.Controls.Add(this.labelSTATUS);
            this.groupBoxConnection.Controls.Add(this.buttonDisconnect);
            this.groupBoxConnection.Location = new System.Drawing.Point(3, 163);
            this.groupBoxConnection.Name = "groupBoxConnection";
            this.groupBoxConnection.Size = new System.Drawing.Size(252, 120);
            this.groupBoxConnection.TabIndex = 15;
            this.groupBoxConnection.TabStop = false;
            this.groupBoxConnection.Text = "Connection";
            // 
            // groupBoxCmd
            // 
            this.groupBoxCmd.Controls.Add(this.buttonSendCmd);
            this.groupBoxCmd.Controls.Add(this.textBoxSendCmd);
            this.groupBoxCmd.Location = new System.Drawing.Point(3, 289);
            this.groupBoxCmd.Name = "groupBoxCmd";
            this.groupBoxCmd.Size = new System.Drawing.Size(252, 49);
            this.groupBoxCmd.TabIndex = 16;
            this.groupBoxCmd.TabStop = false;
            this.groupBoxCmd.Text = "Send Commands";
            // 
            // LightControllerSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBoxCmd);
            this.Controls.Add(this.groupBoxConnection);
            this.Controls.Add(this.groupBoxFindControllers);
            this.Name = "LightControllerSearcher";
            this.Size = new System.Drawing.Size(264, 345);
            this.groupBoxFindControllers.ResumeLayout(false);
            this.groupBoxConnection.ResumeLayout(false);
            this.groupBoxConnection.PerformLayout();
            this.groupBoxCmd.ResumeLayout(false);
            this.groupBoxCmd.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFindControllers;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ListBox listBoxDevice;
        private System.Windows.Forms.Label labelIPaddr;
        private IPAddressControlLib.IPAddressControl ipAddressControl;
        private System.Windows.Forms.TextBox textBoxSendCmd;
        private System.Windows.Forms.Button buttonSendCmd;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Label labelSTATUS;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.GroupBox groupBoxConnection;
        private System.Windows.Forms.GroupBox groupBoxCmd;
    }
}
