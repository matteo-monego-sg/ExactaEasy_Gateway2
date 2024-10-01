
namespace ExactaEasy
{
    partial class DumpUI2Cnt
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHead = new System.Windows.Forms.Panel();
            this.cbUserSettings = new System.Windows.Forms.ComboBox();
            this.labelUsrSett = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.dgvDumpInspectionSettings = new System.Windows.Forms.DataGridView();
            this.panelHead.SuspendLayout();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDumpInspectionSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHead
            // 
            this.panelHead.Controls.Add(this.cbUserSettings);
            this.panelHead.Controls.Add(this.labelUsrSett);
            this.panelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHead.Location = new System.Drawing.Point(0, 0);
            this.panelHead.Name = "panelHead";
            this.panelHead.Padding = new System.Windows.Forms.Padding(5);
            this.panelHead.Size = new System.Drawing.Size(962, 44);
            this.panelHead.TabIndex = 0;
            // 
            // cbUserSettings
            // 
            this.cbUserSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbUserSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUserSettings.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUserSettings.FormattingEnabled = true;
            this.cbUserSettings.Location = new System.Drawing.Point(159, 5);
            this.cbUserSettings.Name = "cbUserSettings";
            this.cbUserSettings.Size = new System.Drawing.Size(198, 29);
            this.cbUserSettings.TabIndex = 2;
            // 
            // labelUsrSett
            // 
            this.labelUsrSett.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelUsrSett.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsrSett.Location = new System.Drawing.Point(5, 5);
            this.labelUsrSett.Name = "labelUsrSett";
            this.labelUsrSett.Padding = new System.Windows.Forms.Padding(3);
            this.labelUsrSett.Size = new System.Drawing.Size(154, 34);
            this.labelUsrSett.TabIndex = 1;
            this.labelUsrSett.Text = "USER SETTINGS";
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.dgvDumpInspectionSettings);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 44);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(962, 349);
            this.panelBody.TabIndex = 1;
            // 
            // dgvDumpInspectionSettings
            // 
            this.dgvDumpInspectionSettings.AllowUserToAddRows = false;
            this.dgvDumpInspectionSettings.AllowUserToDeleteRows = false;
            this.dgvDumpInspectionSettings.AllowUserToResizeColumns = false;
            this.dgvDumpInspectionSettings.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.dgvDumpInspectionSettings.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDumpInspectionSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDumpInspectionSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDumpInspectionSettings.EnableHeadersVisualStyles = false;
            this.dgvDumpInspectionSettings.Location = new System.Drawing.Point(0, 0);
            this.dgvDumpInspectionSettings.Name = "dgvDumpInspectionSettings";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.dgvDumpInspectionSettings.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDumpInspectionSettings.RowTemplate.Height = 30;
            this.dgvDumpInspectionSettings.Size = new System.Drawing.Size(962, 349);
            this.dgvDumpInspectionSettings.TabIndex = 27;
            // 
            // DumpUI2Cnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHead);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "DumpUI2Cnt";
            this.Size = new System.Drawing.Size(962, 393);
            this.panelHead.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDumpInspectionSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHead;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Label labelUsrSett;
        private System.Windows.Forms.ComboBox cbUserSettings;
        private System.Windows.Forms.DataGridView dgvDumpInspectionSettings;
    }
}
