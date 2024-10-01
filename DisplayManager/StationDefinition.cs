using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DisplayManager {

    public class StationDefinition {

        public int Id { get; set; }
        public string StationDescription { get; set; }
        public string StationProviderName { get; set; }
        public int Node { get; set; }
        [XmlIgnore]
        public int MaxInspectionQueueLength { get; set; }
        //[XmlIgnore]
        //public int MaxInspectionImgToSave { get; set; }
        [XmlArrayItem("Id")]
        public List<int> ToolResultsToStoreCollection { get; set; }
    }

    public class StationDefinitionCollection : List<StationDefinition> {
    }
}
