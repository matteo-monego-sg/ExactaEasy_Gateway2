using ExactaEasyEng.Utilities;
using SPAMI.Util.Logger;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace DisplayManager
{
    public class SpectrometerGraphs : Graph
    {
        ZedGraphControl[] _graphCtrl;
        //double xPrev;
        bool disposing;
        //double minX, maxX, minY, maxY;
        //double[] rangeL = new double[4];
        //double[] rangeH = new double[4];
        int _initialWavelength;
        int _finalWavelength;
        ManualResetEvent initDone = new ManualResetEvent(false);
        //Dictionary<string, List<string>> chartPerGraphNames = new Dictionary<string, List<string>>();

        public enum SpectrometerChartType
        {
            Spectra,
            Elaboration,
            ChartsCount
        }

        public SpectrometerGraphs(string name, ZedGraphControl[] graphCtrl, int initialWavelength, int finalWavelength, IStation station)
            : base(name, new StationCollection() { station })
        {

            if (graphCtrl.Length != (int)SpectrometerChartType.ChartsCount)
                throw new Exception("Wrong number of charts!!");
            _graphCtrl = graphCtrl;
            _initialWavelength = initialWavelength;
            _finalWavelength = finalWavelength;
            initDone.Set();
        }

        public override void Dispose()
        {

            disposing = true;
            base.Dispose();
        }

        protected override void station_MeasuresAvailable(object sender, MeasuresAvailableEventArgs e)
        {

            bool success = initDone.WaitOne(5000);
            if (success == false)
                return;

            foreach (ToolResults toolRes in e.InspectionResults.ResToolCollection)
            {
                foreach (MeasureResults measRes in toolRes.ResMeasureCollection)
                {
                    string chartName = e.InspectionResults.NodeId + "_" +
                        e.InspectionResults.InspectionId + "_" +
                        measRes.MeasureName;
                    string graphLabel = "";
                    if (measRes.MeasureName == "PAR_FULL_SPECTRUM_MIS")
                    {
                        graphLabel = "Spectra";
                    }
                    if (measRes.MeasureName == "PAR_FULL_SPECTRUM_BUFFER")
                    {
                        graphLabel = "Spectra";
                    }
                    if (measRes.MeasureName == "PAR_FULL_SPECTRUM_ELAB")
                    {
                        graphLabel = "Elaboration";
                    }
                    if (isChartActive(graphLabel, chartName) == false) continue;
                    try
                    {
                        AddPoints(graphLabel, chartName, measRes.MeasureValue, measRes.MeasureCount);
                    }
                    catch (Exception ex)
                    {
                        Log.Line(LogLevels.Error, "SpectrometerGraphs.station_MeasuresAvailable", "AddPoints error: " + ex.Message);
                    }
                }
            }
            if (_graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane != null)
            {
                Bitmap currentGraph = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane.GetImage();
                (sender as Station).CurrentImage = new Emgu.CV.Image<Emgu.CV.Structure.Rgb, byte>(currentGraph);
                (sender as Station).CurrentIsReject = e.InspectionResults.IsReject;
                (sender as Station).CurrentRejectionBit = e.InspectionResults.RejectionCause;
                (sender as Station).SetMainImage();
            }
        }

        //public void ClearBackground() {
        //    //base.ClearBackground();
        //}

        public override void SetTitles(string graphLabel, string title, string xAxis, string yAxis, double xMin, double xMax, double yMin, double yMax)
        {

            GraphPane _graphPane = null;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
            else
                throw new Exception("Spectrometer chart unknown");

            //TITOLI
            _graphPane.Title.Text = title;
            _graphPane.Title.FontSpec.FontColor = Color.DarkGray;                       //parametrizzare
            //ASSI
            _graphPane.XAxis.Title.Text = xAxis;
            _graphPane.YAxis.Title.Text = yAxis;
            _graphPane.XAxis.MajorGrid.IsVisible = false;                               //parametrizzare
            _graphPane.YAxis.MajorGrid.IsVisible = false;                               //parametrizzare
            _graphPane.XAxis.MinorGrid.IsVisible = false;                               //parametrizzare
            _graphPane.YAxis.MinorGrid.IsVisible = false;                               //parametrizzare
            _graphPane.XAxis.MajorGrid.Color = Color.LightGray;                         //parametrizzare
            _graphPane.YAxis.MajorGrid.Color = Color.LightGray;                         //parametrizzare

            _graphPane.XAxis.Type = AxisType.Linear;
            _graphPane.XAxis.Scale.MagAuto = false;
            //_graphPane.XAxis.Scale.MajorStep = Math.Pow(10, Math.Max(1, Math.Floor(Math.Log10(xMax - xMin))));   //parametrizzare
            //_graphPane.XAxis.Scale.MinorStep = _graphPane.XAxis.Scale.MajorStep / 10;   //parametrizzare
            _graphPane.XAxis.Scale.Format = "#";    //parametrizzare

            _graphPane.YAxis.Type = AxisType.Linear;
            _graphPane.YAxis.Scale.MagAuto = false;
            _graphPane.YAxis.Cross = 0.0;
            _graphPane.YAxis.Color = Color.Gray;
            //_graphPane.YAxis.Scale.MajorStep = 10000;   //parametrizzare
            //_graphPane.YAxis.Scale.MinorStep = 0.1;   //parametrizzare
            _graphPane.YAxis.Scale.Format = "#";    //parametrizzare

            if (xMin != xMax)
            {
                _graphPane.XAxis.Scale.Min = xMin;
                _graphPane.XAxis.Scale.Max = xMax;
            }
            if (yMin != yMax)
            {
                _graphPane.YAxis.Scale.Min = yMin;
                _graphPane.YAxis.Scale.Max = yMax;
            }
            //LEGENDA
            _graphPane.Legend.IsVisible = false;                        //parametrizzare
            _graphPane.Legend.Position = ZedGraph.LegendPos.Bottom;     //parametrizzare
            //BACKGROUND COLOR
            _graphPane.Chart.Fill = new Fill(Color.Black, Color.Black, Color.Black);                   //parametrizzare
            //_graphPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 210), -45F);      //parametrizzare

        }

        public override void SetLimit(string graphLabel, string limitName, LimitType limitType, double limitValue, double fromPt, double toPt, Color color, System.Drawing.Drawing2D.DashStyle dashStyle)
        {

            GraphPane _graphPane = null;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
            else
                throw new Exception("Spectrometer chart unknown");

            // RANGE CHART
            PointPairList list = new PointPairList();
            LineItem curve = _graphPane.AddCurve(limitName, list, color, SymbolType.None);
            curve.Line.IsVisible = true;                //parametrizzare
            curve.Line.Width = 2.0F;                    //parametrizzare
            curve.Line.Style = dashStyle;
            curve.Symbol.Size = 6.0F;                   //parametrizzare
            curve.Symbol.IsVisible = false;             //parametrizzare
            switch (limitType)
            {
                case LimitType.xLimit:
                    if (fromPt == toPt)
                    {
                        curve.AddPoint(limitValue, _graphPane.YAxis.Scale.Min);
                        curve.AddPoint(limitValue, _graphPane.YAxis.Scale.Max);
                    }
                    else
                    {
                        curve.AddPoint(limitValue, fromPt);
                        curve.AddPoint(limitValue, toPt);
                    }
                    break;
                case LimitType.yLimit:
                    if (fromPt == toPt)
                    {
                        curve.AddPoint(_graphPane.XAxis.Scale.Min, limitValue);
                        curve.AddPoint(_graphPane.XAxis.Scale.Max, limitValue);
                    }
                    else
                    {
                        curve.AddPoint(fromPt, limitValue);
                        curve.AddPoint(toPt, limitValue);
                    }
                    break;
                default:
                    Log.Line(LogLevels.Warning, "LineGraph.SetLimit", "Limit type unknown");
                    break;
            }
            updateGraph(graphLabel);
        }

        public override void AddChart(string graphLabel, string chartLabel, Color color, SymbolType symbolType)
        {

            GraphPane _graphPane = null;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
            else
                throw new Exception("Spectrometer chart unknown");
            PointPairList list = new PointPairList();
            LineItem curve = _graphPane.AddCurve(chartLabel, list, color, symbolType);

            curve.Line.IsVisible = true;        //parametrizzare
            curve.Line.Width = 2.0F;            //parametrizzare
            //curve.Symbol.IsVisible = false;     //parametrizzare
            //curve.Symbol.Size = 6.0F;           //parametrizzare

            //if (chartPerGraphNames.ContainsKey(graphLabel) == false)
            //    chartPerGraphNames.Add(graphLabel, new List<string>());
            //chartPerGraphNames[graphLabel].Add(chartLabel);
        }

        public override void RemoveChart(string graphLabel, string chartLabel)
        {

            GraphPane _graphPane = null;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
            else
                throw new Exception("Spectrometer chart unknown");
            _graphPane.CurveList.Remove(_graphPane.CurveList[chartLabel]);

            //if (chartPerGraphNames.ContainsKey(graphLabel) == true)
            //    chartPerGraphNames[graphLabel].Remove(chartLabel);
        }

        public override void AddPoints(string graphLabel, string chartLabel, double[] x, double[] y, int numOfPoints)
        {

            GraphPane _graphPane = null;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
            {
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
            }
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
            {
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
            }
            else
                throw new Exception("Spectrometer chart unknown");
            CurveItem curve = _graphPane.CurveList[chartLabel];
            if (x.Length != y.Length)
            {
                Log.Line(LogLevels.Error, "SpectrometerGraphs.AddPoints", "Wrong number of points: " + x.Length + " X vs " + y.Length + " Y");
                throw new Exception("Wrong number of points: " + x.Length + " X vs " + y.Length + " Y");
            }
            curve.Clear();
            for (int i = 0; i < x.Length; i++)
            {
                curve.AddPoint(_initialWavelength + x[i], y[i]);
            }
            //updateMean();
            updateGraph(graphLabel);
        }

        //public override void Save(string graphLabel, string path) {

        //    GraphPane _graphPane = null;
        //    if (graphLabel == SpectrometerChartType.Spectra.ToString()) {
        //        _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
        //    }
        //    else if (graphLabel == SpectrometerChartType.Elaboration.ToString()) {
        //        _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
        //    }
        //    else
        //        throw new Exception("Spectrometer chart unknown");

        //    if (chartPerGraphNames.ContainsKey(graphLabel) == false)
        //        throw new Exception("Spectrometer chart unknown");

        //    using (StreamWriter sr = new StreamWriter(path)) {

        //        int colNum = chartPerGraphNames[graphLabel].Count;
        //        if (colNum > 0) {
        //            string header = "";
        //            foreach (string curveName in chartPerGraphNames[graphLabel]) {
        //                header += curveName + ";";
        //            }
        //            sr.WriteLine(header);
        //            CurveItem curve = _graphPane.CurveList.Find(ci => ci.Label.Text == chartPerGraphNames[graphLabel][0]);
        //            int rowNum = curve.Points.Count;
        //            for (int i = 0; i < rowNum; i++) {
        //                string line = "";
        //                foreach (string curveName in chartPerGraphNames[graphLabel]) {
        //                    curve = _graphPane.CurveList.Find(ci => ci.Label.Text == curveName);
        //                    if (i < curve.Points.Count)
        //                        line += curve.Points[i].Y + ";";
        //                    else
        //                        line += ";";
        //                }
        //                sr.WriteLine(line);
        //            }
        //        }
        //    }
        //}

        public override bool SetChartVisible(string graphLabel, string chartLabel, bool visible)
        {

            GraphPane _graphPane = null;
            //SpectrometerChartType chartType = SpectrometerChartType.ChartsCount;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
            {
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
                //chartType = SpectrometerChartType.Spectra;
            }
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
            {
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
                //chartType = SpectrometerChartType.Elaboration;
            }
            else
                throw new Exception("Spectrometer chart unknown");
            CurveItem curve = _graphPane.CurveList[chartLabel];
            if (curve != null)
            {
                curve.IsVisible = visible;
                updateGraph(graphLabel);
                return true;
            }
            return false;
        }

        public override bool GetChartVisible(string graphLabel, string chartLabel)
        {

            GraphPane _graphPane = null;
            //SpectrometerChartType chartType = SpectrometerChartType.ChartsCount;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
            {
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
                //chartType = SpectrometerChartType.Spectra;
            }
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
            {
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
                //chartType = SpectrometerChartType.Elaboration;
            }
            else
                throw new Exception("Spectrometer chart unknown");
            CurveItem curve = _graphPane.CurveList[chartLabel];
            if (curve != null)
            {
                return curve.IsVisible;
            }
            return false;
        }

        public override void ResetCounters()
        {

            //    sumY = countY = meanY = 0;
            //    updateMean();
            //    updateGraph();
        }

        void updateGraph(string graphLabel)
        {
            SpectrometerChartType chartType = SpectrometerChartType.ChartsCount;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
            {
                chartType = SpectrometerChartType.Spectra;
            }
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
            {
                chartType = SpectrometerChartType.Elaboration;
            }

            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(_graphCtrl[(int)chartType]))
                return;

            if (disposing || !_graphCtrl[(int)chartType].IsHandleCreated)
                return;

            if (_graphCtrl[(int)chartType].InvokeRequired && _graphCtrl[(int)chartType].IsHandleCreated)
                _graphCtrl[(int)chartType].Invoke(new MethodInvoker(() => updateGraph(graphLabel)));
            else
            {
                _graphCtrl[(int)chartType].AxisChange();
                _graphCtrl[(int)chartType].Refresh();
            }
        }

        //void updateMean() {

        //    if (disposing || !_graphCtrl.IsHandleCreated)
        //        return;

        //    if (_graphCtrl.InvokeRequired)
        //        _graphCtrl.Invoke(new MethodInvoker(updateMean));
        //    else {
        //        if ((_graphPane.CurveList["meanY"] != null) && (_graphPane.CurveList["meanY"].Points.Count == 2)) {
        //            _graphPane.CurveList["meanY"].Points[0].Y = meanY;
        //            _graphPane.CurveList["meanY"].Points[1].Y = meanY;
        //        }
        //    }
        //}

        //void historicize(LineItem curve, Color histCurveColor) {

        //    if (_graphCtrl.InvokeRequired)
        //        _graphCtrl.Invoke(new MethodInvoker(() => historicize(curve, histCurveColor)));
        //    else {
        //        PointPairList list = (PointPairList)curve.Points.Clone();
        //        string histCurveLabel = curve.Label + "_hist";
        //        LineItem curveHist = (LineItem)_graphPane.CurveList[histCurveLabel];
        //        if (curveHist == null) {
        //            curveHist = _graphPane.AddCurve(histCurveLabel,
        //                  list, histCurveColor, curve.Symbol.Type);
        //        }
        //        else {
        //            curve.Clear();
        //            curveHist.Clear();
        //            curveHist.Points = list;
        //        }
        //        curveHist.Line.IsVisible = true;   //parametrizzare
        //        curveHist.Line.Width = 2.0F;  //parametrizzare
        //        curveHist.Symbol.Size = 6.0F; //parametrizzare
        //        curveHist.Symbol.IsVisible = false; //parametrizzare
        //    }
        //}

        protected override bool isChartActive(string graphLabel, string chartLabel)
        {

            GraphPane _graphPane = null;
            if (graphLabel == SpectrometerChartType.Spectra.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Spectra].GraphPane;
            else if (graphLabel == SpectrometerChartType.Elaboration.ToString())
                _graphPane = _graphCtrl[(int)SpectrometerChartType.Elaboration].GraphPane;
            else
                return false;
            if (_graphPane == null)
                return false;
            CurveItem curve = _graphPane.CurveList[chartLabel];
            return (curve != null);
        }
    }
}
