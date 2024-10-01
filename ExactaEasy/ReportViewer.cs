using ExactaEasy.Model;
using ExactaEasyCore;
using ExactaEasyEng;
using ExactaEasyEng.Utilities;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using SPAMI.Util.Logger;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace ExactaEasy
{
    public partial class ReportViewer : UserControl
    {

        /* CODICE VECCHIO */
        public event EventHandler BeforePrintReport;

        string tempReportName = "";
        public string ReportTemplatePath { get; set; }
        public string ReportTemplateName { get; set; }

        YesNoPanel YesNoPanel = new YesNoPanel();

        public ReportViewer()
        {
            InitializeComponent();

            //add the YesNoPanel to panelYesNo
            YesNoPanel.Dock = DockStyle.Right;
            //yesnopanel.Location = new Point((panel1.Width - yesnopanel.Width), 0 - yesnopanel.Height);
            YesNoPanel.Visible = false;
            YesNoPanel.YesNoAnswer += new EventHandler<CamViewerMessageEventArgs>(buttonsYesNoExit);

            btnPrint.Text = frmBase.UIStrings.GetString("Printing");
            btnExportPDF.Text = frmBase.UIStrings.GetString("ExportPDF");
            labelMessage.Text = frmBase.UIStrings.GetString("ExportPDFInProgress");
            labelMessage.Visible = false;


            panel1.Controls.Add(YesNoPanel);
        }

        public event EventHandler PrintingMexDialog;


        //public void ShowReport(string[] reportDataFile) {
        //}

        public bool ShowReport(string[] reportData)
        {

            foreach (string fileName in System.IO.Directory.GetFiles(ReportTemplatePath + @"\", "*.html"))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (IOException)
                {
                    Log.Line(LogLevels.Warning, "ReportViewer.ShowReport", "Cannot delete file \"" + fileName + "\"");
                }
            }

            foreach (string fileName in System.IO.Directory.GetFiles(@"\", "*.html"))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (IOException)
                {
                    Log.Line(LogLevels.Warning, "ReportViewer.ShowReport", "Cannot delete file \"" + fileName + "\"");
                }
            }

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", true);
                if (key == null)
                {
                    Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", true);
                }
                key.SetValue("header", "");
                key.SetValue("footer", "");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ReportViewer.ShowReport", "Registry key error. Error: " + ex.Message);
                return false;
            }

            tempReportName = Guid.NewGuid().ToString() + ".html";

            XmlDocument root = new XmlDocument();
            XmlDocument root1 = new XmlDocument();
            root.LoadXml(reportData[0]);
            for (int ird = 1; ird < reportData.Length; ird++)
            {
                root1.LoadXml(reportData[ird]);
                XmlNode nodeRoot1 = root.ImportNode(root1.DocumentElement, true);
                root.DocumentElement.AppendChild(nodeRoot1);
            }

            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(ReportTemplatePath + @"\" + ReportTemplateName);
            xsl.Transform(root, XmlWriter.Create(ReportTemplatePath + @"\" + tempReportName));

            string outputName = "";
            if (System.IO.File.Exists(ReportTemplatePath + @"\" + tempReportName))
                outputName = ReportTemplatePath + @"\" + tempReportName;
            else if (System.IO.File.Exists(@"\" + tempReportName))
                outputName = @"\" + tempReportName;

            if (outputName != "")
                webBrowser1.Navigate(outputName);

            // webBrowser1.Navigate(ReportTemplatePath + @"\" + tempReportName);

            return true;
        }

        /*
        private void btnPrint_Click(object sender, EventArgs e) {

            OnBeforePrintReport(sender, e);
            webBrowser1.ShowPrintDialog();
        }
        */

        protected void OnBeforePrintReport(object sender, EventArgs e)
        {

            if (BeforePrintReport != null) BeforePrintReport(sender, e);
        }


        /* MODIFICA */

        // Attributi locali del form.
        private SolutionData _dataSet;
        private bool ReportIsLoaded { get; set; }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            // Aggiorna il report.
            Rpv_Preview.RefreshReport();
        }

        public void Display_Report()
        {
            ReportIsLoaded = false;

            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new MethodInvoker(Display_Report));
            else
            {
                // Verifica se è stata caricata una ricetta in memoria.
                if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                {
                    // Inizializza il dataset per il caricamento dei dati.
                    _dataSet = new SolutionData();

                    // Inizializza il report.
                    Rpv_Preview.Reset();
                    Rpv_Preview.ProcessingMode = ProcessingMode.Local;
                    LocalReport report = Rpv_Preview.LocalReport;

                    btnPrint.Enabled = false;
                    btnExportPDF.Enabled = false;

                    // Percorso del template del report.
                    report.ReportPath = System.Windows.Forms.Application.StartupPath + "\\machineConfig\\Recipe.rdlc";

                    // Definisce i dati per la generazione del form.
                    this.Get_Data(report);

                    // Imposta i parametri del form.
                    this.Set_Parameters(report);
                }

                // Aggiorna il report.
                Rpv_Preview.RefreshReport();

                Rpv_Preview.RenderingComplete += new RenderingCompleteEventHandler(RenderingComplete);
            }
        }

        private void RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
            btnPrint.Enabled = true;
            btnExportPDF.Enabled = true;
            ReportIsLoaded = true;
            Rpv_Preview.RenderingComplete -= new RenderingCompleteEventHandler(RenderingComplete);
        }


        private void Get_Data(LocalReport report)
        {
            // Crea un clone della ricetta per generare il report.
            Recipe cloneRec = AppEngine.Current.CurrentContext.ActiveRecipe.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);

            // Legge i parametri di 'Recipe': dal nodo 'Recipe' seleziona l'elemento desiderato e ne legge il valore.
            string _recipeName = cloneRec.RecipeName;
            //string _recipeVersion = cloneRec.RecipeVersion;
            string _recipeVersion = AppEngine.Current.CurrentContext.ActiveRecipeVersion <= -1 ? "-" : AppEngine.Current.CurrentContext.ActiveRecipeVersion.ToString();

            // Seleziona il nodo figlio 'Nodes' e cicla sui nodi figli 'NodeRecipe'.
            foreach (NodeRecipe _node in cloneRec.Nodes)
            {
                // Legge i parametri di 'NodeRecipe'.
                string _nodeId = _node.Id.ToString();
                string _nodeDescription = _node.Description;

                //da modificare
                string _boardId = "-";
                string _fileName = "-";

                // Seleziona il nodo figlio 'FrameGrabbers' e cicla sui nodi figli 'FrameGrabberRecipe'.
                foreach (FrameGrabberRecipe _framegr in _node.FrameGrabbers)
                {
                    //seleziona il boardId e il fileName solo della 'FrameGrabberRecipe' attiva
                    if (_framegr.Active == true)
                    {
                        _boardId = _framegr.BoardId.ToString();
                        _fileName = _framegr.ConfigFileName.ToString();
                    }
                }

                // Seleziona il nodo figlio 'Stations' e cicla sui nodi figli 'StationRecipe'.
                foreach (StationRecipe _station in _node.Stations)
                {
                    // Legge i parametri di 'StationRecipe'.
                    string _stationId = _station.Id.ToString();
                    string _stationDescription = _station.Description;

                    // Seleziona il nodo figlio 'Cameras' e cicla sui nodi figli 'CameraRecipe'.
                    foreach (CameraRecipe _camera in _station.Cameras)
                    {
                        // Legge i parametri di 'CameraRecipe'.
                        string _cameraId = _camera.Id.ToString();

                        // Seleziona il nodo figlio 'AcquisitionParameters' e cicla sui nodi figli 'Parameter'.
                        foreach (IParameter _parameter in _camera.AcquisitionParameters)
                        {
                            // Legge i parametri 'AcquisitionParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "ACQUISITION PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }
                        //DIGITIZER PARAMETERS NON INSERITI NEL LORO VECCHIO FILE DI STILE
                        /*
                        foreach (IParameter _parameter in _camera.DigitizerParameters)
                        {
                            // Legge i parametri 'DigitizerParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "DIGITIZER PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }*/

                        foreach (IParameter _parameter in _camera.RecipeSimpleParameters)
                        {
                            // Legge i parametri 'RecipeSimpleParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "RECIPE SIMPLE PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }

                        foreach (IParameter _parameter in _camera.RecipeAdvancedParameters)
                        {
                            // Legge i parametri 'RecipeAdvancedParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "RECIPE ADVANCED PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }

                        foreach (IParameter _parameter in _camera.ROIParameters)
                        {
                            // Legge i parametri 'ROIParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "ROI PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }

                        foreach (IParameter _parameter in _camera.MachineParameters)
                        {
                            // Legge i parametri 'MachineParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "MACHINE PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }

                        foreach (IParameter _parameter in _camera.StroboParameters)
                        {
                            // Legge i parametri 'StroboParameters'.
                            string _parameterId = _parameter.Id;
                            string _parameterValue = _parameter.Value;
                            string _parameterType = "STROBO PARAMETERS";

                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }

                        if (_camera.LightSensor.LightSensorParameters.Count > 0)
                        {
                            //se il valore di Is Active è 1 stampo le tabelle di light sensor parameters e di Shape parameters
                            if (_camera.LightSensor.LightSensorParameters[0].Value == "1")
                            {
                                foreach (IParameter _parameter in _camera.LightSensor.LightSensorParameters)
                                {

                                    // Legge i parametri 'LightSensorParameters'.
                                    string _parameterId = _parameter.Id;
                                    string _parameterValue = _parameter.Value;
                                    string _parameterType = "LIGHT SENSOR PARAMETERS";

                                    // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                                    _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                        _nodeId, _nodeDescription,
                                                                        _stationId, _stationDescription,
                                                                        _cameraId,
                                                                        _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                                }

                                foreach (Shape _shape in _camera.LightSensor.Shapes)
                                {
                                    foreach (IParameter _parameter in _shape.ShapeParameters)
                                    {

                                        // Legge i parametri 'ShapeParameters'.
                                        string _parameterId = _parameter.Id;
                                        string _parameterValue = _parameter.Value;
                                        string _parameterType = "SHAPE PARAMETERS";

                                        // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                                        _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                            _nodeId, _nodeDescription,
                                                                            _stationId, _stationDescription,
                                                                            _cameraId,
                                                                            _parameterId, _parameterValue, _parameterType, _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                                    }
                                }
                            }
                        }
                        //altrimenti stampo solo una riga che dice che il Light Sensor Parameter non è attivo
                        else
                        {
                            // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di light e tools parameters restano vuoti
                            _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                _nodeId, _nodeDescription,
                                                                _stationId, _stationDescription,
                                                                _cameraId,
                                                                "Is Active", "0", "LIGHT SENSOR PARAMETERS", _boardId, _fileName, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }

                        foreach (LightRecipe _lightRecipe in _camera.Lights)
                        {
                            string _lightRecId = _lightRecipe.Id.ToString();

                            foreach (IParameter _parameter in _lightRecipe.StroboParameters)
                            {

                                // Legge i parametri 'ShapeParameters'.
                                string _lightparameterId = _parameter.Id;
                                string _lightparameterValue = _parameter.Value;

                                // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di camera e tools parameters restano vuoti
                                _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                    _nodeId, _nodeDescription,
                                                                    _stationId, _stationDescription,
                                                                    _cameraId,
                                                                    null, null, null, _boardId, _fileName, _lightRecId, _lightparameterId, _lightparameterValue, null, null, null, null, null, null, null, null, null, null, null);
                            }
                        }
                    }

                    int a = 0;
                    foreach (Tool _tool in _station.Tools)
                    {
                        string _toolId = _tool.Id.ToString();

                        if (_tool.ToolParameters[0].Value == "True")
                        {
                            foreach (IParameter _parameter in _tool.ToolParameters)
                            {
                                //check if param is FlowSceneJson
                                if(_parameter.Id == "FlowSceneJson")
                                {
                                    string json = null;
                                    if(AppEngine.Current.MachineConfiguration.PrintReportJSONMode == 1)
                                        json = _parameter.Value;
                                    //add only json value
                                    _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                        _nodeId, _nodeDescription,
                                                                        _stationId, _stationDescription,
                                                                        null,
                                                                        null, null, null, _boardId, _fileName, null, null, null, _toolId, null, null, null, null, null, null, null, null, null, json);
                                }
                                else
                                {
                                    // Legge i parametri 'ShapeParameters'.
                                    string _toolparameterId = _parameter.Id;
                                    string _toolparameterValue = _parameter.Value;

                                    // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di camera e tools parameters restano vuoti
                                    _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                        _nodeId, _nodeDescription,
                                                                        _stationId, _stationDescription,
                                                                        null,
                                                                        null, null, null, _boardId, _fileName, null, null, null, _toolId, _toolparameterId, _toolparameterValue, null, null, null, null, null, null, null, null);
                                }
                            }

                            foreach (Shape _toolShape in _tool.Shapes)
                            {
                                string _toolShapeId = _toolShape.Id.ToString();

                                foreach (IParameter _parameter in _toolShape.ShapeParameters)

                                {
                                    string _toolShapeparameterId = _parameter.Id;
                                    string _toolShapeparameterValue = _parameter.Value;

                                    // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di camera e tools parameters restano vuoti
                                    _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                        _nodeId, _nodeDescription,
                                                                        _stationId, _stationDescription,
                                                                        null,
                                                                        null, null, null, _boardId, _fileName, null, null, null, _toolId, null, null, _toolShapeId, _toolShapeparameterId, _toolShapeparameterValue, null, null, null, null, null);

                                }
                            }
                        }

                        foreach (ToolOutput _tooloutput in _tool.ToolOutputs)

                        {
                            string _toolOutId = _tooloutput.Id;
                            string _toolOutUsed = _tooloutput.IsUsed.ToString();
                            string _toolOutVal = _tooloutput.Value;

                            if (_toolOutUsed == "True")
                            {
                                // Aggiunge alla tabella per la generazione del report una riga con i dati del parametro corrente. I campi specifici di camera e tools parameters restano vuoti
                                _dataSet.solution.AddsolutionRow(_recipeName, _recipeVersion,
                                                                    _nodeId, _nodeDescription,
                                                                    _stationId, _stationDescription,
                                                                    null,
                                                                    null, null, null, _boardId, _fileName, null, null, null, _toolId, null, null, null, null, null, _toolOutId, _toolOutUsed, _toolOutVal, "yes", null);
                            }
                        }
                    }

                }
            }

            // Sorgenti dati del report.
            report.DataSources.Add(new ReportDataSource("Data", (DataTable)_dataSet.solution));
        }

        private void Set_Parameters(LocalReport report)
        {
            // Abilita immagini da sorgenti esterne.
            report.EnableExternalImages = true;

            // Definisce i parametri del report.
            ReportParameter[] _reportParams = new ReportParameter[]
            {
                // new ReportParameter("ID", "0"),
                // new ReportParameter("CHBOX1", "True"),
                new ReportParameter("LANG", "EN"),
                // new ReportParameter("LANG", ExactaEasyEng.AppEngine.Current.CurrentContext.CultureCode),
                // new ReportParameter("CustomerImage", System.Windows.Forms.Application.StartupPath + "https://nextindustry.net/wp-content/uploads/2018/01/Logo_TV_2015.png"),
                // new ReportParameter("CustomerImage","https://nextindustry.net/wp-content/uploads/2018/01/Logo_TV_2015.png"),
                // new ReportParameter("Version", "1"),
                // new ReportParameter("CHBOX2", "True"),
                new ReportParameter("UserPrinter", "UserPrinter"),
                new ReportParameter("Serial", "Serial"),
                new ReportParameter("DateTimeFormat", "yyyy/MM/dd HH:mm:ss")
            };

            // Imposta i parametri del report.
            try
            {
                report.SetParameters(_reportParams);
            }
            catch
            {

            }
        }

        bool PrintingInProgress = false;

        private async void btnExportPDF_Click(object sender, EventArgs e)
        {
            //no operation if the printing is in progress
            if (PrintingInProgress == true || ReportIsLoaded == false)
                return;

            //only admin and optrel can export on pdf
            if ((AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Administrator) || (AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Optrel))
            {
                //Manage the activation of 'Export PDf' button
                Print reportPrinter = new Print();
                if (Rpv_Preview.LocalReport.DataSources.Count > 0)
                {
                    //timer to dots ....
                    string cloneMex = (string)labelMessage.Text.Clone();
                    int nDots = 0;
                    Timer timerDots = new Timer();
                    timerDots.Interval = 1000;
                    timerDots.Tick += delegate (object objTimerDotsSender, EventArgs arg)
                    {
                        string dots = "";
                        for (int i = 0; i < nDots; i++)
                            dots += ".";
                        nDots++;
                        if (nDots >= 5 + 1) //(max number of dots + 1)
                            nDots = 0;
                        labelMessage.Text = cloneMex + dots;
                    };
                    timerDots.Start();

                    //bts + label
                    labelMessage.Visible = true;
                    btnPrint.Enabled = false;
                    btnExportPDF.Enabled = false;

                    SupervisorModeEnum memMode = AppEngine.Current.CurrentContext.SupervisorMode;
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);

                    Log.Line(LogLevels.Debug, "ReportViewer.btnExportPDF_Click", "PDF export of the current report is required...");

                    //await render
                    await reportPrinter.Print_Report(Rpv_Preview.LocalReport, 2);

                    Log.Line(LogLevels.Debug, "ReportViewer.btnExportPDF_Click", "PDF export of the current report is completed");

                    AppEngine.Current.TrySetSupervisorStatus(memMode);

                    //stop timer
                    timerDots.Stop();
                    timerDots.Dispose();

                    //bts + label
                    labelMessage.Visible = false;
                    labelMessage.Text = cloneMex;
                    btnPrint.Enabled = true;
                    btnExportPDF.Enabled = true;
                }
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            //no operation if the printing is in progress
            if(PrintingInProgress == true || ReportIsLoaded == false)
            {
                return;
            }

            YesNoPanel.Visible = true;

            //go to method of yesno
        }

        private void buttonsYesNoExit(object sender, CamViewerMessageEventArgs e)
        {
            switch (e.Message)
            {
                case "Yes":
                    YesNoPanel.Visible = false;
                    //send the printing in other task
                    BackgroundWorker background = new BackgroundWorker();

                    background.WorkerReportsProgress = true;
                    background.WorkerSupportsCancellation = true;

                    //start the background work
                    background.DoWork += new DoWorkEventHandler(background_DoWork);
                    background.RunWorkerAsync();
                    Log.Line(LogLevels.Debug, "ReportViewer.buttonsYesNoExit(btnPrint_Click)", "printing of the current report is required...");
                    break;
                case "No":
                    YesNoPanel.Visible = false;
                    break;
                    //case "Exit":
                    //    YesNoPanel.Visible = false;
                    //    //update the report
                    //    Rpv_Preview.RefreshReport();
                    //    break;
            }
        }

        private void background_DoWork(object sender, DoWorkEventArgs e)
        {
            // Gestisce l'attivazione del pulsante 'Stampa'.
            Print reportPrinter = new Print();

            reportPrinter.PrintingMexDialog += new EventHandler(PrintDialog);
            reportPrinter.StartPrint += new EventHandler(PrintStart);
            reportPrinter.EndPrint += new EventHandler(PrintEnd);

            if (Rpv_Preview.LocalReport.DataSources.Count > 0)
            {
                _ = reportPrinter.Print_Report(Rpv_Preview.LocalReport, 1);
            }
        }

        private void PrintDialog(object sender, EventArgs e)
        {
            PrintingMexDialog(sender, e);
        }

        private void PrintStart(object sender, EventArgs e)
        {
            PrintingInProgress = true;
        }

        private void PrintEnd(object sender, EventArgs e)
        {
            Log.Line(LogLevels.Debug, "ReportViewer.PrintEnd", "Printing of the current report is completed");
            PrintingInProgress = false;
        }

        private void Rpv_Preview_Load(object sender, EventArgs e)
        {

        }
    }
}
