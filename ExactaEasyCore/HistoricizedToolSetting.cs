using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore
{
    public class HistoricizedToolSetting
    {
        public string Label { get; set; }
        public int NodeId { get; set; }
        public int StationId { get; set; }
        public int ToolIndex { get; set; }
        public int ParameterIndex { get; set; }
    }
}
