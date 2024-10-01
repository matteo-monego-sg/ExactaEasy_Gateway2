namespace SPAMI.LightControllers.GUI
{
    partial class LightControllerTesterGUI
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageEth = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lightControllerSearcher1 = new SPAMI.LightControllers.LightControllerSearcher();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.richTextBoxComLog = new System.Windows.Forms.RichTextBox();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lightControllerSummaryGUI1 = new SPAMI.LightControllers.LightControllerSummaryGUI();
            this.lightControllerConfigurator1 = new SPAMI.LightControllers.LightControllerConfigurator();
            this.tabControl.SuspendLayout();
            this.tabPageEth.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageEth);
            this.tabControl.Controls.Add(this.tabPageConfig);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(781, 500);
            this.tabControl.TabIndex = 0;
            this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageEth
            // 
            this.tabPageEth.BackColor = System.Drawing.Color.Transparent;
            this.tabPageEth.Controls.Add(this.tableLayoutPanel2);
            this.tabPageEth.ForeColor = System.Drawing.SystemColors.MenuText;
            this.tabPageEth.Location = new System.Drawing.Point(4, 22);
            this.tabPageEth.Name = "tabPageEth";
            this.tabPageEth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEth.Size = new System.Drawing.Size(773, 474);
            this.tabPageEth.TabIndex = 0;
            this.tabPageEth.Text = "Ethernet Connection";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lightControllerSearcher1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(767, 468);
            this.tableLayoutPanel2.TabIndex = 14;
            // 
            // lightControllerSearcher1
            // 
            this.lightControllerSearcher1.BackColor = System.Drawing.Color.Transparent;
            this.lightControllerSearcher1.ClassName = "LightControllerSearcher";
            this.lightControllerSearcher1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightControllerSearcher1.LightController = null;
            this.lightControllerSearcher1.Location = new System.Drawing.Point(3, 3);
            this.lightControllerSearcher1.Name = "lightControllerSearcher1";
            this.lightControllerSearcher1.Size = new System.Drawing.Size(254, 462);
            this.lightControllerSearcher1.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.buttonClearLog, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.richTextBoxComLog, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(263, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(501, 462);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClearLog.Location = new System.Drawing.Point(415, 3);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(83, 24);
            this.buttonClearLog.TabIndex = 11;
            this.buttonClearLog.Text = "Clear Log";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // richTextBoxComLog
            // 
            this.richTextBoxComLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxComLog.Location = new System.Drawing.Point(3, 33);
            this.richTextBoxComLog.Name = "richTextBoxComLog";
            this.richTextBoxComLog.Size = new System.Drawing.Size(495, 426);
            this.richTextBoxComLog.TabIndex = 12;
            this.richTextBoxComLog.Text = "";
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.tableLayoutPanel1);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(773, 474);
            this.tabPageConfig.TabIndex = 1;
            this.tabPageConfig.Text = "Configuration";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lightControllerSummaryGUI1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lightControllerConfigurator1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(767, 468);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lightControllerSummaryGUI1
            // 
            this.lightControllerSummaryGUI1.BackColor = System.Drawing.Color.Transparent;
            this.lightControllerSummaryGUI1.ClassName = "LightControllerSummaryGUI";
            this.lightControllerSummaryGUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightControllerSummaryGUI1.LightController = null;
            this.lightControllerSummaryGUI1.Location = new System.Drawing.Point(3, 163);
            this.lightControllerSummaryGUI1.Name = "lightControllerSummaryGUI1";
            this.lightControllerSummaryGUI1.Size = new System.Drawing.Size(761, 302);
            this.lightControllerSummaryGUI1.TabIndex = 0;
            // 
            // lightControllerConfigurator1
            // 
            this.lightControllerConfigurator1.BackColor = System.Drawing.Color.Transparent;
            this.lightControllerConfigurator1.ClassName = "LightControllerConfigurator";
            this.lightControllerConfigurator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightControllerConfigurator1.LightController = null;
            this.lightControllerConfigurator1.Location = new System.Drawing.Point(3, 3);
            this.lightControllerConfigurator1.Name = "lightControllerConfigurator1";
            this.lightControllerConfigurator1.Size = new System.Drawing.Size(761, 154);
            this.lightControllerConfigurator1.TabIndex = 1;
            // 
            // LightControllerTesterGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabControl);
            this.Name = "LightControllerTesterGUI";
            this.Size = new System.Drawing.Size(781, 500);
            this.tabControl.ResumeLayout(false);
            this.tabPageEth.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageEth;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.RichTextBox richTextBoxComLog;
        private LightControllerSearcher lightControllerSearcher1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LightControllerSummaryGUI lightControllerSummaryGUI1;
        private LightControllerConfigurator lightControllerConfigurator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
