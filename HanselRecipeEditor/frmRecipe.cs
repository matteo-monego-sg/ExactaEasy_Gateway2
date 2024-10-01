using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisplayManager;
using ExactaEasyEng;
using ExactaEasyCore;

namespace HanselRecipeEditor {
    public partial class frmRecipe : Form {

        Recipe newRecipe = new Recipe();
        MachineConfiguration machineConfig;

        public frmRecipe() {
            InitializeComponent();

        }

        public void Init(string recipeName, string recipeVersion, MachineConfiguration _machineConfig) {

            newRecipe.RecipeName = recipeName;
            newRecipe.RecipeVersion = recipeVersion;
            newRecipe.Nodes = new List<ExactaEasyEng.Node>();
            newRecipe.Stations = new List<ExactaEasyEng.Station>();
            newRecipe.Cams = new List<Cam>();
            machineConfig = _machineConfig;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            makeRecipe();
            newRecipe.SaveXml(@"C:\test.xml");
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
            //checkTemplate<MachineParameter>(template.MachineParameters);
            //checkTemplate<StroboParameter>(template.StroboParameters);
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

        void makeRecipe() {
            for (int ic = 0; ic < machineConfig.CameraSettings.Count; ic++) {
                CameraSetting camSetting = machineConfig.CameraSettings[ic];
                Cam newCam = new Cam();
                newCam.Id = camSetting.Id;
                newCam.Enabled = true;
                Camera camera = Camera.CreateCamera(camSetting.CameraProviderName);
                RecipeTemplate template = RecipeTemplate.LoadFromFile(templatePath[ic]);
                try {
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
                    return;
                }

                newRecipe.Cams.Add(newCam);
            }
        }

        private void btnBack_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
