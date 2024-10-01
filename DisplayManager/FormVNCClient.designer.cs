namespace DisplayManager {
    partial class FormVNCClient {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVNCClient));
            this.rd = new VncSharp.RemoteDesktop();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnRestart = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlConfirm = new System.Windows.Forms.Panel();
            this.pnlQuestionReply = new System.Windows.Forms.Panel();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.pnlReply = new System.Windows.Forms.Panel();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.pnlRestarting = new System.Windows.Forms.Panel();
            this.lblRestarting = new System.Windows.Forms.Label();
            this.pnlMenu.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlConfirm.SuspendLayout();
            this.pnlQuestionReply.SuspendLayout();
            this.pnlReply.SuspendLayout();
            this.pnlRestarting.SuspendLayout();
            this.SuspendLayout();
            // 
            // rd
            // 
            this.rd.AutoScroll = true;
            this.rd.AutoScrollMinSize = new System.Drawing.Size(608, 427);
            this.rd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rd.Location = new System.Drawing.Point(0, 0);
            this.rd.Name = "rd";
            this.rd.Size = new System.Drawing.Size(700, 500);
            this.rd.TabIndex = 0;
            this.rd.ConnectComplete += new VncSharp.ConnectCompleteHandler(this.rd_ConnectComplete);
            this.rd.ConnectionLost += new System.EventHandler(this.rd_ConnectionLost);
            this.rd.ClipboardChanged += new System.EventHandler(this.rd_ClipboardChanged);
            // 
            // btnExit
            // 
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(650, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 50);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.Black;
            this.pnlMenu.Controls.Add(this.btnRestart);
            this.pnlMenu.Controls.Add(this.btnExit);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(700, 50);
            this.pnlMenu.TabIndex = 0;
            // 
            // btnRestart
            // 
            this.btnRestart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRestart.Enabled = false;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.ForeColor = System.Drawing.Color.Black;
            this.btnRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnRestart.Image")));
            this.btnRestart.Location = new System.Drawing.Point(600, 0);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(50, 50);
            this.btnRestart.TabIndex = 1;
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Visible = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.rd);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 50);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(700, 500);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlConfirm
            // 
            this.pnlConfirm.BackColor = System.Drawing.Color.Black;
            this.pnlConfirm.Controls.Add(this.pnlQuestionReply);
            this.pnlConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfirm.Location = new System.Drawing.Point(0, 50);
            this.pnlConfirm.Name = "pnlConfirm";
            this.pnlConfirm.Size = new System.Drawing.Size(700, 500);
            this.pnlConfirm.TabIndex = 0;
            this.pnlConfirm.Visible = false;
            // 
            // pnlQuestionReply
            // 
            this.pnlQuestionReply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlQuestionReply.BackColor = System.Drawing.Color.DarkGray;
            this.pnlQuestionReply.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlQuestionReply.Controls.Add(this.lblQuestion);
            this.pnlQuestionReply.Controls.Add(this.pnlReply);
            this.pnlQuestionReply.Location = new System.Drawing.Point(175, 75);
            this.pnlQuestionReply.Name = "pnlQuestionReply";
            this.pnlQuestionReply.Size = new System.Drawing.Size(350, 350);
            this.pnlQuestionReply.TabIndex = 4;
            // 
            // lblQuestion
            // 
            this.lblQuestion.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion.ForeColor = System.Drawing.Color.Red;
            this.lblQuestion.Location = new System.Drawing.Point(0, 0);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(346, 276);
            this.lblQuestion.TabIndex = 0;
            this.lblQuestion.Text = "Are you sure?";
            this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlReply
            // 
            this.pnlReply.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlReply.Controls.Add(this.btnNo);
            this.pnlReply.Controls.Add(this.btnYes);
            this.pnlReply.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlReply.Location = new System.Drawing.Point(0, 276);
            this.pnlReply.Name = "pnlReply";
            this.pnlReply.Size = new System.Drawing.Size(346, 70);
            this.pnlReply.TabIndex = 3;
            // 
            // btnNo
            // 
            this.btnNo.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.Location = new System.Drawing.Point(274, 0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(72, 70);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "NO";
            this.btnNo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnYes.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.Black;
            this.btnYes.Image = ((System.Drawing.Image)(resources.GetObject("btnYes.Image")));
            this.btnYes.Location = new System.Drawing.Point(0, 0);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(72, 70);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "YES";
            this.btnYes.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // pnlRestarting
            // 
            this.pnlRestarting.BackColor = System.Drawing.Color.DarkGray;
            this.pnlRestarting.Controls.Add(this.lblRestarting);
            this.pnlRestarting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRestarting.Location = new System.Drawing.Point(0, 50);
            this.pnlRestarting.Name = "pnlRestarting";
            this.pnlRestarting.Size = new System.Drawing.Size(700, 500);
            this.pnlRestarting.TabIndex = 1;
            this.pnlRestarting.Visible = false;
            // 
            // lblRestarting
            // 
            this.lblRestarting.BackColor = System.Drawing.Color.Black;
            this.lblRestarting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRestarting.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestarting.ForeColor = System.Drawing.Color.Red;
            this.lblRestarting.Location = new System.Drawing.Point(0, 0);
            this.lblRestarting.Name = "lblRestarting";
            this.lblRestarting.Size = new System.Drawing.Size(700, 500);
            this.lblRestarting.TabIndex = 0;
            this.lblRestarting.Text = "Restarting ...";
            this.lblRestarting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormVNCClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(700, 550);
            this.Controls.Add(this.pnlConfirm);
            this.Controls.Add(this.pnlRestarting);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormVNCClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Remote Desktop";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormVNCClient_FormClosed);
            this.pnlMenu.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlConfirm.ResumeLayout(false);
            this.pnlQuestionReply.ResumeLayout(false);
            this.pnlReply.ResumeLayout(false);
            this.pnlRestarting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VncSharp.RemoteDesktop rd;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlConfirm;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Panel pnlReply;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Panel pnlRestarting;
        private System.Windows.Forms.Label lblRestarting;
        private System.Windows.Forms.Panel pnlQuestionReply;

    }
}