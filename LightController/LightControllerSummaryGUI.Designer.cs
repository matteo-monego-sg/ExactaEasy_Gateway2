namespace SPAMI.LightControllers
{
    partial class LightControllerSummaryGUI
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeaderChannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBright = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMaxBright = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDelay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRetrigger = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderChannel,
            this.columnHeaderMode,
            this.columnHeaderInput,
            this.columnHeaderBright,
            this.columnHeaderMaxBright,
            this.columnHeaderDelay,
            this.columnHeaderWidth,
            this.columnHeaderRetrigger,
            this.columnHeaderE,
            this.columnHeaderP});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = "listViewGroup3";
            this.listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(685, 286);
            this.listView.TabIndex = 33;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderChannel
            // 
            this.columnHeaderChannel.Text = "Channel";
            this.columnHeaderChannel.Width = 51;
            // 
            // columnHeaderMode
            // 
            this.columnHeaderMode.Text = "Mode";
            this.columnHeaderMode.Width = 80;
            // 
            // columnHeaderInput
            // 
            this.columnHeaderInput.Text = "Input";
            this.columnHeaderInput.Width = 69;
            // 
            // columnHeaderBright
            // 
            this.columnHeaderBright.Text = "Bright [A]";
            this.columnHeaderBright.Width = 78;
            // 
            // columnHeaderMaxBright
            // 
            this.columnHeaderMaxBright.Text = "Max Bright [A]";
            this.columnHeaderMaxBright.Width = 79;
            // 
            // columnHeaderDelay
            // 
            this.columnHeaderDelay.Text = "Delay [ms]";
            this.columnHeaderDelay.Width = 73;
            // 
            // columnHeaderWidth
            // 
            this.columnHeaderWidth.Text = "Pulse Width [ms]";
            this.columnHeaderWidth.Width = 95;
            // 
            // columnHeaderRetrigger
            // 
            this.columnHeaderRetrigger.Text = "Retrigger [ms]";
            this.columnHeaderRetrigger.Width = 77;
            // 
            // columnHeaderE
            // 
            this.columnHeaderE.Text = "E";
            this.columnHeaderE.Width = 37;
            // 
            // columnHeaderP
            // 
            this.columnHeaderP.Text = "P";
            this.columnHeaderP.Width = 38;
            // 
            // LightControllerSummaryGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.listView);
            this.Name = "LightControllerSummaryGUI";
            this.Size = new System.Drawing.Size(685, 286);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeaderChannel;
        private System.Windows.Forms.ColumnHeader columnHeaderMode;
        private System.Windows.Forms.ColumnHeader columnHeaderInput;
        private System.Windows.Forms.ColumnHeader columnHeaderBright;
        private System.Windows.Forms.ColumnHeader columnHeaderMaxBright;
        private System.Windows.Forms.ColumnHeader columnHeaderDelay;
        private System.Windows.Forms.ColumnHeader columnHeaderRetrigger;
        private System.Windows.Forms.ColumnHeader columnHeaderE;
        private System.Windows.Forms.ColumnHeader columnHeaderP;
        private System.Windows.Forms.ColumnHeader columnHeaderWidth;
    }
}
