namespace machineConfiguration
{
    partial class MngCamera
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nodename = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.stationname = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rotation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.nodename,
            this.stationname,
            this.id,
            this.defect,
            this.type,
            this.rotation});
            this.dataGridView1.Location = new System.Drawing.Point(142, 91);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(414, 196);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // index
            // 
            this.index.HeaderText = "Index";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.Width = 50;
            // 
            // nodename
            // 
            this.nodename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nodename.HeaderText = "Node Name";
            this.nodename.Name = "nodename";
            this.nodename.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nodename.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // stationname
            // 
            this.stationname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.stationname.HeaderText = "Station Name";
            this.stationname.Name = "stationname";
            this.stationname.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.stationname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.HeaderText = "Id";
            this.id.Name = "id";
            // 
            // defect
            // 
            this.defect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.defect.HeaderText = "Defect";
            this.defect.Name = "defect";
            // 
            // type
            // 
            this.type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rotation
            // 
            this.rotation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rotation.HeaderText = "Rotation";
            this.rotation.Name = "rotation";
            // 
            // MngCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.Name = "MngCamera";
            this.Size = new System.Drawing.Size(765, 416);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewComboBoxColumn nodename;
        private System.Windows.Forms.DataGridViewComboBoxColumn stationname;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn defect;
        private System.Windows.Forms.DataGridViewComboBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn rotation;
    }
}
