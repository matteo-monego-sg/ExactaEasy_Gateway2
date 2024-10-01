using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayManager {
    public class LightController {

        public int Id { get; private set; }
        public string Description { get; private set; }
        public IStrobeController Strobe { get; private set; }
        public int StrobeChannel { get; private set; }

        public LightController(int id, string description, IStrobeController strobe, int strobeChannel) {

            Id = id;
            Description = description;
            Strobe = strobe;
            StrobeChannel = strobeChannel;
        }
    }
}
