using ExactaEasyEng;
using ExactaEasyEng.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExactaEasy
{

    delegate void SetEnabledDelegate(bool value);
    public delegate void ButtonPressedEvenyHandler(object sender, ButtonPressedEventArgs e);

    public partial class VertMenuBar : UserControl {
        public event ButtonPressedEvenyHandler ButtonPressed;
        public int index = 0;
        Button lastButtonPressed;

        public int IndexBase { get; set; }

        public int Index {
            get {
                return index;
            }
            set {
                index = value;
                //08/11/2016 parto da 1 invece che da 0
                lblIndex.Text = index >= 0 ? (index + IndexBase + 1).ToString() : "";
            }
        }

        int homepage;
        public int CurrentHomeScreen {
            get {
                return homepage;
            }
            set {
                homepage = value;
                updateHomeScreen(homepage);
            }
        }

        void updateHomeScreen(int id) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(lblScreenIndex))
                return;

            if (lblScreenIndex.InvokeRequired && lblScreenIndex.IsHandleCreated)
                lblScreenIndex.Invoke(new MethodInvoker(() => updateHomeScreen(id)));
            else {
                lblScreenIndex.Text = id.ToString() + "/" + TotalScreens.ToString();
            }
        }

        public int TotalScreens { get; set; }

        public VertMenuBar() {
            InitializeComponent();

            btnHome.Click += button_Click;
            btnDown.Click += button_Click;
            btnUp.Click += button_Click;
            btnEdit.Click += button_Click;
            btnUtilities.Click += button_Click;
            btnReport.Click += button_Click;
            btnConfig.Click += button_Click;
            btnDump.Click += button_Click;
            btnFilter.Click += button_Click;
            btnHome.BackColor = Color.FromArgb(28, 28, 28);
            lastButtonPressed = btnHome;
            Index = 0;

            btnHome.Text = frmBase.UIStrings.GetString("Home");
            btnEdit.Text = frmBase.UIStrings.GetString("Tools");
            btnUtilities.Text = frmBase.UIStrings.GetString("Utilities");
            btnDump.Text = frmBase.UIStrings.GetString("Dump");
            btnReport.Text = frmBase.UIStrings.GetString("Print");
            btnConfig.Text = frmBase.UIStrings.GetString("Nodes");
            btnFilter.Text = frmBase.UIStrings.GetString("Filter");

            if(AppEngine.Current.MachineConfiguration != null) //check MachineConfiguration if is not null to avoid the error in design mode
                if (AppEngine.Current.MachineConfiguration.IsLiveImageFilter == false)
                    btnFilter.Visible = false;
        }

        void button_Click(object sender, EventArgs e) {

            OnButtonPressed(this, new ButtonPressedEventArgs(((Button)sender).Tag.ToString()));
            setButtonPressed(sender as Button);
        }

        private void setButtonPressed(Button btnPressed) {

            foreach (Control ctrl in Controls) {
                if (ctrl is Button)
                    (ctrl as Button).BackColor = Color.FromArgb(50, 50, 50);//.FlatAppearance.BorderSize = 0;
            }
            if (btnPressed.Name != "btnSaveToHMI") {
                if (btnPressed.Name == "btnDown" || btnPressed.Name == "btnUp") {
                    lastButtonPressed = btnEdit;
                    btnEdit.BackColor = Color.FromArgb(28, 28, 28); 
                }
                else {
                    lastButtonPressed = btnPressed;
                    btnPressed.BackColor = Color.FromArgb(28, 28, 28); //.FlatAppearance.BorderSize = 1;
                }
            }
            else {
                lastButtonPressed.BackColor = Color.FromArgb(28, 28, 28);
            }
        }

        protected void OnButtonPressed(object sender, ButtonPressedEventArgs e) {

            if (ButtonPressed != null)
                ButtonPressed(sender, e);
        }

        public new bool Enabled {
            get { return base.Enabled; }
            set { setEnabled(value); }
        }

        void setEnabled(bool value) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new SetEnabledDelegate(setEnabled), new object[] { value });
            else {
                base.Enabled = value;
            }
        }

        public void SetNavigationAvailable(bool value) {

            setButtonEnabled(btnDown, value);
            setButtonEnabled(btnUp, value);
        }

        public void SetSettingsEnabled(bool value) {

            setButtonEnabled(btnEdit, value);
        }

        public void SetAdminPanelEnabled(bool value) {

            setButtonEnabled(btnUtilities, value);
        }

        public void SetReportPanelEnabled(bool value) {

            setButtonEnabled(btnReport, value);
        }

        public void SetConfigPanelEnabled(bool value) {

            setButtonEnabled(btnConfig, value);
        }

        public void SetAdminPanelVisible(bool value) {

            setButtonVisible(btnUtilities, value);
        }

        public void SetDumpVisible(bool value)
        {
            setButtonVisible(btnDump, value);
        }

        public void SetReportPanelVisible(bool value)
        {
            setButtonVisible(btnReport, value);
        }

        public void SetDumpEnabled(bool value)
        {
            setButtonEnabled(btnDump, value);
        }

        void setButtonEnabled(Button button, bool value) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new Action(() => { setButtonEnabled(button, value); }));
            else
                button.Enabled = value;
        }

        void setButtonVisible(Button button, bool value) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new Action(() => { setButtonVisible(button, value); }));
            else
                button.Visible = value;
        }

        private void btnReport_Click(object sender, EventArgs e) {

        }

        /*private void btnImgViewer_Click(object sender, EventArgs e) {

            Log.Line(LogLevels.Debug, "VertMenuBar.btnImgViewer_Click", "CIAO DEBUG");
            Log.Line(LogLevels.Pass, "VertMenuBar.btnImgViewer_Click", "CIAO PASS");
            Log.Line(LogLevels.Warning, "VertMenuBar.btnImgViewer_Click", "CIAO WARNING");
            Log.Line(LogLevels.Error, "VertMenuBar.btnImgViewer_Click", "CIAO ERROR");
        }*/
    }

    public class ButtonPressedEventArgs {

        public string ButtonPressedName { get; internal set; }

        public ButtonPressedEventArgs(string buttonPressedName) {

            ButtonPressedName = buttonPressedName;
        }
    }
}
