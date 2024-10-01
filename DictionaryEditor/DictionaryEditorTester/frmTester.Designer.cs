namespace DictionaryEditorTester {
    partial class frmTester {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTester));
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dictEditor = new DictionaryEditorUI.DictionaryEditor();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.Controls.Add(this.btnAddColumn);
            this.pnlMenu.Controls.Add(this.btnExit);
            this.pnlMenu.Controls.Add(this.btnSave);
            this.pnlMenu.Controls.Add(this.btnLoad);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(657, 57);
            this.pnlMenu.TabIndex = 1;
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAddColumn.Enabled = false;
            this.btnAddColumn.FlatAppearance.BorderSize = 0;
            this.btnAddColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddColumn.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddColumn.Image = global::DictionaryEditorTester.Properties.Resources.insert_table;
            this.btnAddColumn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddColumn.Location = new System.Drawing.Point(164, 0);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(82, 57);
            this.btnAddColumn.TabIndex = 3;
            this.btnAddColumn.Text = "Add Column";
            this.btnAddColumn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddColumn.UseVisualStyleBackColor = true;
            this.btnAddColumn.Visible = false;
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = global::DictionaryEditorTester.Properties.Resources.application_exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(575, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 57);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::DictionaryEditorTester.Properties.Resources.document_save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(82, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 57);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoad.Image = global::DictionaryEditorTester.Properties.Resources.folder_documents;
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoad.Location = new System.Drawing.Point(0, 0);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(82, 57);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dictEditor
            // 
            this.dictEditor.CurrParamInfoCollection = null;
            this.dictEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dictEditor.Location = new System.Drawing.Point(0, 57);
            this.dictEditor.Name = "dictEditor";
            this.dictEditor.Size = new System.Drawing.Size(657, 440);
            this.dictEditor.TabIndex = 2;
            // 
            // frmTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 497);
            this.Controls.Add(this.dictEditor);
            this.Controls.Add(this.pnlMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmTester";
            this.Text = "DICTIONARY EDITOR";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTester_KeyDown);
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private DictionaryEditorUI.DictionaryEditor dictEditor;
        private System.Windows.Forms.Button btnAddColumn;
    }
}

