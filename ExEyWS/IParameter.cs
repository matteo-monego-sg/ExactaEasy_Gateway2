using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyEng {

    public interface IParameter {

        string Id { get; set; }
        string Label { get; set; }
        string Description { get; set; }
        string ValueType { get; set; }
        string Value { get; set; }
        string MinValue { get; set; }
        string MaxValue { get; set; }
        int IsVisible { get; set; }
        int IsEditable { get; set; }
        List<string> AdmittedValues { get; set; }

        IParameter Clone();
        void Clone(IParameter source);
        object GetValue();
        void PopulateParameterInfo(ParameterInfoCollection pDict, string cultureCode);        
        bool VerifyValue();
    }
}
