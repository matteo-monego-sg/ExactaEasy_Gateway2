using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class Shape {

        public Shape() {

            ShapeParameters = new ParameterCollection<Parameter>();
        }

        public int Id { get; set; }
        public ParameterCollection<Parameter> ShapeParameters { get; set; }

        public Shape Clone() {

            Shape newShape = new Shape();
            newShape.Id = Id;
            if (ShapeParameters != null) {
                newShape.ShapeParameters = new ParameterCollection<Parameter>();
                for (int ish = 0; ish < ShapeParameters.Count; ish++)
                    newShape.ShapeParameters.Add((Parameter)ShapeParameters[ish].Clone());
            }
            return newShape;
        }

        public bool Compare(Shape shapeToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (shapeToCompare == null) {
                shapeToCompare = new Shape();
                shapeToCompare.Id = Id;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = shapeToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            List<ParameterDiff> _paramDiffList = null;
            if (ShapeParameters != null) {
                ris = ris | ShapeParameters.Compare(shapeToCompare.ShapeParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            return ris;
        }
    }   
}
