using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public interface IParameter {

        string Id { get; set; }
        string Label { get; set; }
        string ExportName { get; set; }                 //new 13/09/2017
        string Value { get; set; }
        string Description { get; set; }
        string MinValue { get; set; }
        string MaxValue { get; set; }
        int IsVisible { get; set; } 
        //int IsEditable { get; set; }                  //del 13/09/2017
        double Increment { get; set; }                  //new 13/09/2017
        string MeasureUnit { get; set; }                //new 13/09/2017
        string ValueType { get; set; }
        List<string> AdmittedValues { get; set; }
        int Decimals { get; set; }                      //new 13/09/2017
        
        string Group { get; set; }
        string ParamId { get; set; }
        string ParentName { get; set; }

        IParameter Clone();
        void Clone(IParameter source);
        object GetValue();
        bool PopulateParameterInfo(ParameterInfoCollection pDict, string cultureCode);        
        bool VerifyValue();
        bool Compare(IParameter paramToCompare, out ParameterDiff paramDiff);
        bool IncrementValue(int sign, ref string newValue);
    }
}
