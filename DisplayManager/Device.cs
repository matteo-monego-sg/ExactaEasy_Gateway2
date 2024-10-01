using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace DisplayManager
{

    public enum DeviceCommunicationStates {

        Disconnected = 0,
        Connecting,
        Connected,
        Faulted
    }

    public enum DeviceWorkingStates {

        Normal = 0,
        Warning,
        Error
    }

    public abstract class Device {

        /*
         * Da spostare a livello di Vision System manager ---------------------
        */

        static int networkInterfaceCount = 0;
        static Dictionary<Guid, NetworkInterfaceState> networkInterfaceState = new Dictionary<Guid, NetworkInterfaceState>();

        static event EventHandler CheckConnection;

        static Device() {

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            networkInterfaceCount = adapters.Length;
            foreach (NetworkInterface ni in adapters) {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    networkInterfaceState.Add(Guid.Parse(ni.Id), new NetworkInterfaceState() { OperationalStatus = ni.OperationalStatus, InterfaceType = ni.NetworkInterfaceType });
            }
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private static List<NetworkInterface> getChangedNetworkInterface() {

            List<NetworkInterface> changedNI = new List<NetworkInterface>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            if (adapters.Length != networkInterfaceCount) {
                // TODO: Gestire l'aggiunta o la rimozione di ni
            }
            foreach (NetworkInterface ni in adapters) {
                Guid guid = Guid.Parse(ni.Id);
                if (networkInterfaceState.ContainsKey(guid)) {
                    NetworkInterfaceState ns = networkInterfaceState[guid];
                    if (ns.OperationalStatus != ni.OperationalStatus) {
                        changedNI.Add(ni);
                        ns.OperationalStatus = ni.OperationalStatus;
                    }
                }
            }
            return changedNI;
        }

        static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e) {

            return; //pier: ritorno perchè in qualche caso è crashato e non ho tempo di vedere....
            //if (getChangedNetworkInterface().Count > 0) {
            //    // TODO: innescare controllo device
            //    OnCheckConnection(null, e);
            //}
        }

        static void OnCheckConnection(object sender, EventArgs e) {

            if (CheckConnection != null)
                CheckConnection(null, new EventArgs());
        }

        /*
         * ------------------------------^^^^^-------------------------------------------
         */


        public event EventHandler<CommunicationStateChangedEventArgs> CommunicationStateChanged;
        public event EventHandler<WorkingStateChangedEventArgs> WorkingStateChanged;

        AutoResetEvent stopWaitRetryConnEvent = new AutoResetEvent(false);
        object startConnLockObj = new object();

        DeviceCommunicationStates _communicationStatus;
        public DeviceCommunicationStates CommunicationStatus {
            get {
                return _communicationStatus;
            }
            protected set {
                if (_communicationStatus != value) {
                    DeviceCommunicationStates _prevCommunicationState = _communicationStatus;
                    _communicationStatus = value;
                    OnCommunicationStateChanged(this, new CommunicationStateChangedEventArgs(_communicationStatus, _prevCommunicationState));
                }
            }
        }

        public int RetryConnectInterval { get; set; }
        public bool ConnectingInProgress { get; private set; }

        DeviceWorkingStates _workingState;
        public DeviceWorkingStates WorkingState {
            get {
                return _workingState;
            }
            protected set {
                if (_workingState != value) {
                    OnWorkingStateChanged(this, new WorkingStateChangedEventArgs(_workingState, value));
                    _workingState = value;
                }
            }
        }

        public Device() {

            RetryConnectInterval = 1000;
            Device.CheckConnection += Device_CheckConnection;
        }

        void Device_CheckConnection(object sender, EventArgs e) {

            if (CommunicationStatus != DeviceCommunicationStates.Disconnected)
                CheckDeviceCommunicationState();
        }

        /// <summary>
        /// Richiesta di connessione asincrona al device.
        /// </summary>
        /// <param name="retryConnnect">Se true, si ritenta la connessione al device nel caso in cui lo stato connessione sia 'Faulted'
        /// Se false, non si ritenta la connessione solo una volta</param>
        /// <param name="retryConnectInterval">Intervallo in millisecondi tra i tentativi di riconnesisone</param>
        public void StartConnect(bool retryConnect) {

            Task connTask = new Task(new Action(() => {
                bool isAcquired = false;
                try {                    
                    Monitor.TryEnter(startConnLockObj, ref isAcquired);
                    if (isAcquired) {
                        ConnectingInProgress = true;
                        do {
                            ExecDeviceConnection();
                            if (CommunicationStatus == DeviceCommunicationStates.Faulted && retryConnect)
                                if (stopWaitRetryConnEvent.WaitOne(RetryConnectInterval))
                                    break;
                        }
                        while (CommunicationStatus == DeviceCommunicationStates.Faulted && retryConnect);
                    }
                }
                finally {
                    if (isAcquired) {
                        Monitor.Exit(startConnLockObj);
                        ConnectingInProgress = false;
                    }
                }
            }));
            connTask.Start();
        }

        public void Connect() {

            ExecDeviceConnection();
        }

        public void Disconnect() {
        }

        /// <summary>
        /// Eseguire l'ovverride di questo metodo per gestire la connessione al device specifica
        /// </summary>
        protected abstract void ExecDeviceConnection();

        /// <summary>
        /// Eseguire l'override di questo metodo per gestire la verifica dello stato di connessione
        /// </summary>
        public abstract void CheckDeviceCommunicationState();

        protected virtual void OnCommunicationStateChanged(object sender, CommunicationStateChangedEventArgs e) {

            if (CommunicationStateChanged != null)
                CommunicationStateChanged(sender, e);
        }

        protected virtual void OnWorkingStateChanged(object sender, WorkingStateChangedEventArgs e) {

            if (WorkingStateChanged != null)
                WorkingStateChanged(sender, e);
        }

    }

    public class CommunicationStateChangedEventArgs : EventArgs {

        public DeviceCommunicationStates CurrentState { get; private set; }
        public DeviceCommunicationStates PreviousState { get; private set; }

        public CommunicationStateChangedEventArgs(DeviceCommunicationStates currentState, DeviceCommunicationStates previousState) {

            CurrentState = currentState;
            PreviousState = previousState;
        }
    }

    public class WorkingStateChangedEventArgs : EventArgs {

        public DeviceWorkingStates CurrentState { get; private set; }
        public DeviceWorkingStates PreviousState { get; private set; }

        public WorkingStateChangedEventArgs(DeviceWorkingStates currentState, DeviceWorkingStates previousState) {

            CurrentState = currentState;
            PreviousState = previousState;
        }
    }

}

public class NetworkInterfaceState {

    public OperationalStatus OperationalStatus { get; set; }
    public NetworkInterfaceType InterfaceType { get; set; }

}



