using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasy {

    public class MachineConfiguration {

        public string Customer { get; set; }
        public string DefaultFolder { get; set; }
        public bool DebugEnabled { get; set; }

        public int TriggerMode { get; set; }
        public bool TriggerSourceManual { get; set; }

        public string MachineType { get; set; }
        public int NumberOfStation { get; set; }
        public int NumberOfCamera { get; set; }
        public int NumberOfSpindles { get; set; }

        public int NumberOfDisplayPages { get; set; }        

        public List<CameraSetting> CameraSettings { get; set; }
    }

    public class CameraSetting {

        public int Id { get; set; }
        public string CameraType { get; set; }
        public int Station { get; set; }
        public int Spindle { get; set; }
        public string IP4Address { get; set; }
        public int PageNumberPosition { get; set; }
        public int DisplayPosition { get; set; }        
    }
}
