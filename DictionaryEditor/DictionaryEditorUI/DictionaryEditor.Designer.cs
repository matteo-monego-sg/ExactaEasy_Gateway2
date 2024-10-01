namespace DictionaryEditorUI
{
    partial class DictionaryEditor
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
            this.pnlTable = new System.Windows.Forms.Panel();
            this.dgvDict = new System.Windows.Forms.DataGridView();
            this.pnlTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDict)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTable
            // 
            this.pnlTable.Controls.Add(this.dgvDict);
            this.pnlTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTable.Location = new System.Drawing.Point(0, 0);
            this.pnlTable.Name = "pnlTable";
            this.pnlTable.Size = new System.Drawing.Size(555, 470);
            this.pnlTable.TabIndex = 1;
            // 
            // dgvDict
            // 
            this.dgvDict.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDict.Location = new System.Drawing.Point(0, 0);
            this.dgvDict.Name = "dgvDict";
            this.dgvDict.Size = new System.Drawing.Size(555, 470);
            this.dgvDict.TabIndex = 0;
            // 
            // DictionaryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTable);
            this.Name = "DictionaryEditor";
            this.Size = new System.Drawing.Size(555, 470);
            this.pnlTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDict)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTable;
        private System.Windows.Forms.DataGridView dgvDict;
    }
}
