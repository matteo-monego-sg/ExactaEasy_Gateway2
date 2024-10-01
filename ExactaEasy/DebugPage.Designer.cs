
namespace ExactaEasy
{
    partial class DebugPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewSizeImagesResult = new System.Windows.Forms.DataGridView();
            this.lblToolSettings = new System.Windows.Forms.Label();
            this.buttonResetSizeImgRes = new System.Windows.Forms.Button();
            this.buttonExportTableSizeImgRes = new System.Windows.Forms.Button();
            this.colNode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCounter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSizeImagesResult)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSizeImagesResult
            // 
            this.dataGridViewSizeImagesResult.AllowUserToAddRows = false;
            this.dataGridViewSizeImagesResult.AllowUserToDeleteRows = false;
            this.dataGridViewSizeImagesResult.AllowUserToResizeColumns = false;
            this.dataGridViewSizeImagesResult.AllowUserToResizeRows = false;
            this.dataGridViewSizeImagesResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSizeImagesResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSizeImagesResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNode,
            this.colStation,
            this.colCounter,
            this.colMin,
            this.colMax,
            this.colAverage,
            this.colSum});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSizeImagesResult.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSizeImagesResult.Location = new System.Drawing.Point(3, 33);
            this.dataGridViewSizeImagesResult.MultiSelect = false;
            this.dataGridViewSizeImagesResult.Name = "dataGridViewSizeImagesResult";
            this.dataGridViewSizeImagesResult.ReadOnly = true;
            this.dataGridViewSizeImagesResult.RowHeadersVisible = false;
            this.dataGridViewSizeImagesResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSizeImagesResult.Size = new System.Drawing.Size(1020, 247);
            this.dataGridViewSizeImagesResult.TabIndex = 0;
            // 
            // lblToolSettings
            // 
            this.lblToolSettings.AutoSize = true;
            this.lblToolSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolSettings.ForeColor = System.Drawing.SystemColors.Control;
            this.lblToolSettings.Location = new System.Drawing.Point(0, 11);
            this.lblToolSettings.Name = "lblToolSettings";
            this.lblToolSettings.Padding = new System.Windows.Forms.Padding(3);
            this.lblToolSettings.Size = new System.Drawing.Size(145, 19);
            this.lblToolSettings.TabIndex = 1;
            this.lblToolSettings.Text = "SIZE IMAGES RESULT";
            // 
            // buttonResetSizeImgRes
            // 
            this.buttonResetSizeImgRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResetSizeImgRes.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonResetSizeImgRes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonResetSizeImgRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResetSizeImgRes.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonResetSizeImgRes.Location = new System.Drawing.Point(912, 286);
            this.buttonResetSizeImgRes.Name = "buttonResetSizeImgRes";
            this.buttonResetSizeImgRes.Size = new System.Drawing.Size(111, 29);
            this.buttonResetSizeImgRes.TabIndex = 2;
            this.buttonResetSizeImgRes.Text = "RESET";
            this.buttonResetSizeImgRes.UseVisualStyleBackColor = false;
            this.buttonResetSizeImgRes.Click += new System.EventHandler(this.buttonResetSizeImgRes_Click);
            // 
            // buttonExportTableSizeImgRes
            // 
            this.buttonExportTableSizeImgRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportTableSizeImgRes.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonExportTableSizeImgRes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportTableSizeImgRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExportTableSizeImgRes.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonExportTableSizeImgRes.Location = new System.Drawing.Point(795, 286);
            this.buttonExportTableSizeImgRes.Name = "buttonExportTableSizeImgRes";
            this.buttonExportTableSizeImgRes.Size = new System.Drawing.Size(111, 29);
            this.buttonExportTableSizeImgRes.TabIndex = 3;
            this.buttonExportTableSizeImgRes.Text = "EXPORT";
            this.buttonExportTableSizeImgRes.UseVisualStyleBackColor = false;
            this.buttonExportTableSizeImgRes.Click += new System.EventHandler(this.buttonExportTableSizeImgRes_Click);
            // 
            // colNode
            // 
            this.colNode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNode.FillWeight = 150F;
            this.colNode.HeaderText = "Node";
            this.colNode.Name = "colNode";
            this.colNode.ReadOnly = true;
            // 
            // colStation
            // 
            this.colStation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStation.FillWeight = 150F;
            this.colStation.HeaderText = "Station";
            this.colStation.Name = "colStation";
            this.colStation.ReadOnly = true;
            // 
            // colCounter
            // 
            this.colCounter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCounter.HeaderText = "Counter";
            this.colCounter.Name = "colCounter";
            this.colCounter.ReadOnly = true;
            // 
            // colMin
            // 
            this.colMin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMin.HeaderText = "Min";
            this.colMin.Name = "colMin";
            this.colMin.ReadOnly = true;
            // 
            // colMax
            // 
            this.colMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMax.HeaderText = "Max";
            this.colMax.Name = "colMax";
            this.colMax.ReadOnly = true;
            // 
            // colAverage
            // 
            this.colAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAverage.HeaderText = "Average";
            this.colAverage.Name = "colAverage";
            this.colAverage.ReadOnly = true;
            // 
            // colSum
            // 
            this.colSum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSum.HeaderText = "Sum";
            this.colSum.Name = "colSum";
            this.colSum.ReadOnly = true;
            // 
            // DebugPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.buttonExportTableSizeImgRes);
            this.Controls.Add(this.buttonResetSizeImgRes);
            this.Controls.Add(this.lblToolSettings);
            this.Controls.Add(this.dataGridViewSizeImagesResult);
            this.Name = "DebugPage";
            this.Size = new System.Drawing.Size(1026, 509);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSizeImagesResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSizeImagesResult;
        private System.Windows.Forms.Label lblToolSettings;
        private System.Windows.Forms.Button buttonResetSizeImgRes;
        private System.Windows.Forms.Button buttonExportTableSizeImgRes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSum;
    }
}
