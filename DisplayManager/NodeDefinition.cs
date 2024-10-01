using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DisplayManager {

    public class NodeDefinition {

        public int Id { get; set; }
        public string NodeDescription { get; set; }
        public string NodeProviderName { get; set; }
        public string ServerIP4Address { get; set; }
        public string IP4Address { get; set; }
        public int Port { get; set; }
        public int RemoteDesktopType { get; set; }
        public string User { get; set; }
        public string Key { get; set; }
        public int RDPort { get; set; }
        [XmlIgnore]
        public int StationsCount { get; set; }
    }

    public class NodeDefinitionCollection : List<NodeDefinition> {

        public new NodeDefinition this[int id] {
            get { return Find(n => n.Id == id); }
        }
    }
}
