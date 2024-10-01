using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using ExactaEasyEng;
using ExactaEasyCore;
using ExactaEasyCore.TrendingTool;
using System.Globalization;

namespace ExactaEasy
{
    public partial class Trending : UserControl
    {
        string _stAllBatchTranslated;
        string _memCbRecipe;
        string _memCbBatchId;
        List<TrendConfigFileSaved> _trendConfigs;
        List<KeyValuePair<DateTime, double>> _readedValues;
        string _memLastReadedComb;


        public Trending()
        {
            InitializeComponent();

            //translations
            lblRecipe.Text = frmBase.UIStrings.GetString("Recipe").ToUpper();
            lblBatchID.Text = frmBase.UIStrings.GetString("BatchID").ToUpper();
            lblTool.Text = $"{frmBase.UIStrings.GetString("Station")}-{frmBase.UIStrings.GetString("Tool")}-{frmBase.UIStrings.GetString("Parameter")}".ToUpper();
            lblRange.Text = frmBase.UIStrings.GetString("Range").ToUpper();

            _stAllBatchTranslated = frmBase.UIStrings.GetString("All");
            _trendConfigs = new List<TrendConfigFileSaved>();
            _readedValues = new List<KeyValuePair<DateTime, double>>();

            //vals range
            var ranges = new[]
            {
                new {Text = frmBase.UIStrings.GetString("All"), Value = -1 },
                new {Text = "10", Value = 10 },
                new {Text = "50", Value = 50 },
                new {Text = "100", Value = 100 },
                new {Text = "500", Value = 500 },
                new {Text = "2000", Value = 2000 },
                new {Text = "5000", Value = 5000 },
            };
            cbRange.DisplayMember = "Text";
            cbRange.ValueMember = "Value";
            cbRange.DataSource = ranges;
            cbRange.Text = "100"; //default

            GenerateGraph();
        }


        public void GenerateGraph()
        {
            //general
            zgcTrending.GraphPane.Title.IsVisible = false;
            zgcTrending.GraphPane.Legend.IsVisible = false;
            zgcTrending.GraphPane.Fill = new Fill(Color.FromArgb(55, 55, 56));
            zgcTrending.GraphPane.Chart.Fill = new Fill(Color.FromArgb(31, 31, 32));
            zgcTrending.IsShowHScrollBar = true;
            zgcTrending.IsAutoScrollRange = true;
            zgcTrending.PointValueEvent += ZgcTrending_PointValueEvent;

            //Y Axis
            zgcTrending.GraphPane.YAxis.Title.Text = frmBase.UIStrings.GetString("Measure").ToUpper();
            zgcTrending.GraphPane.YAxis.Title.FontSpec.Size = 9f;
            zgcTrending.GraphPane.YAxis.Title.FontSpec.FontColor = Color.White;
            zgcTrending.GraphPane.YAxis.Scale.FontSpec.Size = 8f;
            zgcTrending.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.White;
            zgcTrending.GraphPane.YAxis.Scale.MagAuto = false;
            zgcTrending.GraphPane.YAxis.Scale.MajorStepAuto = true;
            zgcTrending.GraphPane.YAxis.Scale.MinorStepAuto = true;
            zgcTrending.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zgcTrending.GraphPane.YAxis.MajorGrid.DashOff = 0.5f;
            zgcTrending.GraphPane.YAxis.MajorGrid.Color = Color.FromArgb(55, 55, 56);

            //X Axis
            zgcTrending.GraphPane.XAxis.Title.Text = frmBase.UIStrings.GetString("DateTime").ToUpper();
            zgcTrending.GraphPane.XAxis.Title.FontSpec.Size = 9f;
            zgcTrending.GraphPane.XAxis.Title.FontSpec.FontColor = Color.White;
            zgcTrending.GraphPane.XAxis.Scale.FontSpec.Size = 8f;
            zgcTrending.GraphPane.XAxis.Scale.FontSpec.FontColor = Color.White;
            zgcTrending.GraphPane.XAxis.Scale.MagAuto = false;
            zgcTrending.GraphPane.XAxis.Scale.MajorStepAuto = true;
            zgcTrending.GraphPane.XAxis.Scale.MinorStepAuto = true;
            zgcTrending.GraphPane.XAxis.Scale.Min = 0;
            zgcTrending.GraphPane.XAxis.Scale.Max = 500;
            zgcTrending.GraphPane.XAxis.ScaleFormatEvent += XAxis_ScaleFormatEvent;
            zgcTrending.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zgcTrending.GraphPane.XAxis.MajorGrid.DashOff = 0.5f;
            zgcTrending.GraphPane.XAxis.MajorGrid.Color = Color.FromArgb(55, 55, 56);

            //refresh
            zgcTrending.AxisChange();
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            PointPairList pointPairs = new PointPairList();
            TrendConfigFileSaved fileSav = null;
            bool readed = false;
            string combination = cbRecipe.Text + cbBatchId.Text + cbTool.Text;
            if(cbBatchId.Text != _stAllBatchTranslated)
            {
                if (_trendConfigs.Count > 0)
                    fileSav = _trendConfigs[cbTool.SelectedIndex];
                if (fileSav != null && _memLastReadedComb != combination)
                {
                    _memLastReadedComb = combination;
                    _readedValues = AppEngine.Current.TrendTool.ReadValues(fileSav);
                    readed = true;
                }
            }
            else
            {
                if(cbRecipe.Text != null && cbRecipe.Text != "" && cbBatchId.Text == _stAllBatchTranslated && cbTool.Text != null && cbTool.Text != "" && _memLastReadedComb != combination)
                {
                    /*    ORDER BY DATE ?????????????    */
                    _memLastReadedComb = combination;
                    _readedValues = AppEngine.Current.TrendTool.ReadValues(cbRecipe.Text, cbTool.Text);
                    readed = true;
                }
            }

            if (readed)
            {
                if (_readedValues == null)
                    return;
                if (_readedValues.Count == 0)
                    return;
                foreach(KeyValuePair<DateTime, double> kvp in _readedValues)
                {
                    PointPair point = new PointPair(pointPairs.Count - 1, kvp.Value);
                    point.Tag = kvp.Key; //add to show on tooltip
                    pointPairs.Add(point);
                }

                zgcTrending.GraphPane.CurveList.Clear();
                zgcTrending.GraphPane.GraphObjList.Clear();

                LineItem line = new LineItem("points", pointPairs, Color.Red, SymbolType.None, 2f);
                line.Symbol.Fill = new Fill(Color.Red);
                zgcTrending.GraphPane.CurveList.Add(line);
            }

            //set max scale x
            if(zgcTrending.GraphPane.CurveList.Count > 0)
            {
                int maxVal = (int)cbRange.SelectedValue;
                if (maxVal < 0)
                    maxVal = _readedValues.Count;
                zgcTrending.GraphPane.XAxis.Scale.Min = 0;
                zgcTrending.GraphPane.XAxis.Scale.Max = maxVal;
                if (maxVal <= 100)
                    ((LineItem)zgcTrending.GraphPane.CurveList[0]).Symbol.Type = SymbolType.Square;
                else
                    ((LineItem)zgcTrending.GraphPane.CurveList[0]).Symbol.Type = SymbolType.None;
            }

            //refresh
            zgcTrending.AxisChange();
            zgcTrending.Invalidate();
        }



        private void cbRecipe_DropDown(object sender, EventArgs e)
        {
            cbRecipe.Items.Clear();
            List<string> recipes = AppEngine.Current.TrendTool.GetListUsedRecipes();
            foreach (string st in recipes)
                cbRecipe.Items.Add(st);
        }

        private void cbBatchId_DropDown(object sender, EventArgs e)
        {
            cbBatchId.Items.Clear();
            string recipeName = cbRecipe.Text;
            List<int> batchIds = AppEngine.Current.TrendTool.GetListUsedBatchId(recipeName);
            //add all
            if (batchIds.Count > 0)
                cbBatchId.Items.Add(_stAllBatchTranslated);
            foreach (int id in batchIds)
                cbBatchId.Items.Add(id);
        }

        private void cbTool_DropDown(object sender, EventArgs e)
        {
            cbTool.Items.Clear();
            string recipeName = cbRecipe.Text;

            if(cbBatchId.Text != _stAllBatchTranslated)
            {
                int batchId = -999999;
                if (int.TryParse(cbBatchId.Text, out int batchIdOut))
                    batchId = batchIdOut;

                _trendConfigs.Clear();
                Dictionary<TrendConfigFileSaved, string> tools = AppEngine.Current.TrendTool.GetListUsedStationTool(recipeName, batchId);
                foreach (KeyValuePair<TrendConfigFileSaved, string> kvp in tools)
                {
                    cbTool.Items.Add(kvp.Value);
                    _trendConfigs.Add(kvp.Key);
                }
            }
            else
            {
                List<int> batchIds = AppEngine.Current.TrendTool.GetListUsedBatchId(recipeName);
                foreach(int batchId in batchIds)
                {
                    Dictionary<TrendConfigFileSaved, string> tools = AppEngine.Current.TrendTool.GetListUsedStationTool(recipeName, batchId);
                    foreach (KeyValuePair<TrendConfigFileSaved, string> kvp in tools)
                    {
                        if (cbTool.Items.Contains(kvp.Value) == false)
                            cbTool.Items.Add(kvp.Value);
                    }
                }
            }
        }



        private void cbRecipe_DropDownClosed(object sender, EventArgs e)
        {
            if(_memCbRecipe != cbRecipe.Text)
            {
                cbBatchId.Items.Clear();
                cbTool.Items.Clear();
                _trendConfigs.Clear();
                _memCbRecipe = cbRecipe.Text;
            }
        }

        private void cbBatchId_DropDownClosed(object sender, EventArgs e)
        {
            if(_memCbBatchId != cbBatchId.Text)
            {
                cbTool.Items.Clear();
                _trendConfigs.Clear();
                _memCbBatchId = cbBatchId.Text;
            }
        }

        private void cbTool_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void cbRange_DropDownClosed(object sender, EventArgs e)
        {

        }


        private string ZgcTrending_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            //return $"DateTime: {((DateTime)curve[iPt].Tag).ToString("yyyy-MM-dd HH:mm:ss.fff")}\nValue: {curve[iPt].Y}";
            string date = ((DateTime)curve[iPt].Tag).ToString(CultureInfo.GetCultureInfo(AppEngine.Current.CurrentContext.CultureCode).DateTimeFormat.ShortDatePattern);
            string time = ((DateTime)curve[iPt].Tag).ToString(CultureInfo.GetCultureInfo(AppEngine.Current.CurrentContext.CultureCode).DateTimeFormat.LongTimePattern);
            //string milli = ((DateTime)curve[iPt].Tag).Millisecond.ToString();
            //return $"DateTime: {date} {time}.{milli}\nValue: {curve[iPt].Y}";
            return $"{frmBase.UIStrings.GetString("DateTime")}: {date} {time}\n{frmBase.UIStrings.GetString("Value")}: {curve[iPt].Y}";
        }

        private string XAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            if (_readedValues == null)
                return "";

            int indexVal = (int)val;
            if (indexVal >= 0 && indexVal < _readedValues.Count)
            {
                //return _readedValues[indexVal].Key.ToString("yyyy-MM-dd\nHH:mm:ss", AppEngine.Current.CurrentContext.CultureCode);
                string date = _readedValues[indexVal].Key.ToString(CultureInfo.GetCultureInfo(AppEngine.Current.CurrentContext.CultureCode).DateTimeFormat.ShortDatePattern);
                string time = _readedValues[indexVal].Key.ToString(CultureInfo.GetCultureInfo(AppEngine.Current.CurrentContext.CultureCode).DateTimeFormat.LongTimePattern);
                return $"{date}\n{time}";
            }
            else
                return "";
        }
    }
}
