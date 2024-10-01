namespace ExactaEasy {
    partial class SaveMenu {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveMenu));
            this.rbtGood = new System.Windows.Forms.RadioButton();
            this.rbtReject = new System.Windows.Forms.RadioButton();
            this.rbtAny = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbHowMuch = new System.Windows.Forms.NumericTextBox(this.components);
            this.btnToSaveDown = new System.Windows.Forms.Button();
            this.btnToSaveUp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.rbtGood.Location = new System.Drawing.Point(0, 23);
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
            this.rbtReject.Location = new System.Drawing.Point(72, 23);
            this.rbtReject.Name = "rbtReject";
            this.rbtReject.Size = new System.Drawing.Size(72, 57);
            this.rbtReject.TabIndex = 16;
            this.rbtReject.TabStop = true;
            this.rbtReject.Text = "Reject";
            this.rbtReject.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtReject.UseVisualStyleBackColor = true;
            this.rbtReject.CheckedChanged += new System.EventHandler(this.rbtReject_CheckedChanged);
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
            this.rbtAny.Location = new System.Drawing.Point(144, 23);
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
            this.label1.Location = new System.Drawing.Point(230, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "TO SAVE";
            // 
            // ntbHowMuch
            // 
            this.ntbHowMuch.AutoValidate = true;
            this.ntbHowMuch.AutoValidationTime = 1000;
            this.ntbHowMuch.BackColor = System.Drawing.Color.White;
            this.ntbHowMuch.DecimalPlaces = 0;
            this.ntbHowMuch.Dock = System.Windows.Forms.DockStyle.Left;
            this.ntbHowMuch.EnableErrorValue = false;
            this.ntbHowMuch.EnableWarningValue = false;
            this.ntbHowMuch.ErrorColor = System.Drawing.Color.OrangeRed;
            this.ntbHowMuch.ErrorValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ntbHowMuch.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ntbHowMuch.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbHowMuch.InterceptArrowKeys = true;
            this.ntbHowMuch.Location = new System.Drawing.Point(269, 23);
            this.ntbHowMuch.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.ntbHowMuch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbHowMuch.Name = "ntbHowMuch";
            this.ntbHowMuch.Size = new System.Drawing.Size(100, 53);
            this.ntbHowMuch.TabIndex = 19;
            this.ntbHowMuch.Text = "10";
            this.ntbHowMuch.ThousandsSeparator = false;
            this.ntbHowMuch.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ntbHowMuch.WarningColor = System.Drawing.Color.Gold;
            this.ntbHowMuch.WarningValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnToSaveDown
            // 
            this.btnToSaveDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToSaveDown.BackgroundImage")));
            this.btnToSaveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnToSaveDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnToSaveDown.FlatAppearance.BorderSize = 0;
            this.btnToSaveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToSaveDown.Location = new System.Drawing.Point(216, 23);
            this.btnToSaveDown.Name = "btnToSaveDown";
            this.btnToSaveDown.Size = new System.Drawing.Size(53, 57);
            this.btnToSaveDown.TabIndex = 18;
            this.btnToSaveDown.UseVisualStyleBackColor = true;
            this.btnToSaveDown.Click += new System.EventHandler(this.btnToSaveDown_Click);
            // 
            // btnToSaveUp
            // 
            this.btnToSaveUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToSaveUp.BackgroundImage")));
            this.btnToSaveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnToSaveUp.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnToSaveUp.FlatAppearance.BorderSize = 0;
            this.btnToSaveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToSaveUp.Location = new System.Drawing.Point(369, 23);
            this.btnToSaveUp.Name = "btnToSaveUp";
            this.btnToSaveUp.Size = new System.Drawing.Size(53, 57);
            this.btnToSaveUp.TabIndex = 17;
            this.btnToSaveUp.UseVisualStyleBackColor = true;
            this.btnToSaveUp.Click += new System.EventHandler(this.btnToSaveUp_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
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
            this.label3.Location = new System.Drawing.Point(10, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "CONDITION";
            // 
            // btnExit
            // 
            this.btnExit.AutoEllipsis = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExit.Image = global::ExactaEasy.Properties.Resources.edit_undo;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(664, 23);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 57);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExitStopCondMenu_Click);
            // 
            // SaveMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnToSaveUp);
            this.Controls.Add(this.ntbHowMuch);
            this.Controls.Add(this.btnToSaveDown);
            this.Controls.Add(this.rbtAny);
            this.Controls.Add(this.rbtReject);
            this.Controls.Add(this.rbtGood);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "SaveMenu";
            this.Size = new System.Drawing.Size(736, 80);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtGood;
        private System.Windows.Forms.RadioButton rbtReject;
        private System.Windows.Forms.RadioButton rbtAny;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericTextBox ntbHowMuch;
        private System.Windows.Forms.Button btnToSaveDown;
        private System.Windows.Forms.Button btnToSaveUp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
    }
}
