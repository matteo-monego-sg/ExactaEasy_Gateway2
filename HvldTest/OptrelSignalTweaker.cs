using Hvld.Controls;
using Hvld.Parser;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HvldTest
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OptrelSignalTweaker : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        private StringBuilder _log;
        /// <summary>
        /// 
        /// </summary>
        private SignalInfo _signalInfo;
        /// <summary>
        /// 
        /// </summary>
        private OptrelSignal _signal;
        /// <summary>
        /// 
        /// </summary>
        public delegate void GetGraphImageDelegate(object sender, System.Drawing.Image image);
        /// <summary>
        /// 
        /// </summary>
        public delegate void SignalHeaderClickedDelegate(object sender, OptrelSignal signal);
        /// <summary>
        /// 
        /// </summary>
        public event GetGraphImageDelegate OnGetGraphImage;
        /// <summary>
        /// 
        /// </summary>
        public event SignalHeaderClickedDelegate OnSignalHeaderClicked;
        /// <summary>
        /// 
        /// </summary>
        public OptrelSignalTweaker(SignalInfo signalInfo, OptrelSignal signalControl)
        {
            InitializeComponent();
            _signalInfo = signalInfo;
            _signal = signalControl;

            // Set up the grid columns.
            DataGridSignal.ColumnCount = 11;
            DataGridSignal.Columns[0].Name = "Title";
            DataGridSignal.Columns[1].Name = "Signal ID";
            DataGridSignal.Columns[2].Name = "Data Type";
            DataGridSignal.Columns[3].Name = "# Points";
            DataGridSignal.Columns[4].Name = "Color RGBA";
            DataGridSignal.Columns[5].Name = "Classification";
            DataGridSignal.Columns[6].Name = "Related To";
            DataGridSignal.Columns[7].Name = "Signal Min Value";
            DataGridSignal.Columns[8].Name = "Signal Max Value";
            DataGridSignal.Columns[9].Name = "Y-Axis Min Clamp Threshold";
            DataGridSignal.Columns[10].Name = "Y-Axis Max Clamp Threshold";

            for (var i = 0; i < DataGridSignal.ColumnCount; i++)
                DataGridSignal.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            LoadSignalInfoData();
            UpdateUi();
        }

        private void LoadSignalInfoData()
        {
            DataGridSignal.Rows.Clear();

            var row = new ArrayList();

            row.Add($"{_signalInfo.Title}");
            row.Add($"{_signalInfo.GretelId}");
            row.Add($"{_signalInfo.DataType}");
            row.Add($"{_signalInfo.NumberOfPoints}");
            row.Add($"0x{_signalInfo.RGBA.ToString("X")}");
            row.Add($"{_signalInfo.Classification}");
            row.Add($"{_signalInfo.OverlaySignalId}");

            if (_signalInfo.Points.Count.Equals(0))
            {
                row.Add($"annihilated by threshold?");
                row.Add($"annihilated by threshold?");
            }
            else
            {
                row.Add($"{_signalInfo.Points.Min(p => p.Y)}");
                row.Add($"{_signalInfo.Points.Max(p => p.Y)}");
            }
            row.Add($"{_signalInfo.YAxisMinClamp}");
            row.Add($"{_signalInfo.YAxisMaxClamp}");

            DataGridSignal.Rows.Add(row.ToArray());
            // Paints the background of the color cell.
            DataGridSignal.Rows[DataGridSignal.RowCount - 1].Cells[4].Style.BackColor = HvldUtilities.GretelRGBAToColor(_signalInfo.RGBA);

        }
        /// <summary>
        /// 
        /// </summary>
        private void BtnGetImage_Click(object sender, EventArgs e)
        {
            var image = _signal.GetImage();
            OnGetGraphImage?.Invoke(this, image);
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateUi()
        {
            LblSignalId.Text = _signalInfo.GretelId.ToString();
            LblSignalName.Text = _signalInfo.Title;
            ChkAntialiasing.Checked = _signal.IsAntiAlias;
            ChkShowBorder.Checked = _signal.BorderVisible;
            ChkAddBoundingLines.Checked = _signal.ShowSignalBoundingLines;
            NudThickness.Value = _signal.BorderSize;
            PnlBorderColor.BackColor = _signal.BorderColor ?? _signal.BorderColor.Value;
            PnlSignalColor.BackColor = _signalInfo.SignalColor;

        }

        private void BtnSimpleClamp_Click(object sender, EventArgs e)
        {
            if (_signal.ClampLowerBound.HasValue && _signal.ClampUpperBound.HasValue)
                _signal.ApplyClamp(ClampingModeEnum.Simple, ClampingSide.Both);
            else if (_signal.ClampLowerBound.HasValue)
                _signal.ApplyClamp(ClampingModeEnum.Simple, ClampingSide.Lower);
            else if (_signal.ClampUpperBound.HasValue)
                _signal.ApplyClamp(ClampingModeEnum.Simple, ClampingSide.Upper);

            _signal.Redraw();
        }

        private void BtnAccurateClamp_Click(object sender, EventArgs e)
        {
            if (_signal.ClampLowerBound.HasValue && _signal.ClampUpperBound.HasValue)
                _signal.ApplyClamp(ClampingModeEnum.Accurate, ClampingSide.Both);
            else if (_signal.ClampLowerBound.HasValue)
                _signal.ApplyClamp(ClampingModeEnum.Accurate, ClampingSide.Lower);
            else if (_signal.ClampUpperBound.HasValue)
                _signal.ApplyClamp(ClampingModeEnum.Accurate, ClampingSide.Upper);

            _signal.Redraw();
        }

        private void BtnRemoveClamp_Click(object sender, EventArgs e)
        {
            _signal.RemoveClamp();
            _signal.Redraw();
        }

        private void ChkAntialiasing_CheckedChanged(object sender, EventArgs e)
        {
            _signal.IsAntiAlias = ChkAntialiasing.Checked;
            _signal.Redraw();
        }

        private void ChkShowBorder_CheckedChanged(object sender, EventArgs e)
        {
            _signal.BorderVisible = ChkShowBorder.Checked;
            _signal.Redraw();
        }

        private void ChkAddBoundingLines_CheckedChanged(object sender, EventArgs e)
        {
            _signal.ShowSignalBoundingLines = ChkAddBoundingLines.Checked;
            _signal.Redraw();
        }

        private void PnlBorderColor_Click(object sender, EventArgs e)
        {
            using (var cd = new ColorDialog())
            {
                switch (cd.ShowDialog())
                {
                    case DialogResult.OK:
                        _signal.BorderColor = cd.Color;
                        PnlBorderColor.BackColor = cd.Color;
                        _signal.Redraw();
                        break;
                }
            }
        }

        private void PnlSignalColor_Click(object sender, EventArgs e)
        {
            using (var cd = new ColorDialog())
            {
                switch (cd.ShowDialog())
                {
                    case DialogResult.OK:

                        foreach (var curve in _signal.GraphPane.CurveList)
                            curve.Color = cd.Color;
                        PnlSignalColor.BackColor = cd.Color;
                        _signal.Redraw();
                        break;
                }
            }
        }

        private void NudThickness_ValueChanged(object sender, EventArgs e)
        {
            _signal.BorderSize = (int)NudThickness.Value;
            _signal.Redraw();
        }

        private void LblSignalName_Click(object sender, EventArgs e)
        {
            OnSignalHeaderClicked?.Invoke(this, _signal);
        }

        private void LblSignalId_Click(object sender, EventArgs e)
        {
            OnSignalHeaderClicked?.Invoke(this, _signal);
        }
    }
}
