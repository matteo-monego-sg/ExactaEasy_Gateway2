
namespace HvldTest
{
    partial class OptrelSignalTweaker
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
            this.ChkShowBorder = new System.Windows.Forms.CheckBox();
            this.PnlBorderColor = new System.Windows.Forms.Panel();
            this.PnlSignalColor = new System.Windows.Forms.Panel();
            this.ChkAddBoundingLines = new System.Windows.Forms.CheckBox();
            this.BtnSimpleClamp = new System.Windows.Forms.Button();
            this.BtnAccurateClamp = new System.Windows.Forms.Button();
            this.BtnRemoveClamp = new System.Windows.Forms.Button();
            this.TlpTweaker = new System.Windows.Forms.TableLayoutPanel();
            this.LblSignalId = new System.Windows.Forms.Label();
            this.LblSignalName = new System.Windows.Forms.Label();
            this.ChkAntialiasing = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NudThickness = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.DataGridSignal = new System.Windows.Forms.DataGridView();
            this.TxtDebug = new System.Windows.Forms.TextBox();
            this.BtnGetImage = new System.Windows.Forms.Button();
            this.TlpTweaker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSignal)).BeginInit();
            this.SuspendLayout();
            // 
            // ChkShowBorder
            // 
            this.ChkShowBorder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChkShowBorder.AutoSize = true;
            this.ChkShowBorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkShowBorder.ForeColor = System.Drawing.Color.Yellow;
            this.ChkShowBorder.Location = new System.Drawing.Point(105, 36);
            this.ChkShowBorder.Name = "ChkShowBorder";
            this.ChkShowBorder.Size = new System.Drawing.Size(97, 17);
            this.ChkShowBorder.TabIndex = 11;
            this.ChkShowBorder.Text = "Show border";
            this.ChkShowBorder.UseVisualStyleBackColor = true;
            this.ChkShowBorder.CheckedChanged += new System.EventHandler(this.ChkShowBorder_CheckedChanged);
            // 
            // PnlBorderColor
            // 
            this.PnlBorderColor.BackColor = System.Drawing.Color.White;
            this.PnlBorderColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlBorderColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlBorderColor.Location = new System.Drawing.Point(105, 93);
            this.PnlBorderColor.Name = "PnlBorderColor";
            this.PnlBorderColor.Size = new System.Drawing.Size(97, 24);
            this.PnlBorderColor.TabIndex = 12;
            this.PnlBorderColor.Click += new System.EventHandler(this.PnlBorderColor_Click);
            // 
            // PnlSignalColor
            // 
            this.PnlSignalColor.BackColor = System.Drawing.Color.White;
            this.PnlSignalColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlSignalColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlSignalColor.Location = new System.Drawing.Point(105, 123);
            this.PnlSignalColor.Name = "PnlSignalColor";
            this.PnlSignalColor.Size = new System.Drawing.Size(97, 24);
            this.PnlSignalColor.TabIndex = 13;
            this.PnlSignalColor.Click += new System.EventHandler(this.PnlSignalColor_Click);
            // 
            // ChkAddBoundingLines
            // 
            this.ChkAddBoundingLines.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChkAddBoundingLines.AutoSize = true;
            this.ChkAddBoundingLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkAddBoundingLines.ForeColor = System.Drawing.Color.Yellow;
            this.ChkAddBoundingLines.Location = new System.Drawing.Point(208, 36);
            this.ChkAddBoundingLines.Name = "ChkAddBoundingLines";
            this.ChkAddBoundingLines.Size = new System.Drawing.Size(145, 17);
            this.ChkAddBoundingLines.TabIndex = 17;
            this.ChkAddBoundingLines.Text = "Max/Min dotted lines";
            this.ChkAddBoundingLines.UseVisualStyleBackColor = true;
            this.ChkAddBoundingLines.CheckedChanged += new System.EventHandler(this.ChkAddBoundingLines_CheckedChanged);
            // 
            // BtnSimpleClamp
            // 
            this.BtnSimpleClamp.BackColor = System.Drawing.Color.Black;
            this.BtnSimpleClamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSimpleClamp.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.BtnSimpleClamp.Location = new System.Drawing.Point(208, 63);
            this.BtnSimpleClamp.Name = "BtnSimpleClamp";
            this.BtnSimpleClamp.Size = new System.Drawing.Size(159, 24);
            this.BtnSimpleClamp.TabIndex = 20;
            this.BtnSimpleClamp.Text = "Simple clamping";
            this.BtnSimpleClamp.UseVisualStyleBackColor = false;
            this.BtnSimpleClamp.Click += new System.EventHandler(this.BtnSimpleClamp_Click);
            // 
            // BtnAccurateClamp
            // 
            this.BtnAccurateClamp.BackColor = System.Drawing.Color.Black;
            this.BtnAccurateClamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAccurateClamp.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.BtnAccurateClamp.Location = new System.Drawing.Point(208, 93);
            this.BtnAccurateClamp.Name = "BtnAccurateClamp";
            this.BtnAccurateClamp.Size = new System.Drawing.Size(159, 24);
            this.BtnAccurateClamp.TabIndex = 21;
            this.BtnAccurateClamp.Text = "Accurate clamping";
            this.BtnAccurateClamp.UseVisualStyleBackColor = false;
            this.BtnAccurateClamp.Click += new System.EventHandler(this.BtnAccurateClamp_Click);
            // 
            // BtnRemoveClamp
            // 
            this.BtnRemoveClamp.BackColor = System.Drawing.Color.Black;
            this.BtnRemoveClamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRemoveClamp.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.BtnRemoveClamp.Location = new System.Drawing.Point(208, 123);
            this.BtnRemoveClamp.Name = "BtnRemoveClamp";
            this.BtnRemoveClamp.Size = new System.Drawing.Size(159, 24);
            this.BtnRemoveClamp.TabIndex = 22;
            this.BtnRemoveClamp.Text = "Remove any clamping";
            this.BtnRemoveClamp.UseVisualStyleBackColor = false;
            this.BtnRemoveClamp.Click += new System.EventHandler(this.BtnRemoveClamp_Click);
            // 
            // TlpTweaker
            // 
            this.TlpTweaker.BackColor = System.Drawing.Color.Black;
            this.TlpTweaker.ColumnCount = 5;
            this.TlpTweaker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTweaker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTweaker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTweaker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTweaker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpTweaker.Controls.Add(this.LblSignalId, 0, 0);
            this.TlpTweaker.Controls.Add(this.LblSignalName, 1, 0);
            this.TlpTweaker.Controls.Add(this.ChkAntialiasing, 0, 1);
            this.TlpTweaker.Controls.Add(this.ChkShowBorder, 1, 1);
            this.TlpTweaker.Controls.Add(this.ChkAddBoundingLines, 2, 1);
            this.TlpTweaker.Controls.Add(this.label1, 0, 2);
            this.TlpTweaker.Controls.Add(this.label6, 0, 3);
            this.TlpTweaker.Controls.Add(this.NudThickness, 1, 2);
            this.TlpTweaker.Controls.Add(this.PnlBorderColor, 1, 3);
            this.TlpTweaker.Controls.Add(this.label5, 0, 4);
            this.TlpTweaker.Controls.Add(this.PnlSignalColor, 1, 4);
            this.TlpTweaker.Controls.Add(this.BtnSimpleClamp, 2, 2);
            this.TlpTweaker.Controls.Add(this.BtnAccurateClamp, 2, 3);
            this.TlpTweaker.Controls.Add(this.BtnRemoveClamp, 2, 4);
            this.TlpTweaker.Controls.Add(this.DataGridSignal, 0, 5);
            this.TlpTweaker.Controls.Add(this.TxtDebug, 4, 1);
            this.TlpTweaker.Controls.Add(this.BtnGetImage, 3, 2);
            this.TlpTweaker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpTweaker.Location = new System.Drawing.Point(0, 0);
            this.TlpTweaker.Name = "TlpTweaker";
            this.TlpTweaker.RowCount = 7;
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TlpTweaker.Size = new System.Drawing.Size(735, 209);
            this.TlpTweaker.TabIndex = 11;
            // 
            // LblSignalId
            // 
            this.LblSignalId.AutoSize = true;
            this.LblSignalId.BackColor = System.Drawing.Color.Black;
            this.LblSignalId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblSignalId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblSignalId.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSignalId.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.LblSignalId.Location = new System.Drawing.Point(0, 0);
            this.LblSignalId.Margin = new System.Windows.Forms.Padding(0);
            this.LblSignalId.Name = "LblSignalId";
            this.LblSignalId.Size = new System.Drawing.Size(102, 30);
            this.LblSignalId.TabIndex = 23;
            this.LblSignalId.Text = "Signal ID";
            this.LblSignalId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LblSignalId.Click += new System.EventHandler(this.LblSignalId_Click);
            // 
            // LblSignalName
            // 
            this.LblSignalName.AutoSize = true;
            this.LblSignalName.BackColor = System.Drawing.Color.Black;
            this.LblSignalName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TlpTweaker.SetColumnSpan(this.LblSignalName, 4);
            this.LblSignalName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblSignalName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSignalName.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.LblSignalName.Location = new System.Drawing.Point(102, 0);
            this.LblSignalName.Margin = new System.Windows.Forms.Padding(0);
            this.LblSignalName.Name = "LblSignalName";
            this.LblSignalName.Size = new System.Drawing.Size(633, 30);
            this.LblSignalName.TabIndex = 0;
            this.LblSignalName.Text = "Signal Name";
            this.LblSignalName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LblSignalName.Click += new System.EventHandler(this.LblSignalName_Click);
            // 
            // ChkAntialiasing
            // 
            this.ChkAntialiasing.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChkAntialiasing.AutoSize = true;
            this.ChkAntialiasing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkAntialiasing.ForeColor = System.Drawing.Color.Yellow;
            this.ChkAntialiasing.Location = new System.Drawing.Point(3, 36);
            this.ChkAntialiasing.Name = "ChkAntialiasing";
            this.ChkAntialiasing.Size = new System.Drawing.Size(91, 17);
            this.ChkAntialiasing.TabIndex = 18;
            this.ChkAntialiasing.Text = "Antialiasing";
            this.ChkAntialiasing.UseVisualStyleBackColor = true;
            this.ChkAntialiasing.CheckedChanged += new System.EventHandler(this.ChkAntialiasing_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(0, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Border thickness";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Yellow;
            this.label6.Location = new System.Drawing.Point(0, 98);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Border color";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NudThickness
            // 
            this.NudThickness.BackColor = System.Drawing.Color.Black;
            this.NudThickness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NudThickness.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NudThickness.ForeColor = System.Drawing.Color.Yellow;
            this.NudThickness.Location = new System.Drawing.Point(105, 63);
            this.NudThickness.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NudThickness.Name = "NudThickness";
            this.NudThickness.Size = new System.Drawing.Size(97, 20);
            this.NudThickness.TabIndex = 19;
            this.NudThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NudThickness.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NudThickness.ValueChanged += new System.EventHandler(this.NudThickness_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Yellow;
            this.label5.Location = new System.Drawing.Point(0, 128);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Signal color";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataGridSignal
            // 
            this.DataGridSignal.AllowUserToAddRows = false;
            this.DataGridSignal.AllowUserToDeleteRows = false;
            this.DataGridSignal.AllowUserToResizeRows = false;
            this.DataGridSignal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridSignal.BackgroundColor = System.Drawing.Color.Gray;
            this.DataGridSignal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TlpTweaker.SetColumnSpan(this.DataGridSignal, 5);
            this.DataGridSignal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridSignal.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridSignal.GridColor = System.Drawing.Color.Blue;
            this.DataGridSignal.Location = new System.Drawing.Point(3, 153);
            this.DataGridSignal.MultiSelect = false;
            this.DataGridSignal.Name = "DataGridSignal";
            this.DataGridSignal.ReadOnly = true;
            this.DataGridSignal.RowHeadersVisible = false;
            this.TlpTweaker.SetRowSpan(this.DataGridSignal, 2);
            this.DataGridSignal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridSignal.Size = new System.Drawing.Size(729, 54);
            this.DataGridSignal.TabIndex = 24;
            // 
            // TxtDebug
            // 
            this.TxtDebug.BackColor = System.Drawing.Color.Black;
            this.TxtDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtDebug.ForeColor = System.Drawing.Color.Gold;
            this.TxtDebug.Location = new System.Drawing.Point(538, 33);
            this.TxtDebug.Multiline = true;
            this.TxtDebug.Name = "TxtDebug";
            this.TxtDebug.ReadOnly = true;
            this.TlpTweaker.SetRowSpan(this.TxtDebug, 4);
            this.TxtDebug.Size = new System.Drawing.Size(194, 114);
            this.TxtDebug.TabIndex = 25;
            // 
            // BtnGetImage
            // 
            this.BtnGetImage.BackColor = System.Drawing.Color.Black;
            this.BtnGetImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGetImage.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.BtnGetImage.Location = new System.Drawing.Point(373, 63);
            this.BtnGetImage.Name = "BtnGetImage";
            this.BtnGetImage.Size = new System.Drawing.Size(159, 24);
            this.BtnGetImage.TabIndex = 26;
            this.BtnGetImage.Text = "Get image";
            this.BtnGetImage.UseVisualStyleBackColor = false;
            this.BtnGetImage.Click += new System.EventHandler(this.BtnGetImage_Click);
            // 
            // OptrelSignalTweaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TlpTweaker);
            this.Name = "OptrelSignalTweaker";
            this.Size = new System.Drawing.Size(735, 209);
            this.TlpTweaker.ResumeLayout(false);
            this.TlpTweaker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSignal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox ChkShowBorder;
        private System.Windows.Forms.Panel PnlBorderColor;
        private System.Windows.Forms.Panel PnlSignalColor;
        private System.Windows.Forms.CheckBox ChkAddBoundingLines;
        private System.Windows.Forms.Button BtnSimpleClamp;
        private System.Windows.Forms.Button BtnAccurateClamp;
        private System.Windows.Forms.Button BtnRemoveClamp;
        private System.Windows.Forms.TableLayoutPanel TlpTweaker;
        private System.Windows.Forms.Label LblSignalName;
        private System.Windows.Forms.CheckBox ChkAntialiasing;
        private System.Windows.Forms.NumericUpDown NudThickness;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LblSignalId;
        private System.Windows.Forms.DataGridView DataGridSignal;
        private System.Windows.Forms.TextBox TxtDebug;
        private System.Windows.Forms.Button BtnGetImage;
    }
}
