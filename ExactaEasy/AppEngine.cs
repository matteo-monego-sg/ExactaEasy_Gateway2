using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ExactaEasyEng;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace ExactaEasy {

    public enum SupervisorServiceStateEnum {

        NotCreated = -1,
        Created = CommunicationState.Created,
        Opening = CommunicationState.Opening,
        Opened = CommunicationState.Opened,
        Closing = CommunicationState.Closing,
        Closed = CommunicationState.Closed,
        Faulted = CommunicationState.Faulted
    }

    public enum HMIServiceStateEnum {

        NotConnected = 0,
        Connecting,
        Connected
    }

    public class AppEngine {

        WebServiceHost sHost;
        Supervisor supervisor;

        public static AppEngine Current { get; internal set; }

        static AppEngine() {

            if (Current == null)
                Current = new AppEngine();
        }

        public AppContext Context { get; internal set; }
        public Supervisor ActiveSupervisor { get; internal set; }

        AppEngine() {

            ActiveSupervisor = new Supervisor();          
        }

        public void StartSupervisorService() {
            
            Uri httpUrl = new Uri("http://localhost:8090/ExactaEasyUI/Supervisor");
            sHost = new WebServiceHost(ActiveSupervisor, httpUrl);
            sHost.Opened += new EventHandler(sHost_Opened);
            sHost.AddServiceEndpoint(typeof(ExactaEasyEng.ISupervisor), new WebHttpBinding(), "");
            sHost.Description.Endpoints[0].Behaviors.Add(new WebHttpBehavior { HelpEnabled = true });
#if DEBUG
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            sHost.Description.Behaviors.Add(smb);
#endif
            sHost.Open();                        
        }

        public SupervisorServiceStateEnum GetServiceStatus() {

            if (sHost == null)
                return SupervisorServiceStateEnum.NotCreated;
            else
                return (SupervisorServiceStateEnum)sHost.State;
        }



        void sHost_Opened(object sender, EventArgs e) {

            startHMIComm();
        }

        void startHMIComm() {

            HMI hmi = new HMI("");
            hmi.SetSupIsAlive();
        }


    }
}
