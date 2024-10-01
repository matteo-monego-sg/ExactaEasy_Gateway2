using Hvld.Controls;

namespace HvldTest
{
    partial class FrmMain
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.hvldSingleDisplay = new Hvld.Controls.HvldDisplayControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PbxStationMainImage = new System.Windows.Forms.PictureBox();
            this.TxtDebug = new System.Windows.Forms.TextBox();
            this.TlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.BtnSimulate = new System.Windows.Forms.Button();
            this.BtnReloadFile = new System.Windows.Forms.Button();
            this.BtnLoadFile = new System.Windows.Forms.Button();
            this.DataGridFrame = new System.Windows.Forms.DataGridView();
            this.TxtSelectedFile = new System.Windows.Forms.TextBox();
            this.TlpStacker = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbxStationMainImage)).BeginInit();
            this.TlpBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // SplitContainer
            // 
            this.SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.splitContainer1);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.TlpBottom);
            this.SplitContainer.Size = new System.Drawing.Size(1388, 905);
            this.SplitContainer.SplitterDistance = 409;
            this.SplitContainer.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.hvldSingleDisplay);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1388, 409);
            this.splitContainer1.SplitterDistance = 894;
            this.splitContainer1.TabIndex = 1;
            // 
            // hvldSingleDisplay
            // 
            this.hvldSingleDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.hvldSingleDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hvldSingleDisplay.EnableGlobalAntialising = false;
            this.hvldSingleDisplay.Location = new System.Drawing.Point(0, 0);
            this.hvldSingleDisplay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.hvldSingleDisplay.Name = "hvldSingleDisplay";
            this.hvldSingleDisplay.Size = new System.Drawing.Size(892, 407);
            this.hvldSingleDisplay.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PbxStationMainImage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TxtDebug, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(488, 407);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // PbxStationMainImage
            // 
            this.PbxStationMainImage.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.PbxStationMainImage, 2);
            this.PbxStationMainImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PbxStationMainImage.Location = new System.Drawing.Point(3, 3);
            this.PbxStationMainImage.Name = "PbxStationMainImage";
            this.PbxStationMainImage.Size = new System.Drawing.Size(482, 197);
            this.PbxStationMainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbxStationMainImage.TabIndex = 0;
            this.PbxStationMainImage.TabStop = false;
            // 
            // TxtDebug
            // 
            this.TxtDebug.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.TxtDebug, 2);
            this.TxtDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtDebug.ForeColor = System.Drawing.Color.Gold;
            this.TxtDebug.Location = new System.Drawing.Point(3, 206);
            this.TxtDebug.Multiline = true;
            this.TxtDebug.Name = "TxtDebug";
            this.TxtDebug.ReadOnly = true;
            this.TxtDebug.Size = new System.Drawing.Size(482, 198);
            this.TxtDebug.TabIndex = 26;
            // 
            // TlpBottom
            // 
            this.TlpBottom.ColumnCount = 4;
            this.TlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.95761F));
            this.TlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.0424F));
            this.TlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 181F));
            this.TlpBottom.Controls.Add(this.BtnSimulate, 3, 0);
            this.TlpBottom.Controls.Add(this.BtnReloadFile, 2, 0);
            this.TlpBottom.Controls.Add(this.BtnLoadFile, 0, 0);
            this.TlpBottom.Controls.Add(this.DataGridFrame, 0, 1);
            this.TlpBottom.Controls.Add(this.TxtSelectedFile, 1, 0);
            this.TlpBottom.Controls.Add(this.TlpStacker, 0, 2);
            this.TlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpBottom.Location = new System.Drawing.Point(0, 0);
            this.TlpBottom.Name = "TlpBottom";
            this.TlpBottom.RowCount = 5;
            this.TlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.TlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.TlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.TlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.TlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TlpBottom.Size = new System.Drawing.Size(1386, 490);
            this.TlpBottom.TabIndex = 4;
            // 
            // BtnSimulate
            // 
            this.BtnSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnSimulate.Location = new System.Drawing.Point(1207, 3);
            this.BtnSimulate.Name = "BtnSimulate";
            this.BtnSimulate.Size = new System.Drawing.Size(176, 38);
            this.BtnSimulate.TabIndex = 13;
            this.BtnSimulate.Text = "Simulazione";
            this.BtnSimulate.UseVisualStyleBackColor = true;
            this.BtnSimulate.Click += new System.EventHandler(this.BtnSimulate_Click);
            // 
            // BtnReloadFile
            // 
            this.BtnReloadFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnReloadFile.Location = new System.Drawing.Point(1025, 3);
            this.BtnReloadFile.Name = "BtnReloadFile";
            this.BtnReloadFile.Size = new System.Drawing.Size(176, 38);
            this.BtnReloadFile.TabIndex = 11;
            this.BtnReloadFile.Text = "Ricarica frame";
            this.BtnReloadFile.UseVisualStyleBackColor = true;
            this.BtnReloadFile.Click += new System.EventHandler(this.BtnReloadFile_Click);
            // 
            // BtnLoadFile
            // 
            this.BtnLoadFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoadFile.Location = new System.Drawing.Point(3, 3);
            this.BtnLoadFile.Name = "BtnLoadFile";
            this.BtnLoadFile.Size = new System.Drawing.Size(167, 38);
            this.BtnLoadFile.TabIndex = 0;
            this.BtnLoadFile.Text = "Carica frame da file...";
            this.BtnLoadFile.UseVisualStyleBackColor = true;
            this.BtnLoadFile.Click += new System.EventHandler(this.BtnLoadFile_Click);
            // 
            // DataGridFrame
            // 
            this.DataGridFrame.AllowUserToAddRows = false;
            this.DataGridFrame.AllowUserToDeleteRows = false;
            this.DataGridFrame.AllowUserToResizeRows = false;
            this.DataGridFrame.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridFrame.BackgroundColor = System.Drawing.Color.Black;
            this.DataGridFrame.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TlpBottom.SetColumnSpan(this.DataGridFrame, 4);
            this.DataGridFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridFrame.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridFrame.GridColor = System.Drawing.Color.Blue;
            this.DataGridFrame.Location = new System.Drawing.Point(3, 47);
            this.DataGridFrame.MultiSelect = false;
            this.DataGridFrame.Name = "DataGridFrame";
            this.DataGridFrame.ReadOnly = true;
            this.DataGridFrame.RowHeadersVisible = false;
            this.DataGridFrame.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridFrame.Size = new System.Drawing.Size(1380, 51);
            this.DataGridFrame.TabIndex = 3;
            this.DataGridFrame.SelectionChanged += new System.EventHandler(this.DataGridFrame_SelectionChanged);
            // 
            // TxtSelectedFile
            // 
            this.TxtSelectedFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtSelectedFile.Enabled = false;
            this.TxtSelectedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSelectedFile.Location = new System.Drawing.Point(176, 3);
            this.TxtSelectedFile.Multiline = true;
            this.TxtSelectedFile.Name = "TxtSelectedFile";
            this.TxtSelectedFile.ReadOnly = true;
            this.TxtSelectedFile.Size = new System.Drawing.Size(843, 38);
            this.TxtSelectedFile.TabIndex = 2;
            // 
            // TlpStacker
            // 
            this.TlpStacker.AutoScroll = true;
            this.TlpStacker.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TlpStacker.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.TlpStacker.ColumnCount = 1;
            this.TlpBottom.SetColumnSpan(this.TlpStacker, 4);
            this.TlpStacker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpStacker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpStacker.Location = new System.Drawing.Point(3, 104);
            this.TlpStacker.Name = "TlpStacker";
            this.TlpStacker.RowCount = 1;
            this.TlpBottom.SetRowSpan(this.TlpStacker, 3);
            this.TlpStacker.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpStacker.Size = new System.Drawing.Size(1380, 383);
            this.TlpStacker.TabIndex = 12;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 905);
            this.Controls.Add(this.SplitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HVLD 2.0 TEST";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbxStationMainImage)).EndInit();
            this.TlpBottom.ResumeLayout(false);
            this.TlpBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer SplitContainer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox PbxStationMainImage;
        private System.Windows.Forms.TableLayoutPanel TlpBottom;
        private System.Windows.Forms.Button BtnReloadFile;
        private System.Windows.Forms.Button BtnLoadFile;
        private System.Windows.Forms.DataGridView DataGridFrame;
        private System.Windows.Forms.TextBox TxtSelectedFile;
        private System.Windows.Forms.TableLayoutPanel TlpStacker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox TxtDebug;
        private HvldDisplayControl hvldSingleDisplay;
        private System.Windows.Forms.Button BtnSimulate;
    }
}

