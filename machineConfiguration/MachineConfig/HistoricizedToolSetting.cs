using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace machineConfiguration.MachineConfig
{
    public class HistoricizedToolSetting
    {
        public string Label { get; set; }
        public string NodeId { get; set; }
        public string StationId { get; set; }
        public string ToolIndex { get; set; }
        public string ParameterIndex { get; set; }
    }
}
