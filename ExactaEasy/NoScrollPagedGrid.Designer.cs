namespace ExactaEasy
{
    partial class NoScrollPagedGrid
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlInspLog = new System.Windows.Forms.Panel();
            this.inspLogGridView = new System.Windows.Forms.DataGridView();
            this.EntryInstant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EntryLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EntryMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanelMinMax = new System.Windows.Forms.TableLayoutPanel();
            this.lblError = new System.Windows.Forms.Label();
            this.lblValoreMax = new System.Windows.Forms.Label();
            this.lblValoreMin = new System.Windows.Forms.Label();
            this.lblMaxValue = new System.Windows.Forms.Label();
            this.lblMinValue = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolTipDesc = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.LogEntryTypeImage = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlInspLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inspLogGridView)).BeginInit();
            this.tableLayoutPanelMinMax.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 32;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 36;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(399, 312);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseLeave);
            this.dataGridView1.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseMove);
            this.dataGridView1.MouseLeave += new System.EventHandler(this.dataGridView1_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlInspLog);
            this.panel1.Controls.Add(this.tableLayoutPanelMinMax);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 343);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(399, 64);
            this.panel1.TabIndex = 1;
            // 
            // pnlInspLog
            // 
            this.pnlInspLog.Controls.Add(this.inspLogGridView);
            this.pnlInspLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInspLog.Location = new System.Drawing.Point(74, 0);
            this.pnlInspLog.Name = "pnlInspLog";
            this.pnlInspLog.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.pnlInspLog.Size = new System.Drawing.Size(251, 64);
            this.pnlInspLog.TabIndex = 3;
            // 
            // inspLogGridView
            // 
            this.inspLogGridView.AllowUserToAddRows = false;
            this.inspLogGridView.AllowUserToDeleteRows = false;
            this.inspLogGridView.AllowUserToResizeRows = false;
            this.inspLogGridView.BackgroundColor = System.Drawing.Color.Black;
            this.inspLogGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inspLogGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inspLogGridView.ColumnHeadersVisible = false;
            this.inspLogGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LogEntryTypeImage,
            this.EntryInstant,
            this.EntryLevel,
            this.EntryMessage});
            this.inspLogGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inspLogGridView.Location = new System.Drawing.Point(0, 3);
            this.inspLogGridView.MultiSelect = false;
            this.inspLogGridView.Name = "inspLogGridView";
            this.inspLogGridView.ReadOnly = true;
            this.inspLogGridView.RowHeadersVisible = false;
            this.inspLogGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inspLogGridView.Size = new System.Drawing.Size(251, 58);
            this.inspLogGridView.TabIndex = 1;
            // 
            // EntryInstant
            // 
            this.EntryInstant.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.EntryInstant.DefaultCellStyle = dataGridViewCellStyle3;
            this.EntryInstant.HeaderText = "Event time";
            this.EntryInstant.Name = "EntryInstant";
            this.EntryInstant.ReadOnly = true;
            this.EntryInstant.Visible = false;
            this.EntryInstant.Width = 80;
            // 
            // EntryLevel
            // 
            this.EntryLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntryLevel.DefaultCellStyle = dataGridViewCellStyle4;
            this.EntryLevel.HeaderText = "Level";
            this.EntryLevel.Name = "EntryLevel";
            this.EntryLevel.ReadOnly = true;
            this.EntryLevel.Width = 80;
            // 
            // EntryMessage
            // 
            this.EntryMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntryMessage.DefaultCellStyle = dataGridViewCellStyle5;
            this.EntryMessage.HeaderText = "EntryMessage";
            this.EntryMessage.Name = "EntryMessage";
            this.EntryMessage.ReadOnly = true;
            // 
            // tableLayoutPanelMinMax
            // 
            this.tableLayoutPanelMinMax.ColumnCount = 2;
            this.tableLayoutPanelMinMax.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMinMax.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMinMax.Controls.Add(this.lblError, 0, 3);
            this.tableLayoutPanelMinMax.Controls.Add(this.lblValoreMax, 1, 2);
            this.tableLayoutPanelMinMax.Controls.Add(this.lblValoreMin, 0, 2);
            this.tableLayoutPanelMinMax.Controls.Add(this.lblMaxValue, 1, 1);
            this.tableLayoutPanelMinMax.Controls.Add(this.lblMinValue, 0, 1);
            this.tableLayoutPanelMinMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMinMax.Location = new System.Drawing.Point(74, 0);
            this.tableLayoutPanelMinMax.Name = "tableLayoutPanelMinMax";
            this.tableLayoutPanelMinMax.RowCount = 4;
            this.tableLayoutPanelMinMax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMinMax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMinMax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMinMax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMinMax.Size = new System.Drawing.Size(251, 64);
            this.tableLayoutPanelMinMax.TabIndex = 2;
            this.tableLayoutPanelMinMax.Click += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.BackColor = System.Drawing.Color.Red;
            this.tableLayoutPanelMinMax.SetColumnSpan(this.lblError, 2);
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(0, 44);
            this.lblError.Margin = new System.Windows.Forms.Padding(0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(251, 20);
            this.lblError.TabIndex = 5;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            // 
            // lblValoreMax
            // 
            this.lblValoreMax.AutoSize = true;
            this.lblValoreMax.BackColor = System.Drawing.Color.White;
            this.lblValoreMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValoreMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValoreMax.Location = new System.Drawing.Point(125, 32);
            this.lblValoreMax.Margin = new System.Windows.Forms.Padding(0);
            this.lblValoreMax.Name = "lblValoreMax";
            this.lblValoreMax.Size = new System.Drawing.Size(126, 12);
            this.lblValoreMax.TabIndex = 4;
            this.lblValoreMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValoreMin
            // 
            this.lblValoreMin.AutoSize = true;
            this.lblValoreMin.BackColor = System.Drawing.Color.White;
            this.lblValoreMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValoreMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValoreMin.Location = new System.Drawing.Point(0, 32);
            this.lblValoreMin.Margin = new System.Windows.Forms.Padding(0);
            this.lblValoreMin.Name = "lblValoreMin";
            this.lblValoreMin.Size = new System.Drawing.Size(125, 12);
            this.lblValoreMin.TabIndex = 3;
            this.lblValoreMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaxValue
            // 
            this.lblMaxValue.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblMaxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxValue.ForeColor = System.Drawing.Color.White;
            this.lblMaxValue.Location = new System.Drawing.Point(125, 20);
            this.lblMaxValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblMaxValue.Name = "lblMaxValue";
            this.lblMaxValue.Size = new System.Drawing.Size(126, 12);
            this.lblMaxValue.TabIndex = 1;
            this.lblMaxValue.Text = "Valore Max";
            this.lblMaxValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMinValue
            // 
            this.lblMinValue.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblMinValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMinValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinValue.ForeColor = System.Drawing.Color.White;
            this.lblMinValue.Location = new System.Drawing.Point(0, 20);
            this.lblMinValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblMinValue.Name = "lblMinValue";
            this.lblMinValue.Size = new System.Drawing.Size(125, 12);
            this.lblMinValue.TabIndex = 0;
            this.lblMinValue.Text = "Valore Min";
            this.lblMinValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Image = global::ExactaEasy.Properties.Resources.go_next;
            this.btnNext.Location = new System.Drawing.Point(325, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(74, 64);
            this.btnNext.TabIndex = 1;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Image = global::ExactaEasy.Properties.Resources.go_previous;
            this.btnPrev.Location = new System.Drawing.Point(0, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(74, 64);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.SystemColors.Control;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(399, 31);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolTipDesc
            // 
            this.toolTipDesc.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.toolTipDesc_Draw);
            this.toolTipDesc.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipDesc_Popup);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.HeaderText = "LogEntryTypeImage";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 30;
            // 
            // LogEntryTypeImage
            // 
            this.LogEntryTypeImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LogEntryTypeImage.HeaderText = "LogEntryTypeImage";
            this.LogEntryTypeImage.Name = "LogEntryTypeImage";
            this.LogEntryTypeImage.ReadOnly = true;
            this.LogEntryTypeImage.Width = 30;
            // 
            // NoScrollPagedGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "NoScrollPagedGrid";
            this.Size = new System.Drawing.Size(399, 407);
            this.Load += new System.EventHandler(this.PagedGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlInspLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inspLogGridView)).EndInit();
            this.tableLayoutPanelMinMax.ResumeLayout(false);
            this.tableLayoutPanelMinMax.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolTip toolTipDesc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMinMax;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblValoreMax;
        private System.Windows.Forms.Label lblValoreMin;
        private System.Windows.Forms.Label lblMaxValue;
        private System.Windows.Forms.Label lblMinValue;
        private System.Windows.Forms.Panel pnlInspLog;
        private System.Windows.Forms.DataGridView inspLogGridView;
        private System.Windows.Forms.DataGridViewImageColumn LogEntryTypeImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn EntryInstant;
        private System.Windows.Forms.DataGridViewTextBoxColumn EntryLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn EntryMessage;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
    }
}
