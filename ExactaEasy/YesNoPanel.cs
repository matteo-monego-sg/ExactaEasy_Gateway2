using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisplayManager;
using System.Threading;
using SPAMI.Util.Logger;

namespace ExactaEasy {
    public partial class YesNoPanel : UserControl {

        public event EventHandler<CamViewerMessageEventArgs> YesNoAnswer;

        public YesNoPanel() {
            InitializeComponent();

            btnYes.Text = frmBase.UIStrings.GetString("Yes");
            btnNo.Text = frmBase.UIStrings.GetString("No");
        }

        private void btnExitStopCondMenu_Click(object sender, EventArgs e) {

            if (YesNoAnswer != null) YesNoAnswer(sender, new CamViewerMessageEventArgs("Exit"));
        }

        private void btnYes_Click(object sender, EventArgs e) {

            if (YesNoAnswer!=null) YesNoAnswer(sender, new CamViewerMessageEventArgs("Yes"));
        }

        private void btnNo_Click(object sender, EventArgs e) {

            if (YesNoAnswer!=null) YesNoAnswer(sender, new CamViewerMessageEventArgs("No"));
        }

    }
}
