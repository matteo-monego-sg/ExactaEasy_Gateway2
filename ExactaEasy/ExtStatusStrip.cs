using ExactaEasyEng;
using ExactaEasyEng.Utilities;
using System.Windows.Forms;


namespace ExactaEasy
{

    public enum ExStatuStripIconEnum {

        None = -1,
        Unknown = 0,
        Waiting = 1,
        OK = 2,
        Warning = 3,
        Error = 4
    }

    public partial class ExtStatusStrip : UserControl {

        delegate void SetStatusMessageDelegate(ToolStripLabel ctrl, string message);
        delegate void SetStatusImageDelegate(ToolStripLabel ctrl, ExStatuStripIconEnum status);

        //ToolStripLabel lblAppStatus = new ToolStripLabel();
        //ToolStripLabel lblTattileStatus = new ToolStripLabel();
        //ToolStripLabel lblWebSvcStatus = new ToolStripLabel();
        //ToolStripLabel lblHMIStatus = new ToolStripLabel();
        ExStatuStripIconEnum cameraCurrentStatus = ExStatuStripIconEnum.Unknown;
        public static ExStatuStripIconEnum CameraLastStatus { get; private set; }
        //ExStatuStripIconEnum hmiCurrentStatus = ExStatuStripIconEnum.Unknown;

        public string StatusMessage {
            get {
                return lblAppStatus.Text;
            }
            set {
                SetStatusMessage(value);
            }
        }

        public string UserLevel {

            get {
                return lblCameraInfo.Text;
            }
            set {
                SetCameraInfo(value);
            }
        }

        public string MachineStatus {
            get {
                return lblMachineStatus.Text;
            }
            set {
                SetMachineStatus(value);
            }
        }

        public ExtStatusStrip() {

            InitializeComponent();
            builComponent();
        }

        void builComponent() {

            //lblHMIStatus.Dock = DockStyle.Right;
            //lblWebSvcStatus.Dock = DockStyle.Right;
            //lblTattileStatus.Dock = DockStyle.Right;
            //lblAppStatus.Dock = DockStyle.Fill;
            //ToolStripStatusLabel aa = new ToolStripStatusLabel();
            //PictureBox ab = new PictureBox();
            //aa.Image = ab.Image;
            //ab.Image = ledImageList.Images[1];


            //intStatusStrip.Items.AddRange(
            //    new ToolStripItem[] {                                         
            //        lblTattileStatus,
            //        new ToolStripSeparator(),
            //        lblWebSvcStatus,
            //        new ToolStripSeparator(),
            //        lblHMIStatus,
            //        new ToolStripSeparator(),
            //        lblAppStatus, aa });
        }

        public void SetCameraInfo(string cameraInfo) {

            setLabelText(lblCameraInfo, cameraInfo);
        }

        public void SetMachineStatus(string machineStatus) {

            setLabelText(lblMachineStatus, machineStatus);
        }

        public void SetStatusMessage(string statusMessage) {

            setLabelText(lblAppStatus, statusMessage);
        }

        public void SetRecipe(Recipe rec, int version)
        {
            if(rec == null)
            {
                //setLabelText(lblActiveRecipeName, "---");
                //setLabelText(lblActiveRecipeVersion, "v. -");
                lblActiveRecipeName.Visible = false;
                lblActiveRecipeVersion.Visible = false;
                lblAppStatus.BorderSides = ToolStripStatusLabelBorderSides.None;
            }
            else
            {
                lblActiveRecipeName.Visible = true;
                lblActiveRecipeVersion.Visible = true;
                lblAppStatus.BorderSides = ToolStripStatusLabelBorderSides.Left;
                setLabelText(lblActiveRecipeName, $"    {rec.RecipeName}    "); //put the spaces to have padding (the length of the control is not fixed)
                string strVersion = version <= -1 ? "-" : version.ToString();
                setLabelText(lblActiveRecipeVersion, $"    v. {strVersion}    "); //put the spaces to have padding (the length of the control is not fixed)
            }
        }

        public bool IsTattileOk() {

            return cameraCurrentStatus == ExStatuStripIconEnum.OK;
        }

        void setLabelText(ToolStripLabel ctrl, string message) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new SetStatusMessageDelegate(setLabelText), new object[] { ctrl, message });
            else {
                ctrl.Text = message;
            }
        }

        void setLabelImage(ToolStripLabel ctrl, ExStatuStripIconEnum status) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new SetStatusImageDelegate(setLabelImage), new object[] { ctrl, status });
            else {
                if (status == ExStatuStripIconEnum.None)
                    ctrl.Image = null;
                else if (status == ExStatuStripIconEnum.Waiting) {
                    ctrl.Image = ExactaEasy.Properties.Resources.blink_white;
                }
                else {
                    if (ledImageList.Images.Count > 0)
                        ctrl.Image = ledImageList.Images[(int)status];
                }
            }
        }

        ExStatuStripIconEnum convertToStripEnum(SupervisorServiceStateEnum state) {

            ExStatuStripIconEnum ris = ExStatuStripIconEnum.Unknown;
            switch (state) {
                case SupervisorServiceStateEnum.Created:
                case SupervisorServiceStateEnum.NotCreated:
                    ris = ExStatuStripIconEnum.Unknown;
                    break;
                case SupervisorServiceStateEnum.Opening:
                case SupervisorServiceStateEnum.Closing:
                    ris = ExStatuStripIconEnum.Waiting;
                    break;
                case SupervisorServiceStateEnum.Opened:
                    ris = ExStatuStripIconEnum.OK;
                    break;
                case SupervisorServiceStateEnum.Closed:
                case SupervisorServiceStateEnum.Faulted:
                    ris = ExStatuStripIconEnum.Error;
                    break;
            }
            return ris;
        }

        ExStatuStripIconEnum convertToStripEnum(GatewayCommunicationStatusEnum state) {

            ExStatuStripIconEnum ris = ExStatuStripIconEnum.Unknown;
            switch (state) {
                case GatewayCommunicationStatusEnum.NotCreated:
                    ris = ExStatuStripIconEnum.Unknown;
                    break;
                case GatewayCommunicationStatusEnum.Connecting:
                    ris = ExStatuStripIconEnum.Waiting;
                    break;
                case GatewayCommunicationStatusEnum.Connected:
                    ris = ExStatuStripIconEnum.OK;
                    break;
                case GatewayCommunicationStatusEnum.NotConnected:
                    ris = ExStatuStripIconEnum.Error;
                    break;
            }
            return ris;
        }
    }
}

