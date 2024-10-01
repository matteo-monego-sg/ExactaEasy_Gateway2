using Hvld.Controls;
using Hvld.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Hvld.Controls.HvldDisplayControl;

namespace HvldTest
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private byte[] _fileBytes;
        /// <summary>
        /// 
        /// </summary>
        private string _fileName;
        /// <summary>
        /// 
        /// </summary>
        private HvldFrame _frame;
        /// <summary>
        /// 
        /// </summary>
        private StringBuilder _debugInfo = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
            hvldSingleDisplay.EnableGlobalAntialising = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            DataGridFrame.ColumnCount = 7;
            DataGridFrame.Columns[0].Name = "Version";
            DataGridFrame.Columns[1].Name = "Id";
            DataGridFrame.Columns[2].Name = "Signals";
            DataGridFrame.Columns[3].Name = "Payload Type";
            DataGridFrame.Columns[4].Name = "Payload Size (bytes)";
            DataGridFrame.Columns[5].Name = "CRC";
            DataGridFrame.Columns[6].Name = "CRC Validation";
            hvldSingleDisplay.DisplayedSignalChanged += HvldSingleDisplay_DisplayedSignalChanged;
        }

        private void HvldSingleDisplay_DisplayedSignalChanged(object source, DisplayedSignalChangedEventArgs e)
        {
            SuspendLayout();
            try
            {
                // Loads the frame header grid with the data contained in the frame header.
                LoadFrameHeader(e.Frame);
                // Gets the SignalInfo for the specific Signal ID.
                e.Frame.Payload.GetSignal(e.Signal.Id, out SignalInfo signalInfo);
                // Create a new signal tweaker control and add it to the bottom grid.
                var ost = new OptrelSignalTweaker(signalInfo, e.Signal);
                ost.OnGetGraphImage += Ost_OnGetGraphImage;
                ost.OnSignalHeaderClicked += Ost_OnSignalHeaderClicked;
                ost.Dock = DockStyle.Fill;

                TlpStacker.Invoke((new MethodInvoker(() =>
                {
                    TlpStacker.Controls.Clear();
                    TlpStacker.Controls.Add(ost);
                })));

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                   this,
                   $"Errore inaspettato durante la lettura del dump del frame: {ex}",
                   "ATTENZIONE",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Ost_OnGetGraphImage(object sender, Image image)
        {
            PbxStationMainImage.Image = image;
        }
        /// <summary>
        /// 
        /// </summary>
        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            SuspendLayout();

            try 
            { 
                using (var ofd = new OpenFileDialog())
                {
                    switch (ofd.ShowDialog())
                    {
                        case DialogResult.OK:
                            _fileName = ofd.FileName;
                            TxtSelectedFile.Text = _fileName;
                            LoadFromDumpFile(_fileName);
                            break;

                        default:
                            return;
                    }
                }
            }
            finally
            {
                ResumeLayout();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadFromDumpFile(string fileName)
        {
            _debugInfo = new StringBuilder();

            TlpStacker.Controls.Clear();
            TxtSelectedFile.Text = fileName;

            var bytes = HvldHelper.ReadDumpByteArray(fileName, out long dumpReadTime);

            _debugInfo.AppendLine($"> dump data read in {dumpReadTime} ms.");

            var frame = HvldHelper.CreateHvldFrameFromByteArray(bytes, out long frameCreationTime);

            _debugInfo.AppendLine($"> HVLD frame object created in {frameCreationTime} ms");

            HvldHelper.LoadSignalsFromHvldFrame(frame, hvldSingleDisplay, out long signalsUpdateTime);

            _debugInfo.AppendLine($"> signals update to control took {signalsUpdateTime} ms");

            HvldHelper.ShotHvldIntoPictureBox(hvldSingleDisplay, PbxStationMainImage, out long hvldImageShotTime);

            _debugInfo.AppendLine($"> signal image shot took {hvldImageShotTime} ms");

            _debugInfo.AppendLine($"");

            LoadFrameHeader(frame);
           
            TxtDebug.Text = _debugInfo.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReloadFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_fileName) || !File.Exists(_fileName))
                return;

            LoadFromDumpFile(_fileName);
        }
      


        private void Ost_OnSignalHeaderClicked(object sender, OptrelSignal signal)
        {
            hvldSingleDisplay.ShowSignal(signal.Id);
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadFrameHeader(HvldFrame frame)
        {
            if (frame is null)
            {
                MessageBox.Show(
                    this, 
                    "Errore di caricamento del frame da dump: DEBUG!", 
                    "ATTENZIONE", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }
          
            DataGridFrame.Invoke((new MethodInvoker(() =>
            {
                DataGridFrame.Rows.Clear();
                var row = new ArrayList();
                row.Add($"{frame.Version}");
                row.Add($"{frame.Id}");
                row.Add($"{frame.NumberOfSignals}");
                row.Add($"{frame.PayloadType}");
                row.Add($"{frame.PayloadSize}");
                row.Add($"{frame.CRC}");

                var crcValidation = frame.ValidateCRC() ? "OK" : "KO";
                row.Add($"{crcValidation}");
                DataGridFrame.Rows.Add(row.ToArray());
            })));
        }
        /// <summary>
        /// 
        /// </summary>
        private void DataGridFrame_SelectionChanged(object sender, EventArgs e)
        {
            DataGridFrame.Invoke((new MethodInvoker(() =>
            {
                DataGridFrame.ClearSelection();
            })));
        }
        /// <summary>
        /// 
        /// </summary>
        private void BtnSimulate_Click(object sender, EventArgs e)
        {
            string[] dumpFileNames;
            using (var fBrowser = new FolderBrowserDialog())
            {
                fBrowser.Description = "Choose the test dump directory...";
                switch (fBrowser.ShowDialog(this))
                { 
                    case DialogResult.OK:
                        break;
                    default:
                        return;
                }
                dumpFileNames = Directory.GetFiles(fBrowser.SelectedPath);
            }

            if (dumpFileNames is null || dumpFileNames.Length == 0)
                return;

            using (var simForm = new FrmSimulation(dumpFileNames))
            { 
                simForm.ShowDialog(this);
            }
        }
    }
}
