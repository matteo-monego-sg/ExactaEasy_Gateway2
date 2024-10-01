using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration.MachineConfig
{
    public class CameraSetting
    {
        public string Id { get; set; }
        public string Head { get; set; }
        public string BufferSize { get; set; }
        public string CameraType { get; set; }
        public string CameraDescription { get; set; }
        public string CameraProviderName { get; set; }
        public string Station { get; set; }
        public string Node { get; set; }
        public string Spindle { get; set; }
        public string IP4Address { get; set; }
        public string PageNumberPosition { get; set; }
        public string DisplayPositionRow { get; set; }
        public string DisplayPositionCol { get; set; }
        public string Visualizer { get; set; }
        public string Rotation { get; set; }

        //
        public static readonly string[] Types = new string[] { "GretelCamera", "EoptisTurbidimeter", "Hvld" };
    }
}
