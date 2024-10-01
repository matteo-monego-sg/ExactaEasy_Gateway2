namespace Gretel2spvRecipeConverter {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnCreateRecipe = new System.Windows.Forms.Button();
            this.sourceRecipe5 = new Gretel2spvRecipeConverter.SourceRecipe();
            this.sourceRecipe4 = new Gretel2spvRecipeConverter.SourceRecipe();
            this.sourceRecipe3 = new Gretel2spvRecipeConverter.SourceRecipe();
            this.sourceRecipe2 = new Gretel2spvRecipeConverter.SourceRecipe();
            this.sourceRecipe1 = new Gretel2spvRecipeConverter.SourceRecipe();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnCreateRecipe);
            this.pnlMain.Controls.Add(this.sourceRecipe5);
            this.pnlMain.Controls.Add(this.sourceRecipe4);
            this.pnlMain.Controls.Add(this.sourceRecipe3);
            this.pnlMain.Controls.Add(this.sourceRecipe2);
            this.pnlMain.Controls.Add(this.sourceRecipe1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(647, 612);
            this.pnlMain.TabIndex = 9;
            // 
            // btnCreateRecipe
            // 
            this.btnCreateRecipe.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateRecipe.Location = new System.Drawing.Point(35, 569);
            this.btnCreateRecipe.Name = "btnCreateRecipe";
            this.btnCreateRecipe.Size = new System.Drawing.Size(169, 31);
            this.btnCreateRecipe.TabIndex = 14;
            this.btnCreateRecipe.Text = "Create recipe";
            this.btnCreateRecipe.UseVisualStyleBackColor = true;
            this.btnCreateRecipe.Click += new System.EventHandler(this.btnCreateRecipe_Click);
            // 
            // sourceRecipe5
            // 
            this.sourceRecipe5.Dock = System.Windows.Forms.DockStyle.Top;
            this.sourceRecipe5.IdClient = 4;
            this.sourceRecipe5.Location = new System.Drawing.Point(0, 440);
            this.sourceRecipe5.Name = "sourceRecipe5";
            this.sourceRecipe5.Size = new System.Drawing.Size(647, 110);
            this.sourceRecipe5.TabIndex = 13;
            this.sourceRecipe5.Used = false;
            // 
            // sourceRecipe4
            // 
            this.sourceRecipe4.Dock = System.Windows.Forms.DockStyle.Top;
            this.sourceRecipe4.IdClient = 3;
            this.sourceRecipe4.Location = new System.Drawing.Point(0, 330);
            this.sourceRecipe4.Name = "sourceRecipe4";
            this.sourceRecipe4.Size = new System.Drawing.Size(647, 110);
            this.sourceRecipe4.TabIndex = 12;
            this.sourceRecipe4.Used = false;
            // 
            // sourceRecipe3
            // 
            this.sourceRecipe3.Dock = System.Windows.Forms.DockStyle.Top;
            this.sourceRecipe3.IdClient = 2;
            this.sourceRecipe3.Location = new System.Drawing.Point(0, 220);
            this.sourceRecipe3.Name = "sourceRecipe3";
            this.sourceRecipe3.Size = new System.Drawing.Size(647, 110);
            this.sourceRecipe3.TabIndex = 11;
            this.sourceRecipe3.Used = false;
            // 
            // sourceRecipe2
            // 
            this.sourceRecipe2.Dock = System.Windows.Forms.DockStyle.Top;
            this.sourceRecipe2.IdClient = 1;
            this.sourceRecipe2.Location = new System.Drawing.Point(0, 110);
            this.sourceRecipe2.Name = "sourceRecipe2";
            this.sourceRecipe2.Size = new System.Drawing.Size(647, 110);
            this.sourceRecipe2.TabIndex = 10;
            this.sourceRecipe2.Used = false;
            // 
            // sourceRecipe1
            // 
            this.sourceRecipe1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sourceRecipe1.IdClient = 0;
            this.sourceRecipe1.Location = new System.Drawing.Point(0, 0);
            this.sourceRecipe1.Name = "sourceRecipe1";
            this.sourceRecipe1.Size = new System.Drawing.Size(647, 110);
            this.sourceRecipe1.TabIndex = 9;
            this.sourceRecipe1.Used = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 612);
            this.Controls.Add(this.pnlMain);
            this.Name = "Form1";
            this.Text = "Gretel 2 SPV Recipe Converter";
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private SourceRecipe sourceRecipe5;
        private SourceRecipe sourceRecipe4;
        private SourceRecipe sourceRecipe3;
        private SourceRecipe sourceRecipe2;
        private SourceRecipe sourceRecipe1;
        private System.Windows.Forms.Button btnCreateRecipe;
    }
}

