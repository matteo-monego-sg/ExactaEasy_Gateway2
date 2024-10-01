using SPAMI.Util.Logger;
using System;
using System.Drawing;
using System.Globalization;
using ZedGraph;

namespace DisplayManager
{
    public abstract class Graph {

        public string Name { get; private set; }
        //public int DataToShowMax { get; set; }
        //public int OldDataToRemove { get; set; }
        public StationCollection SourceStations { get; private set; }

        public Graph(string name, StationCollection sourceStations)
        {

            Name = name;
            SourceStations = sourceStations;
            foreach (IStation station in sourceStations)
            {
                if (station != null)
                {
                    station.HvldDataAvailable += station_HvldDataAvailable;
                    station.MeasuresAvailable += station_MeasuresAvailable;
                }
            }
        }

        public virtual void Dispose() {

            foreach (IStation station in SourceStations) 
            {
                if (station != null) 
                {
                    station.HvldDataAvailable -= station_HvldDataAvailable;
                    station.MeasuresAvailable -= station_MeasuresAvailable;
                }
            }
        }

        //public abstract void Save(string graphLabel, string path);
        public abstract void SetTitles(string graphLabel, string title, string xAxis, string yAxis, double xMin, double xMax, double yMin, double yMax);
        public abstract void SetLimit(string graphLabel, string name, LimitType limitType, double limitValue, double fromPt, double toPt, Color color, System.Drawing.Drawing2D.DashStyle dashStyle);
        public abstract void AddChart(string graphLabel, string chartLabel, Color color, SymbolType symbolType);
        public abstract void RemoveChart(string graphLabel, string chartLabel);
        public abstract void AddPoints(string graphLabel, string chartLabel, double[] x, double[] y, int numOfPoints);
        public abstract bool SetChartVisible(string graphLabel, string chartLabel, bool visible);
        public abstract bool GetChartVisible(string graphLabel, string chartLabel);
        public abstract void ResetCounters();
        protected abstract bool isChartActive(string graphLabel, string chartLabel);

        public virtual void AddPoints(string graphLabel, string chartLabel, string measureValue, int numOfPoints) {

            double[] X, Y;
            if (parseResults(measureValue, out X, out Y, numOfPoints) == true) {
                AddPoints(graphLabel, chartLabel, X, Y, numOfPoints);
            }
            else
                Log.Line(LogLevels.Warning, "Graph.AddPoints", "Error while parsing " + measureValue);
        }

        protected virtual void station_HvldDataAvailable(object sender, HvldDataAvailableEventArgs e)
        { 
        }

        protected virtual void station_MeasuresAvailable(object sender, MeasuresAvailableEventArgs e)
        {
        }

        char[] splitChars = { ';', '\t', '\r', '\n' };

        protected bool parseResults(string measureValue, out double[] X, out double[] Y, int measureCount) {

            //Log.Line(LogLevels.Pass, "Graph.parseResults", "measureValue = " + measureValue);
            X = new double[measureCount];
            Y = new double[measureCount];
            string[] temp = measureValue.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            int idx = 0;
            bool isX = true;
            for (int i = 0; i < temp.Length; i++) {
                if (string.IsNullOrEmpty(temp[i]) == true)
                    continue;

                double value;
                if (double.TryParse(temp[i], NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value)) {
                    if (isX == true)
                        X[idx] = value;
                    else {
                        Y[idx] = value;
                        idx++;
                    }
                }
                else {
                    Log.Line(LogLevels.Warning, "SpectrometerCharts.station_MeasuresAvailable", "Error while parsing " + temp[i] + " to double.");
                }
                isX = !isX;
            }
            return true;
        }
    }
}
