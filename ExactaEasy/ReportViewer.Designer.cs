namespace ExactaEasy {
    partial class ReportViewer {
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.pnlHtmlViewer = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.Rpv_Preview = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlHtmlViewer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panel1.Controls.Add(this.labelMessage);
            this.panel1.Controls.Add(this.btnExportPDF);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 364);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 70);
            this.panel1.TabIndex = 0;
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(144, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.labelMessage.Size = new System.Drawing.Size(676, 70);
            this.labelMessage.TabIndex = 23;
            this.labelMessage.Text = "loading........";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHtmlViewer
            // 
            this.pnlHtmlViewer.Controls.Add(this.webBrowser1);
            this.pnlHtmlViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHtmlViewer.Location = new System.Drawing.Point(0, 0);
            this.pnlHtmlViewer.Name = "pnlHtmlViewer";
            this.pnlHtmlViewer.Padding = new System.Windows.Forms.Padding(5);
            this.pnlHtmlViewer.Size = new System.Drawing.Size(820, 364);
            this.pnlHtmlViewer.TabIndex = 1;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(163, 73);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(211, 195);
            this.webBrowser1.TabIndex = 0;
            // 
            // Rpv_Preview
            // 
            this.Rpv_Preview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Rpv_Preview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Rpv_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Rpv_Preview.DocumentMapWidth = 1;
            this.Rpv_Preview.LocalReport.ReportEmbeddedResource = "ExactaEasy.Modello.Recipe.rdlc";
            this.Rpv_Preview.Location = new System.Drawing.Point(0, 0);
            this.Rpv_Preview.Name = "Rpv_Preview";
            this.Rpv_Preview.ServerReport.BearerToken = null;
            this.Rpv_Preview.ShowContextMenu = false;
            this.Rpv_Preview.ShowToolBar = false;
            this.Rpv_Preview.Size = new System.Drawing.Size(820, 364);
            this.Rpv_Preview.TabIndex = 1;
            this.Rpv_Preview.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.AutoEllipsis = true;
            this.btnExportPDF.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExportPDF.FlatAppearance.BorderSize = 0;
            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExportPDF.Image = global::ExactaEasy.ResourcesArtic.pdf;
            this.btnExportPDF.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExportPDF.Location = new System.Drawing.Point(72, 0);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnExportPDF.Size = new System.Drawing.Size(72, 70);
            this.btnExportPDF.TabIndex = 6;
            this.btnExportPDF.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportPDF.UseVisualStyleBackColor = true;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AutoEllipsis = true;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Image = global::ExactaEasy.ResourcesArtic.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.Location = new System.Drawing.Point(0, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.btnPrint.Size = new System.Drawing.Size(72, 70);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.Rpv_Preview);
            this.Controls.Add(this.pnlHtmlViewer);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "ReportViewer";
            this.Size = new System.Drawing.Size(820, 434);
            this.panel1.ResumeLayout(false);
            this.pnlHtmlViewer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlHtmlViewer;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExportPDF;
        private Microsoft.Reporting.WinForms.ReportViewer Rpv_Preview;
        private System.Windows.Forms.Label labelMessage;
    }
}
