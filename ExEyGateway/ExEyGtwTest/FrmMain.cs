using ExactaEasy;
using ExactaEasyEng;
using ExEyGateway;
using ExEyGtwTest.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExEyGtwTest
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private ExEyGatewayCtrl gtw = null;
        /// <summary>
        /// 
        /// </summary>
        private bool exit = false;
        /// <summary>
        /// 
        /// </summary>
        private Thread statusTh;
        /// <summary>
        /// 
        /// </summary>
        private Recipe currRecipe = null;
        /// <summary>
        /// 
        /// </summary>
        private int changePosCounter = 0;
        /// <summary>
        /// 
        /// </summary>
        private Stopwatch _sw = new Stopwatch();
        /// <summary>
        /// 
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
            currRecipe = null;

            gtw = new ExEyGatewayCtrl();
            gtw.ExternalProcess = true;

            gtw.SetSupIsAliveEx += Gtw_SetSupIsAliveEx;
            //gtw.SetSupIsAlive += Gtw_OnSetSupIsAlive;
            gtw.SetSupervisorMode += Gtw_OnSetSupervisorMode;
            gtw.SaveRecipeModification += Gtw_OnSaveRecipeModification;
            gtw.SupervisorButtonClick += Gtw_OnSupervisorButtonClick;
            gtw.SupervisorUIClosed += Gtw_OnSupervisorUIClosed;
            gtw.AuditMessage += Gtw_OnAuditMessage;
            gtw.SendResults += Gtw_OnSendResults;
            gtw.SupervisorConnected += Gtw_OnSupervisorConnected;
            gtw.SupervisorDisconnected += Gtw_OnSupervisorDisconnected;
            gtw.SetSupervisorBypassSendRecipe += Gtw_SetSupervisorBypassSendRecipe;
          
            statusTh = new Thread(new ThreadStart(statusThread));
            statusTh.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_SetSupervisorBypassSendRecipe(bool bypass)
        {
            SetLabelBypassRecipe(gtw.BypassSendRecipe);
        }
        /// <summary>
        /// 
        /// </summary>
        void SetLabelBypassRecipe(bool bypass)
        {
            if (labelBypassSendRecipe.InvokeRequired)
            {
                labelBypassSendRecipe.Invoke(new MethodInvoker(() => { SetLabelBypassRecipe(bypass); }));
            }
            else
            {
                labelBypassSendRecipe.Text = $"Bypass send recipe: {$"{bypass}".ToUpper()}";
                if (bypass)
                    labelBypassSendRecipe.BackColor = Color.MistyRose;
                else
                    labelBypassSendRecipe.BackColor = Color.PaleGreen;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSupervisorDisconnected(object sender, EventArgs e)
        {
            PbxConnectionStatus.Image = Resources.black_circle;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSupervisorConnected(object sender, EventArgs e)
        {
            PbxConnectionStatus.Image = Resources.green_circle;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSendResults(string results)
        {
            Trace.WriteLine(results);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnAuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit)
        {
            using (var sr = new StreamWriter(@"audit.txt", true))
                sr.WriteLine(messageToAudit);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSupervisorUIClosed(bool restartForm)
        {
            if (restartForm)
                gtw.ShowSupervisorUI();
            else
                gtw.StopSupervisor();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSupervisorButtonClick(string button)
        {
            MessageBox.Show($"Pulsante premuto: {button}");
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSaveRecipeModification(string recipe)
        {
            currRecipe = Recipe.LoadFromXml(recipe);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSetSupervisorMode(int supervisorMode)
        {
            // Not implemented yet.
        }

        private bool Gtw_SetSupIsAliveEx()
        {
            var tsk = Task.Run(() =>
            {
                var fnResponse = 0;
                //SUPERVISORE TORNA ATTIVO
                fnResponse += gtw.SetSupervisorPos(50, 50, 1200, 600);
                fnResponse += gtw.SetLanguage(button3.Text.Substring(1, 2));
                Thread.Sleep(1000);
                fnResponse += gtw.SetUserLevel(9);
                fnResponse += gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
                if (currRecipe != null)
                {
                    fnResponse += gtw.SetActiveRecipe("Test", currRecipe.ToString());
                }
                gtw.ShowSupervisorUI();
                fnResponse += gtw.SetMachineMode(1);
                fnResponse += gtw.SetHMIIsAlive(0);
                return (fnResponse == 0);
                // FINE SEQUENZA
            });

            SetLabelBypassRecipe(gtw.BypassSendRecipe);

            return tsk.Result;
        }


        /// <summary>
        /// 
        /// </summary>
        private void Gtw_OnSetSupIsAlive()
        {
            var fnResponse = 1;
            //SUPERVISORE TORNA ATTIVO
            fnResponse = gtw.SetSupervisorPos(50, 50, 1200, 600);

            if (fnResponse == 1)
                MessageBox.Show("SetSupervisorPos error!");
           

            fnResponse = gtw.SetLanguage(button3.Text.Substring(1, 2));

            if (fnResponse == 1)
                MessageBox.Show("SetLanguage error!");

            Thread.Sleep(1000);
            fnResponse = gtw.SetUserLevel(9);

            if (fnResponse == 1)
                MessageBox.Show("SetUserLevel error!");

            fnResponse = gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");

            if (fnResponse == 1)
                MessageBox.Show("SetSupervisorMachineInfo error!");


            if (currRecipe != null)
            {
                fnResponse = gtw.SetActiveRecipe("Test", currRecipe.ToString());
            }
            gtw.ShowSupervisorUI();

            fnResponse = gtw.SetMachineMode(1);

            if (fnResponse == 1)
                MessageBox.Show("SetMachineMode error!");

            fnResponse = gtw.SetHMIIsAlive(0);


            if (fnResponse == 1)
                MessageBox.Show("SetHMIIsAlive error!");

            // FINE SEQUENZA


            //Task.Run(() =>
            //{
            //    var fnResponse = 1;
            //    //SUPERVISORE TORNA ATTIVO
            //    gtw.SetSupervisorPos(50, 50, 1200, 600);
            //    gtw.SetLanguage(button3.Text.Substring(1, 2));
            //    Thread.Sleep(1000);
            //    gtw.SetUserLevel(9);
            //    gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
            //    if (currRecipe != null)
            //    {
            //        fnResponse = gtw.SetActiveRecipe("Test", currRecipe.ToString());
            //    }
            //    gtw.ShowSupervisorUI();
            //    gtw.SetMachineMode(1);
            //    gtw.SetHMIIsAlive(0);
            //    // FINE SEQUENZA
            //});
        }
        /// <summary>
        /// Connects to an instance of exacta easy.
        /// </summary>
        private void BtnStartIpcGateway_Click(object sender, EventArgs e)
        {
            //if (gtw.WaitingForSupervisorConnection) return;

            gtw.ExternalProcess = true;
            // Matteo: forcing this to 0 just to see the changes.
            gtw.Status = 0;
            // We are waiting for a connection of an ExactaEasy client. 
            PbxConnectionStatus.Image = Resources.orange_circle;
            // Starts the supervisor.
            gtw.StartSupervisor();
            // Check if the supervisor started.
            //if (!gtw.WaitingForSupervisorConnection)
            PbxConnectionStatus.Image = Resources.black_circle;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
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
        private void button3_Click(object sender, EventArgs e)
        {

            changeLangCounter = (changeLangCounter + 1) % 16;
            if (changeLangCounter == 0)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("en");
                button3.Text = "(en)";
            }
            if (changeLangCounter == 1)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("it");
                button3.Text = "(it)";
            }
            if (changeLangCounter == 2)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("ru");
                button3.Text = "(ru)";
            }
            if (changeLangCounter == 3)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("zh");
                button3.Text = "(zh)";
            }
            if (changeLangCounter == 4)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("de");
                button3.Text = "(de)";
            }
            if (changeLangCounter == 5)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("es");
                button3.Text = "(es)";
            }
            if (changeLangCounter == 6)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("ja");
                button3.Text = "(ja)";
            }
            if (changeLangCounter == 7)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("fr");
                button3.Text = "(fr)";
            }
            if (changeLangCounter == 8)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("sv");
                button3.Text = "(sv)";
            }
            if (changeLangCounter == 9)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("sk");
                button3.Text = "(sk)";
            }
            if (changeLangCounter == 10)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("pt");
                button3.Text = "(pt)";
            }
            if (changeLangCounter == 11)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("nl");
                button3.Text = "(nl)";
            }
            if (changeLangCounter == 12)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("da");
                button3.Text = "(da)";
            }
            if (changeLangCounter == 13)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("hu");
                button3.Text = "(hu)";
            }
            if (changeLangCounter == 14)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("cs");
                button3.Text = "(cs)";
            }
            if (changeLangCounter == 15)
            {
                //if (gtw.WaitingForSupervisorConnection) 
                gtw.SetLanguage("ko");
                button3.Text = "(ko)";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.SetSupervisorHide();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.SetSupervisorOnTop();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.ShowSupervisorUI();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void BtnStopIpcGateway_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.StopSupervisor();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        UserLevelEnum currUserLevel = UserLevelEnum.None;
        private void button8_Click(object sender, EventArgs e)
        {
            string[] names = Enum.GetNames(typeof(UserLevelEnum));
            UserLevelEnum[] userLevelValues = (UserLevelEnum[])Enum.GetValues(typeof(UserLevelEnum));
            UserLevelEnum nextUserLevel = currUserLevel;
            for (int i = 0; i < userLevelValues.Length; i++)
            {
                int j = i;
                if (userLevelValues[i] == currUserLevel)
                {
                    j = (i + 1) % userLevelValues.Length;
                    nextUserLevel = userLevelValues[j];
                }
            }
            currUserLevel = nextUserLevel;
            _sw.Restart();
            gtw.SetUserLevel((int)currUserLevel);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
            button8.Text = "User level = " + currUserLevel;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int num = (int)nudVialsCountToSave.Value;
            num = Math.Max(1, Math.Min(num, 1000));

            _sw.Restart();
            gtw.SetDataBase(num);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.PrintActiveRecipe();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //gtw.RaiseAuditMessage(1, 3, "A;B;C");
        }

        bool runStop = false;
        private void button11_Click(object sender, EventArgs e)
        {

            if (runStop)
            {
                gtw.SetMachineMode(1);
                button11.Text = "Run";
            }
            else
            {
                gtw.SetMachineMode(2);
                button11.Text = "Stop";
            }
            runStop = !runStop;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.SetWorkingMode(2);
            //opzionale: TODO (altrimenti seguo file configurazione)
            //se invio machine info devo fare AND tra file e machine info
            gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
            gtw.StartKnapp(1, 120, 1, 3);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Recipe Files (*.xml)|*.xml";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string recipeXml;
                    var fileNameOnly = Path.GetFileNameWithoutExtension(ofd.FileName);

                    using (var reader = new StreamReader(ofd.FileName))
                        recipeXml = reader.ReadToEnd();

                    try
                    {
                        _sw.Restart();
                        int result = gtw.SetActiveRecipe(fileNameOnly, recipeXml);
                        DisplayExecutionTime(_sw.ElapsedMilliseconds);

                        switch (result)
                        {
                            case 0:
                                // OK.
                                DisplayMessage("SetActiveRecipe: OK.");
                                break;

                            default:
                                DisplayMessage("SetActiveRecipe: recipe has not been received/recipe is invalid.");
                                break;
                        }

                    }
                    catch (OutOfMemoryException)
                    {
                        DisplayMessage("SetActiveRecipe: recipe file is too large.");
                    }
                }
            }


            //using (OpenFileDialog ofd = new OpenFileDialog())
            //{
            //    ofd.Filter = "Recipe Files (*.xml)|*.xml";
            //    if (ofd.ShowDialog(this) == DialogResult.OK)
            //    {
            //        string ricetta;
            //        var fileNameOnly = Path.GetFileName(ofd.FileName);

            //        StreamReader sr = null;
            //        try
            //        {
            //            sr = new StreamReader(ofd.FileName);
            //            ricetta = sr.ReadToEnd();

            //            _sw.Restart();
            //            int result = gtw.SetActiveRecipe(fileNameOnly, ricetta);
            //            DisplayExecutionTime(_sw.ElapsedMilliseconds);

            //            switch (result)
            //            {
            //                case 0:
            //                    // OK.
            //                    DisplayMessage("SetActiveRecipe: OK.");
            //                    break;

            //                default:
            //                    DisplayMessage("SetActiveRecipe: recipe has not been received/recipe is invalid.");
            //                    break;
            //            }

            //        }
            //        catch (OutOfMemoryException)
            //        {
            //            DisplayMessage("SetActiveRecipe: recipe file is too large.");
            //        }
            //        finally
            //        {
            //            if(!(sr is null))
            //                sr.Dispose();
            //        }
            //    }
            //}
        }

        private void button19_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.StartBatch(1); //PRODUCTION
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.StopBatch();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            exit = true;
            if (statusTh != null)
                statusTh.Join(5000);
        }

        void statusThread()
        {
            while (!exit)
            {
                setLabelStatus(gtw.Status);

                switch ((SupervisorModeEnum)gtw.Status)
                {
                    case SupervisorModeEnum.Busy:
                        LockUIWhenBusy(true);
                        break;
                    default:
                        LockUIWhenBusy(false);
                        break;
                }
                Thread.Sleep(500);
            }
        }

        void setLabelStatus(int status)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(lblStatus))
                return;

            if (lblStatus.IsHandleCreated && !lblStatus.IsDisposed)
            {
                try
                {
                    if (lblStatus.InvokeRequired && lblStatus.IsHandleCreated)
                        lblStatus.Invoke(new MethodInvoker(() => setLabelStatus(status)));
                    else
                        lblStatus.Text = $"{(SupervisorModeEnum)status} [{status}]";
                }
                catch
                {
                }
            }
        }

        private void LockUIWhenBusy(bool lockUi)
        {  
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (IsHandleCreated && !IsDisposed)
            {
                try
                {
                    if (InvokeRequired && IsHandleCreated)
                        Invoke(new MethodInvoker(() => LockUIWhenBusy(lockUi)));
                    else
                    {
                        Enabled = !lockUi;
                    }
                }
                catch { }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.SetSupervisorMachineInfo(createStatInfo(), "24000");
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private string createStatInfo()
        {
            string statInfo = (checkBox1.Checked ? "1" : "0") + ";" +
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

        private void button15_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.StartKnapp(-1, 0, 0, 0);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //gtw.SetSupervisorPos(-196, -1058, 1788, 866);
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(1920, 1080);
            Location = new Point(0, 0);
            buttonClose.Visible = true;
            button18.Visible = true;

            button6.Location = new Point(12, 343);
            button2.Location = new Point(12, 403);
            button5.Location = new Point(12, 464);
            button10.Location = new Point(12, 523);
            button12.Location = new Point(12, 583);
            BtnStopIpcGateway.Location = new Point(12, 643);
            button8.Location = new Point(12, 703);
            button13.Location = new Point(12, 763);
            button15.Location = new Point(12, 823);
            button16.Location = new Point(12, 883);
            nudVialsCountToSave.Location = new Point(13, 930);
            panel1.Location = new Point(12, 960);

            button17.Location = new Point(12, 1023);

            BackgroundImage = ExEyGtwTest.Properties.Resources.SfondoArtic;

            panelSupervisor.Size = new Size(1788, 866);
            panelSupervisor.Location = new Point(120, 102);

            _sw.Restart();
            gtw.SetSupervisorPos(120, 102, 1788, 866);
            gtw.SetSupervisorOnTop();
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        bool visibilityButtons = true;
        private void button18_Click(object sender, EventArgs e)
        {
            visibilityButtons = !visibilityButtons;

            BtnStartIpcGateway.Visible = visibilityButtons;
            button3.Visible = visibilityButtons;
            button4.Visible = visibilityButtons;
            button9.Visible = visibilityButtons;
            button11.Visible = visibilityButtons;


            button6.Visible = visibilityButtons;
            button2.Visible = visibilityButtons;
            button5.Visible = visibilityButtons;
            button10.Visible = visibilityButtons;
            button12.Visible = visibilityButtons;
            BtnStopIpcGateway.Visible = visibilityButtons;
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
            _sw.Restart();
            gtw.SetBatchId((int)numericUpDownIdBatch.Value);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.SetActiveRecipeVersion((int)numRecipeVersion.Value);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _sw.Restart();
            gtw.SetActiveRecipeStatus(CboRecipeStatus.Text);
            DisplayExecutionTime(_sw.ElapsedMilliseconds);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Recipe Files (*.xml)|*.xml";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string ricetta;
                    var fileNameOnly = Path.GetFileName(ofd.FileName);

                    StreamReader sr = null;
                    try
                    {
                        using (sr = new StreamReader(ofd.FileName))
                            ricetta = sr.ReadToEnd();

                        _sw.Restart();
                        var result = gtw.SetActiveRecipeAllParams(
                            $"{fileNameOnly}-v.{(int)numRecipeVersion.Value}-state:{CboRecipeStatus.Text}",
                            ricetta,
                            (int)numRecipeVersion.Value,
                            CboRecipeStatus.Text);
                        DisplayExecutionTime(_sw.ElapsedMilliseconds);

                        switch (result)
                        {
                            case 0:
                                // OK.
                                DisplayMessage("SetActiveRecipeAllParams: OK.");
                                break;

                            default:
                                DisplayMessage("SetActiveRecipeAllParams: recipe has not been received/recipe is invalid.");
                                break;
                        }
                    }
                    catch (OutOfMemoryException)
                    {
                        DisplayMessage("SetActiveRecipeAllParams: recipe file is too large.");
                    }
                    finally
                    {
                        if (!(sr is null))
                            sr.Dispose();
                    }
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            CboRecipeStatus.DataSource = Enum.GetNames(typeof(RecipeStatusEnum));
            TopMost = true;
        }


        private void DisplayExecutionTime(long millis)
        {
            try
            {
                LblExecutionTime.Text = $"Execution time: {millis} ms";
            }
            finally
            {
                _sw.Reset();
            }
        }

        private void DisplayMessage(string message)
        {
            TxtExecutionResult.Clear();
            TxtExecutionResult.Text = message;
        }
    }
}
