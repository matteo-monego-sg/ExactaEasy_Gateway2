using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyCore;
using ExactaEasyEng;

namespace Gretel2spvRecipeConverter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnCreateRecipe_Click(object sender, EventArgs e) {

            Recipe convertedRecipe = new Recipe();
            convertedRecipe.Nodes = new List<NodeRecipe>();
            foreach (Control ctrl in this.pnlMain.Controls) {
                if (ctrl is SourceRecipe) {
                    SourceRecipe sr = (SourceRecipe)ctrl;
                    if (!sr.Used) continue;
                    NodeRecipe newNode = sr.GetNodeRecipe();
                    if (newNode != null) {
                        convertedRecipe.Nodes.Add(newNode);
                        sr.SaveNodeRecipeV2(newNode, @"C:\");
                    }
                }
            }
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.RestoreDirectory = true;
                sfd.Filter = "XML File (*.xml)|*.xml";
                convertedRecipe.Nodes = convertedRecipe.Nodes.OrderBy(nn => nn.Id).ToList();
                if (DialogResult.OK == sfd.ShowDialog()) {
                    convertedRecipe.SaveXml(sfd.FileName);
                }
            }
        }
    }
}
