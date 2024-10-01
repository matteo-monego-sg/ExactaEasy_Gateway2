namespace ExactaEasy {
    partial class frmSplash {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
            this.pnlTasks = new System.Windows.Forms.Panel();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlWait = new System.Windows.Forms.Panel();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.lblWaitTxt = new System.Windows.Forms.Label();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.pnlTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlWait.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTasks
            // 
            this.pnlTasks.BackColor = System.Drawing.Color.DarkGray;
            this.pnlTasks.Controls.Add(this.dgvTasks);
            this.pnlTasks.Controls.Add(this.pnlHeader);
            this.pnlTasks.Controls.Add(this.pictureBox1);
            this.pnlTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTasks.Location = new System.Drawing.Point(0, 0);
            this.pnlTasks.Name = "pnlTasks";
            this.pnlTasks.Padding = new System.Windows.Forms.Padding(10);
            this.pnlTasks.Size = new System.Drawing.Size(680, 680);
            this.pnlTasks.TabIndex = 1;
            // 
            // dgvTasks
            // 
            this.dgvTasks.AllowUserToAddRows = false;
            this.dgvTasks.AllowUserToDeleteRows = false;
            this.dgvTasks.AllowUserToResizeColumns = false;
            this.dgvTasks.AllowUserToResizeRows = false;
            this.dgvTasks.BackgroundColor = System.Drawing.Color.Black;
            this.dgvTasks.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.ColumnHeadersVisible = false;
            this.dgvTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colDescription,
            this.colStatus});
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvTasks.GridColor = System.Drawing.Color.Black;
            this.dgvTasks.Location = new System.Drawing.Point(10, 125);
            this.dgvTasks.Margin = new System.Windows.Forms.Padding(10);
            this.dgvTasks.MultiSelect = false;
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.ReadOnly = true;
            this.dgvTasks.RowHeadersVisible = false;
            this.dgvTasks.RowTemplate.Height = 40;
            this.dgvTasks.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTasks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvTasks.Size = new System.Drawing.Size(660, 475);
            this.dgvTasks.TabIndex = 2;
            this.dgvTasks.SelectionChanged += new System.EventHandler(this.dgvTasks_SelectionChanged);
            // 
            // colID
            // 
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Visible = false;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.FillWeight = 167.5127F;
            this.colDescription.HeaderText = "Description";
            this.colDescription.MinimumWidth = 50;
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colStatus.FillWeight = 42F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 42;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 42;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnExit);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(10, 10);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(660, 115);
            this.pnlHeader.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.Enabled = false;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = global::ExactaEasy.Properties.Resources.application_exit1;
            this.btnExit.Location = new System.Drawing.Point(562, 20);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(73, 57);
            this.btnExit.TabIndex = 1;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Black;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(77)))));
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(660, 115);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "VISION SPV IS STARTING";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BackgroundImage = global::ExactaEasy.Properties.Resources.SG_Optrel_New;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(10, 603);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(660, 67);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pnlWait
            // 
            this.pnlWait.BackColor = System.Drawing.Color.DarkGray;
            this.pnlWait.Controls.Add(this.pnlMessage);
            this.pnlWait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWait.ForeColor = System.Drawing.Color.Black;
            this.pnlWait.Location = new System.Drawing.Point(0, 0);
            this.pnlWait.Name = "pnlWait";
            this.pnlWait.Padding = new System.Windows.Forms.Padding(10);
            this.pnlWait.Size = new System.Drawing.Size(680, 680);
            this.pnlWait.TabIndex = 4;
            // 
            // pnlMessage
            // 
            this.pnlMessage.BackColor = System.Drawing.Color.Black;
            this.pnlMessage.Controls.Add(this.lblWaitTxt);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessage.Location = new System.Drawing.Point(10, 10);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMessage.Size = new System.Drawing.Size(660, 660);
            this.pnlMessage.TabIndex = 2;
            // 
            // lblWaitTxt
            // 
            this.lblWaitTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaitTxt.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblWaitTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(77)))));
            this.lblWaitTxt.Location = new System.Drawing.Point(10, 10);
            this.lblWaitTxt.Name = "lblWaitTxt";
            this.lblWaitTxt.Size = new System.Drawing.Size(640, 640);
            this.lblWaitTxt.TabIndex = 0;
            this.lblWaitTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.FillWeight = 42F;
            this.dataGridViewImageColumn1.HeaderText = "Status";
            this.dataGridViewImageColumn1.MinimumWidth = 42;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 42;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(680, 680);
            this.Controls.Add(this.pnlWait);
            this.Controls.Add(this.pnlTasks);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Gray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PingCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSplash_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSplash_FormClosed);
            this.pnlTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlWait.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTasks;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewImageColumn colStatus;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlWait;
        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Label lblWaitTxt;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}