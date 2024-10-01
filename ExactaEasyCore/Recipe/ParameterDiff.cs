using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public enum ParameterCompareDifferenceType {

        None = 0,
        Added,
        Modified,
        Deleted
    }

    public class ParameterDiff {
        
        public string ParameterId { get; set; }
        public string ParameterLabel  { get; set; }
        public string ParameterLocLabel { get; set; }
        public string ComparedValue { get; set; }
        public string CurrentValue { get; set; }
        public string ParameterPosition { get; set; }
        public ParameterCompareDifferenceType DifferenceType { get; set; }
    }
}
