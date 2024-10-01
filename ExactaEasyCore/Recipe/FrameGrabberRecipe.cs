using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {
    public class FrameGrabberRecipe {

        public int BoardId { get; set; }
        public bool Active { get; set; }
        public string ConfigFileName { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }

        public FrameGrabberRecipe Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            FrameGrabberRecipe newFrmGrbRcp = new FrameGrabberRecipe();
            newFrmGrbRcp.BoardId = BoardId;
            newFrmGrbRcp.Active = Active;
            newFrmGrbRcp.ConfigFileName = ConfigFileName;
            newFrmGrbRcp.Type = Type;
            newFrmGrbRcp.Id = Id;
            return newFrmGrbRcp;
        }


        public bool Compare(FrameGrabberRecipe frameGrabberToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (frameGrabberToCompare == null) {
                frameGrabberToCompare = new FrameGrabberRecipe();
                frameGrabberToCompare.BoardId = BoardId;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = frameGrabberToCompare.BoardId.ToString();
                paramDiff.CurrentValue = BoardId.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (frameGrabberToCompare.Active != Active) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Active";
                paramDiff.ParameterLabel = "Active";
                paramDiff.ParameterLocLabel = "Active";
                paramDiff.ComparedValue = frameGrabberToCompare.Active.ToString();// (frameGrabberToCompare.Active == false) ? "0" : "1";
                paramDiff.CurrentValue = Active.ToString(); // (Active == false) ? "0" : "1";
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (frameGrabberToCompare.ConfigFileName != ConfigFileName) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "cfgFile";
                paramDiff.ParameterLabel = "cfgFile";
                paramDiff.ParameterLocLabel = "cfgFile";
                paramDiff.ComparedValue = frameGrabberToCompare.ConfigFileName;
                paramDiff.CurrentValue = ConfigFileName;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (frameGrabberToCompare.Type != Type) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Type";
                paramDiff.ParameterLabel = "Type";
                paramDiff.ParameterLocLabel = "Type";
                paramDiff.ComparedValue = frameGrabberToCompare.Type;
                paramDiff.CurrentValue = Type;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (frameGrabberToCompare.Id != Id) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Id";
                paramDiff.ParameterLabel = "Id";
                paramDiff.ParameterLocLabel = "Id";
                paramDiff.ComparedValue = frameGrabberToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            return ris;
        }
    }
}
