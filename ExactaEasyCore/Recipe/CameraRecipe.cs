using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class CameraRecipe {

        public CameraRecipe() {

            AcquisitionParameters = new ParameterCollection<Parameter>();
            DigitizerParameters = new ParameterCollection<Parameter>();
            RecipeSimpleParameters = new ParameterCollection<Parameter>();
            RecipeAdvancedParameters = new ParameterCollection<Parameter>();
            ROIParameters = new List<ParameterCollection<Parameter>>();
            MachineParameters = new ParameterCollection<Parameter>();
            StroboParameters = new ParameterCollection<Parameter>();    //rimuovere
            LightSensor = new LightSensor();    //change to BrightnessDetectionArea
            Lights = new List<LightRecipe>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Program { get; set; }

        public ParameterCollection<Parameter> AcquisitionParameters { get; set; }
        public ParameterCollection<Parameter> DigitizerParameters { get; set; }
        public ParameterCollection<Parameter> RecipeSimpleParameters { get; set; }
        public ParameterCollection<Parameter> RecipeAdvancedParameters { get; set; }
        public List<ParameterCollection<Parameter>> ROIParameters { get; set; }
        public ParameterCollection<Parameter> MachineParameters { get; set; }
        public ParameterCollection<Parameter> StroboParameters { get; set; }    //rimuovere
        public LightSensor LightSensor { get; set; }
        public List<LightRecipe> Lights { get; set; }

        public CameraRecipe Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            CameraRecipe newCamRcp = new CameraRecipe();
            newCamRcp.Id = Id;
            newCamRcp.Enabled = Enabled;
            newCamRcp.Program = Program;
            if (AcquisitionParameters != null)
                newCamRcp.AcquisitionParameters = (ParameterCollection<Parameter>)AcquisitionParameters.Clone(paramDictionary, cultureCode);
            if (DigitizerParameters != null)
                newCamRcp.DigitizerParameters = (ParameterCollection<Parameter>)DigitizerParameters.Clone(paramDictionary, cultureCode);
            if (RecipeSimpleParameters != null)
                newCamRcp.RecipeSimpleParameters = (ParameterCollection<Parameter>)RecipeSimpleParameters.Clone(paramDictionary, cultureCode);
            if (RecipeAdvancedParameters != null)
                newCamRcp.RecipeAdvancedParameters = (ParameterCollection<Parameter>)RecipeAdvancedParameters.Clone(paramDictionary, cultureCode);
            if (ROIParameters != null) {
                newCamRcp.ROIParameters = new List<ParameterCollection<Parameter>>();
                for (int ir = 0; ir < ROIParameters.Count; ir++)
                    newCamRcp.ROIParameters.Add((ParameterCollection<Parameter>)ROIParameters[ir].Clone(paramDictionary, cultureCode));
            }
            if (MachineParameters != null)
                newCamRcp.MachineParameters = (ParameterCollection<Parameter>)MachineParameters.Clone(paramDictionary, cultureCode);
            if (StroboParameters != null)
                newCamRcp.StroboParameters = (ParameterCollection<Parameter>)StroboParameters.Clone(paramDictionary, cultureCode);
            if (LightSensor != null)
                newCamRcp.LightSensor = LightSensor.Clone(paramDictionary, cultureCode);
            if (Lights != null) {
                for (int il = 0; il < Lights.Count; il++)
                    newCamRcp.Lights.Add(Lights[il].Clone(paramDictionary, cultureCode));
            }
            return newCamRcp;
        }

        public bool Compare(CameraRecipe cameraToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (cameraToCompare == null) {
                cameraToCompare = new CameraRecipe();
                cameraToCompare.Id = Id;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = cameraToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (cameraToCompare.Enabled != Enabled) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Active";
                paramDiff.ParameterLabel = "Active";
                paramDiff.ParameterLocLabel = "Active";
                paramDiff.ComparedValue = cameraToCompare.Enabled.ToString(); // (cameraToCompare.Enabled == false) ? "0" : "1";
                paramDiff.CurrentValue = Enabled.ToString(); // (Enabled == false) ? "0" : "1";
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            List<ParameterDiff> _paramDiffList = null;
            if (AcquisitionParameters != null) {
                ris = ris | AcquisitionParameters.Compare(cameraToCompare.AcquisitionParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (DigitizerParameters != null) {
                ris = ris | DigitizerParameters.Compare(cameraToCompare.DigitizerParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (RecipeSimpleParameters != null) {
                ris = ris | RecipeSimpleParameters.Compare(cameraToCompare.RecipeSimpleParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (RecipeAdvancedParameters != null) {
                ris = ris | RecipeAdvancedParameters.Compare(cameraToCompare.RecipeAdvancedParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (ROIParameters != null) {
                for (int ir = 0; ir < ROIParameters.Count; ir++) {
                    if (ir > cameraToCompare.ROIParameters.Count - 1) {
                        ParameterCollection<Parameter> newROI = new ParameterCollection<Parameter>();
                        cameraToCompare.ROIParameters.Add(newROI);
                    }
                    ris = ris | ROIParameters[ir].Compare(cameraToCompare.ROIParameters[ir], cultureCode, position, out _paramDiffList);
                    if (_paramDiffList != null)
                        paramDiffList.AddRange(_paramDiffList);
                }
            }
            if (MachineParameters != null) {
                ris = ris | MachineParameters.Compare(cameraToCompare.MachineParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (StroboParameters != null) {
                ris = ris | StroboParameters.Compare(cameraToCompare.StroboParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (LightSensor != null) {
                ris = ris | LightSensor.Compare(cameraToCompare.LightSensor, cultureCode, position + "- LightSensor", paramDiffList);
            }
            if (Lights != null) {
                for (int il = 0; il < Lights.Count; il++) {
                    if (il > cameraToCompare.Lights.Count - 1) {
                        LightRecipe newLR = new LightRecipe();
                        newLR.Id = Lights[il].Id;
                        cameraToCompare.Lights.Add(newLR);
                    }
                    ris = ris | Lights[il].Compare(cameraToCompare.Lights[il], cultureCode, position + "- Light " + il.ToString() + " ", paramDiffList);
                }
            }
            return ris;
        }
    }
}
