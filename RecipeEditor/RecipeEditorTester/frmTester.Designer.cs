namespace RecipeEditorTester {
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
            this.recipeCreator1 = new RecipeEditorUI.RecipeCreator();
            this.SuspendLayout();
            // 
            // recipeCreator1
            // 
            this.recipeCreator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recipeCreator1.Location = new System.Drawing.Point(0, 0);
            this.recipeCreator1.Name = "recipeCreator1";
            this.recipeCreator1.Size = new System.Drawing.Size(695, 508);
            this.recipeCreator1.TabIndex = 0;
            // 
            // frmTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 508);
            this.Controls.Add(this.recipeCreator1);
            this.Name = "frmTester";
            this.Text = "RECIPE EDITOR";
            this.ResumeLayout(false);

        }

        #endregion

        private RecipeEditorUI.RecipeCreator recipeCreator1;
    }
}

