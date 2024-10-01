using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class LightSensor {

        public LightSensor() {

            LightSensorParameters = new ParameterCollection<Parameter>();
            Shapes = new List<Shape>();
        }

        public ParameterCollection<Parameter> LightSensorParameters { get; set; }
        public List<Shape> Shapes { get; set; }

        public LightSensor Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            LightSensor newObj = new LightSensor();
            if (LightSensorParameters != null) {
                newObj.LightSensorParameters = new ParameterCollection<Parameter>();
                for (int ils = 0; ils < LightSensorParameters.Count; ils++)
                    newObj.LightSensorParameters.Add((Parameter)LightSensorParameters[ils].Clone());
            }
            if (Shapes!=null) {
                newObj.Shapes = new List<Shape>();
                for (int ils = 0; ils < Shapes.Count; ils++)
                    newObj.Shapes.Add(Shapes[ils].Clone());
            }
            return newObj;
        }

        public bool Compare(LightSensor lightSensorToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (lightSensorToCompare == null) {
                lightSensorToCompare = new LightSensor();
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = "";
                paramDiff.CurrentValue = "";
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            List<ParameterDiff> _paramDiffList = null;
            if (LightSensorParameters != null) {
                ris = ris | LightSensorParameters.Compare(lightSensorToCompare.LightSensorParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (Shapes != null) {
                for (int iS = 0; iS < Shapes.Count; iS++) {
                    if (iS > lightSensorToCompare.Shapes.Count - 1) {
                        Shape newSh = new Shape();
                        newSh.Id = Shapes[iS].Id;
                        lightSensorToCompare.Shapes.Add(newSh);
                    }
                    ris = ris | Shapes[iS].Compare(lightSensorToCompare.Shapes[iS], cultureCode, position + " Shape " + Shapes[iS].Id, paramDiffList);
                    if (_paramDiffList != null)
                        paramDiffList.AddRange(_paramDiffList);
                }
            }
            return ris;
        }
    }
}
