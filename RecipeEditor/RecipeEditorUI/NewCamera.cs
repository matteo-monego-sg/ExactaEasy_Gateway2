using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyEng;

namespace RecipeEditorUI {
    public partial class NewCamera : UserControl {

        public string RecipeExtension { get; internal set; }
        public string TemplateDir { get; set; }

        List<string> templatesNames;
        public List<string> TemplatesNames {
            get {
                return templatesNames;
            }
            set {
                templatesNames = value;
                cbCamSelector.Items.AddRange(templatesNames.ToArray());
            }
        }

        public int Id {
            get {
                return (int)nudId.Value;
            }
            set {
                nudId.Value = value;
            }
        }

        public List<Cam> Cams {
            get {
                try {
                    return loadTemplate(cbCamSelector.SelectedText);
                }
                catch {
                    return null;
                }
            }
        }

        public NewCamera() {
            InitializeComponent();
        }

        List<Cam> loadTemplate(string filename) {
            string path = TemplateDir + @"\" + filename + "." + RecipeExtension;
            Recipe templateRecipe = Recipe.LoadFromFile(path);
            if (templateRecipe != null && templateRecipe.Cams != null && templateRecipe.Cams.Count > 0)
                return templateRecipe.Cams;
            return null;
        }
    }
}
