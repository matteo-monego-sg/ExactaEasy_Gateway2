namespace ExactaEasy
{
    partial class Trending
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.zgcTrending = new ZedGraph.ZedGraphControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRecipe = new System.Windows.Forms.Label();
            this.cbRecipe = new System.Windows.Forms.ComboBox();
            this.lblBatchID = new System.Windows.Forms.Label();
            this.cbBatchId = new System.Windows.Forms.ComboBox();
            this.lblTool = new System.Windows.Forms.Label();
            this.cbTool = new System.Windows.Forms.ComboBox();
            this.lblRange = new System.Windows.Forms.Label();
            this.cbRange = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1664, 585);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.zgcTrending);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1664, 585);
            this.panel1.TabIndex = 28;
            // 
            // zgcTrending
            // 
            this.zgcTrending.BackColor = System.Drawing.SystemColors.Control;
            this.zgcTrending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgcTrending.EditButtons = System.Windows.Forms.MouseButtons.None;
            this.zgcTrending.EditModifierKeys = System.Windows.Forms.Keys.None;
            this.zgcTrending.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zgcTrending.IsEnableHPan = false;
            this.zgcTrending.IsEnableVPan = false;
            this.zgcTrending.IsEnableWheelZoom = false;
            this.zgcTrending.IsPrintFillPage = false;
            this.zgcTrending.IsPrintKeepAspectRatio = false;
            this.zgcTrending.IsShowContextMenu = false;
            this.zgcTrending.IsShowPointValues = true;
            this.zgcTrending.Location = new System.Drawing.Point(0, 36);
            this.zgcTrending.Name = "zgcTrending";
            this.zgcTrending.ScrollGrace = 0D;
            this.zgcTrending.ScrollMaxX = 0D;
            this.zgcTrending.ScrollMaxY = 0D;
            this.zgcTrending.ScrollMaxY2 = 0D;
            this.zgcTrending.ScrollMinX = 0D;
            this.zgcTrending.ScrollMinY = 0D;
            this.zgcTrending.ScrollMinY2 = 0D;
            this.zgcTrending.Size = new System.Drawing.Size(1664, 549);
            this.zgcTrending.TabIndex = 26;
            this.zgcTrending.Tag = "";
            this.zgcTrending.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblRecipe);
            this.flowLayoutPanel1.Controls.Add(this.cbRecipe);
            this.flowLayoutPanel1.Controls.Add(this.lblBatchID);
            this.flowLayoutPanel1.Controls.Add(this.cbBatchId);
            this.flowLayoutPanel1.Controls.Add(this.lblTool);
            this.flowLayoutPanel1.Controls.Add(this.cbTool);
            this.flowLayoutPanel1.Controls.Add(this.lblRange);
            this.flowLayoutPanel1.Controls.Add(this.cbRange);
            this.flowLayoutPanel1.Controls.Add(this.btnFilter);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1664, 36);
            this.flowLayoutPanel1.TabIndex = 29;
            // 
            // lblRecipe
            // 
            this.lblRecipe.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecipe.Location = new System.Drawing.Point(3, 0);
            this.lblRecipe.Name = "lblRecipe";
            this.lblRecipe.Padding = new System.Windows.Forms.Padding(3);
            this.lblRecipe.Size = new System.Drawing.Size(120, 36);
            this.lblRecipe.TabIndex = 2;
            this.lblRecipe.Text = "RECIPE";
            this.lblRecipe.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbRecipe
            // 
            this.cbRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRecipe.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRecipe.FormattingEnabled = true;
            this.cbRecipe.Location = new System.Drawing.Point(129, 3);
            this.cbRecipe.Name = "cbRecipe";
            this.cbRecipe.Size = new System.Drawing.Size(221, 29);
            this.cbRecipe.TabIndex = 3;
            this.cbRecipe.DropDown += new System.EventHandler(this.cbRecipe_DropDown);
            this.cbRecipe.DropDownClosed += new System.EventHandler(this.cbRecipe_DropDownClosed);
            // 
            // lblBatchID
            // 
            this.lblBatchID.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatchID.Location = new System.Drawing.Point(356, 0);
            this.lblBatchID.Name = "lblBatchID";
            this.lblBatchID.Padding = new System.Windows.Forms.Padding(3);
            this.lblBatchID.Size = new System.Drawing.Size(141, 36);
            this.lblBatchID.TabIndex = 23;
            this.lblBatchID.Text = "BATCH ID";
            this.lblBatchID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbBatchId
            // 
            this.cbBatchId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBatchId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBatchId.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBatchId.FormattingEnabled = true;
            this.cbBatchId.Location = new System.Drawing.Point(503, 3);
            this.cbBatchId.Name = "cbBatchId";
            this.cbBatchId.Size = new System.Drawing.Size(139, 29);
            this.cbBatchId.TabIndex = 24;
            this.cbBatchId.DropDown += new System.EventHandler(this.cbBatchId_DropDown);
            this.cbBatchId.DropDownClosed += new System.EventHandler(this.cbBatchId_DropDownClosed);
            // 
            // lblTool
            // 
            this.lblTool.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTool.Location = new System.Drawing.Point(648, 0);
            this.lblTool.Name = "lblTool";
            this.lblTool.Padding = new System.Windows.Forms.Padding(3);
            this.lblTool.Size = new System.Drawing.Size(267, 36);
            this.lblTool.TabIndex = 4;
            this.lblTool.Text = "STATION-TOOL-PARAMETER";
            this.lblTool.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbTool
            // 
            this.cbTool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTool.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool.FormattingEnabled = true;
            this.cbTool.Location = new System.Drawing.Point(921, 3);
            this.cbTool.Name = "cbTool";
            this.cbTool.Size = new System.Drawing.Size(394, 29);
            this.cbTool.TabIndex = 5;
            this.cbTool.DropDown += new System.EventHandler(this.cbTool_DropDown);
            this.cbTool.DropDownClosed += new System.EventHandler(this.cbTool_DropDownClosed);
            // 
            // lblRange
            // 
            this.lblRange.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRange.Location = new System.Drawing.Point(1321, 0);
            this.lblRange.Name = "lblRange";
            this.lblRange.Padding = new System.Windows.Forms.Padding(3);
            this.lblRange.Size = new System.Drawing.Size(141, 36);
            this.lblRange.TabIndex = 25;
            this.lblRange.Text = "RANGE";
            this.lblRange.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbRange
            // 
            this.cbRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRange.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRange.FormattingEnabled = true;
            this.cbRange.Location = new System.Drawing.Point(1468, 3);
            this.cbRange.Name = "cbRange";
            this.cbRange.Size = new System.Drawing.Size(139, 29);
            this.cbRange.TabIndex = 26;
            this.cbRange.DropDownClosed += new System.EventHandler(this.cbRange_DropDownClosed);
            // 
            // btnFilter
            // 
            this.btnFilter.AutoEllipsis = true;
            this.btnFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnFilter.BackgroundImage = global::ExactaEasy.ResourcesArtic.checkmark2;
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFilter.Location = new System.Drawing.Point(1615, 0);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(36, 36);
            this.btnFilter.TabIndex = 22;
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // Trending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Name = "Trending";
            this.Size = new System.Drawing.Size(1664, 585);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ZedGraph.ZedGraphControl zgcTrending;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblRecipe;
        private System.Windows.Forms.ComboBox cbRecipe;
        private System.Windows.Forms.Label lblTool;
        private System.Windows.Forms.ComboBox cbTool;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label lblBatchID;
        private System.Windows.Forms.ComboBox cbBatchId;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.ComboBox cbRange;
    }
}
