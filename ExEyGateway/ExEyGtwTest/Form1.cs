using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ServiceModel;
using ExactaEasyEng;
using ExEyGateway;
using System.Threading;
using System.Diagnostics;

namespace ExEyGtwTest {
    public partial class Form1 : Form {

        ExEyGatewayCtrl gtw = null;
        bool exit = false;
        Thread statusTh;
        Recipe currRecipe = null;

        public Form1() {
            InitializeComponent();

            //ChannelFactory<ISupervisor> factory = new ChannelFactory<ISupervisor>(new NetTcpBinding( SecurityMode.None ), new EndpointAddress(@"net.tcp://localhost:8090/ExactaEasyUI/Supervisor"));
            //ISupervisor proxy = factory.CreateChannel();
            //proxy.SetSupervisorPos(0, 0, 1000, 600);

            //string filePath = System.IO.Path.GetFullPath(@"C:\Dynamics\DotNet Component\ExactaEasy\globalConfig.xml");
            //StreamReader reader = new StreamReader(filePath);
            currRecipe = null;// Recipe.LoadFromFile(@"C:\recipe.xml");

            gtw = new ExEyGatewayCtrl();
            gtw.ExternalProcess = true;
            gtw.SetSupIsAlive += new SetSupIsAliveDelegate(gtw_SetSupIsAlive);
            gtw.SetSupervisorMode += new SetSupervisorModeDelegate(gtw_SetSupervisorMode);
            gtw.SetSupervisorBypassSendRecipe += new SetSupervisorBypassSendRecipe(gtw_SetSupervisorBypassSendRecipe);
            gtw.SaveRecipeModification += new SaveRecipeModificationDelegate(gtw_SaveRecipeModification);
            gtw.SupervisorButtonClick += new SupervisorButtonClickDelegate(gtw_SupervisorButtonClick);
            gtw.SupervisorUIClosed += new SupervisorUIClosedDelegate(gtw_SupervisorUIClosed);
            gtw.AuditMessage += new AuditMessageDelegate(gtw_AuditMessage);
            gtw.SendResults += gtw_SendResults;
            statusTh = new Thread(new ThreadStart(statusThread));
            statusTh.Start();
        }

        void gtw_SendResults(string results) {

            Trace.WriteLine(results);
        }

        void gtw_AuditMessage(int typeId, int messageCount, string messageToAudit) {

            //MessageBox.Show(string.Format("{0} - {1} - {2}", typeId, messageCount, messageToAudit));
            using (StreamWriter sr = new StreamWriter(@"audit.txt", true)) {
                sr.WriteLine(messageToAudit);
            }
            //gtw.SetLastAuditMessageResult(messageCount);
        }

        void gtw_SupervisorUIClosed(bool restartForm) {

            if (restartForm)
                gtw.ShowSupervisorUI();
            else
                gtw.StopSupervisor();
        }

        void gtw_SupervisorButtonClick(string button) {

            MessageBox.Show("Bottone: " + button);
        }

        void gtw_SaveRecipeModification(string recipe) {

            /*using (StreamWriter sr = new StreamWriter(@"C:\recipe.txt", false))
            {
                sr.Write(recipe);
            }*/
            currRecipe = Recipe.LoadFromXml(recipe);
        }

        void gtw_SetSupervisorMode(int supervisorMode) {


        }

        void gtw_SetSupervisorBypassSendRecipe(bool bypass)
        {
            SetLabelBypassRecipe(gtw.BypassSendRecipe);
        }

        void gtw_SetSupIsAlive() {

            // SEQUENZA IFIX VERIFICATA 3/3/2016
            gtw.SetSupervisorPos(50, 50, 1200, 600);
            gtw.SetLanguage(button3.Text.Substring(1,2));
            gtw.SetUserLevel(9);
            gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
            if (currRecipe!=null)
                gtw.SetActiveRecipe("Test", currRecipe.ToString());
            gtw.ShowSupervisorUI();
            gtw.SetMachineMode(1);
            gtw.SetHMIIsAlive(0);
            // FINE SEQUENZA

            SetLabelBypassRecipe(gtw.BypassSendRecipe);
            
            //StreamReader sr = new StreamReader("ricettaTestMultipleCam.xml");
            //string ricetta = sr.ReadToEnd();
            //sr.Close();
            
            //MessageBox.Show("SetSupIsAlive invoked!");
        }

        private void button1_Click(object sender, EventArgs e) {

            gtw.ExternalProcess = true;
            gtw.StartSupervisor();
        }

        int changePosCounter = 0;
        private void button2_Click(object sender, EventArgs e) {
            if(Control.ModifierKeys == Keys.Control)
            {
                changePosCounter = (changePosCounter + 1) % 3;
                gtw.SetSupervisorPos(changePosCounter * 50, changePosCounter * 50, 1200 + changePosCounter * 50, 600 + changePosCounter * 50);
            }
            else
            {
                changePosCounter = (changePosCounter + 1) % 3;
                gtw.SetSupervisorPos(changePosCounter * 50, changePosCounter * 50, 1788 + changePosCounter * 50, 866 + changePosCounter * 50);
            }

        }

        int changeLangCounter = 0;
        private void button3_Click(object sender, EventArgs e) {

            changeLangCounter = (changeLangCounter + 1) % 16;
            if (changeLangCounter == 0) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("en");
                button3.Text = "(en)";
            }
            if (changeLangCounter == 1) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("it");
                button3.Text = "(it)";
            }
            if (changeLangCounter == 2) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("ru");
                button3.Text = "(ru)";
            }
            if (changeLangCounter == 3) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("zh");
                button3.Text = "(zh)";
            }
            if (changeLangCounter == 4) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("de");
                button3.Text = "(de)";
            }
            if (changeLangCounter == 5) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("es");
                button3.Text = "(es)";
            }
            if (changeLangCounter == 6) {
                if (gtw.SupervisorStarted) gtw.SetLanguage("ja");
                button3.Text = "(ja)";
            }
            if (changeLangCounter == 7)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("fr");
                button3.Text = "(fr)";
            }
            if (changeLangCounter == 8)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("sv");
                button3.Text = "(sv)";
            }
            if (changeLangCounter == 9)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("sk");
                button3.Text = "(sk)";
            }
            if (changeLangCounter == 10)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("pt");
                button3.Text = "(pt)";
            }
            if (changeLangCounter == 11)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("nl");
                button3.Text = "(nl)";
            }
            if (changeLangCounter == 12)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("da");
                button3.Text = "(da)";
            }
            if (changeLangCounter == 13)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("hu");
                button3.Text = "(hu)";
            }
            if (changeLangCounter == 14)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("cs");
                button3.Text = "(cs)";
            }
            if (changeLangCounter == 15)
            {
                if (gtw.SupervisorStarted) gtw.SetLanguage("ko");
                button3.Text = "(ko)";
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void button4_Click(object sender, EventArgs e) {

            gtw.SetSupervisorHide();
        }

        private void button5_Click(object sender, EventArgs e) {
            gtw.SetSupervisorOnTop();
        }

        private void button6_Click(object sender, EventArgs e) {

            gtw.ShowSupervisorUI();
        }

        private void button7_Click(object sender, EventArgs e) {

            gtw.StopSupervisor();
        }

        UserLevelEnum currUserLevel = UserLevelEnum.None;
        private void button8_Click(object sender, EventArgs e) {
            string[] names = Enum.GetNames(typeof(UserLevelEnum));
            UserLevelEnum[] userLevelValues = (UserLevelEnum[])Enum.GetValues(typeof(UserLevelEnum));
            UserLevelEnum nextUserLevel = currUserLevel;
            for (int i = 0; i < userLevelValues.Length; i++) {
                int j = i;
                if (userLevelValues[i] == currUserLevel) {
                    j = (i + 1) % userLevelValues.Length;
                    nextUserLevel = userLevelValues[j];
                }
            }
            currUserLevel = nextUserLevel;
            gtw.SetUserLevel((int)currUserLevel);
            button8.Text = "User level = " + currUserLevel;
        }

        private void button16_Click(object sender, EventArgs e) {

            int num = (int)nudVialsCountToSave.Value;
            num = Math.Max(1, Math.Min(num, 1000));
            gtw.SetDataBase(num);

            //button16.Text = "Record Data";
        }

        private void button9_Click(object sender, EventArgs e) {

            gtw.PrintActiveRecipe();
        }

        private void button10_Click(object sender, EventArgs e) {

            //gtw.RaiseAuditMessage(1, 3, "A;B;C");
        }

        bool runStop = false;
        private void button11_Click(object sender, EventArgs e) {

            if (runStop) {
                gtw.SetMachineMode(1);
                button11.Text = "Run";
            }
            else {
                gtw.SetMachineMode(2);
                button11.Text = "Stop";
            }
            runStop = !runStop;
        }

        private void button12_Click(object sender, EventArgs e) {

            gtw.SetWorkingMode(2);
            //opzionale: TODO (altrimenti seguo file configurazione)
            //se invio machine info devo fare AND tra file e machine info
            gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
            gtw.StartKnapp(1, 120, 1, 3);
        }

        private void button13_Click(object sender, EventArgs e) {

            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = "Recipe Files (*.xml)|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK) {
                    StreamReader sr = new StreamReader(ofd.FileName);
                    string ricetta = sr.ReadToEnd();
                    int tmp = gtw.SetActiveRecipe("test!!!ExGtwTESTTTT", ricetta);
                    MessageBox.Show(tmp.ToString());
                    sr.Dispose();
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            gtw.StartBatch(1); //PRODUCTION
        }

        private void button20_Click(object sender, EventArgs e)
        {
            gtw.StopBatch();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            exit = true;
            if (statusTh != null)
                statusTh.Join(5000);
        }

        void statusThread() {
            while (!exit) {
                setLabelStatus(gtw.Status);
                Thread.Sleep(500);
            }
        }

        void setLabelStatus(int status) {
            if (lblStatus.IsHandleCreated && !lblStatus.IsDisposed) {
                try {
                    if (lblStatus.InvokeRequired && lblStatus.IsHandleCreated)
                        lblStatus.Invoke(new MethodInvoker(() => setLabelStatus(status)));
                    else
                        lblStatus.Text = status.ToString();
                }
                catch {
                }
            }
        }

        private void button14_Click(object sender, EventArgs e) {

            gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
        }

        private string createStatInfo() {

            string statInfo = (checkBox1.Checked ? "1" : "0") + ";" +
                              (checkBox2.Checked ? "1" : "0") + ";" +
                              (checkBox3.Checked ? "1" : "0") + ";" +
                              (checkBox4.Checked ? "1" : "0") + ";" +
                              (checkBox5.Checked ? "1" : "0") + ";" +
                              (checkBox6.Checked ? "1" : "0") + ";" +
                              (checkBox7.Checked ? "1" : "0") + ";" +
                              (checkBox8.Checked ? "1" : "0") + ";" +
                              (checkBox9.Checked ? "1" : "0") + ";" +
                              (checkBox10.Checked ? "1" : "0") + ";" +
                              (checkBox11.Checked ? "1" : "0") + ";" +
                              (checkBox12.Checked ? "1" : "0") + ";" +
                              (checkBox13.Checked ? "1" : "0") + ";" +
                              (checkBox14.Checked ? "1" : "0") + ";" +
                              (checkBox15.Checked ? "1" : "0");
            return statInfo;
        }

        private void button15_Click(object sender, EventArgs e) {

            gtw.StartKnapp(-1, 0, 0, 0);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //gtw.SetSupervisorPos(-196, -1058, 1788, 866);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(1920, 1080);
            this.Location = new Point(0, 0);
            buttonClose.Visible = true;
            button18.Visible = true;

            button6.Location = new Point(12, 343);
            button2.Location = new Point(12, 403);
            button5.Location = new Point(12, 464);
            button10.Location = new Point(12, 523);
            button12.Location = new Point(12, 583);
            button7.Location = new Point(12, 643);
            button8.Location = new Point(12, 703);
            button13.Location = new Point(12, 763);
            button15.Location = new Point(12, 823);
            button16.Location = new Point(12, 883);
            nudVialsCountToSave.Location = new Point(13, 930);
            panel1.Location = new Point(12, 960);

            button17.Location = new Point(12, 1023);

            this.BackgroundImage = ExEyGtwTest.Properties.Resources.SfondoArtic;

            panelSupervisor.Size = new Size(1788, 866);
            panelSupervisor.Location = new Point(120, 102);
            gtw.SetSupervisorPos(120, 102, 1788, 866);
            gtw.SetSupervisorOnTop();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        bool visibilityButtons = true;
        private void button18_Click(object sender, EventArgs e)
        {
            visibilityButtons = !visibilityButtons;

            button1.Visible = visibilityButtons;
            button3.Visible = visibilityButtons;
            button4.Visible = visibilityButtons;
            button9.Visible = visibilityButtons;
            button11.Visible = visibilityButtons;


            button6.Visible = visibilityButtons;
            button2.Visible = visibilityButtons;
            button5.Visible = visibilityButtons;
            button10.Visible = visibilityButtons;
            button12.Visible = visibilityButtons;
            button7.Visible = visibilityButtons;
            button8.Visible = visibilityButtons;
            button13.Visible = visibilityButtons;
            button15.Visible = visibilityButtons;
            button16.Visible = visibilityButtons;
            nudVialsCountToSave.Visible = visibilityButtons;
            panel1.Visible = visibilityButtons;

            button17.Visible = visibilityButtons;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            gtw.SetBatchId((int)numericUpDownIdBatch.Value);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            gtw.SetActiveRecipeVersion((int)numRecipeVersion.Value);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            gtw.SetActiveRecipeStatus(textBoxRecipeStatus.Text);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Recipe Files (*.xml)|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(ofd.FileName);
                    string ricetta = sr.ReadToEnd();
                    int tmp = gtw.SetActiveRecipeAllParams($"ExGtwTESTTTT-v.{(int)numRecipeVersion.Value}-state:{textBoxRecipeStatus.Text}", ricetta, (int)numRecipeVersion.Value, textBoxRecipeStatus.Text);
                    MessageBox.Show(tmp.ToString());
                    sr.Dispose();
                }
            }
        }

        void SetLabelBypassRecipe(bool bypass)
        {
            labelBypassSendRecipe.Text = $"Bypass send recipe: {$"{bypass}".ToUpper()}";
            if (bypass)
                labelBypassSendRecipe.BackColor = Color.MistyRose;
            else
                labelBypassSendRecipe.BackColor = Color.PaleGreen;
        }
    }
}
