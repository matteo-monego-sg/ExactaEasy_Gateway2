using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExactaEasyEng.AppDebug;
using System.IO;
namespace ExactaEasy
{
    public partial class DebugPage : UserControl
    {
        Timer _timer;
        bool _loaded = false;
        string[] _sizesMeasures;

        public DebugPage()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Tick += _timer_Tick;
            _timer.Start();

            _sizesMeasures = new string[] { "B", "KB", "MB", "GB", "TB", "???", "???", "???", "???" };
        }


        private void _timer_Tick(object sender, EventArgs e)
        {
            if (this.Visible == false)
                return;

            try
            {
                if (_loaded == false)
                {
                    LoadUI();
                    _loaded = true;
                }

                UpdateUI();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Fatal error debug page: {ex.Message}", "ERROR DEBUG PAGE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        void LoadUI()
        {
            foreach(DebugSizeImagesResults imgSize in AppDebug.SizeImagesResults)
            {
                dataGridViewSizeImagesResult.Rows.Add();
                int index = dataGridViewSizeImagesResult.RowCount - 1;
                dataGridViewSizeImagesResult.Rows[index].Cells[colNode.Index].Value = $"{imgSize.NodeName} [{imgSize.NodeId}]";
                dataGridViewSizeImagesResult.Rows[index].Cells[colStation.Index].Value = $"{imgSize.StationName} [{imgSize.StationId}]";
            }    
        }

        void UpdateUI()
        {
            foreach(DataGridViewRow dgvr in dataGridViewSizeImagesResult.Rows)
            {
                DebugSizeImagesResults img = AppDebug.SizeImagesResults[dgvr.Index];
                dgvr.Cells[colCounter.Index].Value = img.Counter;
                dgvr.Cells[colMin.Index].Value = GetSizeStr(img.Min);
                dgvr.Cells[colMax.Index].Value = GetSizeStr(img.Max);
                dgvr.Cells[colAverage.Index].Value = GetSizeStr(img.Average);
                dgvr.Cells[colSum.Index].Value = GetSizeStr(img.Sum);
            }
        }


        string GetSizeStr(long size)
        {
            int increment = 0;
            double actSize = size;
            while(actSize > 1024)
            {
                actSize /= 1024;
                increment++;
            }

            actSize = Math.Round(actSize, 2);
            return $"{actSize} {_sizesMeasures[increment]}";
        }


        private void buttonResetSizeImgRes_Click(object sender, EventArgs e)
        {
            AppDebug.SizeImagesResults.ResetAll();
        }

        private void buttonExportTableSizeImgRes_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            DateTime dt = DateTime.Now;
            sv.Filter = "Txt Files (*.txt)|*.txt";
            sv.FileName = $"SizeImagesResult_{dt.ToString("yyyyMMdd_hhmmss")}";

            if (sv.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sbToWrite = new StringBuilder();
                    sbToWrite.Append("SIZE IMAGES RESULTS\n");

                    List<int> maxCharCol = new List<int>();
                    foreach (DataGridViewRow dgvr in dataGridViewSizeImagesResult.Rows)
                    {
                        foreach (DataGridViewCell cel in dgvr.Cells)
                        {
                            object obj = cel.Value;
                            string objStr = obj == null ? "" : obj.ToString();
                            if (maxCharCol.Count < dgvr.Cells.Count)
                                maxCharCol.Add(0);
                            if (maxCharCol[cel.ColumnIndex] < objStr.Length)
                                maxCharCol[cel.ColumnIndex] = objStr.Length;
                        }
                    }

                    foreach (DataGridViewColumn col in dataGridViewSizeImagesResult.Columns)
                    {
                        if (maxCharCol[col.Index] < col.HeaderText.Length)
                            maxCharCol[col.Index] = col.HeaderText.Length;
                        sbToWrite.Append(col.HeaderText.PadRight(maxCharCol[col.Index] + 5));
                    }
                    sbToWrite.Append("\n");

                    foreach (DataGridViewRow dgvr in dataGridViewSizeImagesResult.Rows)
                    {
                        foreach (DataGridViewCell cel in dgvr.Cells)
                        {
                            object obj = cel.Value;
                            string objStr = obj == null ? "" : obj.ToString();
                            sbToWrite.Append(objStr.PadRight(maxCharCol[cel.ColumnIndex] + 5));
                        }
                        sbToWrite.Append("\n");
                    }
                    sbToWrite.Remove(sbToWrite.Length - 1, 1);

                    string fullTxt = sbToWrite.ToString();
                    File.WriteAllText(sv.FileName, fullTxt);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
