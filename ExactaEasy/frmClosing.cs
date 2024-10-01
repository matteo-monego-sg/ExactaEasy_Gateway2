using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExactaEasy {
    public partial class frmClosing : Form {
        public frmClosing() {
            InitializeComponent();
            timer1.Interval = 100;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e) {

        }
    }
}
