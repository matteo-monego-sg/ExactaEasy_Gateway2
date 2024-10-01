using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//using SPAMI.LightControllers;

namespace ExactaEasyCore {

    public class StationSetting {

        public int Id { get; set; }
        public string StationDescription { get; set; }
        public string StationProviderName { get; set; }
        public int Node { get; set; }
        [XmlArrayItem("Id")]
        public List<int> ToolResultsToStoreCollection { get; set; }
    }
}
