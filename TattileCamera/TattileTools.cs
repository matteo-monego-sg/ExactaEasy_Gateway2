using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasy;
using ExactaEasy.DAL;
using ExactaEasyEng;
using SPAMI.Util.Logger;
using System.Collections;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using DisplayManager;
using ExactaEasyCore;

namespace TattileCameras
{
    public partial class TattileTools : AssistantControl
    {
        
        AssistantsEnum currentAssistant = AssistantsEnum.Strobo;
        Queue<Bitmap> assistantsImgQueue = new Queue<Bitmap>();
        Thread assistantsImgTh;
        public ManualResetEvent UpdateImgEv = new ManualResetEvent(false);
        ManualResetEvent KillEv = new ManualResetEvent(false);
        WaitHandle[] ImgThHandles = new WaitHandle[2];
        TattileCameraBase _camera;

        public TattileTools()
        {
            InitializeComponent();
        }

        private void TattileTools_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                btnAssStrobo.Text = frmBase.UIStrings.GetString("Strobe");
                btnAssNormalization.Text = frmBase.UIStrings.GetString("Normalization");
                btnAssVialAxis.Text = frmBase.UIStrings.GetString("VialAxis");
                btnAssCheckVialAxis.Text = frmBase.UIStrings.GetString("CheckVialAxis");
                btnAssResetTriggerEnc.Text = frmBase.UIStrings.GetString("ResetTriggerEncoder");
                ImgThHandles[0] = KillEv;
                ImgThHandles[1] = UpdateImgEv;
                assistantsImgQueue.Clear();
                assistantsImgTh = new Thread(new ThreadStart(ImgShower));
                assistantsImgTh.Start();
                this.Disposed += new EventHandler(TattileTools_Disposed);
            }
        }

        void TattileTools_Disposed(object sender, EventArgs e)
        {
            UpdateImgEv.Reset();
            KillEv.Set();
            if (assistantsImgTh != null && assistantsImgTh.IsAlive)
                assistantsImgTh.Join();
        }

        public override void LoadAssistant()
        {
            switch (currentAssistant)
            {
                case AssistantsEnum.Strobo:
                    DataSource.StroboAssistantParameters.PopulateParametersInfo();
                    refreshData(new ParametersDataBinder<StroboAssistantParameter>(DataSource.StroboAssistantParameters, null, UserLevel));
                    break;
                case AssistantsEnum.Normalization:
                    DataSource.NormalizationAssistantParameters.PopulateParametersInfo();
                    refreshData(new ParametersDataBinder<NormalizationAssistantParameter>(DataSource.NormalizationAssistantParameters, null, UserLevel));
                    break;
                case AssistantsEnum.VialAxis:
                    DataSource.VialAxisAssistantParameters.PopulateParametersInfo();
                    refreshData(new ParametersDataBinder<VialAxisAssistantParameter>(DataSource.VialAxisAssistantParameters, null, UserLevel));
                    break;
                case AssistantsEnum.CheckVialAxis:
                    DataSource.CheckVialAxisAssistantParameters.PopulateParametersInfo();
                    refreshData(new ParametersDataBinder<CheckVialAxisAssistantParameter>(DataSource.CheckVialAxisAssistantParameters, null, UserLevel));
                    break;
                case AssistantsEnum.ResetTriggerEnc:
                    DataSource.ResetTriggerEncAssistantParameters.PopulateParametersInfo();
                    refreshData(new ParametersDataBinder<ResetTriggerEncAssistantParameter>(DataSource.ResetTriggerEncAssistantParameters, null, UserLevel));
                    break;
            }
            MakeUpButtons();
        }

        void refreshData(IDataBinder paramData)
        {
            pgAssistantParams.Hide();
            pgAssistantParams.DataSource = paramData;
            pgAssistantParams.HideColumn("PropertyId");
            pgAssistantParams.HideColumn("Editable");
            pgAssistantParams.HideColumn("MinValue");
            pgAssistantParams.HideColumn("MaxValue");
            pgAssistantParams.SetColumnCaption("PropertyDescription", frmBase.UIStrings.GetString("Parameter"));
            pgAssistantParams.SetColumnCaption("Value", frmBase.UIStrings.GetString("Value"));
            pgAssistantParams.SetColumnCaption("ActualValue", frmBase.UIStrings.GetString("ActualValue"));
            pgAssistantParams.SetColumnPercentageWidth("PropertyDescription", 50);
            pgAssistantParams.SetColumnPercentageWidth("Value", 25);
            pgAssistantParams.SetColumnPercentageWidth("ActualValue", 25);
            //pgAssistantParams.TableLayoutPanelMinMax.Visible = false;
            pgAssistantParams.Show();
        }

        
        private void btnAssStrobo_Click(object sender, EventArgs e)
        {
            currentAssistant = AssistantsEnum.Strobo;
            LoadAssistant();
        }

        private void btnAssNormalization_Click(object sender, EventArgs e)
        {
            currentAssistant = AssistantsEnum.Normalization;
            LoadAssistant();
        }

        private void btnAssVialAxis_Click(object sender, EventArgs e)
        {
            currentAssistant = AssistantsEnum.VialAxis;
            LoadAssistant();
        }

        private void btnAssCheckVialAxis_Click(object sender, EventArgs e)
        {
            currentAssistant = AssistantsEnum.CheckVialAxis;
            LoadAssistant();
        }

        private void btnAssResetTriggerEnc_Click(object sender, EventArgs e)
        {
            currentAssistant = AssistantsEnum.ResetTriggerEnc;
            LoadAssistant();
        }

        void MakeUpButtons()
        {
            switch (currentAssistant)
            {
                case AssistantsEnum.Strobo:
                    btnAssStrobo.BackColor = SystemColors.ControlDarkDark;
                    btnAssNormalization.BackColor = SystemColors.ButtonShadow;
                    btnAssVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssCheckVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssResetTriggerEnc.BackColor = SystemColors.ButtonShadow;
                    break;
                case AssistantsEnum.Normalization:
                    btnAssStrobo.BackColor = SystemColors.ButtonShadow;
                    btnAssNormalization.BackColor = SystemColors.ControlDarkDark;
                    btnAssVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssCheckVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssResetTriggerEnc.BackColor = SystemColors.ButtonShadow;
                    break;
                case AssistantsEnum.VialAxis:
                    btnAssStrobo.BackColor = SystemColors.ButtonShadow;
                    btnAssNormalization.BackColor = SystemColors.ButtonShadow;
                    btnAssVialAxis.BackColor = SystemColors.ControlDarkDark;
                    btnAssCheckVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssResetTriggerEnc.BackColor = SystemColors.ButtonShadow;
                    break;
                case AssistantsEnum.CheckVialAxis:
                    btnAssStrobo.BackColor = SystemColors.ButtonShadow;
                    btnAssNormalization.BackColor = SystemColors.ButtonShadow;
                    btnAssVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssCheckVialAxis.BackColor = SystemColors.ControlDarkDark;
                    btnAssResetTriggerEnc.BackColor = SystemColors.ButtonShadow;
                    break;
                case AssistantsEnum.ResetTriggerEnc:
                    btnAssStrobo.BackColor = SystemColors.ButtonShadow;
                    btnAssNormalization.BackColor = SystemColors.ButtonShadow;
                    btnAssVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssCheckVialAxis.BackColor = SystemColors.ButtonShadow;
                    btnAssResetTriggerEnc.BackColor = SystemColors.ControlDarkDark;
                    break;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                _camera.StopGrab();
                switch (currentAssistant)
                {
                    case AssistantsEnum.Strobo:
                        _camera.SetLearningStrobo(DataSource.StroboAssistantParameters); 
                        break;
                    case AssistantsEnum.Normalization:
                        _camera.SetLearningNormalization(DataSource.NormalizationAssistantParameters);
                        _camera.Connect();
                        break;
                    case AssistantsEnum.VialAxis:
                        _camera.SetLearningVialAxis(DataSource.VialAxisAssistantParameters);
                        break;
                    case AssistantsEnum.CheckVialAxis:
                        _camera.SetCheckVialAxis(DataSource.VialAxisAssistantParameters);
                        break;
                    case AssistantsEnum.ResetTriggerEnc:
                        _camera.SetLearningResetTriggerEncoder(DataSource.ResetTriggerEncAssistantParameters);
                        _camera.Connect();
                        break;
                }
                _camera.Grab();
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "TattileTools.btnSet_Click", "Learning set fails: " + ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                _camera.StartLearning();
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "TattileTools.btnStart_Click", "StartLearning fails: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _camera.StopLearning(true);
                List<FamIdParameter> paramsToEdit = new List<FamIdParameter>();
                if (currentAssistant == AssistantsEnum.Strobo) {
                    //_camera.GetAcquisitionParameters();
                    paramsToEdit.Add(new FamIdParameter(ParameterTypeEnum.Acquisition, "cameraShutter"));
                } 
                if (currentAssistant == AssistantsEnum.VialAxis) 
                {
                    _camera.ExportParameters( 
                        AppEngine.Current.GlobalConfig.SettingsFolder + "/" +
                        _camera.IP4Address + "_" +
                        DataSource.VialAxisAssistantParameters["savedVialAxisFilename"].GetValue() + ".xml");
                }
                
                OnAppliedLearning(this, new LearningEventArgs(_camera, paramsToEdit));
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "TattileTools.btnSave_Click", "StopLearning fails: " + ex.Message);
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            try
            {
                _camera.StopLearning(false);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "TattileTools.btnDiscard_Click", "StopLearning fails: " + ex.Message);
            }
        }

        private void ImgShower()
        {
            while (true)
            {
                int ret = WaitHandle.WaitAny(ImgThHandles);
                if (ret == 0) break;    //exit
                try
                {
                    Bitmap lastImg = _camera.Snap();
                    if (lastImg != null)
                    {
                        UpdateImage(lastImg);
                    }
                }
                catch (Exception ex)
                {
                    //if (ex.Message.ToLower() == "Camera not initialized".ToLower())
                        Log.Line(LogLevels.Debug, "TattileTools.ImgShower", "GetImage failed: " + ex.Message);
                    //else
                    //    Log.Line(LogLevels.Error, "TattileTools.ImgShower", "GetImage fails: " + ex.Message);
                }

                Thread.Sleep(100);
            }
        }

        delegate void ImageInvoker(Bitmap image);
        private void UpdateImage(Bitmap image) {
            if (pbImage.InvokeRequired) {
                pbImage.Invoke(new ImageInvoker(UpdateImage), image);
            }
            else {
                if (pbImage != null && pbImage.Visible)
                    pbImage.Image = image;
            }
        }

        private void pbImage_VisibleChanged(object sender, EventArgs e)
        {
            if (pbImage.Visible) UpdateImgEv.Set();
            else UpdateImgEv.Reset();
        }
    }

    
}
