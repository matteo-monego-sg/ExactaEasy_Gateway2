using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DisplayManager;

namespace TestCamera {
    public class TestCameraNode : Node {

        public bool IsDisposed { get; private set; }

        public TestCameraNode(NodeDefinition nodeDefinition)
            : base(nodeDefinition) {
        }

        public void Connect() {
        }


        public void Disconnect() {
        }

        public void Dispose() {
            foreach (TestCameraStation s in Stations) {
                if (!IsDisposed) {
                    s.StopGrab();
                    s.exit = true;
                    s.resultTh.Join(3000);
                    base.Dispose();
                    IsDisposed = true;
                }
            }
        }

        public override void StartImagesDump(List<ExactaEasyCore.StationDumpSettings> statDumpSettingsCollection) {
            
        }
    }
}
