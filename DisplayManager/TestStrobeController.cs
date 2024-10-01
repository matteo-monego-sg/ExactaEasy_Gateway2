using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayManager {

    public class TestStrobeController : IStrobeController {

        public int Id { get; private set; }

        public TestStrobeController(int stroboId, string stroboAddress, string settingsPath) {

            Id = stroboId;
        }

        public void Connect() { }

        public ExactaEasyCore.ParameterCollection<ExactaEasyCore.Parameter> GetStrobeParameter(int channelId) {
            throw new NotImplementedException();
        }

        public void SetStrobeParameter(int channelId, ExactaEasyCore.ParameterCollection<ExactaEasyCore.Parameter> parameters) {
            throw new NotImplementedException();
        }

        public void ApplyParameters(int channelId) {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
