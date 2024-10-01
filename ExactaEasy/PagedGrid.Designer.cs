namespace ExactaEasy
{
    partial class PagedGrid
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlInspLog = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
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
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.dataGridViewParent = new System.Windows.Forms.DataGridView();
            this.pnlParent = new System.Windows.Forms.Panel();
            this.pnlChild = new System.Windows.Forms.Panel();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlInspLog.SuspendLayout();
            this.tableLayoutPanelMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParent)).BeginInit();
            this.pnlParent.SuspendLayout();
            this.pnlChild.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 36;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.Size = new System.Drawing.Size(367, 228);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseLeave);
            this.dataGridView1.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseMove);
            this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
            this.dataGridView1.MouseLeave += new System.EventHandler(this.dataGridView1_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlInspLog);
            this.panel1.Controls.Add(this.tableLayoutPanelMinMax);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(399, 48);
            this.panel1.TabIndex = 1;
            // 
            // pnlInspLog
            // 
            this.pnlInspLog.BackColor = System.Drawing.Color.Black;
            this.pnlInspLog.Controls.Add(this.richTextBox1);
            this.pnlInspLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInspLog.Location = new System.Drawing.Point(74, 0);
            this.pnlInspLog.Name = "pnlInspLog";
            this.pnlInspLog.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.pnlInspLog.Size = new System.Drawing.Size(251, 48);
            this.pnlInspLog.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(3, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(245, 35);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
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
            this.tableLayoutPanelMinMax.Size = new System.Drawing.Size(251, 48);
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
            this.lblError.Location = new System.Drawing.Point(0, 28);
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
            this.lblValoreMax.Location = new System.Drawing.Point(125, 24);
            this.lblValoreMax.Margin = new System.Windows.Forms.Padding(0);
            this.lblValoreMax.Name = "lblValoreMax";
            this.lblValoreMax.Size = new System.Drawing.Size(126, 4);
            this.lblValoreMax.TabIndex = 4;
            this.lblValoreMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValoreMin
            // 
            this.lblValoreMin.AutoSize = true;
            this.lblValoreMin.BackColor = System.Drawing.Color.White;
            this.lblValoreMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValoreMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValoreMin.Location = new System.Drawing.Point(0, 24);
            this.lblValoreMin.Margin = new System.Windows.Forms.Padding(0);
            this.lblValoreMin.Name = "lblValoreMin";
            this.lblValoreMin.Size = new System.Drawing.Size(125, 4);
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
            this.lblMaxValue.Size = new System.Drawing.Size(126, 4);
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
            this.lblMinValue.Size = new System.Drawing.Size(125, 4);
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
            this.btnNext.Size = new System.Drawing.Size(74, 48);
            this.btnNext.TabIndex = 1;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Visible = false;
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
            this.btnPrev.Size = new System.Drawing.Size(74, 48);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
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
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.LargeChange = 1;
            this.vScrollBar.Location = new System.Drawing.Point(367, 0);
            this.vScrollBar.Maximum = 5;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(32, 228);
            this.vScrollBar.TabIndex = 3;
            this.vScrollBar.Visible = false;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // dataGridViewParent
            // 
            this.dataGridViewParent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewParent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewParent.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewParent.Name = "dataGridViewParent";
            this.dataGridViewParent.RowTemplate.Height = 36;
            this.dataGridViewParent.Size = new System.Drawing.Size(399, 90);
            this.dataGridViewParent.TabIndex = 4;
            this.dataGridViewParent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewParent_CellClick);
            // 
            // pnlParent
            // 
            this.pnlParent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.pnlParent.Controls.Add(this.dataGridViewParent);
            this.pnlParent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParent.Location = new System.Drawing.Point(0, 31);
            this.pnlParent.Name = "pnlParent";
            this.pnlParent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlParent.Size = new System.Drawing.Size(399, 100);
            this.pnlParent.TabIndex = 5;
            // 
            // pnlChild
            // 
            this.pnlChild.Controls.Add(this.dataGridView1);
            this.pnlChild.Controls.Add(this.vScrollBar);
            this.pnlChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChild.Location = new System.Drawing.Point(0, 131);
            this.pnlChild.Name = "pnlChild";
            this.pnlChild.Size = new System.Drawing.Size(399, 228);
            this.pnlChild.TabIndex = 6;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.HeaderText = "LogEntryTypeImage";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 30;
            // 
            // PagedGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.pnlChild);
            this.Controls.Add(this.pnlParent);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "PagedGrid";
            this.Size = new System.Drawing.Size(399, 407);
            this.Load += new System.EventHandler(this.PagedGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlInspLog.ResumeLayout(false);
            this.tableLayoutPanelMinMax.ResumeLayout(false);
            this.tableLayoutPanelMinMax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParent)).EndInit();
            this.pnlParent.ResumeLayout(false);
            this.pnlChild.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.DataGridView dataGridViewParent;
        private System.Windows.Forms.Panel pnlParent;
        private System.Windows.Forms.Panel pnlChild;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
