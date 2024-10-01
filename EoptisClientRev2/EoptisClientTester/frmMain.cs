using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisplayManager;
using EoptisClient;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace EoptisClientTester {
    public partial class frmMain : Form {

        NodeDefinition nd = new NodeDefinition();
        StationDefinition sd = new StationDefinition();
        CameraDefinition cd = new CameraDefinition();
        EoptisNodeBase2 client;
        Logger logger1 = new Logger();

        public frmMain() {

            InitializeComponent();
            logger1.WriteToConsole = true;
            logger1.WriteToConsoleLevel = LogLevels.Debug;
            logger1.Init();
            var values = Enum.GetValues(typeof(CameraWorkingMode)).Cast<CameraWorkingMode>();
            foreach (var val in values)
                cbWorkMode.Items.Add(val.ToString());
            recalcButton();
        }

        private void btnConnect_Click(object sender, EventArgs e) {

            try {
                nd.IP4Address = ipAddressCtrl.Text;
                nd.Port = Convert.ToInt32(nudPort.Value);
                sd.StationProviderName = "EoptisStationBase";
                sd.Node = nd.Id;
                cd.CameraProviderName = "EoptisDeviceBase";
                cd.IP4Address = nd.IP4Address;
                cd.Station = sd.Id;
                client = new EoptisNodeBase2(nd);
                client.Connect();
                client.Stations.Add(new EoptisStationBase(sd));
                client.Stations[0].Cameras.Add(new EoptisDeviceBase(cd, false));
                string currWorkMode = client.Stations[0].Cameras[0].GetWorkingMode().ToString();
                cbWorkMode.SelectedIndex = cbWorkMode.FindString(currWorkMode);
                tbWorkMode.Text = currWorkMode;
                cbWorkMode.SelectedValueChanged += cbWorkMode_SelectedIndexChanged;
                timer1.Start();
                recalcButton();
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "frmMain.btnConnect_Click", "Error while connecting: " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e) {

            try {
                cbWorkMode.SelectedValueChanged -= cbWorkMode_SelectedIndexChanged;
                if (client != null) {
                    client.Disconnect();
                    client.Dispose();
                    client = null;
                }
                timer1.Stop();
                recalcButton();
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "frmMain.btnDisconnect_Click", "Error while disconnecting: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) {

            //try {
            //    if (client != null && client.Connected)
            //        EoptisSvc.Test(nd.IP4Address);
            //    else
            //        MessageBox.Show("Not connected to " + nd.IP4Address + " !", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //catch (Exception ex) {
            //    Log.Line(LogLevels.Error, "frmMain.button1_Click", "Error while testing: " + ex.Message);
            //}
        }

        private void btnGet_Click(object sender, EventArgs e) {

            try {
                if (client != null && client.Connected) {
                    decimal value;
                    NodeRecipe nr = client.GetParameters();
                    ParameterCollection<Parameter> recSimpleParams = nr.Stations[0].Cameras[0].RecipeSimpleParameters;
                    IParameter param = recSimpleParams["TH_WARNING_UPPER"];
                    if (decimal.TryParse(param.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value))
                        nudWarnHighTh.Value = value;
                    param = recSimpleParams["TH_WARNING_LOWER"];
                    if (decimal.TryParse(param.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value))
                        nudWarnLowTh.Value = value;
                    param = recSimpleParams["TH_ERROR_UPPER"];
                    if (decimal.TryParse(param.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value))
                        nudErrHighTh.Value = value;
                    param = recSimpleParams["TH_ERROR_LOWER"];
                    if (decimal.TryParse(param.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value))
                        nudErrLowTh.Value = value;
                    tbWorkMode.Text = client.Stations[0].Cameras[0].GetWorkingMode().ToString();
                }
                else
                    MessageBox.Show("Not connected to " + nd.IP4Address + " !", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "frmMain.btnGet_Click", "GET PARAMETER ERROR: " + ex.Message);
            }
        }

        static bool led = false;
        private void btnSet_Click(object sender, EventArgs e) {

            try {
                if (client != null && client.Connected) {
                    NodeRecipe nr = new NodeRecipe();
                    nr.Id = 0;
                    nr.Stations = new List<StationRecipe>();
                    StationRecipe sr = new StationRecipe();
                    sr.Id = 0;
                    CameraRecipe cr = new CameraRecipe();
                    cr.AcquisitionParameters = new ParameterCollection<Parameter>();
                    cr.AcquisitionParameters.Add("PAR_LED_GREEN_POWER", led.ToString());
                    led = !led;
                    cr.AcquisitionParameters.Add("TH_WARNING_UPPER", nudWarnHighTh.Value.ToString());
                    cr.AcquisitionParameters.Add("TH_WARNING_UPPER", nudWarnHighTh.Value.ToString());
                    cr.AcquisitionParameters.Add("TH_WARNING_LOWER", nudWarnLowTh.Value.ToString());
                    cr.AcquisitionParameters.Add("TH_ERROR_UPPER", nudErrHighTh.Value.ToString());
                    cr.AcquisitionParameters.Add("TH_ERROR_LOWER", nudErrLowTh.Value.ToString());
                    sr.Cameras.Add(cr);
                    nr.Stations.Add(sr);
                    client.SetParameters("", nr, new ParameterInfoCollection(), "en");
                }
                else
                    MessageBox.Show("Not connected to " + nd.IP4Address + " !", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "frmMain.btnGet_Click", "GET PARAMETER ERROR: " + ex.Message);
            }
        }

        private void cbWorkMode_SelectedIndexChanged(object sender, EventArgs e) {

            //if (client != null && client.Connected) {
            //    CameraWorkingMode workMode;
            //    if (Enum.TryParse<CameraWorkingMode>(cbWorkMode.Text, out workMode))
            //        client.Stations[0].Cameras[0].SetWorkingMode(workMode);
            //    tbWorkMode.Text = client.Stations[0].Cameras[0].GetWorkingMode().ToString();
            //}
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e) {

            if (client != null) {
                try {
                    client.Disconnect();
                }
                catch {
                }
                finally {
                    client.Dispose();
                    client = null;
                }
            }
            logger1.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e) {

            //EoptisDeviceBase edb = client.Stations[0].Cameras[0] as EoptisDeviceBase;
            //tbWorkMode.Text = edb.CurrWorkingMode.ToString();
        }

        private void cbSetBusy_CheckedChanged(object sender, EventArgs e) {

            bool prevCheck = !cbSetBusy.Checked;
            bool res = cbSetBusy.Checked ? client.Lock() : client.Unlock();
            if (!res) {
                cbSetBusy.CheckedChanged -= cbSetBusy_CheckedChanged;
                cbSetBusy.Checked = prevCheck;
                cbSetBusy.CheckedChanged += cbSetBusy_CheckedChanged;
            }
            recalcButton();
        }

        private void recalcButton() {

            if (client != null && client.Connected) {
                cbSetBusy.Enabled = true;
                btnGet.Enabled = true;
                btnSet.Enabled = cbSetBusy.Checked;
            }
            else {
                cbSetBusy.Enabled = false;
                btnGet.Enabled = true;//false;
                btnSet.Enabled = false;
            }
        }

        private void btnLive_Click(object sender, EventArgs e) {

            EoptisDeviceBase edb = client.Stations[0].Cameras[0] as EoptisDeviceBase;
            edb.SetWorkingMode(CameraWorkingMode.Timed);
        }

        void setOutput(CheckBox cb, string id) {

            Parameter par = new Parameter();
            par.Id = id;
            par.Value = cb.Checked ? 1.ToString() : 0.ToString();
            EoptisDeviceBase edb = client.Stations[0].Cameras[0] as EoptisDeviceBase;
            edb.SetParameter(par);
        }

        private void cbReady_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_READY");
        }

        private void cbAcqReady_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_ACQ_READY");
        }

        private void cbVialId0_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_VIAL_ID_0");
        }

        private void cbVialId1_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_VIAL_ID_1");
        }

        private void cbDataValid_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_DATA_VALID");
        }

        private void cbReject0_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_REJECT_1");
        }

        private void cbReject1_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_REJECT_2");
        }

        private void cbReject2_CheckedChanged(object sender, EventArgs e) {

            setOutput(cbReady, "OUT_REJECT_3");
        }
    }
}
