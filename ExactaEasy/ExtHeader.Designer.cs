namespace ExactaEasy {
    partial class ExtHeader
    {
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
            this.lblRecipeName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRecipeName
            // 
            this.lblRecipeName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.lblRecipeName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecipeName.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecipeName.ForeColor = System.Drawing.Color.Red;
            this.lblRecipeName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRecipeName.Location = new System.Drawing.Point(0, 0);
            this.lblRecipeName.Name = "lblRecipeName";
            this.lblRecipeName.Size = new System.Drawing.Size(294, 27);
            this.lblRecipeName.TabIndex = 3;
            this.lblRecipeName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ExtHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.Controls.Add(this.lblRecipeName);
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "ExtHeader";
            this.Size = new System.Drawing.Size(294, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblRecipeName;


    }
}
