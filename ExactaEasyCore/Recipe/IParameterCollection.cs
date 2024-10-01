using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExactaEasyCore {

    public interface IParameterCollection : IList {

        IParameter this[string id] { get; set; }
        IParameter this[IParameter parameter] { get; set; }

        void Add(string id, string value);
        //IParameterCollection Clone();
        IParameterCollection Clone(ParameterInfoCollection paramDictionary, string cultureCode);
        //void CopyParametersInfo(IParameterCollection sourceParamCollection);
        void PopulateParametersInfo();
        void PopulateParametersInfo(IParameterCollection paramCollection);
        void Replace(IParameterCollection paramCollection);
    }
}
