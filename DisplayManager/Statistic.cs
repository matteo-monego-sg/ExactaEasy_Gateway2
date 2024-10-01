using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace DisplayManager {

    public class Statistic<T> where T : IResultId {

        internal T Key;
        internal double currentValue;
        internal double samplesNum;
        internal double prevMean;
        internal double prevStdDev;
        internal double currentMean;
        internal double currentStdDev;
        internal double rejectedNum;
        internal string name;
        internal string um;
        internal bool used;

        public int ID {
            get {
                var stringKey = "";
                foreach (int k in Key.IDs) {
                    stringKey += k.ToString();
                }
                int id;
                int.TryParse(stringKey, out id);
                return id;
            }
        }

        public string IdDesc {
            get {
                string label = Key.Description;
                //label.Substring(0, label.LastIndexOf(" - "));
                return label;
                //string _id = "";
                //for (int ik = 0; ik < key.IDs.Count; ik++) {
                //    if (ik > 0) _id += " - ";
                //    if (key.IDLabels.Count > ik && key.IDLabels[ik].Length > 0)
                //        _id += key.IDLabels[ik].ToString();
                //    else
                //        _id += key.IDs[ik].ToString();
                //}
                //return _id;
            }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string Used {
            get {
                if (used)
                    return VisionSystemManager.UIStrings.GetString("Yes");
                return VisionSystemManager.UIStrings.GetString("No");
            }
        }

        public string CurrentValue {
            get { return currentValue.ToString("f4"); }
        }

        public string UM {
            get { return um; }
            set { um = value; }
        }

        public string CurrentMean {
            get { return currentMean.ToString("f4"); }
        }

        public string CurrentStdDev {
            get { return currentStdDev.ToString("f4"); }
        }

        public string SamplesNum {
            get { return samplesNum.ToString(CultureInfo.InvariantCulture); }
        }

        public string RejectPercent {
            get {
                if (samplesNum == 0) return 0.ToString("f2", CultureInfo.InvariantCulture);
                return (rejectedNum / samplesNum * 100F).ToString("f2", CultureInfo.InvariantCulture);
            }
        }

        public Statistic(T key, double _currentValue, bool rejected, string _name, string _um, bool _used) {

            Key = key;
            currentMean = currentValue = _currentValue;
            samplesNum = 1F;
            prevMean = 0F;
            currentStdDev = prevStdDev = 0F;
            rejectedNum = (rejected) ? 1F : 0F;
            name = _name;
            um = _um;
            used = _used;
        }
    }

    public class StatisticCollection<T> : List<Statistic<T>> where T : IResultId {

        public Statistic<T> this[T item] {
            get {
                return Find(statistic => (statistic.Key.Equals(item)));
            }
        }

        public StatisticCollection<T> Filter(T filterKey) {
            return (StatisticCollection<T>)this.Where(m => m.Key.Equals(filterKey));
        }

        //internal bool NewStatistic;
        object syncObj = new object();
        public void Update(T key, MeasureResults measureRes) {

            double value;
            switch (measureRes.MeasureType) {
                case MeasureTypeEnum.BOOL:
                    bool boolValue;
                    if (!Boolean.TryParse(measureRes.MeasureValue, out boolValue)) {
                        Log.Line(LogLevels.Error, "MeasureStatisticCollection.Update", "Error while casting value " + measureRes.MeasureValue + " from string to bool");
                        throw new Exception("Error while casting value " + measureRes.MeasureValue + " from string to bool");
                    }
                    value = boolValue ? 1 : 0;
                    break;
                case MeasureTypeEnum.STRING:
                    //se la misura è una stringa non ha senso farci della statistica
                    //Log.Line(LogLevels.Debug, "MeasureStatisticCollection.Update", "No statistic if value is a string");
                    return;
                case MeasureTypeEnum.INT:
                    int intValue;
                    if (!Int32.TryParse(measureRes.MeasureValue, out intValue)) {
                        Log.Line(LogLevels.Error, "MeasureStatisticCollection.Update", "Error while casting value " + measureRes.MeasureValue + " from string to int");
                        throw new Exception("Error while casting value " + measureRes.MeasureValue + " from string to int");
                    }
                    value = intValue;
                    break;
                case MeasureTypeEnum.DOUBLE:
                    double doubleValue;
                    if (!Double.TryParse(measureRes.MeasureValue, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleValue)) {
                        Log.Line(LogLevels.Error, "MeasureStatisticCollection.Update", "Error while casting value " + measureRes.MeasureValue + " from string to double");
                        throw new Exception("Error while casting value " + measureRes.MeasureValue + " from string to double");
                    }
                    value = doubleValue;
                    break;
                default:
                    throw new Exception("Data type not supported!!!");
            }
            lock (syncObj) {
                Statistic<T> currentStatistic = this[key];
                if (currentStatistic == null) {
                    //n==1
                    Add(new Statistic<T>(key, value, !measureRes.IsOk, measureRes.MeasureName, measureRes.MeasureUnit, measureRes.IsUsed));
                    //NewStatistic = true;
                }
                else {
                    //n>=2
                    currentStatistic.prevMean = currentStatistic.currentMean;
                    currentStatistic.prevStdDev = currentStatistic.currentStdDev;
                    currentStatistic.currentValue = value;
                    currentStatistic.samplesNum++;
                    currentStatistic.currentMean = currentStatistic.prevMean +
                        ((currentStatistic.currentValue - currentStatistic.prevMean) / currentStatistic.samplesNum);
                    currentStatistic.currentStdDev = currentStatistic.prevStdDev +
                        Math.Pow(currentStatistic.prevMean, 2.0F) -
                        Math.Pow(currentStatistic.currentMean, 2.0F) +
                        ((Math.Pow(currentStatistic.currentValue, 2.0F) - currentStatistic.prevStdDev - Math.Pow(currentStatistic.prevMean, 2.0F)) / currentStatistic.samplesNum);
                    if (!measureRes.IsOk) {
                        currentStatistic.rejectedNum++;
                    }
                    //TODO: aggiungere statistica per scartate da questa misura
                    //TODO: aggiungere statistica per buone relativamente a questa misura
                }
            }
        }
    }
}
