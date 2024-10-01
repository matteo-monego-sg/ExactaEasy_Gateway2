using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyCore;
using System.IO;
using GretelClients;
using DisplayManager;

namespace Gretel2spvRecipeConverter {
    public partial class SourceRecipe : UserControl {

        int idClient;
        public int IdClient {
            get {
                return idClient;
            }
            set {
                idClient = value;
                numericUpDown1.Value = idClient;
            }
        }

        bool used;
        public bool Used {
            get {
                return used;
            }
            set {
                used = value;
                cbUse.Checked = used;
            }
        }

        GretelNodeBase gnb;

        public SourceRecipe() {
            InitializeComponent();
            gnb = new GretelNodeBase(new NodeDefinition());
        }

        public NodeRecipe GetNodeRecipe() {

            NodeRecipe retNodeRecipe = null;
            if (File.Exists(textBox1.Text)) {
                string gretelXml = "";
                //pier: se uso UTF7 ok mm² MA spariscono + e -....
                using (StreamReader reader = new StreamReader(textBox1.Text, Encoding.GetEncoding(1252))) {
                    gretelXml = reader.ReadToEnd();
                }
                retNodeRecipe = gnb.ReadNodeRecipeV2(gretelXml);
                retNodeRecipe.Id = (int)numericUpDown1.Value;
            }
            return retNodeRecipe;
        }

        public void SaveNodeRecipeV2(NodeRecipe nr, string folderPath) {

            if (Directory.Exists(folderPath)) {
                StreamWriter writer = new StreamWriter(folderPath + "/gretelRecipeClient_" + IdClient + ".xml");
                writer.Write(gnb.SaveRecipeV2(nr));
                writer.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e) {

            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Filter = "XML File (*.xml)|*.xml";
                if (DialogResult.OK == ofd.ShowDialog()) {
                    textBox1.Text = ofd.FileName;
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {

            IdClient = (int)numericUpDown1.Value;
        }

        private void cbUse_CheckedChanged(object sender, EventArgs e) {

            Used = cbUse.Checked;
        }
    }
}
