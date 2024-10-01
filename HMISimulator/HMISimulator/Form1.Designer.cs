namespace HMISimulator {
    partial class Form1 {
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

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            this.txtPos = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSetSupervisorPos = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLanguage = new System.Windows.Forms.TextBox();
            this.btnSetLanguage = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSetUserLevel = new System.Windows.Forms.ComboBox();
            this.btnSetUserLevel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRecipe = new System.Windows.Forms.TextBox();
            this.btnActiveRecipe = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbMachineMode = new System.Windows.Forms.ComboBox();
            this.btnMachineMode = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.cmbSupervisorMode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEnables = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.errDanneggiaRicetta = new System.Windows.Forms.CheckBox();
            this.errSimulaErroreRicezione = new System.Windows.Forms.CheckBox();
            this.errStatoSupervisore = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtPos
            // 
            this.txtPos.Location = new System.Drawing.Point(103, 33);
            this.txtPos.Name = "txtPos";
            this.txtPos.Size = new System.Drawing.Size(172, 20);
            this.txtPos.TabIndex = 0;
            this.txtPos.Text = "0;0;1366;738";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Position";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(289, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSetSupervisorPos
            // 
            this.btnSetSupervisorPos.Location = new System.Drawing.Point(289, 27);
            this.btnSetSupervisorPos.Name = "btnSetSupervisorPos";
            this.btnSetSupervisorPos.Size = new System.Drawing.Size(143, 31);
            this.btnSetSupervisorPos.TabIndex = 3;
            this.btnSetSupervisorPos.Text = "SetSupervisorPos";
            this.btnSetSupervisorPos.UseVisualStyleBackColor = true;
            this.btnSetSupervisorPos.Click += new System.EventHandler(this.btnSetSupervisorPos_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Language";
            // 
            // txtLanguage
            // 
            this.txtLanguage.Location = new System.Drawing.Point(103, 74);
            this.txtLanguage.Name = "txtLanguage";
            this.txtLanguage.Size = new System.Drawing.Size(172, 20);
            this.txtLanguage.TabIndex = 5;
            this.txtLanguage.Text = "it";
            // 
            // btnSetLanguage
            // 
            this.btnSetLanguage.Location = new System.Drawing.Point(289, 68);
            this.btnSetLanguage.Name = "btnSetLanguage";
            this.btnSetLanguage.Size = new System.Drawing.Size(143, 31);
            this.btnSetLanguage.TabIndex = 6;
            this.btnSetLanguage.Text = "SetLanguage";
            this.btnSetLanguage.UseVisualStyleBackColor = true;
            this.btnSetLanguage.Click += new System.EventHandler(this.btnSetLanguage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "User level";
            // 
            // cmbSetUserLevel
            // 
            this.cmbSetUserLevel.FormattingEnabled = true;
            this.cmbSetUserLevel.Location = new System.Drawing.Point(103, 113);
            this.cmbSetUserLevel.Name = "cmbSetUserLevel";
            this.cmbSetUserLevel.Size = new System.Drawing.Size(172, 21);
            this.cmbSetUserLevel.TabIndex = 8;
            // 
            // btnSetUserLevel
            // 
            this.btnSetUserLevel.Location = new System.Drawing.Point(289, 107);
            this.btnSetUserLevel.Name = "btnSetUserLevel";
            this.btnSetUserLevel.Size = new System.Drawing.Size(143, 31);
            this.btnSetUserLevel.TabIndex = 9;
            this.btnSetUserLevel.Text = "SetUserLevel";
            this.btnSetUserLevel.UseVisualStyleBackColor = true;
            this.btnSetUserLevel.Click += new System.EventHandler(this.btnSetUserLevel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Recipe";
            // 
            // txtRecipe
            // 
            this.txtRecipe.Location = new System.Drawing.Point(103, 152);
            this.txtRecipe.Name = "txtRecipe";
            this.txtRecipe.Size = new System.Drawing.Size(172, 20);
            this.txtRecipe.TabIndex = 11;
            this.txtRecipe.Text = "ricettaTestMultipleCam.xml";
            // 
            // btnActiveRecipe
            // 
            this.btnActiveRecipe.Location = new System.Drawing.Point(289, 146);
            this.btnActiveRecipe.Name = "btnActiveRecipe";
            this.btnActiveRecipe.Size = new System.Drawing.Size(143, 31);
            this.btnActiveRecipe.TabIndex = 12;
            this.btnActiveRecipe.Text = "SetActiveRecipe";
            this.btnActiveRecipe.UseVisualStyleBackColor = true;
            this.btnActiveRecipe.Click += new System.EventHandler(this.btnActiveRecipe_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Machine mode";
            // 
            // cmbMachineMode
            // 
            this.cmbMachineMode.FormattingEnabled = true;
            this.cmbMachineMode.Location = new System.Drawing.Point(103, 212);
            this.cmbMachineMode.Name = "cmbMachineMode";
            this.cmbMachineMode.Size = new System.Drawing.Size(172, 21);
            this.cmbMachineMode.TabIndex = 14;
            // 
            // btnMachineMode
            // 
            this.btnMachineMode.Location = new System.Drawing.Point(289, 206);
            this.btnMachineMode.Name = "btnMachineMode";
            this.btnMachineMode.Size = new System.Drawing.Size(143, 31);
            this.btnMachineMode.TabIndex = 15;
            this.btnMachineMode.Text = "SetMachineMode";
            this.btnMachineMode.UseVisualStyleBackColor = true;
            this.btnMachineMode.Click += new System.EventHandler(this.btnMachineMode_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(289, 319);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(143, 31);
            this.button2.TabIndex = 16;
            this.button2.Text = "SetMachineInfo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(289, 245);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(143, 31);
            this.button3.TabIndex = 17;
            this.button3.Text = "SetSupervisorOnTop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(289, 282);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(143, 31);
            this.button4.TabIndex = 18;
            this.button4.Text = "SetSupervisorHide";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cmbSupervisorMode
            // 
            this.cmbSupervisorMode.FormattingEnabled = true;
            this.cmbSupervisorMode.Location = new System.Drawing.Point(103, 251);
            this.cmbSupervisorMode.Name = "cmbSupervisorMode";
            this.cmbSupervisorMode.Size = new System.Drawing.Size(172, 21);
            this.cmbSupervisorMode.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Supervisor mode";
            // 
            // txtEnables
            // 
            this.txtEnables.Location = new System.Drawing.Point(103, 325);
            this.txtEnables.Name = "txtEnables";
            this.txtEnables.Size = new System.Drawing.Size(172, 20);
            this.txtEnables.TabIndex = 21;
            this.txtEnables.Text = "1;1;0;0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 328);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Station Enables";
            // 
            // errDanneggiaRicetta
            // 
            this.errDanneggiaRicetta.AutoSize = true;
            this.errDanneggiaRicetta.Location = new System.Drawing.Point(103, 178);
            this.errDanneggiaRicetta.Name = "errDanneggiaRicetta";
            this.errDanneggiaRicetta.Size = new System.Drawing.Size(110, 17);
            this.errDanneggiaRicetta.TabIndex = 23;
            this.errDanneggiaRicetta.Text = "Danneggia ricetta";
            this.errDanneggiaRicetta.UseVisualStyleBackColor = true;
            this.errDanneggiaRicetta.CheckedChanged += new System.EventHandler(this.errDanneggiaRicetta_CheckedChanged);
            // 
            // errSimulaErroreRicezione
            // 
            this.errSimulaErroreRicezione.AutoSize = true;
            this.errSimulaErroreRicezione.Location = new System.Drawing.Point(18, 403);
            this.errSimulaErroreRicezione.Name = "errSimulaErroreRicezione";
            this.errSimulaErroreRicezione.Size = new System.Drawing.Size(164, 17);
            this.errSimulaErroreRicezione.TabIndex = 24;
            this.errSimulaErroreRicezione.Text = "Simula errore ricezione ricetta";
            this.errSimulaErroreRicezione.UseVisualStyleBackColor = true;
            this.errSimulaErroreRicezione.CheckedChanged += new System.EventHandler(this.errSimulaErroreRicezione_CheckedChanged);
            // 
            // errStatoSupervisore
            // 
            this.errStatoSupervisore.AutoSize = true;
            this.errStatoSupervisore.Location = new System.Drawing.Point(18, 426);
            this.errStatoSupervisore.Name = "errStatoSupervisore";
            this.errStatoSupervisore.Size = new System.Drawing.Size(90, 17);
            this.errStatoSupervisore.TabIndex = 25;
            this.errStatoSupervisore.Text = "Simula errore ";
            this.errStatoSupervisore.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 475);
            this.Controls.Add(this.errStatoSupervisore);
            this.Controls.Add(this.errSimulaErroreRicezione);
            this.Controls.Add(this.errDanneggiaRicetta);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtEnables);
            this.Controls.Add(this.cmbSupervisorMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnMachineMode);
            this.Controls.Add(this.cmbMachineMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnActiveRecipe);
            this.Controls.Add(this.txtRecipe);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSetUserLevel);
            this.Controls.Add(this.cmbSetUserLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSetLanguage);
            this.Controls.Add(this.txtLanguage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSetSupervisorPos);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPos);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSetSupervisorPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLanguage;
        private System.Windows.Forms.Button btnSetLanguage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSetUserLevel;
        private System.Windows.Forms.Button btnSetUserLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRecipe;
        private System.Windows.Forms.Button btnActiveRecipe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbMachineMode;
        private System.Windows.Forms.Button btnMachineMode;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cmbSupervisorMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEnables;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox errDanneggiaRicetta;
        private System.Windows.Forms.CheckBox errSimulaErroreRicezione;
        private System.Windows.Forms.CheckBox errStatoSupervisore;
    }
}

