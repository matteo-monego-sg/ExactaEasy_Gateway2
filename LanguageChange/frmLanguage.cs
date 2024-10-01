using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LanguageChange {
    public partial class frmLanguage : Form {
        public frmLanguage() {
            InitializeComponent();
            BackColor = Color.FromArgb(222, 222, 222);
        }

        private void frmLanguage_KeyDown(object sender, KeyEventArgs e) {

            
        }

        private void frmLanguage_Load(object sender, EventArgs e) {

            frmLabelChangeLanguage waitForm = new frmLabelChangeLanguage();
            waitForm.StartPosition = FormStartPosition.CenterScreen;
            waitForm.ShowDialog();
            this.Close();
        }
    }
}
