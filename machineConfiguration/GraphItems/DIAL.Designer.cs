namespace machineConfiguration
{
    partial class DIAL
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelDefect = new System.Windows.Forms.Label();
            this.labelXY = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 49);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(340, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // labelDefect
            // 
            this.labelDefect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDefect.Location = new System.Drawing.Point(3, 73);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(340, 23);
            this.labelDefect.TabIndex = 3;
            this.labelDefect.Text = "defect";
            this.labelDefect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelXY
            // 
            this.labelXY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelXY.AutoSize = true;
            this.labelXY.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXY.Location = new System.Drawing.Point(319, 121);
            this.labelXY.Name = "labelXY";
            this.labelXY.Size = new System.Drawing.Size(24, 12);
            this.labelXY.TabIndex = 4;
            this.labelXY.Text = "[0;0]";
            // 
            // DIAL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelXY);
            this.Controls.Add(this.labelDefect);
            this.Controls.Add(this.comboBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "DIAL";
            this.Size = new System.Drawing.Size(346, 143);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelDefect;
        private System.Windows.Forms.Label labelXY;
    }
}
