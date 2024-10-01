using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration.MachineConfig
{
    public class StationSetting
    {
        public string Id { get; set; }
        public string StationDescription { get; set; }
        public string StationProviderName { get; set; }
        public string Node { get; set; }


        //
        public static readonly string[] Providers = new string[] { "GretelStationBase", "EoptisStationBase" };
    }
}
