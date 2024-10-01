namespace TattileTesterUI {
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
            this.logger1 = new SPAMI.Util.Logger.Logger();
            this.tattileTester1 = new TattileTesterUI.TattileTester();
            this.SuspendLayout();
            // 
            // logger1
            // 
            this.logger1.ClassName = "Logger";
            this.logger1.FileMode = SPAMI.Util.Logger.FileModeEnum.CreateNewAtStartup;
            this.logger1.LogFolder = ".\\";
            this.logger1.WriteToConsole = true;
            this.logger1.WriteToConsoleLevel = SPAMI.Util.Logger.LogLevels.Debug;
            this.logger1.WriteToFile = true;
            this.logger1.WriteToFileLevel = SPAMI.Util.Logger.LogLevels.Debug;
            // 
            // tattileTester1
            // 
            this.tattileTester1.Location = new System.Drawing.Point(2, 12);
            this.tattileTester1.Name = "tattileTester1";
            this.tattileTester1.Size = new System.Drawing.Size(534, 122);
            this.tattileTester1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 498);
            this.Controls.Add(this.tattileTester1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SPAMI.Util.Logger.Logger logger1;
        private TattileTester tattileTester1;

    }
}

