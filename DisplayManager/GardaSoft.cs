using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExactaEasyCore;
using System.Globalization;
using ExactaEasyEng;
using System.Net.Sockets;

namespace DisplayManager {
    public class GardaSoft : IStrobeController {

        SPAMI.LightControllers.LightController lc = null;

        public int Id { get; private set; }

        public GardaSoft(int stroboId, string stroboAddress, string settingsPath) {

            Id = stroboId;
            lc = new SPAMI.LightControllers.LightController();
            lc.ClassName = "LightController";
            lc.Name = SPAMI.LightControllers.ControllerName.None;
            lc.XMLFilePath = settingsPath;
            try {
                lc.SearchControllers();
                lc.Bind(stroboAddress, SPAMI.LightControllers.ControllerVendor.NotSet, SPAMI.LightControllers.ControllerName.None);
                if (lc.ControllerSpecs != null) {
                    lc.Connect(lc.ControllerSpecs.IP, lc.ControllerSpecs.Port);
                    lc.SyncLoadFromController(-1);
                }
            }
            catch (Exception ex) {
                lc.Destroy();
                throw (ex);
            }
        }

        public void ApplyParameters(int channelId) {

            if (lc != null) {
                lc.SyncSendToController(channelId);
                //lc.SaveToXml();
                //LightCtrl.SyncLoadFromController(-1);
                //lc.SyncLoadFromController(channelId);
                //mando N impulsi software per inizializzare il controller coi nuovi parametri
                for (int i = 0; i < 100; i++) {
                    lc.SimulateInputTrigger(channelId);
                    System.Threading.Thread.Sleep(10);
                }
            }
            else throw new CameraException("No light controller associated");

        }

        public ParameterCollection<Parameter> GetStrobeParameter(int channelId) {

            ParameterCollection<Parameter> list = new ParameterCollection<Parameter>();
            list.Add("stroboOperatingMode", lc.ControllerSpecs.GetStringOperatingMode(channelId));
            list.Add("stroboCurrent", lc.ControllerSpecs.ChanConfig[channelId].OperatingCurrent.ToString(CultureInfo.InvariantCulture).Replace(",",""));
            list.Add("stroboPulseDelay", lc.ControllerSpecs.ChanConfig[channelId].OperatingDelay.ToString(CultureInfo.InvariantCulture).Replace(",",""));
            list.Add("stroboPulseWidth", lc.ControllerSpecs.ChanConfig[channelId].OperatingPulseWidth.ToString(CultureInfo.InvariantCulture).Replace(",",""));
            list.Add("stroboRetriggerTime", lc.ControllerSpecs.ChanConfig[channelId].OperatingRetrigger.ToString(CultureInfo.InvariantCulture).Replace(",",""));
            list.Add("stroboContinuousMaxCurrent", lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentContinuousModeA.ToString(CultureInfo.InvariantCulture).Replace(",",""));
            list.Add("stroboPulsedMaxCurrent", lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentPulsedModeA.ToString(CultureInfo.InvariantCulture).Replace(",",""));
            return list;
        }

        public void SetStrobeParameter(int channelId, ParameterCollection<Parameter> parameters) {

            lc.ControllerSpecs.ChanConfig[channelId].IDchan = channelId;
            if (parameters.Count > 0) {
                try {
                    lc.ControllerSpecs.SetStringOperatingMode(channelId, parameters["stroboOperatingMode"].Value);
                    lc.ControllerSpecs.ChanConfig[channelId].OperatingCurrent = Convert.ToDouble(parameters["stroboCurrent"].GetValue(), CultureInfo.InvariantCulture);
                    lc.ControllerSpecs.ChanConfig[channelId].OperatingDelay = Convert.ToDouble(parameters["stroboPulseDelay"].GetValue(), CultureInfo.InvariantCulture);
                    lc.ControllerSpecs.ChanConfig[channelId].OperatingPulseWidth = Convert.ToDouble(parameters["stroboPulseWidth"].GetValue(), CultureInfo.InvariantCulture);
                    lc.ControllerSpecs.ChanConfig[channelId].OperatingRetrigger = Convert.ToDouble(parameters["stroboRetriggerTime"].GetValue(), CultureInfo.InvariantCulture);
                    lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentContinuousModeA = Convert.ToDouble(parameters["stroboContinuousMaxCurrent"].GetValue(), CultureInfo.InvariantCulture);
                    lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentPulsedModeA = Convert.ToDouble(parameters["stroboPulsedMaxCurrent"].GetValue(), CultureInfo.InvariantCulture);
                }
                catch {
                    throw;
                }
            }
        }


        public SPAMI.LightControllers.LightController GetLightController() {

            return lc;
        }

        public void Dispose() {

            if (lc != null)
                lc.Dispose();
        }
    }

    public class TGardaSoft : Device, IStrobeController {

        SPAMI.LightControllers.LightController lc = null;
        Recipe recipe = null;

        UdpClient client = null;
        

        public int Id { get; private set; }
        public string StrobeAddress { get; private set; }

        public TGardaSoft(int strobeId, string strobeAddress, string settingsPath) {

            //lc = new SPAMI.LightControllers.LightController();
            //lc.ControllerSpecs = new SPAMI.LightControllers.ControllerSpecs();
            //lc.ControllerSpecs.IP = strobeAddress;
            Id = strobeId;
            StrobeAddress = strobeAddress;            
            recipe = Recipe.LoadFromFile("TGardasoft.xml");
        }

        protected override void ExecDeviceConnection() {

            client = new UdpClient(StrobeAddress, 30313);
        }

        public ParameterCollection<Parameter> GetStrobeParameter(int channelId) {

            Light light = recipe.Cams[0].Lights.Find((Light ll) => { return ll.Id == channelId; });

            ParameterCollection<Parameter> list = new ParameterCollection<Parameter>();
            if (light != null) {
                list.Add("stroboOperatingMode", light.StroboParameters["stroboOperatingMode"].GetValue().ToString());
                list.Add("stroboCurrent", light.StroboParameters["stroboCurrent"].GetValue().ToString());
                list.Add("stroboPulseDelay", light.StroboParameters["stroboPulseDelay"].GetValue().ToString());
                list.Add("stroboPulseWidth", light.StroboParameters["stroboPulseWidth"].GetValue().ToString());
                list.Add("stroboRetriggerTime", light.StroboParameters["stroboRetriggerTime"].GetValue().ToString());
                list.Add("stroboContinuousMaxCurrent", light.StroboParameters["stroboContinuousMaxCurrent"].GetValue().ToString());
                list.Add("stroboPulsedMaxCurrent", light.StroboParameters["stroboPulsedMaxCurrent"].GetValue().ToString());
            }
            return list;
        }

        public void SetStrobeParameter(int channelId, ParameterCollection<Parameter> parameters) {

            Light light = recipe.Cams[0].Lights.Find((Light ll) => { return ll.Id == channelId; });

            try {
                light.StroboParameters["stroboOperatingMode"].Value = parameters["stroboOperatingMode"].Value;
                light.StroboParameters["stroboCurrent"].Value = parameters["stroboCurrent"].Value;
                light.StroboParameters["stroboPulseDelay"].Value = parameters["stroboPulseDelay"].Value;
                light.StroboParameters["stroboPulseWidth"].Value = parameters["stroboPulseWidth"].Value;
                light.StroboParameters["stroboRetriggerTime"].Value = parameters["stroboRetriggerTime"].Value;
                light.StroboParameters["stroboContinuousMaxCurrent"].Value = parameters["stroboContinuousMaxCurrent"].Value;
                light.StroboParameters["stroboPulsedMaxCurrent"].Value = parameters["stroboPulsedMaxCurrent"].Value;

                recipe.SaveXml("TGardasoft.xml");
            }
            catch {
                throw;
            }

        }

        public void ApplyParameters(int channelId) {

        }

        public SPAMI.LightControllers.LightController GetLightController() {

            return lc;
        }


        public void Dispose() {

        }



        public override void CheckDeviceCommunicationState() {
            //TODO!!!!!!!
            return;
        }
    }
}
