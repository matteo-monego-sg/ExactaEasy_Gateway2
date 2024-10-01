
namespace ExactaEasy
{
    partial class LiveImageFilterPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveImageFilterPage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewFilterLive = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilterLive)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewFilterLive
            // 
            this.dataGridViewFilterLive.AllowUserToAddRows = false;
            this.dataGridViewFilterLive.AllowUserToDeleteRows = false;
            this.dataGridViewFilterLive.AllowUserToResizeColumns = false;
            this.dataGridViewFilterLive.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.dataGridViewFilterLive.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewFilterLive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridViewFilterLive, "dataGridViewFilterLive");
            this.dataGridViewFilterLive.EnableHeadersVisualStyles = false;
            this.dataGridViewFilterLive.Name = "dataGridViewFilterLive";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewFilterLive.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewFilterLive.RowTemplate.Height = 30;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.dataGridViewFilterLive);
            this.panel1.Name = "panel1";
            // 
            // LiveImageFilterPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.panel1);
            this.Name = "LiveImageFilterPage";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilterLive)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewFilterLive;
        private System.Windows.Forms.Panel panel1;
    }
}
