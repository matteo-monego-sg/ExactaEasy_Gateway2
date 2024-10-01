using System.Collections.Generic;

namespace DisplayManager
{

    public class CameraDefinition {
        public int Id { get; set; }
        public string CameraType { get; set; }
        public string CameraDescription { get; set; }
        public string CameraProviderName { get; set; }
        public int Station { get; set; }
        public int Node { get; set; }
        public int Spindle { get; set; }
        public string IP4Address { get; set; }
        public int Head { get; set; }
        public int BufferSize { get; set; }
        public List<LightControllerDefinition> LightControllers { get; set; }
        public string Visualizer { get; set; }
        public int Rotation { get; set; }

        public CameraDefinition() {

            LightControllers = new List<LightControllerDefinition>();
        }
    }

    public class CameraDefinitionCollection : List<CameraDefinition> {

    }
}
