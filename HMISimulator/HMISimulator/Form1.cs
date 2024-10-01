using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.Diagnostics;

namespace HMISimulator {
    public partial class Form1 : Form {

        WebServiceHost sHost;
        HMIService sv = new HMIService();
        Stopwatch timeDebugger = new Stopwatch();

        class valueItem {
            public string Value { get; set; }
            public string Description { get; set; }
        }

        public Form1() {
            InitializeComponent();

            cmbSetUserLevel.Items.Add(new valueItem() { Value = "0", Description = "Unknown" });
            cmbSetUserLevel.Items.Add(new valueItem() { Value = "1", Description = "Operator" });
            cmbSetUserLevel.Items.Add(new valueItem() { Value = "5", Description = "Supervisor" });
            cmbSetUserLevel.Items.Add(new valueItem() { Value = "7", Description = "Engineer" });            
            cmbSetUserLevel.Items.Add(new valueItem() { Value = "9", Description = "Administrator" });
            cmbSetUserLevel.Items.Add(new valueItem() { Value = "10", Description = "Optrel" });
            cmbSetUserLevel.ValueMember = "Value";
            cmbSetUserLevel.DisplayMember = "Description";

            cmbSetUserLevel.SelectedIndex = 3;  //pier: set livello utente default

            cmbMachineMode.ValueMember = "Value";
            cmbMachineMode.DisplayMember = "Description";
            cmbMachineMode.Items.Add(new valueItem() { Value = "0", Description = "Unknown" });
            cmbMachineMode.Items.Add(new valueItem() { Value = "1", Description = "Stop" });
            cmbMachineMode.Items.Add(new valueItem() { Value = "2", Description = "Running" });
            cmbMachineMode.Items.Add(new valueItem() { Value = "3", Description = "Error" });
            cmbMachineMode.SelectedIndex = 1;

            cmbSupervisorMode.ValueMember = "Value";
            cmbSupervisorMode.DisplayMember = "Description";
            cmbSupervisorMode.Items.Add(new valueItem() { Value = "0", Description = "Unknown" });
            cmbSupervisorMode.Items.Add(new valueItem() { Value = "1", Description = "ReadyToRun" });
            cmbSupervisorMode.Items.Add(new valueItem() { Value = "2", Description = "Editing" });
            cmbSupervisorMode.Items.Add(new valueItem() { Value = "3", Description = "Error" });
            cmbSupervisorMode.Items.Add(new valueItem() { Value = "4", Description = "Busy" });            
            cmbSupervisorMode.SelectedIndex = 0;

            sv.SetSupIsAliveInvoked += new EventHandler(sv_SetSupIsAliveInvoked);
            sv.SetSupervisorModeInvoked += new EventHandler(sv_SetSupervisorModeInvoked);
            //startHMIService();
        }

        void sv_SetSupervisorModeInvoked(object sender, EventArgs e) {

            string mode = "Unknow";
            switch (sv.SupervisorMode) {
                case "1":
                    mode = "ReadyToRun";
                    break;
                case "2":
                    mode="Editing";
                    break;
                case "3":
                    mode = "Error";
                    break;
                case "4":
                    mode = "Busy";
                    break;
            }
            this.Text = "Supervisor mode: " + mode;
        }

        void sv_SetSupIsAliveInvoked(object sender, EventArgs e) {

            string[] data = txtPos.Text.Split(new char[] { ';' });   
            timeDebugger.Restart();
            sv.SetSupervisorMachineInfo(txtEnables.Text, "100");
            Debug.WriteLine("SetSupervisorMachineInfo - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
            timeDebugger.Restart();
            sv.SetSupervisorPos(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]));
            Debug.WriteLine("SetSupervisorPos - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
            timeDebugger.Restart();
            sv.SetLanguage(txtLanguage.Text);
            Debug.WriteLine("SetLanguage - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
            timeDebugger.Restart(); 
            sv.SetUserLevel(((valueItem)cmbSetUserLevel.SelectedItem).Value.ToString());
            Debug.WriteLine("SetUserLevel - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
            timeDebugger.Restart();
            sv.SetActiveRecipe(loadRecipe(txtRecipe.Text));
            Debug.WriteLine("SetActiveRecipe - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
            timeDebugger.Restart();
            sv.SetMachineMode(((valueItem)cmbMachineMode.SelectedItem).Value.ToString());
            Debug.WriteLine("SetMachineMode - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
        }

        void startHMIService() {
            //Uri httpUrl = new Uri("http://192.168.1.101:8091");
            Uri httpUrl = new Uri("http://localhost:6543");
            //sv.RemoteServiceUrl = @"http://192.168.13.158:6543/ExactaEasyUI/Supervisor";
            //sv.RemoteServiceUrl = @"http://localhost:8090/ExactaEasyUI/Supervisor";
            //sv.RemoteServiceUrl = @"http://192.168.69.69:8090/ExactaEasyUI/Supervisor";   //BERNO
            //sv.RemoteServiceUrl = @"http://localhost:8090/ExactaEasyUI/Supervisor"; //RUDY
            sv.RemoteServiceUrl = @"http://localhost:8090/ExactaEasyUI/Supervisor";   //BERNO
            sHost = new WebServiceHost(sv, httpUrl);
            
            WebHttpBinding wbind = new WebHttpBinding();
            wbind.MaxReceivedMessageSize = 1048576;
            wbind.ReaderQuotas.MaxStringContentLength = 1048576;
            wbind.ReaderQuotas.MaxBytesPerRead = 1048576;
            //sHost.AddServiceEndpoint(typeof(ExactaEasyEng.ISupervisor), wbind, "");


            sHost.AddServiceEndpoint(typeof(IHMIService), wbind, "");
            sHost.Description.Endpoints[0].Behaviors.Add(new WebHttpBehavior { HelpEnabled = true });
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            sHost.Description.Behaviors.Add(smb);
            sHost.Open();

        }

        private void button1_Click(object sender, EventArgs e) {

            startHMIService();
            button1.Enabled = false;
        }

        private void btnSetSupervisorPos_Click(object sender, EventArgs e) {

            string[] data = txtPos.Text.Split(new char[] { ';' });
            timeDebugger.Restart();
            sv.SetSupervisorPos(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]));
            Debug.WriteLine("SetSupervisorPos - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
        }

        private void btnSetLanguage_Click(object sender, EventArgs e) {

            timeDebugger.Restart();
            sv.SetLanguage(txtLanguage.Text);
            Debug.WriteLine("SetLanguage - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
        }

        private void btnSetUserLevel_Click(object sender, EventArgs e) {

            timeDebugger.Restart();
            sv.SetUserLevel(((valueItem)cmbSetUserLevel.Items[cmbSetUserLevel.SelectedIndex]).Value.ToString());
            Debug.WriteLine("SetUserLevel - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
        }

        private void btnActiveRecipe_Click(object sender, EventArgs e) {

            timeDebugger.Restart();
            string ricetta = loadRecipe(txtRecipe.Text);
            if (errDanneggiaRicetta.Checked)
                ricetta = ricetta.Substring(100);
            sv.SetActiveRecipe(ricetta);
            //sv.SaveRecipeModfication(loadRecipe(txtRecipe.Text));
            Debug.WriteLine("SetActiveRecipe - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
        }

        private void btnMachineMode_Click(object sender, EventArgs e) {

            timeDebugger.Restart();
            sv.SetMachineMode(((valueItem)cmbMachineMode.SelectedItem).Value.ToString());
            Debug.WriteLine("SetMachineMode - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
            this.Text = "Supervisor mode: " + ((valueItem)cmbMachineMode.SelectedItem).Description;
        }

        string loadRecipe(string filePath) {

            if ((filePath != null) && (filePath != "")) {
                StreamReader file = new StreamReader(Application.StartupPath + @"\" + filePath);
                string ris = file.ReadToEnd();
                file.Close();
                return ris;
            }
            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timeDebugger.Restart();
            sv.SetSupervisorMachineInfo(txtEnables.Text, "100");
            Debug.WriteLine("SetSupervisorMachineInfo - Time elapsed = " + timeDebugger.ElapsedMilliseconds);
        }

        private void button3_Click(object sender, EventArgs e) {

            sv.SetSupervisorOnTop();
        }

        private void button4_Click(object sender, EventArgs e) {

            sv.SetSupervisorHide();
        }

        private void errSimulaErroreRicezione_CheckedChanged(object sender, EventArgs e) {

            sv.SimulaErroreRicezione = errSimulaErroreRicezione.Checked;
        }

        private void errDanneggiaRicetta_CheckedChanged(object sender, EventArgs e) {


        }

    }
}
