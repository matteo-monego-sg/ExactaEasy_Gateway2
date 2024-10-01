using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class LightRecipe {

        public LightRecipe() {

            StroboParameters = new ParameterCollection<Parameter>(); 
        }

        public int Id { get; set; }
        public ParameterCollection<Parameter> StroboParameters { get; set; }

        public LightRecipe Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            LightRecipe newObj = new LightRecipe();
            newObj.Id = Id;
            if (StroboParameters != null)
                newObj.StroboParameters = (ParameterCollection<Parameter>)StroboParameters.Clone(paramDictionary, cultureCode);
            return newObj;
        }

        public bool Compare(LightRecipe lightToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (lightToCompare == null) {
                lightToCompare = new LightRecipe();
                lightToCompare.Id = Id;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = lightToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            List<ParameterDiff> _paramDiffList;
            if (StroboParameters != null) {
                ris = ris | StroboParameters.Compare(lightToCompare.StroboParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            return ris;
        }

    }
}
