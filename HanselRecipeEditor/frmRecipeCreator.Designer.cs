namespace HanselRecipeEditor {
    partial class frmRecipeCreator {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecipeCreator));
            this.pnlInit = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.pnlRecipe = new System.Windows.Forms.Panel();
            this.pnlConfigurator = new System.Windows.Forms.Panel();
            this.btnSelRecipeFile = new System.Windows.Forms.Button();
            this.tbRecipePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRecipeVersion = new System.Windows.Forms.TextBox();
            this.tbRecipeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRecipeName = new System.Windows.Forms.Label();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBack2 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.reportViewer1 = new ExactaEasy.ReportViewer();
            this.pnlInit.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlRecipe.SuspendLayout();
            this.pnlConfigurator.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInit
            // 
            this.pnlInit.BackColor = System.Drawing.SystemColors.Info;
            this.pnlInit.Controls.Add(this.lblWelcome);
            this.pnlInit.Controls.Add(this.tableLayoutPanel1);
            this.pnlInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInit.Location = new System.Drawing.Point(0, 0);
            this.pnlInit.Name = "pnlInit";
            this.pnlInit.Size = new System.Drawing.Size(803, 462);
            this.pnlInit.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(12, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(779, 329);
            this.lblWelcome.TabIndex = 2;
            this.lblWelcome.Text = "Welcome to Hansel Recipe Editor. \r\nThis wizard will guide you \r\nthrough the rest " +
    "of the recipe creation...";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCreate, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnEdit, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(129, 354);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(544, 96);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.Navy;
            this.btnExit.Location = new System.Drawing.Point(372, 10);
            this.btnExit.Margin = new System.Windows.Forms.Padding(10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(162, 76);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreate.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.ForeColor = System.Drawing.Color.Navy;
            this.btnCreate.Location = new System.Drawing.Point(10, 10);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(10);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(161, 76);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "CREATE";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.Enabled = false;
            this.btnEdit.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.Navy;
            this.btnEdit.Location = new System.Drawing.Point(191, 10);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(161, 76);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "EDIT";
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // pnlRecipe
            // 
            this.pnlRecipe.BackColor = System.Drawing.SystemColors.Info;
            this.pnlRecipe.Controls.Add(this.reportViewer1);
            this.pnlRecipe.Controls.Add(this.label3);
            this.pnlRecipe.Controls.Add(this.tableLayoutPanel3);
            this.pnlRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecipe.Location = new System.Drawing.Point(0, 0);
            this.pnlRecipe.Name = "pnlRecipe";
            this.pnlRecipe.Size = new System.Drawing.Size(803, 462);
            this.pnlRecipe.TabIndex = 1;
            // 
            // pnlConfigurator
            // 
            this.pnlConfigurator.BackColor = System.Drawing.SystemColors.Info;
            this.pnlConfigurator.Controls.Add(this.btnSelRecipeFile);
            this.pnlConfigurator.Controls.Add(this.tbRecipePath);
            this.pnlConfigurator.Controls.Add(this.label2);
            this.pnlConfigurator.Controls.Add(this.tbRecipeVersion);
            this.pnlConfigurator.Controls.Add(this.tbRecipeName);
            this.pnlConfigurator.Controls.Add(this.label1);
            this.pnlConfigurator.Controls.Add(this.lblRecipeName);
            this.pnlConfigurator.Controls.Add(this.lblStepDescription);
            this.pnlConfigurator.Controls.Add(this.tableLayoutPanel2);
            this.pnlConfigurator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfigurator.Location = new System.Drawing.Point(0, 0);
            this.pnlConfigurator.Name = "pnlConfigurator";
            this.pnlConfigurator.Size = new System.Drawing.Size(803, 462);
            this.pnlConfigurator.TabIndex = 2;
            // 
            // btnSelRecipeFile
            // 
            this.btnSelRecipeFile.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelRecipeFile.Location = new System.Drawing.Point(737, 302);
            this.btnSelRecipeFile.Name = "btnSelRecipeFile";
            this.btnSelRecipeFile.Size = new System.Drawing.Size(51, 33);
            this.btnSelRecipeFile.TabIndex = 16;
            this.btnSelRecipeFile.Text = "...";
            this.btnSelRecipeFile.UseVisualStyleBackColor = true;
            this.btnSelRecipeFile.Click += new System.EventHandler(this.btnSelRecipeFile_Click);
            // 
            // tbRecipePath
            // 
            this.tbRecipePath.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbRecipePath.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRecipePath.ForeColor = System.Drawing.Color.Red;
            this.tbRecipePath.Location = new System.Drawing.Point(119, 302);
            this.tbRecipePath.Name = "tbRecipePath";
            this.tbRecipePath.ReadOnly = true;
            this.tbRecipePath.Size = new System.Drawing.Size(606, 33);
            this.tbRecipePath.TabIndex = 15;
            this.tbRecipePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 302);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 36);
            this.label2.TabIndex = 14;
            this.label2.Text = "PATH";
            // 
            // tbRecipeVersion
            // 
            this.tbRecipeVersion.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.tbRecipeVersion.ForeColor = System.Drawing.Color.Red;
            this.tbRecipeVersion.Location = new System.Drawing.Point(547, 254);
            this.tbRecipeVersion.Name = "tbRecipeVersion";
            this.tbRecipeVersion.Size = new System.Drawing.Size(231, 39);
            this.tbRecipeVersion.TabIndex = 13;
            this.tbRecipeVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRecipeName
            // 
            this.tbRecipeName.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.tbRecipeName.ForeColor = System.Drawing.Color.Red;
            this.tbRecipeName.Location = new System.Drawing.Point(119, 254);
            this.tbRecipeName.Name = "tbRecipeName";
            this.tbRecipeName.Size = new System.Drawing.Size(281, 39);
            this.tbRecipeName.TabIndex = 12;
            this.tbRecipeName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(423, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 36);
            this.label1.TabIndex = 11;
            this.label1.Text = "VERSION";
            // 
            // lblRecipeName
            // 
            this.lblRecipeName.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecipeName.Location = new System.Drawing.Point(26, 257);
            this.lblRecipeName.Name = "lblRecipeName";
            this.lblRecipeName.Size = new System.Drawing.Size(87, 36);
            this.lblRecipeName.TabIndex = 10;
            this.lblRecipeName.Text = "NAME";
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.Location = new System.Drawing.Point(12, 9);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(779, 234);
            this.lblStepDescription.TabIndex = 9;
            this.lblStepDescription.Text = resources.GetString("lblStepDescription.Text");
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.btnBack, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnManual, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAuto, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(129, 354);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(544, 96);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.Navy;
            this.btnBack.Location = new System.Drawing.Point(372, 10);
            this.btnBack.Margin = new System.Windows.Forms.Padding(10);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(162, 76);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "BACK";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManual.Enabled = false;
            this.btnManual.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.ForeColor = System.Drawing.Color.Navy;
            this.btnManual.Location = new System.Drawing.Point(10, 10);
            this.btnManual.Margin = new System.Windows.Forms.Padding(10);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(161, 76);
            this.btnManual.TabIndex = 1;
            this.btnManual.Text = "MANUAL";
            this.btnManual.UseVisualStyleBackColor = false;
            // 
            // btnAuto
            // 
            this.btnAuto.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAuto.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnAuto.ForeColor = System.Drawing.Color.Navy;
            this.btnAuto.Location = new System.Drawing.Point(191, 10);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(10);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(161, 76);
            this.btnAuto.TabIndex = 0;
            this.btnAuto.Text = "AUTO";
            this.btnAuto.UseVisualStyleBackColor = false;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Info;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.btnBack2, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.button2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSave, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(129, 354);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(544, 96);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // btnBack2
            // 
            this.btnBack2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBack2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack2.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnBack2.ForeColor = System.Drawing.Color.Navy;
            this.btnBack2.Location = new System.Drawing.Point(372, 10);
            this.btnBack2.Margin = new System.Windows.Forms.Padding(10);
            this.btnBack2.Name = "btnBack2";
            this.btnBack2.Size = new System.Drawing.Size(162, 76);
            this.btnBack2.TabIndex = 2;
            this.btnBack2.Text = "BACK";
            this.btnBack2.UseVisualStyleBackColor = false;
            this.btnBack2.Click += new System.EventHandler(this.btnBack2_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Navy;
            this.button2.Location = new System.Drawing.Point(10, 10);
            this.button2.Margin = new System.Windows.Forms.Padding(10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 76);
            this.button2.TabIndex = 1;
            this.button2.Text = "---";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.Navy;
            this.btnSave.Location = new System.Drawing.Point(191, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(161, 76);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(257, 36);
            this.label3.TabIndex = 15;
            this.label3.Text = "RECIPE VALUES";
            // 
            // reportViewer1
            // 
            this.reportViewer1.Location = new System.Drawing.Point(16, 48);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ReportTemplateName = null;
            this.reportViewer1.ReportTemplatePath = null;
            this.reportViewer1.Size = new System.Drawing.Size(770, 300);
            this.reportViewer1.TabIndex = 16;
            // 
            // frmRecipeCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 462);
            this.Controls.Add(this.pnlRecipe);
            this.Controls.Add(this.pnlConfigurator);
            this.Controls.Add(this.pnlInit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmRecipeCreator";
            this.Text = "frmRecipeCreator";
            this.pnlInit.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlRecipe.ResumeLayout(false);
            this.pnlConfigurator.ResumeLayout(false);
            this.pnlConfigurator.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlInit;
        private System.Windows.Forms.Panel pnlRecipe;
        private System.Windows.Forms.Panel pnlConfigurator;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSelRecipeFile;
        private System.Windows.Forms.TextBox tbRecipePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRecipeVersion;
        private System.Windows.Forms.TextBox tbRecipeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecipeName;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnBack2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private ExactaEasy.ReportViewer reportViewer1;
    }
}