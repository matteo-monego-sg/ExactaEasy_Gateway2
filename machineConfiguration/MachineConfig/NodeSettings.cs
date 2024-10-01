using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration.MachineConfig
{
    public class NodeSetting
    {
        public string Id { get; set; }
        public string NodeDescription { get; set; }
        public string NodeProviderName { get; set; }
        public string ServerIP4Address { get; set; }
        public string IP4Address { get; set; }
        public string Port { get; set; }
        public string RemoteDesktopType { get; set; }
        public string User { get; set; }
        public string Key { get; set; }
        //public int RDPort { get; set; }

        //
        public static readonly string[] Providers = new string[] { "GretelNodeBase", "EoptisNodeBase2" };
    }
}
