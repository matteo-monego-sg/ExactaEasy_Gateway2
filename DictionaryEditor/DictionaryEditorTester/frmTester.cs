using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace DictionaryEditorTester {
    public partial class frmTester : Form {

        public frmTester() {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e) {

            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.InitialDirectory = Directory.GetCurrentDirectory();
            oDialog.Filter = "Dictionary Files (*.xml)|*.xml";
            if (oDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                try {
                    ParameterInfoCollection pic = ParameterInfoCollection.LoadFromFile(oDialog.FileName);
                    if (pic == null)
                        Log.Line(LogLevels.Error, "frmTester.btnLoad_Click", "Parameters dictionary not found or corrupted");
                    else
                        dictEditor.CurrParamInfoCollection = pic;
                }
                catch (Exception) {
                    Log.Line(LogLevels.Error, "frmTester.btnLoad_Click", "Parameters dictionary not found or corrupted");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            ParameterInfoCollection pic = dictEditor.CurrParamInfoCollection;
            if (pic != null) {
                SaveFileDialog saveFileDlg = new SaveFileDialog();
                //saveFileDlg.InitialDirectory = 
                //saveFileDlg.RestoreDirectory = true;
                saveFileDlg.Filter = "Dictionary Files (*.xml)|*.xml";
                if (DialogResult.OK == saveFileDlg.ShowDialog()) {
                    pic.SaveFile(saveFileDlg.FileName);
                }
            }
        }

        private void btnAddColumn_Click(object sender, EventArgs e) {

            dictEditor.AddColumn();
        }

        private void frmTester_KeyDown(object sender, KeyEventArgs e) {

            if (e.Control && e.KeyCode == Keys.V) {
                dictEditor.PasteClipboard();
            }
        }
    }
}
