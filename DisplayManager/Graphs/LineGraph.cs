using ExactaEasyEng.Utilities;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace DisplayManager
{
    public class LineGraph : Graph
    {

        ZedGraphControl _graphCtrl;
        GraphPane _graphPane;
        double xPrev;
        bool disposing;
        double minX, maxX, minY, maxY;
        double sumY, countY, meanY;

        public LineGraph(string name, ZedGraphControl graphCtrl, IStation station)
            : base(name, new StationCollection() { station })
        {

            _graphCtrl = graphCtrl;
            _graphPane = graphCtrl.GraphPane;
        }

        public override void Dispose()
        {

            disposing = true;
            base.Dispose();
        }

        protected override void station_MeasuresAvailable(object sender, MeasuresAvailableEventArgs e)
        {

            try
            {
                foreach (ToolResults toolRes in e.InspectionResults.ResToolCollection)
                {
                    foreach (MeasureResults measRes in toolRes.ResMeasureCollection)
                    {
                        string chartName = e.InspectionResults.NodeId + "_" +
                            e.InspectionResults.InspectionId + "_" +
                            measRes.MeasureName;
                        if (!isChartActive("", chartName)) continue;
                        double[] X = new double[] { e.InspectionResults.SpindleId };
                        double[] Y = new double[1];
                        if (double.TryParse(measRes.MeasureValue, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out Y[0]))
                        {
                            //Color color = e.InspectionResults.IsReject ? Color.Red : Color.Green;
                            AddPoints("", chartName, X, Y, 1);
                        }
                        else
                            Log.Line(LogLevels.Warning, "LineGraph.station_MeasuresAvailable", "Error while parsing " + measRes.MeasureValue + " to double");



                        uint spindle = e.InspectionResults.SpindleId;
                        double valueRaw = 0;
                        double.TryParse(measRes.MeasureValue, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out valueRaw);

                        //Qui Ricevo misura. Come la mando alla mia tabella???
                        //ho aggiunto in Camera una lista di double per i valori
                        //in Graph setto a Camera i punti ricevuti
                        //in DataViewer li visualizzo
                        ValuesTurbidimeter vt = new ValuesTurbidimeter();
                        vt.Spindle = Convert.ToInt32(spindle);
                        vt.Value = valueRaw;

                        if ((sender as Station).Cameras.First().eoptisPoints == null)
                            (sender as Station).Cameras.First().eoptisPoints = new List<ValuesTurbidimeter>();
                        int numTurn = (sender as Station).Cameras.First().eoptisPoints.Where(x => x.Spindle == spindle).Count();
                        vt.Turn = numTurn + 1;
                        if (vt.Turn <= 10)
                            (sender as Station).Cameras.First().eoptisPoints.Add(vt);
                    }
                }
                if (_graphPane != null)
                {
                    Bitmap currentGraph = _graphPane.GetImage();
                    (sender as Station).CurrentImage = new Emgu.CV.Image<Emgu.CV.Structure.Rgb, byte>(currentGraph);
                    (sender as Station).CurrentIsReject = e.InspectionResults.IsReject;
                    (sender as Station).CurrentRejectionBit = e.InspectionResults.RejectionCause;
                    (sender as Station).SetMainImage();
                }
            }
            catch { }
        }

        //public override void Save(string graphLabel, string path) {
        //    throw new NotImplementedException();
        //}

        public void ClearBackground()
        {
            //base.ClearBackground();
        }

        public override void SetTitles(string graphLabel, string title, string xAxis, string yAxis, double xMin, double xMax, double yMin, double yMax)
        {

            //TITOLI
            _graphPane.Title.Text = title;
            _graphPane.Title.FontSpec.FontColor = Color.DarkGray;   //parametrizzare
            //ASSI
            _graphPane.XAxis.Title.Text = xAxis;
            _graphPane.YAxis.Title.Text = yAxis;
            _graphPane.XAxis.MajorGrid.IsVisible = true;    //parametrizzare
            _graphPane.YAxis.MajorGrid.IsVisible = true;    //parametrizzare
            _graphPane.XAxis.MinorGrid.IsVisible = false;    //parametrizzare
            _graphPane.YAxis.MinorGrid.IsVisible = false;    //parametrizzare
            _graphPane.XAxis.MajorGrid.Color = Color.LightGray; //parametrizzare
            _graphPane.YAxis.MajorGrid.Color = Color.LightGray; //parametrizzare

            _graphPane.XAxis.Type = AxisType.Linear;
            _graphPane.XAxis.Scale.MajorStep = 1;   //parametrizzare
            _graphPane.XAxis.Scale.MinorStep = 1;   //parametrizzare
            _graphPane.XAxis.Scale.Format = "#";    //parametrizzare

            _graphPane.YAxis.Type = AxisType.Linear;
            _graphPane.YAxis.Scale.MajorStep = 1;   //parametrizzare
            //_graphPane.YAxis.Scale.MinorStep = 0.1;   //parametrizzare
            //_graphPane.YAxis.Scale.Format = "#";    //parametrizzare

            if (xMin != xMax)
            {
                _graphPane.XAxis.Scale.Min = minX = xMin;
                _graphPane.XAxis.Scale.Max = maxX = xMax;
            }
            if (yMin != yMax)
            {
                _graphPane.YAxis.Scale.Min = minY = yMin;
                _graphPane.YAxis.Scale.Max = maxY = yMax;
            }
            //LEGENDA
            _graphPane.Legend.IsVisible = false;                        //parametrizzare
            _graphPane.Legend.Position = ZedGraph.LegendPos.Bottom;     //parametrizzare
            //BACKGROUND COLOR
            _graphPane.Chart.Fill = new Fill(Color.White,
                Color.FromArgb(255, 255, 210), -45F);                   //parametrizzare
            // MEAN CHART
            PointPairList list = new PointPairList();
            LineItem curve = _graphPane.AddCurve("meanY",
                  list, Color.Black, SymbolType.Default);

            curve.Line.IsVisible = true;                                //parametrizzare
            curve.Line.Width = 2.0F;                                    //parametrizzare
            curve.Symbol.Size = 6.0F;                                   //parametrizzare
            curve.Symbol.IsVisible = false;                             //parametrizzare
            curve.AddPoint(_graphPane.XAxis.Scale.Min, meanY);
            curve.AddPoint(_graphPane.XAxis.Scale.Max, meanY);
        }

        public override void SetLimit(string graphLabel, string limitName, LimitType limitType, double limitValue, double fromPt, double toPt, Color color, System.Drawing.Drawing2D.DashStyle dashStyle)
        {

            PointPairList list = new PointPairList();
            LineItem curve = _graphPane.AddCurve(limitName,
                  list, color, SymbolType.Default);

            curve.Line.Style = dashStyle;
            curve.Line.IsVisible = true;   //parametrizzare
            curve.Line.Width = 2.0F;  //parametrizzare
            curve.Symbol.Size = 6.0F; //parametrizzare
            curve.Symbol.IsVisible = false; //parametrizzare

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
        }

        public override void AddChart(string graphLabel, string chartLabel, Color color, SymbolType symbolType)
        {

            PointPairList list = new PointPairList();
            LineItem curve = _graphPane.AddCurve(chartLabel,
                  list, color, symbolType);

            curve.Line.IsVisible = true;   //parametrizzare
            curve.Line.Width = 2.0F;  //parametrizzare
            curve.Symbol.Size = 6.0F; //parametrizzare
        }

        public override void RemoveChart(string graphLabel, string chartLabel)
        {

            _graphPane.CurveList.Remove(_graphPane.CurveList[chartLabel]);
        }

        public override void AddPoints(string graphLabel, string chartLabel, double[] x, double[] y, int numOfPoints)
        {

            LineItem curve = (LineItem)_graphPane.CurveList[chartLabel];
            for (int i = 0; i < numOfPoints; i++)
            {
                if (curve != null)
                {
                    //curve.Symbol.Fill = new Fill(color); //parametrizzare
                    if (x[i] < xPrev)
                    {
                        historicize(curve, Color.Green);
                        curve.Clear();
                    }
                    curve.AddPoint(x[i], y[i]);
                    if (x[i] < minX || x[i] > maxX || y[i] < minY || y[i] > maxY)
                    {
                        minX = Math.Min(minX, x[i]);
                        maxX = Math.Max(maxX, x[i]);
                        minY = Math.Min(minY, y[i]);
                        maxY = Math.Max(maxY, y[i]);

                        //PIER: DA SCOMMENTARE SE SI VUOLE SCALA VARIABILE
                        //_graphPane.XAxis.Scale.Min = minX;// -Math.Abs(maxX - minX) / 10;
                        //_graphPane.XAxis.Scale.Max = maxX;// +Math.Abs(maxX - minX) / 10;
                        //_graphPane.YAxis.Scale.Min = minY;// -Math.Abs(maxY - minY) / 10;
                        //_graphPane.YAxis.Scale.Max = maxY + Math.Abs(maxY - minY) / 10;
                    }
                    sumY += y[i];
                    countY++;
                    meanY = sumY / countY;
                    updateMean();
                    updateGraph();
                    xPrev = x[i];
                }
            }
        }

        public override bool SetChartVisible(string graphLabel, string chartLabel, bool visible)
        {
            throw new NotImplementedException();
        }

        public override bool GetChartVisible(string graphLabel, string chartLabel)
        {
            throw new NotImplementedException();
        }

        public override void ResetCounters()
        {

            sumY = countY = meanY = 0;
            updateMean();
            updateGraph();
        }

        void updateGraph()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(_graphCtrl))
                return;

            if (disposing || !_graphCtrl.IsHandleCreated)
                return;

            if (_graphCtrl.InvokeRequired && _graphCtrl.IsHandleCreated)
                _graphCtrl.Invoke(new MethodInvoker(updateGraph));
            else
            {
                _graphCtrl.AxisChange();
                _graphCtrl.Refresh();
            }
        }

        void updateMean()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(_graphCtrl))
                return;

            if (disposing || !_graphCtrl.IsHandleCreated)
                return;

            if (_graphCtrl.InvokeRequired && _graphCtrl.IsHandleCreated)
                _graphCtrl.Invoke(new MethodInvoker(updateMean));
            else
            {
                if ((_graphPane.CurveList["meanY"] != null) && (_graphPane.CurveList["meanY"].Points.Count == 2))
                {
                    _graphPane.CurveList["meanY"].Points[0].Y = meanY;
                    _graphPane.CurveList["meanY"].Points[1].Y = meanY;
                }
            }
        }

        void historicize(LineItem curve, Color histCurveColor)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(_graphCtrl))
                return;

            if (_graphCtrl.InvokeRequired && _graphCtrl.IsHandleCreated)
                _graphCtrl.Invoke(new MethodInvoker(() => historicize(curve, histCurveColor)));
            else
            {
                PointPairList list = (PointPairList)curve.Points.Clone();
                string histCurveLabel = curve.Label + "_hist";
                LineItem curveHist = (LineItem)_graphPane.CurveList[histCurveLabel];
                if (curveHist == null)
                {
                    curveHist = _graphPane.AddCurve(histCurveLabel,
                          list, histCurveColor, curve.Symbol.Type);
                }
                else
                {
                    curve.Clear();
                    curveHist.Clear();
                    curveHist.Points = list;
                }
                curveHist.Line.IsVisible = true;   //parametrizzare
                curveHist.Line.Width = 2.0F;  //parametrizzare
                curveHist.Symbol.Size = 6.0F; //parametrizzare
                curveHist.Symbol.IsVisible = false; //parametrizzare
            }
        }

        protected override bool isChartActive(string graphLabel, string chartLabel)
        {

            LineItem curve = (LineItem)_graphPane.CurveList[chartLabel];
            return (curve != null);
        }
    }
}
