using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ExactaEasyEng;
using SPAMI.Util.Logger;

namespace RecipeEditorUI {
    public partial class RecipeCreator : UserControl {
        int devCount = 0;
        bool firstTime = true;
        List<string> templatesNames = new List<string>();
        const string templateDir = @".\templates";
        const string recipeExtension = "xml";

        public RecipeCreator() {
            InitializeComponent();

        }

        private void btnStart_Click(object sender, EventArgs e) {

            pnlDevCustomization.Controls.Clear();
            if (firstTime) {
                loadTemplates();
                firstTime = false;
            }
            devCount = (int)nudDevCount.Value;
            //Size sz = new System.Drawing.Size();
            SuspendLayout();
            for (int i = devCount - 1; i >= 0; i--) {
                NewCamera newRc = new NewCamera();
                newRc.Dock = DockStyle.Top;
                newRc.Dock = System.Windows.Forms.DockStyle.Top;
                newRc.Location = new System.Drawing.Point(0, 0);
                newRc.Name = "nc" + i;
                newRc.Size = new System.Drawing.Size(389, 31);
                newRc.TabIndex = i + 1;
                newRc.Id = i;
                newRc.TemplatesNames = templatesNames;
                newRc.RecipeExtension = recipeExtension;
                pnlDevCustomization.Controls.Add(newRc);
                //sz.Width = Math.Max(sz.Width, newRc.Width);
                //sz.Height = Math.Min(Height + newRc.Height, 400);
            }
            //Size = sz;
            ResumeLayout();
            Refresh();
        }

        void loadTemplates() {

            List<string> templatesFilenames = Directory.EnumerateFiles(templateDir, "*." + recipeExtension).ToList();
            foreach (string filename in templatesFilenames) {
                try {
                    Recipe recipe = Recipe.LoadFromFile(filename);
                    if (recipe.Cams == null || recipe.Cams.Count == 0)
                        continue;
                    FileInfo fi = new FileInfo(filename);
                    templatesNames.Add(fi.Name.Replace("." + recipeExtension, ""));
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "RecipeCreator.loadTemplates", "Recipe not valid or not supported yet");
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e) {

            Recipe newRecipe = new Recipe();
            foreach (Control ctrl in pnlDevCustomization.Controls) {
                if (ctrl is NewCamera) {
                    NewCamera currCamera = ctrl as NewCamera;
                    currCamera.TemplateDir = templateDir;
                    List<Cam> newCams = currCamera.Cams;
                    if (newCams == null) {
                        Log.Line(LogLevels.Error, "RecipeCreator.btnCreate_Click", "Cannot create camera with id " + currCamera.Id);
                        MessageBox.Show("Cannot create camera with id " + currCamera.Id, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    for (int i = 0; i < newCams.Count; i++) {
                        newCams[i].Id = currCamera.Id + i;
                    }
                    newRecipe.Cams.AddRange(newCams);
                }
            }
        }
    }
}
