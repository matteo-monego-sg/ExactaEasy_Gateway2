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
    public partial class LoadSaveMenu : UserControl {

        public event EventHandler<CamViewerMessageEventArgs> MenuAction;

        public LoadSaveMenu() {
            InitializeComponent();

            btnLoad.Text = frmBase.UIStrings.GetString("Load");
            btnSave.Text = frmBase.UIStrings.GetString("Save");
        }

        private void btnExit_Click(object sender, EventArgs e) {

            if (MenuAction != null) MenuAction(sender, new CamViewerMessageEventArgs("Exit"));
        }

        private void btnLoad_Click(object sender, EventArgs e) {

            if (MenuAction != null) MenuAction(sender, new CamViewerMessageEventArgs("Load"));
        }

        private void btnSave_Click(object sender, EventArgs e) {

            if (MenuAction != null) MenuAction(sender, new CamViewerMessageEventArgs("Save"));
        }

    }
}
