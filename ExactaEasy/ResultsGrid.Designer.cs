namespace ExactaEasy {
    partial class ResultsGrid {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvInspectionResults = new System.Windows.Forms.DataGridView();
            this.lblResultsHeader = new System.Windows.Forms.Label();
            this.panelTools = new System.Windows.Forms.Panel();
            this.dataGridViewTool = new System.Windows.Forms.DataGridView();
            this.tName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tSample = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tBtnStart = new System.Windows.Forms.DataGridViewImageColumn();
            this.tBtnStop = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewDisableButtonColumn1 = new ExactaEasy.DataGridViewDisableButtonColumn();
            this.dataGridViewDisableButtonColumn2 = new ExactaEasy.DataGridViewDisableButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInspectionResults)).BeginInit();
            this.panelTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTool)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInspectionResults
            // 
            this.dgvInspectionResults.AllowUserToAddRows = false;
            this.dgvInspectionResults.AllowUserToDeleteRows = false;
            this.dgvInspectionResults.AllowUserToResizeRows = false;
            this.dgvInspectionResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInspectionResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInspectionResults.Location = new System.Drawing.Point(0, 41);
            this.dgvInspectionResults.Margin = new System.Windows.Forms.Padding(0);
            this.dgvInspectionResults.Name = "dgvInspectionResults";
            this.dgvInspectionResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInspectionResults.Size = new System.Drawing.Size(333, 185);
            this.dgvInspectionResults.TabIndex = 2;
            // 
            // lblResultsHeader
            // 
            this.lblResultsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblResultsHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultsHeader.Location = new System.Drawing.Point(0, 0);
            this.lblResultsHeader.Name = "lblResultsHeader";
            this.lblResultsHeader.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblResultsHeader.Size = new System.Drawing.Size(333, 41);
            this.lblResultsHeader.TabIndex = 3;
            this.lblResultsHeader.Text = "---";
            this.lblResultsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTools
            // 
            this.panelTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTools.Controls.Add(this.dataGridViewTool);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 225);
            this.panelTools.Name = "panelTools";
            this.panelTools.Size = new System.Drawing.Size(333, 111);
            this.panelTools.TabIndex = 4;
            // 
            // dataGridViewTool
            // 
            this.dataGridViewTool.AllowUserToAddRows = false;
            this.dataGridViewTool.AllowUserToDeleteRows = false;
            this.dataGridViewTool.AllowUserToResizeColumns = false;
            this.dataGridViewTool.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTool.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTool.ColumnHeadersHeight = 21;
            this.dataGridViewTool.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTool.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tName,
            this.tType,
            this.tSample,
            this.tVal,
            this.tBtnStart,
            this.tBtnStop});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTool.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTool.EnableHeadersVisualStyles = false;
            this.dataGridViewTool.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTool.MultiSelect = false;
            this.dataGridViewTool.Name = "dataGridViewTool";
            this.dataGridViewTool.ReadOnly = true;
            this.dataGridViewTool.RowHeadersVisible = false;
            this.dataGridViewTool.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTool.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTool.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTool.Size = new System.Drawing.Size(331, 109);
            this.dataGridViewTool.TabIndex = 29;
            // 
            // tName
            // 
            this.tName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.tName.DefaultCellStyle = dataGridViewCellStyle2;
            this.tName.HeaderText = "tName";
            this.tName.Name = "tName";
            this.tName.ReadOnly = true;
            // 
            // tType
            // 
            this.tType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.tType.DefaultCellStyle = dataGridViewCellStyle3;
            this.tType.FillWeight = 60F;
            this.tType.HeaderText = "tType";
            this.tType.Name = "tType";
            this.tType.ReadOnly = true;
            // 
            // tSample
            // 
            this.tSample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.tSample.DefaultCellStyle = dataGridViewCellStyle4;
            this.tSample.FillWeight = 40F;
            this.tSample.HeaderText = "tSample";
            this.tSample.Name = "tSample";
            this.tSample.ReadOnly = true;
            // 
            // tVal
            // 
            this.tVal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            this.tVal.DefaultCellStyle = dataGridViewCellStyle5;
            this.tVal.FillWeight = 60F;
            this.tVal.HeaderText = "tVal";
            this.tVal.Name = "tVal";
            this.tVal.ReadOnly = true;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "tBtnStart";
            this.dataGridViewImageColumn1.Image = global::ExactaEasy.Properties.Resources.checkmark1;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewImageColumn1.Width = 30;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "tBtnStop";
            this.dataGridViewImageColumn2.Image = global::ExactaEasy.Properties.Resources.button_fewer;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewImageColumn2.Width = 30;
            // 
            // tBtnStart
            // 
            this.tBtnStart.HeaderText = "tBtnStart";
            this.tBtnStart.Image = global::ExactaEasy.Properties.Resources.checkmark1;
            this.tBtnStart.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.tBtnStart.Name = "tBtnStart";
            this.tBtnStart.ReadOnly = true;
            this.tBtnStart.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.tBtnStart.Width = 30;
            // 
            // tBtnStop
            // 
            this.tBtnStop.HeaderText = "tBtnStop";
            this.tBtnStop.Image = global::ExactaEasy.Properties.Resources.button_fewer;
            this.tBtnStop.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.tBtnStop.Name = "tBtnStop";
            this.tBtnStop.ReadOnly = true;
            this.tBtnStop.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.tBtnStop.Width = 30;
            // 
            // dataGridViewDisableButtonColumn1
            // 
            this.dataGridViewDisableButtonColumn1.HeaderText = "tBtnStart";
            this.dataGridViewDisableButtonColumn1.Name = "dataGridViewDisableButtonColumn1";
            this.dataGridViewDisableButtonColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDisableButtonColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewDisableButtonColumn1.Width = 20;
            // 
            // dataGridViewDisableButtonColumn2
            // 
            this.dataGridViewDisableButtonColumn2.HeaderText = "tBtnStop";
            this.dataGridViewDisableButtonColumn2.Name = "dataGridViewDisableButtonColumn2";
            this.dataGridViewDisableButtonColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDisableButtonColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewDisableButtonColumn2.Width = 20;
            // 
            // ResultsGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.panelTools);
            this.Controls.Add(this.dgvInspectionResults);
            this.Controls.Add(this.lblResultsHeader);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "ResultsGrid";
            this.Size = new System.Drawing.Size(333, 336);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInspectionResults)).EndInit();
            this.panelTools.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTool)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInspectionResults;
        private System.Windows.Forms.Label lblResultsHeader;
        private System.Windows.Forms.Panel panelTools;
        private System.Windows.Forms.DataGridView dataGridViewTool;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn tName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tType;
        private System.Windows.Forms.DataGridViewTextBoxColumn tSample;
        private System.Windows.Forms.DataGridViewTextBoxColumn tVal;
        private System.Windows.Forms.DataGridViewImageColumn tBtnStart;
        private System.Windows.Forms.DataGridViewImageColumn tBtnStop;
    }
}
