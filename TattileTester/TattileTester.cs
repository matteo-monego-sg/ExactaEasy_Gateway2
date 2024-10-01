using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisplayManager;
using TattileCameras;
using SPAMI.Util.Logger;

namespace TattileTesterUI {
    public partial class TattileTester : UserControl {

        [Browsable(false)]
        public CameraDefinition CamDef { get; private set; }
        [Browsable(false)]
        public Camera Cam { get; private set; }


        public TattileTester() {
            InitializeComponent();
        }

        private void TattileTester_Load(object sender, EventArgs e) {

        }

        public void Form2Par() {
            string camProvider = "TestCamera";
            if (cbType.Text == "M9")
                camProvider = "TattileCameraM9";
            if (cbType.Text == "M12")
                camProvider = "TattileCameraM12";
            CamDef = new CameraDefinition() {
                IP4Address = tbIP.Text,
                Id = Convert.ToInt32(tbID.Text),
                BufferSize = 20,
                CameraDescription = "ST 0 - CAM 0",
                CameraProviderName = camProvider,
                CameraType = cbType.Text,
                Head = 0,
                Station = 0,
                Rotation = 0
            };
        }

        private void btnCreate_Click(object sender, EventArgs e) {

            Form2Par();
            try {
                bool scan = cbScan.Checked;
                if (CamDef.CameraProviderName == "TestCamera")
                    Cam = new TestCamera.TestCamera(CamDef, scan);
                if (CamDef.CameraProviderName == "TattileCameraM9") {
                    Cam = new TattileCameraM9(CamDef, scan);
                    Cam.LoadExternalDependencies(new List<string>() { tbExtLib.Text });
                }
                if (CamDef.CameraProviderName == "TattileCameraM12") {
                    Cam = new TattileCameraM12(CamDef, scan);
                    Cam.LoadExternalDependencies(new List<string>() { tbExtLib.Text });
                }
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "TattileTester.btnCreate_Click", "Camera creation failed: " + ex.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e) {
            if (Cam == null) return;
            Cam.CameraConnect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e) {
            if (Cam == null) return;
            (Cam as IStation).Disconnect();
        }

        private void btnReset_Click(object sender, EventArgs e) {
            Cam.HardReset();
        }
    }
}
