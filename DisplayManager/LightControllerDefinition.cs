using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayManager {
    public class LightControllerDefinition {

        public int Id { get; set; }
        public string Description { get; set; }
        public int StrobeId { get; set; }
        public int StrobeChannel { get; set; }
    }
}
