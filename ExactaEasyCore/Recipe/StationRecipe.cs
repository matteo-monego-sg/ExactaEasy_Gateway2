using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class StationRecipe {

        public StationRecipe() {

            Cameras = new List<CameraRecipe>();
            Tools = new List<Tool>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool Enable { get; set; }
        public bool Default { get; set; }               //pier: 2/12/2015 lo tengo solo per retrocompatibilità
        public string InspectionType { get; set; }      //pier: aggiunto 2/12/2015
        public List<CameraRecipe> Cameras { get; set; }
        public List<Tool> Tools { get; set; }

        public StationRecipe Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            StationRecipe newStatRcp = new StationRecipe();
            newStatRcp.Id = Id;
            newStatRcp.Description = Description;
            newStatRcp.Enable = Enable;
            newStatRcp.Default = Default;
            newStatRcp.InspectionType = InspectionType;
            if (Cameras != null) {
                newStatRcp.Cameras = new List<CameraRecipe>();
                foreach (CameraRecipe cr in Cameras)
                    newStatRcp.Cameras.Add(cr.Clone(paramDictionary, cultureCode));
            }
            if (Tools != null) {
                newStatRcp.Tools = new List<Tool>();
                foreach (Tool tool in Tools)
                    newStatRcp.Tools.Add(tool.Clone(paramDictionary, cultureCode));
            }
            return newStatRcp;
        }

        public bool Compare(StationRecipe stationToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (stationToCompare == null) {
                stationToCompare = new StationRecipe();
                stationToCompare.Id = Id;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = Id.ToString();
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = stationToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (stationToCompare.Description != Description) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Label";
                paramDiff.ParameterLabel = "Label";
                paramDiff.ParameterLocLabel = "Label";
                paramDiff.ComparedValue = stationToCompare.Description;
                paramDiff.CurrentValue = Description;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (stationToCompare.Enable != Enable) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = Id.ToString();
                paramDiff.ParameterLabel = "Active";
                paramDiff.ParameterLocLabel = "Active";
                paramDiff.ComparedValue = stationToCompare.Enable.ToString(); // (stationToCompare.Enable == false) ? "0" : "1";
                paramDiff.CurrentValue = Enable.ToString(); // (Enable == false) ? "0" : "1";
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (stationToCompare.InspectionType != InspectionType) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Inspection Type";
                paramDiff.ParameterLabel = "Inspection Type";
                paramDiff.ParameterLocLabel = "Inspection Type";
                paramDiff.ComparedValue = stationToCompare.InspectionType;
                paramDiff.CurrentValue = InspectionType;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (Cameras != null) {
                foreach (CameraRecipe cr in Cameras) {
                    CameraRecipe crToCompare = stationToCompare.Cameras.Find((CameraRecipe c) => { return c.Id == cr.Id; });
                    ris = ris | cr.Compare(crToCompare, cultureCode, position + " - Cam " + cr.Id, paramDiffList);
                }
            }
            if (Tools != null) {
                foreach (Tool tool in Tools) {
                    Tool toolToCompare = stationToCompare.Tools.Find((Tool t) => { return t.Id == tool.Id; });
                    ris = ris | tool.Compare(toolToCompare, cultureCode, position + " - Tool \"" + tool.Label + "\"" + "(" + (tool.Id + 1) + ")", paramDiffList);
                }
            }
            return ris;

        }
    }
}
