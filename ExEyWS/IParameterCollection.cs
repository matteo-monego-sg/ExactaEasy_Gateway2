using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExactaEasyEng {

    public interface IParameterCollection : IList {

        IParameter this[string id] { get; set; }

        void Add(string id, string value);
        IParameterCollection Clone();
        void CopyParametersInfo(IParameterCollection sourceParamCollection);
        void PopulateParametersInfo();
        void Replace(IParameterCollection paramCollection);
    }
}
