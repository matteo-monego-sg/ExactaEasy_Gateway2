namespace machineConfiguration
{
    partial class MngNode
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.providername = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.serverip4address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ip4address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desktoptype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.id,
            this.name,
            this.providername,
            this.serverip4address,
            this.ip4address,
            this.port,
            this.user,
            this.key,
            this.desktoptype});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(874, 369);
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
            // 
            // providername
            // 
            this.providername.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Control;
            this.providername.DefaultCellStyle = dataGridViewCellStyle2;
            this.providername.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.providername.HeaderText = "Provider Name";
            this.providername.Name = "providername";
            this.providername.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.providername.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // serverip4address
            // 
            this.serverip4address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.serverip4address.HeaderText = "Server IP4 Address";
            this.serverip4address.Name = "serverip4address";
            // 
            // ip4address
            // 
            this.ip4address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ip4address.HeaderText = "IP4 Address";
            this.ip4address.Name = "ip4address";
            // 
            // port
            // 
            this.port.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.port.HeaderText = "Port";
            this.port.Name = "port";
            // 
            // user
            // 
            this.user.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.user.HeaderText = "User";
            this.user.Name = "user";
            // 
            // key
            // 
            this.key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.key.HeaderText = "Key";
            this.key.Name = "key";
            // 
            // desktoptype
            // 
            this.desktoptype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.desktoptype.HeaderText = "Desktop Type";
            this.desktoptype.Name = "desktoptype";
            // 
            // MngNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.Name = "MngNode";
            this.Size = new System.Drawing.Size(874, 369);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewComboBoxColumn providername;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverip4address;
        private System.Windows.Forms.DataGridViewTextBoxColumn ip4address;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn user;
        private System.Windows.Forms.DataGridViewTextBoxColumn key;
        private System.Windows.Forms.DataGridViewTextBoxColumn desktoptype;
    }
}
