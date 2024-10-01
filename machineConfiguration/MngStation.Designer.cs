namespace machineConfiguration
{
    partial class MngStation
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
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.providername = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.nodename,
            this.id,
            this.name,
            this.providername});
            this.dataGridView1.Location = new System.Drawing.Point(94, 90);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(301, 158);
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
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.HeaderText = "Id";
            this.id.Name = "id";
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // providername
            // 
            this.providername.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.providername.HeaderText = "Provider Name";
            this.providername.Name = "providername";
            this.providername.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.providername.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MngStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.Name = "MngStation";
            this.Size = new System.Drawing.Size(603, 319);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewComboBoxColumn nodename;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewComboBoxColumn providername;
    }
}
