using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//using SPAMI.LightControllers;

namespace ExactaEasyCore {

    public class CameraSetting {

        public int Id { get; set; }
        public int Head { get; set; }
        public int BufferSize { get; set; }
        public string CameraType { get; set; }
        public string CameraDescription { get; set; }
        public string CameraProviderName { get; set; }
        public int Station { get; set; }
        public int Node { get; set; }
        public int Spindle { get; set; }
        public string IP4Address { get; set; }
        public int PageNumberPosition { get; set; }
        public int DisplayPositionRow { get; set; }
        public int DisplayPositionCol { get; set; }
        public string Visualizer { get; set; }
        public int Rotation { get; set; }

        //public int StroboManager { get; set; }
        public string StroboIP4Address { get; set; }
        public int StroboChannel { get; set; }
        [XmlIgnore]
        public SPAMI.LightControllers.LightController LightCtrl;
        public List<LightControllerSettings> LightControllers { get; set; }

    }
}
