using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExactaEasyCore {

    public class RegistryKeyCtrl {

        public string Label { get; set; }
        public string HKLM_or_HKCU { get; set; }
        public string Root { get; set; }
        public string Value { get; set; }
        public string ValueExpected { get; set; }
    }
}
