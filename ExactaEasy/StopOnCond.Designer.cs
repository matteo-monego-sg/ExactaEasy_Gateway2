namespace ExactaEasy {
    partial class StopOnCond {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StopOnCond));
            this.ntbHead = new System.Windows.Forms.NumericTextBox(this.components);
            this.bttHeadDown = new System.Windows.Forms.Button();
            this.bttHeadUp = new System.Windows.Forms.Button();
            this.rbtGood = new System.Windows.Forms.RadioButton();
            this.rbtReject = new System.Windows.Forms.RadioButton();
            this.rbtPUM = new System.Windows.Forms.RadioButton();
            this.rbtAny = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbTimeout = new System.Windows.Forms.NumericTextBox(this.components);
            this.bttTimeoutDown = new System.Windows.Forms.Button();
            this.bttTimeoutUp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExitStopCondMenu = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ntbHead
            // 
            this.ntbHead.AutoValidate = true;
            this.ntbHead.AutoValidationTime = 5000;
            this.ntbHead.BackColor = System.Drawing.Color.White;
            this.ntbHead.DecimalPlaces = 0;
            this.ntbHead.Dock = System.Windows.Forms.DockStyle.Left;
            this.ntbHead.EnableErrorValue = false;
            this.ntbHead.EnableWarningValue = false;
            this.ntbHead.ErrorColor = System.Drawing.Color.OrangeRed;
            this.ntbHead.ErrorValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ntbHead.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbHead.InterceptArrowKeys = true;
            this.ntbHead.Location = new System.Drawing.Point(53, 23);
            this.ntbHead.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.ntbHead.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbHead.Name = "ntbHead";
            this.ntbHead.Size = new System.Drawing.Size(75, 53);
            this.ntbHead.TabIndex = 12;
            this.ntbHead.Text = "1";
            this.ntbHead.ThousandsSeparator = false;
            this.ntbHead.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbHead.WarningColor = System.Drawing.Color.Gold;
            this.ntbHead.WarningValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // bttHeadDown
            // 
            this.bttHeadDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bttHeadDown.BackgroundImage")));
            this.bttHeadDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bttHeadDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttHeadDown.FlatAppearance.BorderSize = 0;
            this.bttHeadDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttHeadDown.Location = new System.Drawing.Point(0, 23);
            this.bttHeadDown.Name = "bttHeadDown";
            this.bttHeadDown.Size = new System.Drawing.Size(53, 57);
            this.bttHeadDown.TabIndex = 11;
            this.bttHeadDown.UseVisualStyleBackColor = true;
            this.bttHeadDown.Click += new System.EventHandler(this.bttHeadDown_Click);
            // 
            // bttHeadUp
            // 
            this.bttHeadUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bttHeadUp.BackgroundImage")));
            this.bttHeadUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bttHeadUp.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttHeadUp.FlatAppearance.BorderSize = 0;
            this.bttHeadUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttHeadUp.Location = new System.Drawing.Point(128, 23);
            this.bttHeadUp.Name = "bttHeadUp";
            this.bttHeadUp.Size = new System.Drawing.Size(53, 57);
            this.bttHeadUp.TabIndex = 10;
            this.bttHeadUp.UseVisualStyleBackColor = true;
            this.bttHeadUp.Click += new System.EventHandler(this.bttHeadUp_Click);
            // 
            // rbtGood
            // 
            this.rbtGood.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtGood.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtGood.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbtGood.FlatAppearance.BorderSize = 0;
            this.rbtGood.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtGood.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbtGood.Image = global::ExactaEasy.Properties.Resources.checkmark;
            this.rbtGood.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbtGood.Location = new System.Drawing.Point(181, 23);
            this.rbtGood.Name = "rbtGood";
            this.rbtGood.Size = new System.Drawing.Size(72, 57);
            this.rbtGood.TabIndex = 15;
            this.rbtGood.TabStop = true;
            this.rbtGood.Text = "Good";
            this.rbtGood.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtGood.UseVisualStyleBackColor = true;
            this.rbtGood.CheckedChanged += new System.EventHandler(this.rbtGood_CheckedChanged);
            // 
            // rbtReject
            // 
            this.rbtReject.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtReject.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbtReject.FlatAppearance.BorderSize = 0;
            this.rbtReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtReject.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbtReject.Image = global::ExactaEasy.Properties.Resources.button_fewer;
            this.rbtReject.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbtReject.Location = new System.Drawing.Point(253, 23);
            this.rbtReject.Name = "rbtReject";
            this.rbtReject.Size = new System.Drawing.Size(72, 57);
            this.rbtReject.TabIndex = 16;
            this.rbtReject.TabStop = true;
            this.rbtReject.Text = "Reject";
            this.rbtReject.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtReject.UseVisualStyleBackColor = true;
            this.rbtReject.CheckedChanged += new System.EventHandler(this.rbtReject_CheckedChanged);
            // 
            // rbtPUM
            // 
            this.rbtPUM.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtPUM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtPUM.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtPUM.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbtPUM.FlatAppearance.BorderSize = 0;
            this.rbtPUM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtPUM.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbtPUM.Image = global::ExactaEasy.Properties.Resources.nepomuk;
            this.rbtPUM.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbtPUM.Location = new System.Drawing.Point(397, 23);
            this.rbtPUM.Name = "rbtPUM";
            this.rbtPUM.Size = new System.Drawing.Size(72, 57);
            this.rbtPUM.TabIndex = 14;
            this.rbtPUM.TabStop = true;
            this.rbtPUM.Text = "P.U.M.";
            this.rbtPUM.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtPUM.UseVisualStyleBackColor = true;
            this.rbtPUM.CheckedChanged += new System.EventHandler(this.rbtPUM_CheckedChanged);
            // 
            // rbtAny
            // 
            this.rbtAny.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtAny.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtAny.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbtAny.FlatAppearance.BorderSize = 0;
            this.rbtAny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtAny.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbtAny.Image = global::ExactaEasy.Properties.Resources.roll;
            this.rbtAny.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rbtAny.Location = new System.Drawing.Point(325, 23);
            this.rbtAny.Name = "rbtAny";
            this.rbtAny.Size = new System.Drawing.Size(72, 57);
            this.rbtAny.TabIndex = 13;
            this.rbtAny.TabStop = true;
            this.rbtAny.Text = "Any";
            this.rbtAny.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtAny.UseVisualStyleBackColor = true;
            this.rbtAny.CheckedChanged += new System.EventHandler(this.rbtAny_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(476, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "TIMEOUT [sec]";
            // 
            // ntbTimeout
            // 
            this.ntbTimeout.AutoValidate = true;
            this.ntbTimeout.AutoValidationTime = 1000;
            this.ntbTimeout.BackColor = System.Drawing.Color.White;
            this.ntbTimeout.DecimalPlaces = 0;
            this.ntbTimeout.Dock = System.Windows.Forms.DockStyle.Left;
            this.ntbTimeout.EnableErrorValue = false;
            this.ntbTimeout.EnableWarningValue = false;
            this.ntbTimeout.ErrorColor = System.Drawing.Color.OrangeRed;
            this.ntbTimeout.ErrorValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ntbTimeout.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbTimeout.InterceptArrowKeys = true;
            this.ntbTimeout.Location = new System.Drawing.Point(522, 23);
            this.ntbTimeout.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.ntbTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbTimeout.Name = "ntbTimeout";
            this.ntbTimeout.Size = new System.Drawing.Size(100, 53);
            this.ntbTimeout.TabIndex = 19;
            this.ntbTimeout.Text = "300";
            this.ntbTimeout.ThousandsSeparator = false;
            this.ntbTimeout.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ntbTimeout.WarningColor = System.Drawing.Color.Gold;
            this.ntbTimeout.WarningValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // bttTimeoutDown
            // 
            this.bttTimeoutDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bttTimeoutDown.BackgroundImage")));
            this.bttTimeoutDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bttTimeoutDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttTimeoutDown.FlatAppearance.BorderSize = 0;
            this.bttTimeoutDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttTimeoutDown.Location = new System.Drawing.Point(469, 23);
            this.bttTimeoutDown.Name = "bttTimeoutDown";
            this.bttTimeoutDown.Size = new System.Drawing.Size(53, 57);
            this.bttTimeoutDown.TabIndex = 18;
            this.bttTimeoutDown.UseVisualStyleBackColor = true;
            this.bttTimeoutDown.Click += new System.EventHandler(this.bttTimeoutDown_Click);
            // 
            // bttTimeoutUp
            // 
            this.bttTimeoutUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bttTimeoutUp.BackgroundImage")));
            this.bttTimeoutUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bttTimeoutUp.Dock = System.Windows.Forms.DockStyle.Left;
            this.bttTimeoutUp.FlatAppearance.BorderSize = 0;
            this.bttTimeoutUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bttTimeoutUp.Location = new System.Drawing.Point(622, 23);
            this.bttTimeoutUp.Name = "bttTimeoutUp";
            this.bttTimeoutUp.Size = new System.Drawing.Size(53, 57);
            this.bttTimeoutUp.TabIndex = 17;
            this.bttTimeoutUp.UseVisualStyleBackColor = true;
            this.bttTimeoutUp.Click += new System.EventHandler(this.bttTimeoutUp_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 23);
            this.panel1.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(196, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "CONDITION";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "HEAD";
            // 
            // btnExitStopCondMenu
            // 
            this.btnExitStopCondMenu.AutoEllipsis = true;
            this.btnExitStopCondMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExitStopCondMenu.FlatAppearance.BorderSize = 0;
            this.btnExitStopCondMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitStopCondMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExitStopCondMenu.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExitStopCondMenu.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExitStopCondMenu.Location = new System.Drawing.Point(664, 23);
            this.btnExitStopCondMenu.Name = "btnExitStopCondMenu";
            this.btnExitStopCondMenu.Size = new System.Drawing.Size(72, 57);
            this.btnExitStopCondMenu.TabIndex = 22;
            this.btnExitStopCondMenu.Text = "Exit";
            this.btnExitStopCondMenu.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitStopCondMenu.UseVisualStyleBackColor = true;
            this.btnExitStopCondMenu.Click += new System.EventHandler(this.btnExitStopCondMenu_Click);
            // 
            // StopOnCond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.btnExitStopCondMenu);
            this.Controls.Add(this.bttTimeoutUp);
            this.Controls.Add(this.ntbTimeout);
            this.Controls.Add(this.bttTimeoutDown);
            this.Controls.Add(this.rbtPUM);
            this.Controls.Add(this.rbtAny);
            this.Controls.Add(this.rbtReject);
            this.Controls.Add(this.rbtGood);
            this.Controls.Add(this.bttHeadUp);
            this.Controls.Add(this.ntbHead);
            this.Controls.Add(this.bttHeadDown);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "StopOnCond";
            this.Size = new System.Drawing.Size(736, 80);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericTextBox ntbHead;
        private System.Windows.Forms.Button bttHeadDown;
        private System.Windows.Forms.Button bttHeadUp;
        private System.Windows.Forms.RadioButton rbtGood;
        private System.Windows.Forms.RadioButton rbtReject;
        private System.Windows.Forms.RadioButton rbtPUM;
        private System.Windows.Forms.RadioButton rbtAny;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericTextBox ntbTimeout;
        private System.Windows.Forms.Button bttTimeoutDown;
        private System.Windows.Forms.Button bttTimeoutUp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExitStopCondMenu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
