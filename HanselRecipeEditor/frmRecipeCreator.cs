using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyEng;
using ExactaEasyCore;
using DisplayManager;

namespace HanselRecipeEditor {

    public partial class frmRecipeCreator : Form {

        MachineConfiguration machineConfig;
        Recipe newRecipe = new Recipe();

        public frmRecipeCreator() {
            InitializeComponent();
            pnlInit.Visible = true;
            pnlConfigurator.Visible = false;
            pnlRecipe.Visible = false;
        }

        private void btnCreate_Click(object sender, EventArgs e) {
            pnlInit.Visible = false;
            pnlRecipe.Visible = false;
            pnlConfigurator.Visible = true;
            pnlRecipe.BringToFront();
        }

        private void btnExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e) {
            pnlInit.Visible = true;
            pnlConfigurator.Visible = false;
            pnlRecipe.Visible = false;
            pnlInit.BringToFront();
        }

        private void btnAuto_Click(object sender, EventArgs e) {
            OpenFileDialog oDialog = new OpenFileDialog();
            //oDialog.RestoreDirectory = true;
            //oDialog.InitialDirectory = AppEngine.Current.GlobalConfig.RecipesFolder;
            oDialog.CheckFileExists = true;
            oDialog.CheckPathExists = true;
            oDialog.Filter = "XML File (*.xml)|*.xml";
            oDialog.Title = "Please select a valid machine configuration file.";
            if (oDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                try {
                    machineConfig = MachineConfiguration.LoadFromFile(oDialog.FileName);
                    if (tbRecipePath.Text == null) {
                        MessageBox.Show("Please select the path in which to store the new recipe", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnSelRecipeFile_Click(sender, e);
                    }
                }
                catch {
                    MessageBox.Show("Invalid machine configuration file!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                pnlInit.Visible = false;
                pnlConfigurator.Visible = false;
                pnlRecipe.Visible = true;
                pnlRecipe.BringToFront();
                newRecipe.RecipeName = tbRecipeName.Text;
                newRecipe.RecipeVersion = tbRecipeVersion.Text;
                newRecipe.Nodes = new List<ExactaEasyEng.Node>();
                newRecipe.Stations = new List<ExactaEasyEng.Station>();
                newRecipe.Cams = new List<Cam>();
                //frmRecipe recipe = new frmRecipe();
                //recipe.Init(tbRecipeName.Text, tbRecipeVersion.Text, machineConfig);
                //recipe.ShowDialog();

            }
        }
        

        private void btnSelRecipeFile_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "XML File (*.xml)|*.xml";
            saveFileDialog.Title = "Please select where to save your recipe";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                tbRecipePath.Text = saveFileDialog.FileName;
            }
        }

        private void btnBack2_Click(object sender, EventArgs e) {
            pnlInit.Visible = true;
            pnlConfigurator.Visible = false;
            pnlRecipe.Visible = false;
            pnlInit.BringToFront();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (makeRecipe())
                newRecipe.SaveXml(tbRecipePath.Text);
            else
                MessageBox.Show("An error occurred while creating a new recipe!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        string[] templatePath = new string[8] {
            @"C:\Test_RecipeTemplate\M12_Cosmetic_strobe.xml",
            @"C:\Test_RecipeTemplate\M12_Cosmetic_no_strobe.xml",
            @"C:\Test_RecipeTemplate\M12_Cosmetic_strobe.xml",
            @"C:\Test_RecipeTemplate\M12_Cosmetic_no_strobe.xml",
            @"C:\Test_RecipeTemplate\M9_Particles.xml",
            @"C:\Test_RecipeTemplate\M9_Particles.xml",
            @"C:\Test_RecipeTemplate\M9_Particles.xml",
            @"C:\Test_RecipeTemplate\M9_Particles.xml",
        };

        void checkTemplate(RecipeTemplate template, Cam cam) {
            checkTemplate<AcquisitionParameter>(template.AcquisitionParameters, cam.AcquisitionParameters);
            checkTemplate<FeaturesEnableParameter>(template.FeaturesEnableParameters, cam.FeaturesEnableParameters);
            checkTemplate<RecipeSimpleParameter>(template.RecipeSimpleParameters, cam.RecipeSimpleParameters);
            checkTemplate<RecipeAdvancedParameter>(template.RecipeAdvancedParameters, cam.RecipeAdvancedParameters);
            for (int roi = 0; roi < template.ROIParameters.Count; roi++)
                checkTemplate<ROIParameter>(template.ROIParameters[roi], cam.ROIParameters[roi]);
            checkTemplate<MachineParameter>(template.MachineParameters, cam.MachineParameters);
            checkTemplate<StroboParameter>(template.StroboParameters, cam.StroboParameters);
        }

        void checkTemplate<T>(ParameterCollection<T> templateList, ParameterCollection<T> camParamList) where T : IParameter, new() {
            foreach (T param in camParamList) {
                if (!templateList.Exists(parameter => parameter.Id == param.Id))
                    throw new Exception("Invalid template! " + param.Id + " does not exist...");
                //T templateParam = templateList.Find(parameter => parameter.Id == param.Id);
                //param.IsVisible = templateParam.IsVisible;
                //param.IsEditable = templateParam.IsEditable;
                //param.MaxValue = templateParam.MaxValue;
                //param.MinValue = templateParam.MinValue;
                //param.AdmittedValues = templateParam.AdmittedValues;
                //param.Label = null;
            }
        }

        bool makeRecipe() {
            if (machineConfig == null || machineConfig.CameraSettings == null)
                return false;
            for (int ic = 0; ic < machineConfig.CameraSettings.Count; ic++) {
                CameraSetting camSetting = machineConfig.CameraSettings[ic];
                Cam newCam = new Cam();
                newCam.Id = camSetting.Id;
                newCam.Enabled = true;
                Camera camera = Camera.CreateCamera(camSetting.CameraProviderName);
                RecipeTemplate template = RecipeTemplate.LoadFromFile(templatePath[ic]);
                try {
                    //pier: funzionava fino a quando non sono stati modificati i costruttori
                    //dei ParameterCollection...ora si spacca perchè vuole l'Engine...
                    //da rivedere tutto da qui in poi...
                    if (template.AcquisitionParameters != null)
                        newCam.AcquisitionParameters = camera.GetAcquisitionParametersList();
                    if (template.FeaturesEnableParameters != null)
                        newCam.FeaturesEnableParameters = camera.GetFeaturesEnableParametersList();
                    if (template.RecipeSimpleParameters != null)
                        newCam.RecipeSimpleParameters = camera.GetRecipeSimpleParametersList();
                    if (template.RecipeAdvancedParameters != null)
                        newCam.RecipeAdvancedParameters = camera.GetRecipeAdvancedParametersList();
                    if (template.ROIParameters != null) {
                        newCam.ROIParameters = new List<ParameterCollection<ROIParameter>>();
                        for (int ir = 0; ir < template.ROIParameters.Count; ir++)
                            newCam.ROIParameters.Add(camera.GetROIParametersList());
                    }
                    if (template.MachineParameters != null)
                        newCam.MachineParameters = camera.GetMachineParametersList();
                    if (template.StroboParameters != null)
                        newCam.StroboParameters = camera.GetStrobeParametersList();
                    //ora che ho i valori devo completare la ricetta con i parametri "accessori" tipo
                    //MinValue, MaxValue, AdmittedValues, ecc...
                    //presi da param DICTIONARY?!?!?
                    checkTemplate(template, newCam);
                    newCam.AcquisitionParameters = template.AcquisitionParameters;
                    newCam.FeaturesEnableParameters = template.FeaturesEnableParameters;
                    newCam.RecipeSimpleParameters = template.RecipeSimpleParameters;
                    newCam.RecipeAdvancedParameters = template.RecipeAdvancedParameters;
                    newCam.ROIParameters = template.ROIParameters;
                    newCam.StroboParameters = template.StroboParameters;
                    ///newCam.
                }
                catch (Exception ex) {
                    return false;
                }

                newRecipe.Cams.Add(newCam);
            }
            return true;
        }
    }
}
