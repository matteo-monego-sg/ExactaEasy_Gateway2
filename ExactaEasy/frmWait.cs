using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExactaEasy {
    public partial class frmWait : Form {

        public string Message {
            get {
                return lblWaitTxt.Text;
            }
            set {
                lblWaitTxt.Text = value;
            }
        }

        public frmWait() {
            InitializeComponent();
        }

    }
}
