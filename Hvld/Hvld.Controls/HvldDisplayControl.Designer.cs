namespace Hvld.Controls
{
    partial class HvldDisplayControl
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.BtnNext = new System.Windows.Forms.Button();
            this.BtnPrev = new System.Windows.Forms.Button();
            this.MainTablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(56)))));
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainPanel.Name = "MainPanel";
            this.MainTablePanel.SetRowSpan(this.MainPanel, 3);
            this.MainPanel.Size = new System.Drawing.Size(414, 221);
            this.MainPanel.TabIndex = 6;
            // 
            // MainTablePanel
            // 
            this.MainTablePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(56)))));
            this.MainTablePanel.ColumnCount = 2;
            this.MainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTablePanel.Controls.Add(this.BtnNext, 1, 2);
            this.MainTablePanel.Controls.Add(this.MainPanel, 0, 0);
            this.MainTablePanel.Controls.Add(this.BtnPrev, 1, 0);
            this.MainTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTablePanel.Location = new System.Drawing.Point(0, 0);
            this.MainTablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainTablePanel.Name = "MainTablePanel";
            this.MainTablePanel.RowCount = 3;
            this.MainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTablePanel.Size = new System.Drawing.Size(454, 221);
            this.MainTablePanel.TabIndex = 7;
            // 
            // BtnNext
            // 
            this.BtnNext.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.BtnNext.BackgroundImage = global::Hvld.Properties.Resources.arrow_down;
            this.BtnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNext.Location = new System.Drawing.Point(414, 181);
            this.BtnNext.Margin = new System.Windows.Forms.Padding(0);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(40, 40);
            this.BtnNext.TabIndex = 5;
            this.BtnNext.UseVisualStyleBackColor = false;
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnPrev
            // 
            this.BtnPrev.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.BtnPrev.BackgroundImage = global::Hvld.Properties.Resources.arrow_up;
            this.BtnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrev.Location = new System.Drawing.Point(414, 0);
            this.BtnPrev.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(40, 40);
            this.BtnPrev.TabIndex = 4;
            this.BtnPrev.UseVisualStyleBackColor = false;
            this.BtnPrev.Click += new System.EventHandler(this.BtnPrev_Click);
            // 
            // HvldSingleDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.MainTablePanel);
            this.Name = "HvldSingleDisplay";
            this.Size = new System.Drawing.Size(454, 221);
            this.MainTablePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnPrev;
        private System.Windows.Forms.TableLayoutPanel MainTablePanel;
    }
}
