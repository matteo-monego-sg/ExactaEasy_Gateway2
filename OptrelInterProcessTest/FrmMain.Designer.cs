namespace OptrelInterProcessTest
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.BtnStartServer = new System.Windows.Forms.Button();
            this.BtnSetSupervisorOnTop = new System.Windows.Forms.Button();
            this.PnlCommands = new System.Windows.Forms.Panel();
            this.BtnStopGateway2Server = new System.Windows.Forms.Button();
            this.BtnSetSupervisorHide = new System.Windows.Forms.Button();
            this.TxtLog = new System.Windows.Forms.TextBox();
            this.PnlCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnStartServer
            // 
            this.BtnStartServer.Location = new System.Drawing.Point(13, 12);
            this.BtnStartServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnStartServer.Name = "BtnStartServer";
            this.BtnStartServer.Size = new System.Drawing.Size(186, 37);
            this.BtnStartServer.TabIndex = 0;
            this.BtnStartServer.Text = "Start GATEWAY2 server";
            this.BtnStartServer.UseVisualStyleBackColor = true;
            this.BtnStartServer.Click += new System.EventHandler(this.BtnStartGateway2Server_Click);
            // 
            // BtnSetSupervisorOnTop
            // 
            this.BtnSetSupervisorOnTop.Location = new System.Drawing.Point(392, 3);
            this.BtnSetSupervisorOnTop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnSetSupervisorOnTop.Name = "BtnSetSupervisorOnTop";
            this.BtnSetSupervisorOnTop.Size = new System.Drawing.Size(186, 37);
            this.BtnSetSupervisorOnTop.TabIndex = 1;
            this.BtnSetSupervisorOnTop.Text = "SetSupervisorOnTop";
            this.BtnSetSupervisorOnTop.UseVisualStyleBackColor = true;
            this.BtnSetSupervisorOnTop.Click += new System.EventHandler(this.BtnSetSupervisorOnTop_Click);
            // 
            // PnlCommands
            // 
            this.PnlCommands.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.PnlCommands.Controls.Add(this.BtnStopGateway2Server);
            this.PnlCommands.Controls.Add(this.BtnSetSupervisorHide);
            this.PnlCommands.Controls.Add(this.BtnSetSupervisorOnTop);
            this.PnlCommands.Location = new System.Drawing.Point(13, 55);
            this.PnlCommands.Name = "PnlCommands";
            this.PnlCommands.Size = new System.Drawing.Size(909, 157);
            this.PnlCommands.TabIndex = 2;
            // 
            // BtnStopGateway2Server
            // 
            this.BtnStopGateway2Server.BackColor = System.Drawing.Color.IndianRed;
            this.BtnStopGateway2Server.Location = new System.Drawing.Point(4, 3);
            this.BtnStopGateway2Server.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnStopGateway2Server.Name = "BtnStopGateway2Server";
            this.BtnStopGateway2Server.Size = new System.Drawing.Size(186, 37);
            this.BtnStopGateway2Server.TabIndex = 4;
            this.BtnStopGateway2Server.Text = "Stop GATEWAY2 server";
            this.BtnStopGateway2Server.UseVisualStyleBackColor = false;
            this.BtnStopGateway2Server.Click += new System.EventHandler(this.BtnStopGateway2Server_Click);
            // 
            // BtnSetSupervisorHide
            // 
            this.BtnSetSupervisorHide.Location = new System.Drawing.Point(198, 3);
            this.BtnSetSupervisorHide.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnSetSupervisorHide.Name = "BtnSetSupervisorHide";
            this.BtnSetSupervisorHide.Size = new System.Drawing.Size(186, 37);
            this.BtnSetSupervisorHide.TabIndex = 2;
            this.BtnSetSupervisorHide.Text = "SetSupervisorHide";
            this.BtnSetSupervisorHide.UseVisualStyleBackColor = true;
            this.BtnSetSupervisorHide.Click += new System.EventHandler(this.BtnSetSupervisorHide_Click);
            // 
            // TxtLog
            // 
            this.TxtLog.BackColor = System.Drawing.Color.Black;
            this.TxtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtLog.ForeColor = System.Drawing.Color.Lime;
            this.TxtLog.Location = new System.Drawing.Point(13, 230);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.ReadOnly = true;
            this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtLog.Size = new System.Drawing.Size(909, 487);
            this.TxtLog.TabIndex = 3;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 729);
            this.Controls.Add(this.TxtLog);
            this.Controls.Add(this.PnlCommands);
            this.Controls.Add(this.BtnStartServer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GATEWAY2 communication test";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.PnlCommands.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStartServer;
        private System.Windows.Forms.Button BtnSetSupervisorOnTop;
        private System.Windows.Forms.Panel PnlCommands;
        private System.Windows.Forms.Button BtnSetSupervisorHide;
        private System.Windows.Forms.TextBox TxtLog;
        private System.Windows.Forms.Button BtnStopGateway2Server;
    }
}

