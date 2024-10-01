using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
//using System.Reflection;

using SPAMI.Util;
using SPAMI.Util.XML;
using SPAMI.Util.Logger;
using System.Net.NetworkInformation;

namespace SPAMI.LightControllers
{
    public partial class LightController : Component, ICommon
    {
        [Browsable(false)]
        public string ClassName {get; set;}
        public LightController()
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            InitializeComponent();
        }

        public LightController(IContainer container)
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            container.Add(this);

            InitializeComponent();
        }

        //[Browsable(false)]
        //public string ControllerNameString{ get; set; }
        public delegate void DelegateMessageReceived(object sender, MessageReceivedEventArgs args);
        public event DelegateMessageReceived OnPostMessageReceived;
        public delegate void DelegateConnected(object sender, ConnectedEventArgs args);
        public event DelegateConnected OnPostConnection;
        public event DelegateConnected OnPostDisconnection;
        public delegate void DelegateChannel(object sender, ChannelEventArgs args);
        public event DelegateChannel OnPostLoadSettingsFromController;
        public event DelegateChannel OnPostSendSettingsToController;
        public event EventHandler OnPostSaveSettingsToController;
        public event EventHandler OnPostRestoreFactorySettings;

        [Browsable(true), Category("Controller"), Description("TODO")]
        public ControllerName Name { get; set; }

        public ControllerSpecs ControllerSpecs;
        private AutoResetEvent KillEv = new AutoResetEvent(false);
        private AutoResetEvent AsyncSendToControllerEv = new AutoResetEvent(false);
        WaitHandle[] waitEvents = new WaitHandle[2];
        private Thread SendTh;
        private Thread ReceiveTh;
        //private bool exit = false;
        private object SendLocker = new object();
        private TcpClient myClient;
        private NetworkStream stream;
        private string sendWord;
        private ManualResetEvent ReadyForNewCmdEv = new ManualResetEvent(true);
        private const int SendTimeout = 10000;
        //private const string sXMLDefaultFolder = @".\etc\";
        public readonly string XMLExt = ".xml";
        public List<DeviceDetected> DeviceConnectedList;
        private bool Initialized;

        public void Bind(string ip, ControllerVendor vendor, ControllerName name)
        {
            if (DeviceConnectedList != null)
            {
                if (XMLFilePath == null)
                {
                    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                    ofd.Title = "Select an XML file for the controller...";
                    ofd.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                    ofd.InitialDirectory = Directory.GetCurrentDirectory();
                    if (DialogResult.OK == ofd.ShowDialog() && File.Exists(ofd.FileName))
                    {
                        XMLFilePath = ofd.FileName;
                    }
                }
                foreach (DeviceDetected dd in DeviceConnectedList)
                {
                    if ((dd.IP == ip) &&
                        (dd.Vendor == vendor || vendor == ControllerVendor.NotSet) &&
                        (dd.Model == name || name == ControllerName.None) &&
                        (XMLFilePath != null && File.Exists(XMLFilePath)))
                    {
                        vendor = dd.Vendor;
                        Name = name = dd.Model;
                        if (LoadFromXml(XMLFilePath) != 0)
                            throw new LightControllerException("LoadFromXml fallisce.");
                        else
                        {
                            if (!Initialized)
                                Init();
                            Log.Line(LogLevels.Pass, ClassName + ".Bind", "Bind con controller " +
                                vendor.ToString() + " " + name.ToString() + "[" + ip + "] eseguito con successo.");
                            return; 
                        }
                    }
                }
            }
            throw new LightControllerException("Non ho trovato nessun controller che soddisfi i criteri di bind.");
        }

        void Init()
        {
            if (!this.DesignMode)
            {
                /*switch (Name)
                {
                    case ControllerName.None:
                        Log.Line(LogLevels.Error, ClassName + ".Init", "LightController non settato.");
                        break;
                    case ControllerName.RT820F_2:
                        ControllerSpecs = new RT820F_2();
                        break;
                    case ControllerName.RT820F_20:
                        ControllerSpecs = new RT820F_20();
                        break;
                    case ControllerName.PP820:
                        ControllerSpecs = new PP820();
                        break;
                    default:
                        Log.Line(LogLevels.Error, ClassName + ".Init", "LightController non contemplato.");
                        break;
                }*/
                this.Disposed += new EventHandler(LightController_Disposed);
                //Multilang.LanguageChanged += new EventHandler(OnLanguageChanged);
                //Multilang.AddMeToMultilanguage(this);
                waitEvents[0] = KillEv;
                waitEvents[1] = AsyncSendToControllerEv;
                SendTh = new Thread(new ThreadStart(SendThread));
                SendTh.Name = "Light Controller Send Thread";
                SendTh.Priority = ThreadPriority.Normal;
                SendTh.Start();
                Initialized = true;
            }
        }

        public void LightController_Disposed(object sender, EventArgs args)
        {
            Destroy();
        }

        public void Destroy()
        {
            Initialized = false;
            KillEv.Set();
            ExitReceiveThEv.Set();
            if (SendTh != null)
                SendTh.Join();
            if (ControllerStatusRequestTh != null)
                ControllerStatusRequestTh.Join();
            if (ControllerSendParamTh != null)
                ControllerSendParamTh.Join();
            if (broadcastReceiveTh != null) {
                foreach (Thread th in broadcastReceiveTh) {
                    if (broadcastReceiveSocket != null) {
                        //broadcastReceiveSocket.Shutdown(SocketShutdown.Both);
                        broadcastReceiveSocket.Close();
                    }
                    if (th != null && th.IsAlive)
                        th.Join(1000);
                }
            }
            Disconnect();
        }

        public void Connect(string ip, int port)
        {
            Disconnect();
            ControllerSpecs.IP = ip;
            ControllerSpecs.Port = port;
            if (ControllerSpecs.Port < 1 || ControllerSpecs.Port > Math.Pow(2, 16) - 1)
            {
                string error = "Port out of range: " + ControllerSpecs.Port.ToString();
                Log.Line(LogLevels.Error, ClassName + ".Connect", error);
                throw new LightControllerException(error);
            }
            try
            {
                if (ControllerSpecs.ComMode == ComMode.Ethernet)
                {
                    myClient = new TcpClient(ip, port);
                    stream = myClient.GetStream();
                    ReceiveTh = new Thread(new ThreadStart(ReceiveThread));
                    ReceiveTh.Name = "Light Controller Receive Thread";
                    ReceiveTh.Priority = ThreadPriority.Normal;
                    ReceiveTh.Start();
                }
                if (ControllerSpecs.ComMode == ComMode.RS232)
                {
                    //TODO: APRI PORTA
                }
                Log.Line(LogLevels.Pass, ClassName + ".Connect", "Connesso a " + ip + ":{0}.", port);
                if (OnPostConnection != null) OnPostConnection(this, new ConnectedEventArgs(true, ip, port));
            }
            catch (Exception ex)
            {
                string error = "ERRORE! Impossibile stabilire connessione con " + ip + ":{0}." + port.ToString();
                Log.Line(LogLevels.Error, ClassName + ".Connect", error);
                Log.Line(LogLevels.Error, ClassName + ".Connect", ex.ToString());
                if (OnPostConnection != null) OnPostConnection(this, new ConnectedEventArgs(false, ip, port));
                throw new LightControllerException(error);
            }
        }

        public void Disconnect()
        {
            if (ControllerSpecs!=null && 
                ControllerSpecs.ComMode == ComMode.Ethernet)
            {
                if (stream != null)
                {
                    Log.Line(LogLevels.Warning, ClassName + ".Disconnect", "Disconesso da " + ControllerSpecs.IP + ".");
                    stream.Close();
                }
                if (myClient != null)
                    myClient.Close();
                Thread.Sleep(5);
                stream = null;
                myClient = null;
                for (int ibc = 0; ibc < bcNum; ibc++)
                {
                    if (ReceiveTh != null)
                        ReceiveTh.Join();
                }
            }
            if (ControllerSpecs != null && 
                ControllerSpecs.ComMode == ComMode.RS232)
            {
                //TODO CHIUDI PORTA
                //Log.Line(LogLevels.Warning, ClassName + ".Disconnect", "Disconesso da " + IP + ".");
            }
            if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
        }

        /*public void OnLanguageChanged(object sender, EventArgs args)
        {
            string sAssName = Multilang.AssemblyNameExtract(System.Reflection.Assembly.GetExecutingAssembly());
            if (Multilang.IsAssemblyStringTableLoaded(sAssName))
            {
                //button1.Text = Multilanguage.GetString(sAssName, "Buongiorno");
            }
        }*/

        #region Search Controllers
        const int bcNum = 1;
        int[] bcSendPort = new int[bcNum] { 30311 };
        int[] bcReceivePort = new int[bcNum] { 30310 };
        string[] bcSearchMessage = new string[bcNum] { "Gardasoft Search" };
        int[] bcTimeout = new int[bcNum] { 500 };                 // [ms]
        string[] bcValSeparator = new string[bcNum] { "," };
        Thread[] broadcastReceiveTh;
        public bool SearchControllers()
        {
            DeviceConnectedList = new List<DeviceDetected>();
            broadcastReceiveTh = new Thread[bcNum];
            for (int iBC = 0; iBC < bcNum; iBC++)
            {
                ExitReceiveThEv.Reset();
                /*DeviceConnectedList.Clear();
                if (ControllerSpecs.BroadcastSendPort < 1 || ControllerSpecs.BroadcastSendPort > Math.Pow(2, 16) - 1)
                {
                    Log.Line(LogLevels.Error, ClassName + ".SearchControllers", "BroadcastSendPort out of range: {0}", ControllerSpecs.BroadcastSendPort);
                    return false;
                }
                if (ControllerSpecs.BroadcastReceivePort < 1 || ControllerSpecs.BroadcastReceivePort > Math.Pow(2, 16) - 1)
                {
                    Log.Line(LogLevels.Error, ClassName + ".SearchControllers", "BroadcastSendPort out of range: {0}", ControllerSpecs.BroadcastReceivePort);
                    return false;
                }*/
                broadcastReceiveTh[iBC] = new Thread(new ParameterizedThreadStart(BroadcastRecThread));
                broadcastReceiveTh[iBC].Start((object)iBC);
                /*string message = ControllerSpecs.BroadcastSearchMessage;
                if (ControllerSpecs.BroadcastSearchMessage == null || message.Length < 1)
                {
                    Log.Line(LogLevels.Error, ClassName + ".SearchControllers", "BroadcastSearchMessage not valid: "+ ControllerSpecs.BroadcastSearchMessage);
                    return false;
                }*/
                //string message = "Light Controller Search. If you are a light controller please reply!";
            
                byte[] sendData = Encoding.ASCII.GetBytes(bcSearchMessage[iBC]);
                var ni = NetworkInterface.GetAllNetworkInterfaces();
                var ni2 = ni.OrderBy(x => x.Id).ToArray();
                for (int i = 0; i < ni.Length; i++)
                {
                    foreach (var ua in ni2[i].GetIPProperties().UnicastAddresses)
                    {
                        if ((ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) &&
                            ni2[i].OperationalStatus == OperationalStatus.Up)
                        {
                            try
                            {
                                //Log.Line(LogLevels.Debug, ClassName + ".SearchControllers", "Ip Addr send from: " + ua.Address.ToString());
                                Socket sendSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                sendSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                                sendSock.Bind(new IPEndPoint(ua.Address, bcSendPort[iBC]));
                                sendSock.SendTo(sendData, SocketFlags.None, new IPEndPoint(IPAddress.Broadcast, bcSendPort[iBC]));
                                sendSock.Close();
                            }
                            catch (SocketException ex)
                            {
                                string error = "Socket Error " + ex.ErrorCode.ToString() + ex.ToString();
                                Log.Line(LogLevels.Error, ClassName + ".SearchControllers", error);
                                throw new LightControllerException(error);
                            }
                        }
                    }
                }
                Thread.Sleep(2000); //pier: meglio fare su evento con timeout
                if (broadcastReceiveSocket != null)
                broadcastReceiveSocket.Close();
                ExitReceiveThEv.Set();
            }
            
            return true;
        }

        Socket broadcastReceiveSocket;
        AutoResetEvent ExitReceiveThEv = new AutoResetEvent(false);
        private void BroadcastRecThread(object ID)
        {
            int id = (int)ID;
            //List<DeviceDetected> DevsDetectedList = (List<DeviceDetected>) DevsDetectedObj;
            IPEndPoint iep3;
            EndPoint ep;
            try
            {
                broadcastReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                iep3 = new IPEndPoint(IPAddress.Any, bcReceivePort[id]);
                broadcastReceiveSocket.Bind(iep3);
                ep = (EndPoint)iep3;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".BroadcastRecThread", "Socket error. " + ex.ToString());
                return;
            }
            byte[] recData = new byte[1024];
            while (true)
            {
                try
                {
                    if (ExitReceiveThEv.WaitOne(5))
                        break;
                    int recv = broadcastReceiveSocket.ReceiveFrom(recData, ref ep);
                    string responseData = Encoding.ASCII.GetString(recData, 0, recv);
                    string[] responseDataSplit = responseData.Split(bcValSeparator, StringSplitOptions.RemoveEmptyEntries);
                    //controllo che non ce ne sia già uno con lo stesso S.N.
                    bool toAdd = true;
                    foreach (DeviceDetected dd in DeviceConnectedList)
                    {
                        if (dd.SerialNumber == responseDataSplit[2])
                            toAdd = false;
                    }
                    if (toAdd)
                    {
                        DeviceConnectedList.Add(new DeviceDetected(responseDataSplit[0], responseDataSplit[1], responseDataSplit[2], ((IPEndPoint)ep).Address.ToString()));
                        Log.Line(LogLevels.Debug, ClassName + ".BroadcastRecThread", "NUOVO DEVICE: " + DeviceConnectedList.Last().ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log.Line(LogLevels.Debug, ClassName + ".BroadcastRecThread", "ReceiveFrom Cancelled. " + ex.ToString());
                }
            }
            Log.Line(LogLevels.Debug, ClassName + ".BroadcastRecThread", "FINE BROADCAST RECEIVE THREAD.");
        }
        #endregion

        #region SendFunctions
        public bool AppendCmdToSend(string command)
        {
            if (myClient == null || !myClient.Connected)
            {
                Log.Line(LogLevels.Warning, ClassName + ".AppendCmdToSend", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return false;
            }
            bool ret = true;
            lock (SendLocker)
            {
                if (sendWord.Length > 0)
                    sendWord += ControllerSpecs.CmdSeparator;
                sendWord += command;
                ReadyForNewCmdEv.Reset();
                AsyncSendToControllerEv.Set();
                if (!ReadyForNewCmdEv.WaitOne(SendTimeout))
                {
                    Log.Line(LogLevels.Error, ClassName + ".AppendCmdToSend", "Timeout nell'invio del comando: " + sendWord);
                    ret = false;
                }
            }
            return ret;
        }

        public bool CmdSend()
        {
            if (myClient == null || !myClient.Connected)
            {
                Log.Line(LogLevels.Warning, ClassName + ".CmdSend", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return false;
            }
            bool ret = true;
            if (sendWord.Length > 0)
            {
                sendWord += "\n";
                lock (SendLocker)
                {
                    ReadyForNewCmdEv.Reset();
                    AsyncSendToControllerEv.Set();
                    if (!ReadyForNewCmdEv.WaitOne(SendTimeout))
                    {
                        Log.Line(LogLevels.Error, ClassName + ".CmdSend", "Timeout nell'invio del comando: " + sendWord);
                        ret = false;
                    }
                }
            }
            return ret;
        }

        public bool CmdSend(string command)
        {
            if (myClient == null || !myClient.Connected)
            {
                Log.Line(LogLevels.Warning, ClassName + ".CmdSend", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return false;
            }
            bool ret = true;
            lock (SendLocker)
            {
                ReadyForNewCmdEv.Reset();
                sendWord += command + "\n";
                AsyncSendToControllerEv.Set();
                if (!ReadyForNewCmdEv.WaitOne(SendTimeout))
                {
                    Log.Line(LogLevels.Error, ClassName + ".CmdSend", "Timeout nell'invio del comando: " + sendWord);
                    ret = false;
                }
            }
            return ret;
        }

        public bool SimulateInputTrigger(int channel)
        {
            string command = "";
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                command = "TR" + (channel + 1).ToString();
            }
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                command = "TR" + channel.ToString();
            }
            return CmdSend(command);
        }

        Thread ControllerSendParamTh;
        public int AsyncSendToController(int channelID)    //-1 for all channels
        {
            if (myClient != null && myClient.Connected)
            {
                ControllerSendParamTh = new Thread(new ParameterizedThreadStart(SendToControllerThread));
                ControllerSendParamTh.Start(channelID);
                return 0;
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".AsyncSendToController", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return -10;
            }
        }

        public int SyncSendToController(int channelID)    //-1 for all channels
        {
            if (myClient != null && myClient.Connected)
            {
                return SendToController(channelID);
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".SyncSendToController", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return -10;
            } 
        }

        private void SendToControllerThread(object channelIDobj)
        {
            SendToController(channelIDobj);
        }

        private int SendToController(object channelIDobj)
        {
            int channelID = (int)channelIDobj;
            int initChan = 0;
            int endChan = ControllerSpecs.FixedOutputChannelNumber;
            if (channelID > -1)
            {
                initChan = channelID;
                endChan = initChan + 1;
            }
            bool ok = true;
            ok &= CmdClearErrors();
            for (int ch = initChan; ch < endChan; ch++)
            {
                double dCurrentMaxValA = Math.Min(ControllerSpecs.FixedCurrentFullScaleA, ControllerSpecs.GetMaxBright(ch));
                if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
                {
                    ok &= CmdSend("VL" + (ch + 1).ToString() + ControllerSpecs.MulNumericValSeparator +
                        0.ToString() + ControllerSpecs.MulNumericValSeparator + dCurrentMaxValA.ToGBString("f3"));
                }
                ok &= CmdOperatingMode(ch);
                ok &= CmdOptionFlags(ch);
                ok &= CmdTriggerMode(ch);
                ok &= CmdTrigger(ch);
                
                Thread.Sleep(50);
            }
            if (ok)
            {
                if (OnPostSendSettingsToController != null) OnPostSendSettingsToController(this, new ChannelEventArgs(channelID));
                Log.Line(LogLevels.Pass, ClassName + ".SendToController", "Configurazione inviata al dispositivo con successo.");
                return 0;
            }
            return -1;
        }

        private bool CmdTriggerMode(int mode)   
        {
            // 0: all chs triggered individually
            // 1: all chs triggered by input 0
            // 2: channels 0-3 from trigger 0, channels 4-7 from trigger 4
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                return CmdSend("FP"+mode.ToString());
            }
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                Log.Line(LogLevels.Debug, ClassName + ".CmdTriggerMode", "Modalità trigger non disponibile per serie RT Gardasoft");
            }
            return true;
        }

        private bool CmdTrigger(int channel)
        {
            string command = "";
            if (ControllerSpecs.InternalTrigger==true)
            {
                if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
                {
                    command = "TT1" + ControllerSpecs.MulNumericValSeparator + ControllerSpecs.InternalTriggerMs.ToGBString("f3");
                }
                if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
                {
                    if (ControllerSpecs.InternalTriggerMs <= 0)
                        command = "TT1";    // 25Hz -> 40ms
                    else
                        command = "TT1" + ControllerSpecs.MulNumericValSeparator + (ControllerSpecs.InternalTriggerMs*1000).ToGBString("f3");
                }
            }
            else
            {
                command = "TT0";
                if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
                {
                    if ((LightModeGardasoftRT)ControllerSpecs.ChanConfig[channel].OperatingMode == LightModeGardasoftRT.Pulsed ||
                        (LightModeGardasoftRT)ControllerSpecs.ChanConfig[channel].OperatingMode == LightModeGardasoftRT.Switched)
                    {
                        if (command.Length > 0) command += ControllerSpecs.CmdSeparator;
                        command += "RP" + (channel + 1).ToString() + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingInputTrigger.ToString();
                    }
                }
            }
            if (command.Length > 0) return CmdSend(command);
            else return true;
        }

        private bool CmdClearErrors() {
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES) {
                string command = "GR";
                return CmdSend(command);
            }
            return true;
        }

        private bool CmdOptionFlags(int channel)
        {
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                int P = (ControllerSpecs.ChanConfig[channel].P == true) ? 1 : 0;
                int E = (ControllerSpecs.ChanConfig[channel].E == true) ? 1 : 0;
                byte flags = (byte)((P << 2) | (E << 1));
                string command = "RE" + (channel + 1).ToString() + ControllerSpecs.MulNumericValSeparator + flags.ToString();
                return CmdSend(command);
            }
            return true;
        }

        private bool CmdOperatingMode(int channel)
        {
            bool ret = true;
            string command = "";
            int ch = channel;       //PP SERIES
            if (ControllerSpecs.ControllerFam==ControllerFamily.Gardasoft_RT_SERIES) 
            {
                ch = channel + 1;   //RT SERIES
                double percentVal = 0; 
                switch ((LightModeGardasoftRT)ControllerSpecs.ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftRT.OFF:
                        ControllerSpecs.ChanConfig[channel].OperatingCurrent = 0;
                        command = "RS" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator + "0";
                        break;
                    case LightModeGardasoftRT.Continuous:
                        ControllerSpecs.ChanConfig[channel].OperatingCurrent = Math.Min(ControllerSpecs.ChanConfig[channel].OperatingCurrent, ControllerSpecs.FixedCurrentFullScaleA);
                        percentVal = Utilities.IngToPercent(ControllerSpecs.ChanConfig[channel].OperatingCurrent, 0, Math.Min(ControllerSpecs.ChanConfig[channel].MaxCurrentContinuousModeA, ControllerSpecs.FixedCurrentFullScaleA));
                        command = "RS" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            percentVal.ToGBString("f3");
                        break;
                    case LightModeGardasoftRT.Pulsed:
                        ControllerSpecs.ChanConfig[channel].OperatingCurrent = Math.Min(ControllerSpecs.ChanConfig[channel].OperatingCurrent, ControllerSpecs.ChanConfig[channel].MaxCurrentPulsedModeA);
                        percentVal = Utilities.IngToPercent(ControllerSpecs.ChanConfig[channel].OperatingCurrent, 0, Math.Min(ControllerSpecs.ChanConfig[channel].MaxCurrentPulsedModeA, ControllerSpecs.FixedCurrentFullScaleA));
                        command = "RT" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingPulseWidth.ToGBString("f3") + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingDelay.ToGBString("f3") + ControllerSpecs.MulNumericValSeparator +
                            percentVal.ToGBString("f3");
                        if (ControllerSpecs.ChanConfig[channel].OperatingRetrigger >= 0)
                        {
                            command += ControllerSpecs.MulNumericValSeparator + ControllerSpecs.ChanConfig[channel].OperatingRetrigger.ToGBString("f3");
                        }
                        break;
                    case LightModeGardasoftRT.Selected:
                        command = "RU" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            Utilities.IngToPercent(ControllerSpecs.ChanConfig[channel].OperatingCurrent, 0, ControllerSpecs.ChanConfig[channel].MaxCurrentContinuousModeA).ToGBString("f3") + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingSelectedModeAddParam.ToGBString("f3");   //pier: convertire da eu in percentuale
                        break;
                    case LightModeGardasoftRT.Switched:
                        command = "RW" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            Utilities.IngToPercent(ControllerSpecs.ChanConfig[channel].OperatingCurrent, 0, ControllerSpecs.ChanConfig[channel].MaxCurrentContinuousModeA).ToGBString("f3");
                        break;
                }
            }
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                ch = channel; //PP SERIES
                switch ((LightModeGardasoftPP)ControllerSpecs.ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftPP.None:
                    case LightModeGardasoftPP.OFF:
                        ControllerSpecs.ChanConfig[channel].OperatingCurrent = 0;
                        command = "RS" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator + "0";
                        break;
                    case LightModeGardasoftPP.Continuous:
                        command = "RS" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingCurrent.ToGBString("f3");
                        break;
                    case LightModeGardasoftPP.Pulsed:
                        command = "RT" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            (ControllerSpecs.ChanConfig[channel].OperatingPulseWidth * 1000).ToGBString("f3") + ControllerSpecs.MulNumericValSeparator +
                            (ControllerSpecs.ChanConfig[channel].OperatingDelay * 1000).ToGBString("f3") + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingCurrent.ToGBString("f3");
                        if (ControllerSpecs.ChanConfig[channel].OperatingRetrigger >= 0)
                        {
                            command += ControllerSpecs.MulNumericValSeparator + (ControllerSpecs.ChanConfig[channel].OperatingRetrigger * 1000).ToGBString("f3");
                        }
                        break;
                    case LightModeGardasoftPP.Switched:
                        command = "RW" + (ch).ToString() + ControllerSpecs.MulNumericValSeparator +
                            ControllerSpecs.ChanConfig[channel].OperatingCurrent.ToGBString("f3");
                        break;
                }
            }
            ret = CmdSend(command);
            return ret;
        }

        Thread ControllerStatusRequestTh;
        public int AsyncLoadFromController(int channelID)
        {
            if (myClient != null && myClient.Connected)
            {
                ControllerStatusRequestTh = new Thread(new ParameterizedThreadStart(LoadFromControllerThread));
                ControllerStatusRequestTh.Start(channelID);
                return 0;
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".AsyncLoadFromController", "Controller disconnesso.");
                string ip = (ControllerSpecs == null) ? "" : ControllerSpecs.IP;
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ip, -1));
                return -10;
            }
        }

        public int SyncLoadFromController(int channelID)
        {
            if (myClient != null && myClient.Connected)
            {
                return LoadFromController(channelID);
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".SyncLoadFromController", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return -10;
            }
        }

        private void LoadFromControllerThread(object channelIDobj)
        {
            LoadFromController(channelIDobj);
        }

        private int LoadFromController(object channelIDobj)
        {
            int channelID = (int)channelIDobj;
            int initChan = 0;
            int endChan = ControllerSpecs.FixedOutputChannelNumber;
            if (channelID > -1)
            {
                initChan = channelID;
                endChan = initChan + 1;
            }
            bool ok = true;
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                ok = CmdSend("ST0");
                for (int ch = initChan; ch < endChan; ch++)
                {
                    ok &= CmdSend("ST" + (ch + 1).ToString());
                    Thread.Sleep(50);
                }
                if (ok)
                {
                    if (OnPostLoadSettingsFromController != null) OnPostLoadSettingsFromController(this, new ChannelEventArgs(channelID));
                    Log.Line(LogLevels.Pass, ClassName + ".LoadFromController", "Configurazione attualmente presente sul dispositivo caricata con successo.");
                    return 0;
                }
            }
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                ok = CmdSend("ST8");
                for (int ch = initChan; ch < endChan; ch++)
                {
                    ok &= CmdSend("ST" + ch.ToString());
                    Thread.Sleep(50);
                }
                if (ok)
                {
                    if (OnPostLoadSettingsFromController != null) OnPostLoadSettingsFromController(this, new ChannelEventArgs(channelID));
                    Log.Line(LogLevels.Pass, ClassName + ".LoadFromController", "Configurazione attualmente presente sul dispositivo caricata con successo.");
                    return 0;
                }
            }
            return -1;  //generic error
        }

        public int SaveToFlash()
        {
            if (myClient != null && myClient.Connected)
            {
                if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES ||
                    ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
                {
                    if (CmdSend("AW"))
                    {
                        if (OnPostSaveSettingsToController != null) OnPostSaveSettingsToController(this, EventArgs.Empty);
                        Log.Line(LogLevels.Pass, ClassName + ".SaveToFlash", "Configurazione attualmente presente sul dispositivo memorizzata su flash.");
                        return 0;
                    }
                }
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".SaveToFlash", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return -10;
            }
            return -1;
        }

        public int RestoreFactorySettings()
        {
            if (myClient != null && myClient.Connected)
            {
                if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES ||
                    ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
                {
                    if (CmdSend("CL"))
                    {
                        if (OnPostRestoreFactorySettings != null) OnPostRestoreFactorySettings(this, EventArgs.Empty);
                        Log.Line(LogLevels.Pass, ClassName + ".RestoreFactorySettings", "Ricaricati valori di fabbrica sul dispositivo.");
                        return 0;
                    }
                }
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".RestoreFactorySettings", "Controller disconnesso.");
                if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
                return -10;
            }
            return -1;
        }

        private void ParseReply(string reply)
        {
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                string[] splitter = new string[4] { ControllerSpecs.MulNumericValSeparator, "\n", "\r", ControllerSpecs.ReplyTerminator };
                string[] replySplitted = reply.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                if (replySplitted[0].Contains("ST"))    //
                    ParseReplyST(replySplitted);
            }
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                string[] splitter = new string[4] { " ", "\n", "\r", ControllerSpecs.ReplyTerminator };
                string[] replySplitted = reply.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                if (replySplitted[0].Contains("ST"))    //
                    ParseReplyST(replySplitted);
            }

        }

        private void ParseReplyST(string[] replySplitted)
        {
            int IDchan = 0;
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                double percentCurrentVal = 0;
                for (int len = 0; len < replySplitted.Length; len++)
                {
                    string s = replySplitted[len];
                    s = s.Replace(" ", "");
                    try
                    {
                        if (s.Substring(0, 2) == "ST")   //channel
                        {
                            continue;
                        }
                        if (s.Substring(0, 2) == "CH")   //channel
                        {
                            s = s.Replace("CH", "");
                            IDchan = System.Convert.ToInt32(s) - 1;
                            ControllerSpecs.ChanConfig[IDchan].IDchan = IDchan;
                            continue;
                        }
                        if (s.Substring(0, 2) == "MD")   //operating mode
                        {
                            s = s.Replace("MD", "");
                            ControllerSpecs.ChanConfig[IDchan].OperatingMode = System.Convert.ToInt32(s);
                            continue;
                        }
                        if (s.Substring(0, 1) == "S")   //brightness percentage settings
                        {
                            s = s.Replace("S", "");
                            percentCurrentVal = Utilities.FromGBString(s);
                            len++;
                            ControllerSpecs.ChanConfig[IDchan].OperatingSelectedModeAddParam = Utilities.FromGBString(replySplitted[len]);  //pier: convertire da percentuale a eu
                            continue;
                        }
                        if (s.Substring(0, 2) == "DL")   //pulse delay
                        {
                            ControllerSpecs.SetOperatingDelay(IDchan, MixedValue("DL", s, "T"));
                            continue;
                        }
                        if (s.Substring(0, 2) == "PU")   //pulse width
                        {
                            ControllerSpecs.SetOperatingPulseWidth(IDchan, MixedValue("PU", s, "T"));
                            continue;
                        }
                        if (s.Substring(0, 2) == "RT")   //retrigger delay
                        {
                            ControllerSpecs.ChanConfig[IDchan].OperatingRetrigger = MixedValue("RT", s, "T");
                            continue;
                        }
                        if (s.Substring(0, 2) == "IP")   //input trigger
                        {
                            s = s.Replace("IP", "");
                            ControllerSpecs.ChanConfig[IDchan].OperatingInputTrigger = System.Convert.ToInt32(s);
                            continue;
                        }
                        if (s.Substring(0, 2) == "FL")   //input trigger
                        {
                            s = s.Replace("FL", "");
                            byte flags = System.Convert.ToByte(s);
                            ControllerSpecs.ChanConfig[IDchan].E = (((flags >> 1) & 0x1) > 0) ? true : false;
                            ControllerSpecs.ChanConfig[IDchan].P = (((flags >> 2) & 0x1) > 0) ? true : false;
                            continue;
                        }
                        if (s.Substring(0, 2) == "CS")   //rating of the light
                        {
                            //int pippo = 1;
                            //chConf.OperatingRetrigger = MixedValue("CS", s);
                            continue;
                        }
                        if (s.Substring(0, 2) == "RA")   //rating of the light
                        {
                            double fullscale = MixedValue("RA", s, "A");
                            ControllerSpecs.ChanConfig[IDchan].OperatingCurrent = Math.Round(Utilities.PercentToIng(percentCurrentVal, 0, fullscale), 2, MidpointRounding.ToEven);
                            continue;
                        }
                        if (s.Substring(0, 2) == "TM")   //rating of the light
                        {
                            s = s.Replace("TM", "");
                            ControllerSpecs.InternalTrigger = (System.Convert.ToInt32(s) == 1) ? true : false;
                            continue;
                        }
                        if (s.Substring(0, 2) == "TP")   //rating of the light
                        {
                            ControllerSpecs.InternalTriggerMs = MixedValue("TP", s, "T");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Line(LogLevels.Error, ClassName + ".ParseReply", "Errore: " + ex.ToString());
                    }
                }
            }
            if (ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                for (int len = 0; len < replySplitted.Length; len++)
                {
                    string s = replySplitted[len];
                    s = s.Replace(" ", "");
                    try
                    {
                        if (s.Length>1 && s.Substring(0, 2) == "ST")   //channel
                        {
                            continue;
                        }
                        if (s == "CH")   //channel
                        {
                            IDchan = System.Convert.ToInt32(replySplitted[++len]);
                            ControllerSpecs.ChanConfig[IDchan].IDchan = IDchan;
                            continue;
                        }
                        if (s == "M")   //operating mode
                        {
                            ControllerSpecs.ChanConfig[IDchan].OperatingMode = System.Convert.ToInt32(replySplitted[++len]);
                            continue;
                        }
                        if (s == "V")   //current
                        {
                            ControllerSpecs.ChanConfig[IDchan].OperatingCurrent = MixedValue("", replySplitted[++len], "A");
                            continue;
                        }
                        if (s == "D")   //pulse delay
                        {
                            ControllerSpecs.SetOperatingDelay(IDchan, MixedValue("", replySplitted[++len], "T"));                            
                            continue;
                        }
                        if (s == "P")   //pulse width
                        {
                            ControllerSpecs.SetOperatingPulseWidth(IDchan, MixedValue("", replySplitted[++len], "T"));  
                            continue;
                        }
                        if (s == "R")   //retrigger delay
                        {
                            ControllerSpecs.ChanConfig[IDchan].OperatingRetrigger = MixedValue("", replySplitted[++len], "T");  
                            continue;
                        }
                        if (s == "TT")   //internal/external trigger
                        {
                            ControllerSpecs.InternalTrigger = (System.Convert.ToInt32(replySplitted[++len]) == 1) ? true : false;
                            continue;
                        }
                        if (s == "TP")   //internal trigger period
                        {
                            ControllerSpecs.InternalTriggerMs = MixedValue("", replySplitted[++len], "T");
                            continue;
                        }
                        if (s == "FP")   //external trigger type
                        {
                            ControllerSpecs.ExternalTriggerType = System.Convert.ToInt32(replySplitted[++len]);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Line(LogLevels.Error, ClassName + ".ParseReply", "Errore: " + ex.ToString());
                    }
                }
            }
        }

        private double MixedValue(string prefisso, string s, string TimeAmpereEMU)
        {
            try
            {
                if (prefisso.Length>0)
                    s = s.Replace(prefisso, "");
                double multiplier = 1.0;
                if (TimeAmpereEMU == "T")
                {
                    if (s.Contains("us")) multiplier = 0.001;                   //microsecondi
                    else if (s.Contains("ms")) multiplier = 1.0;                //millisecondi
                    else multiplier = 1000.0;                                   //secondi
                    s = s.Replace("us", "");
                    s = s.Replace("ms", "");
                    s = s.Replace("s", "");
                }
                else if (TimeAmpereEMU == "A")
                {
                    if (s.Contains("mA")) multiplier = 0.001;                   //milliampere
                    else multiplier = 1.0;                                      //ampere
                    s = s.Replace("mA", "");
                    s = s.Replace("A", "");
                }
                return Utilities.FromGBString(s) * multiplier;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".MixedValue", "Errore: " + ex.ToString());
                return 0.0;
            }
        }
        #endregion

        private void SendThread()
        {
            string sReport = "";
            while (true)
            {
                int ret = WaitHandle.WaitAny(waitEvents, ControllerSpecs.SendTimeoutSec * 1000);
                if (ret==0)    //exit
                    break;
                if ((ControllerSpecs.ComMode == ComMode.Ethernet) && (stream == null)) continue;
                //TODO  if ((ComMode == ComMode.RS232) && ...) continue;
                //lock (SendLocker)
                try
                {
                    if (ret == WaitHandle.WaitTimeout)  //timeout
                    {
                        if (ControllerSpecs.ComMode == ComMode.RS232)
                        {
                            //TODO
                        }
                        if (ControllerSpecs.ComMode == ComMode.Ethernet)
                        {
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(ControllerSpecs.HeartBeatString);
                            if (stream.CanWrite)
                                stream.Write(data, 0, data.Length);
                            Log.Line(LogLevels.Debug, ClassName + ".SendThread", "INVIATO MESSAGGIO: " + ControllerSpecs.HeartBeatString);
                        }
                    }
                    else
                    {
                        if (ControllerSpecs.ComMode == ComMode.RS232)
                        {
                            //TODO: WRITE!
                        }
                        if (ControllerSpecs.ComMode == ComMode.Ethernet)
                        {
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(sendWord);
                            if (stream.CanWrite)
                                stream.Write(data, 0, data.Length);
                            Log.Line(LogLevels.Debug, ClassName + ".SendThread", "INVIATO MESSAGGIO: " + sendWord);
                        }
                        sReport = sendWord;
                        sendWord = "";
                    }
                }
                catch (Exception ex)
                {
                    Log.Line(LogLevels.Error, ClassName + ".SendThread", "ERRORE: " + ex.ToString());
                }

                //if (send) Log.Line(LogLevels.Pass, ClassName + ".SendThread", "Inviato messaggio: " + sReport);
            }
        }

        private void ReceiveThread()
        {
            Byte[] dataRec = new Byte[7000];
            string responseData = string.Empty;
            while (stream != null && myClient != null && myClient.Connected)
            {
                if (!stream.DataAvailable)
                {
                    System.Threading.Thread.Sleep(20);
                    continue;
                }
                Int32 bytes = stream.Read(dataRec, 0, dataRec.Length);
                responseData = "";
                responseData = System.Text.Encoding.ASCII.GetString(dataRec, 0, bytes);
                if (!responseData.Contains(ControllerSpecs.ReplyTerminator))
                {
                    Log.Line(LogLevels.Debug, ClassName + ".ReceiveThread", "MESSAGGIO INCOMPLETO");
                    ReadyForNewCmdEv.Set();
                    if (OnPostMessageReceived != null) OnPostMessageReceived(this, new MessageReceivedEventArgs(false, responseData));
                    continue;
                }
                Log.Line(LogLevels.Debug, ClassName + ".ReceiveThread", "RICEVUTO MESSAGGIO: " + responseData);
                ParseReply(responseData);
                ReadyForNewCmdEv.Set();
                if (OnPostMessageReceived != null) OnPostMessageReceived(this, new MessageReceivedEventArgs(true, responseData));
                /*if (responseData.Contains(strLifeServer))
                {
                    while (responseData.Contains(strLifeServer))
                        responseData = responseData.Remove(responseData.LastIndexOf(strLifeServer), strLifeServer.Length);
                }
                string[] words = responseData.Split(respDataSepNewLine, StringSplitOptions.RemoveEmptyEntries);
                //RespList.AddRange(bars);
                string strArmReply = "";
                for (int i = 0; i < words.Length; i++)
                {
                    if (!words[i].Contains("BAR"))
                    {
                        strArmReply += (words[i] + "\r\n");
                        continue;
                    }
                }*/
            }
            Log.Line(LogLevels.Warning, ClassName + ".ReceiveThread", "Controller disconnesso.");
            if (OnPostDisconnection != null) OnPostDisconnection(this, new ConnectedEventArgs(false, ControllerSpecs.IP, -1));
        }

        #region XML
        public string XMLFilePath { get; set; }
       
        public int SaveToXml()
        {
            return SaveToXml(XMLFilePath);
        }

        public int SaveToXml(string filePath)
        {
            XMLFilePath = filePath;
            for (int ch=0; ch<ControllerSpecs.ChanConfig.Count; ch++)
            {
                ControllerSpecs.CheckOperatingCurrent(ch);
            }
            if (SPAMI.Util.XML.XmlDumper.XmlConfigSerialize(XMLFilePath, ControllerSpecs))   //scrittura su xml
                return 0;
            return -1;
        }

        public int LoadFromXml()
        {
            if (!System.IO.File.Exists(XMLFilePath))
            {
                Log.Line(LogLevels.Error, ClassName + ".LoadFromXML", "Errore! Non esiste file \"" + XMLFilePath + "\"");
                return -1;
            }
            return LoadFromXml(XMLFilePath);
        }
        
        public int LoadFromXml(string filePath)
        {
            int ret = 0;
            XMLFilePath = filePath;
            switch (Name)
            {
                case ControllerName.RT820F_2:
                    ControllerSpecs = (RT820F_2)XmlDumper.XmlConfigDeserialize(XMLFilePath, typeof(RT820F_2));
                    if (ControllerSpecs == null)
                    {
                        Log.Line(LogLevels.Error, ClassName + ".LoadFromXml", "Impossibile leggere da file XML! Path: " + XMLFilePath);
                        ControllerSpecs = new RT820F_2();
                        ControllerSpecs.Reset();
                        ret = SaveToXml(System.IO.Path.GetDirectoryName(XMLFilePath) + @"\" + Name + XMLExt);
                        ControllerSpecs = null;
                    }
                    break;
                case ControllerName.RT820F_20:
                    ControllerSpecs = (RT820F_20)XmlDumper.XmlConfigDeserialize(XMLFilePath, typeof(RT820F_20));
                    if (ControllerSpecs == null)
                    {
                        Log.Line(LogLevels.Error, ClassName + ".LoadFromXml", "Impossibile leggere da file XML! Path: " + XMLFilePath);
                        ControllerSpecs = new RT820F_20();
                        ControllerSpecs.Reset();
                        ret = SaveToXml(System.IO.Path.GetDirectoryName(XMLFilePath) + @"\" + Name + XMLExt);
                        ControllerSpecs = null;
                    }
                    break;
                case ControllerName.PP820:
                    ControllerSpecs = (PP820)XmlDumper.XmlConfigDeserialize(XMLFilePath, typeof(PP820));
                    if (ControllerSpecs == null)
                    {
                        Log.Line(LogLevels.Error, ClassName + ".LoadFromXml", "Impossibile leggere da file XML! Path: " + XMLFilePath);
                        ControllerSpecs = new PP820();
                        ControllerSpecs.Reset();
                        ret = SaveToXml(System.IO.Path.GetDirectoryName(XMLFilePath) + @"\" + Name + XMLExt);
                        ControllerSpecs = null;
                    }
                    break;
                default:
                    Log.Line(LogLevels.Error, ClassName + ".LoadFromXml", "LightController non contemplato");
                    ControllerSpecs = null;
                    return -2;
            }
            for (int ch = 0; ch < ControllerSpecs.ChanConfig.Count; ch++)
            {
                ControllerSpecs.CheckOperatingCurrent(ch);
            }
            return ret;
        }
        #endregion
    }

    #region Events Definition
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(bool _complete, string _message)
        {
            complete = _complete;
            message = _message;
        }
        public bool complete;
        public string message;
    }

    public class ConnectedEventArgs : EventArgs
    {
        public ConnectedEventArgs(bool _connected, string _ip, int _port)
        {
            connected = _connected;
            ip = _ip;
            port = _port;
        }
        public bool connected;
        public string ip;
        public int port;
    }

    public class ChannelEventArgs : EventArgs 
    {
        public int chanID;
        public ChannelEventArgs(int _chanID)
        {
            chanID = _chanID;
        }
    }
    #endregion

    


  
}
