using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace DisplayManager {

    public class KnappManager {

        public event EventHandler<VialSurveyEventArgs> KnappNewData;
        public event EventHandler KnappStopped;
        public bool InKnappMode { get; protected set; }
        public KnappSettings KnappSettings { get; private set; }
        public Survey Survey { get; private set; }
        readonly StationCollection _stations;
        int _resultsToIgnore;
        readonly object _surveyLock = new object();

        public int StationsEnabledForKnappNum {
            get {
                int numStationsEnabledForKnapp = 0;
                foreach (IStation station in _stations) {
                    var stationKnappSettings = KnappSettings.KnappStationsSettings.SingleOrDefault(kss => kss.IdStation == station.IdStation && kss.IdNode == station.NodeId);
                    if (stationKnappSettings != null) {
                        if (stationKnappSettings.EnableKnapp) {
                            numStationsEnabledForKnapp++;
                        }
                    }
                }
                return numStationsEnabledForKnapp;
                //return _stations.Select(station => KnappSettings.KnappStationsSettings.SingleOrDefault(kss => kss.IdStation == station.IdStation && kss.IdNode == station.NodeId)).Where(stationKnappSettings => stationKnappSettings != null).Count(stationKnappSettings => stationKnappSettings.EnableKnapp);
            }
        }

        public KnappManager(KnappSettings knappSettings, StationCollection stations) {

            KnappSettings = knappSettings;
            _stations = stations;
        }

        public void StartKnapp() {

            uint initialVialId = Convert.ToUInt32(KnappSettings.VialOffset) + 1;
            try {
                ResetKnapp(initialVialId, KnappSettings.NumberOfSpindles);
                Survey.LowVialId = initialVialId;
                Survey.HighVialId = initialVialId + Convert.ToUInt32(KnappSettings.NumberOfSpindles) - 1;

                _resultsToIgnore = KnappSettings.VialsToIgnoreAtStartup * StationsEnabledForKnappNum;
                Log.Line(LogLevels.Debug, "KnappManager.StartKnapp", "Fiale (risultati) da ignorare: {0}({1})", KnappSettings.VialsToIgnoreAtStartup, _resultsToIgnore);
                foreach (IStation station in _stations) {
                    KnappStationSettings stationKnappSettings = KnappSettings.KnappStationsSettings.SingleOrDefault(kss => kss.IdStation == station.IdStation && kss.IdNode == station.NodeId);
                    stationKnappSettings.StationVialsToIgnoreRemained = stationKnappSettings.StationVialsToIgnore;
                    if (stationKnappSettings != null) {
                        if (stationKnappSettings.EnableKnapp)
                            station.MeasuresAvailable += station_MeasuresAvailable;
                    }
                    else throw new Exception("Settings unavailable for this station: id = " + station.IdStation.ToString(CultureInfo.InvariantCulture));
                }
                foreach (KnappStationSettings kss in KnappSettings.KnappStationsSettings) {
                    kss.OfflineSpindleCurrentStationIncrement = 0;
                }
                InKnappMode = true;
            }
            catch (Exception ex) {
                InKnappMode = false;
                Log.Line(LogLevels.Error, "KnappManager.StartKnapp", ex.Message);
            }
        }

        public void StopKnapp() {

            foreach (IStation station in _stations) {
                station.MeasuresAvailable -= station_MeasuresAvailable;
            }
            InKnappMode = false;
            OnKnappStopped(this, EventArgs.Empty);
        }

        protected void OnKnappStopped(object sender, EventArgs e) {

            if (KnappStopped != null) KnappStopped(sender, e);
        }

        public void ResetKnapp(uint startId, int count) {

            if (InKnappMode)
                StopKnapp();
            if (count < 0)
                Survey = null;
            if (Survey == null) {
                Survey = new Survey(KnappSettings);
            }
            for (uint i = startId; i < startId + count; i++) {
                Survey.ResetVialSurvey(i);
            }
        }

        void station_MeasuresAvailable(object sender, MeasuresAvailableEventArgs e) {

            lock (_surveyLock) {
                try {
                    int currStatId = ((IStation)sender).IdStation;
                    int currNodeId = ((IStation)sender).NodeId;
                    KnappStationSettings stationKnappSettings = KnappSettings.KnappStationsSettings.SingleOrDefault(kss => kss.IdStation == currStatId && kss.IdNode == currNodeId);
                    if (stationKnappSettings == null) {
                        string errString = "Settings unavailable for this station. Node: " + currNodeId.ToString(CultureInfo.InvariantCulture) + " station: " + currStatId.ToString(CultureInfo.InvariantCulture);
                        Log.Line(LogLevels.Error, "KnappManager.station_MeasuresAvailable", errString);
                        throw new Exception(errString);
                    }
                    //if (!stationKnappSettings.EnableKnapp)
                    //    return;
                    if (Survey != null) {
                        // incremento lo spindle ID dell'offset impostato esternamente (es: set di fiale da 81 a 120 => VialOffset = 80)
                        //long spindleId = e.InspectionResults.SpindleId;
                        if (KnappSettings.OfflineKnapp) {
                            long spindleId = 0;// Math.Max(0, e.InspectionResults.SpindleId - 1); //spindle id in range [0, NumberOfSpindles-1]
                            // nel caso non abbia il corretto spindleId dalla visione (che a sua volta non lo sta ricevendo da PLC) allora
                            // ad ogni nuovo risultato 
                            // 1) aggiungo l'offset uguale per tutte le stazioni e quello per singola stazione
                            spindleId += KnappSettings.MachineOffset;
                            spindleId += (stationKnappSettings.StationSpindleInitialPosition - 1);
                            spindleId += stationKnappSettings.OfflineSpindleCurrentStationIncrement;

                            //OLD
                            //spindleId += KnappSettings.MachineOffset - 1;
                            //spindleId += stationKnappSettings.StationSpindleOffset;
                            //spindleId += stationKnappSettings.OfflineSpindleCurrentStationIncrement;

                            // dopo gli incrementi normalizzo lo spindle id al numero di spindle 
                            if (KnappSettings.NumberOfSpindles > 0) {
                                spindleId = (spindleId < 0) ?
                                    KnappSettings.NumberOfSpindles + spindleId :
                                    spindleId % KnappSettings.NumberOfSpindles;
                            }
                            e.InspectionResults.SpindleId = (uint)spindleId + 1;     //spindle id in range [1, NumberOfSpindles]
                            Log.Line(LogLevels.Debug, "KnappManager.station_MeasuresAvailable", "NODE {0} STATION {1} - Spindle = {2}", currNodeId, currStatId, e.InspectionResults.SpindleId);
                        }
                        e.InspectionResults.VialId = Convert.ToUInt32(KnappSettings.VialOffset) + Convert.ToUInt32(e.InspectionResults.SpindleId);

                        _resultsToIgnore--;
                        if (_resultsToIgnore < 0) {
                            if (Survey.NewSample(currNodeId, currStatId, e.InspectionResults)) {
                                // KNAPP FINITO
                                Log.Line(LogLevels.Pass, "KnappManager.station_MeasuresAvailable", "NODE {0} STATION {1} - Knapp completed successfully after {2} rounds", currNodeId, currStatId, KnappSettings.RoundsNum);
                                StopKnapp();
                            }
                        }
                        else
                            Log.Line(LogLevels.Debug, "KnappManager.station_MeasuresAvailable", "NODE {0} STATION {1} - Risultati da ignorare: {2}", currNodeId, currStatId, _resultsToIgnore);

                        if (KnappSettings.OfflineKnapp) {
                            // infine incremento l'offset della stazione corrente
                            stationKnappSettings.OfflineSpindleCurrentStationIncrement += KnappSettings.SpindleIncrement;
                        }
                    }
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "KnappManager.station_MeasuresAvailable", ex.Message);
                }
                if (_resultsToIgnore < 0) {
                    try {
                        if (Survey != null)
                            OnVialKnappNewData(this, new VialSurveyEventArgs(Survey[e.InspectionResults.VialId]));
                    }
                    catch {
                        Log.Line(LogLevels.Debug, "", "");
                    }
                }
            }
        }

        void OnVialKnappNewData(object sender, VialSurveyEventArgs e) {

            if (KnappNewData != null) KnappNewData(sender, e);
        }

        public void LoadSurvey(string filepath) {

            //int lowVialId = 0, highVialId = 0;
            if (Survey == null) {
                Survey = new Survey(KnappSettings);
            }
            //if (Survey != null) {
            //    lowVialId = Survey.LowVialId;
            //    highVialId = Survey.HighVialId;
            //}
            //Survey = new Survey(KnappSettings);
            //Survey.LowVialId = lowVialId;
            //Survey.HighVialId = highVialId;
            using (var file = new StreamReader(filepath)) {
                string line;
                while ((line = file.ReadLine()) != null) {
                    var values = line.Split(new[] { ";", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    uint id;
                    if (values.Length <= 0 || !uint.TryParse(values[0], out id)) continue;
                    if (Survey[id] == null)
                        Survey.Add(new VialSurvey(id, KnappSettings.SurveyBase));
                    double customerSurvey;
                    if (values.Length > 1 && double.TryParse(values[1], out customerSurvey))
                        Survey[id].CustomerEvaluation = customerSurvey;
                    double machineSurvey;
                    if (values.Length > 2 && double.TryParse(values[2], out machineSurvey))
                        Survey[id].MachineEvaluation = machineSurvey;
                    long rejected;
                    if (values.Length > 3 && long.TryParse(values[3], out rejected))
                        Survey[id].Rejected = rejected;
                    long samples;
                    if (values.Length > 4 && long.TryParse(values[4], out samples))
                        Survey[id].Samples = samples;
                    //pier: TODO --> AGGIUNGERE VALUTAZIONI SINGOLE STAZIONI
                    //if (values.Length > 5) {
                    //    Survey[id].StationsEvaluation = new StationsEvaluationCollection();
                    //    for (int iStat = 0; iStat < values.Length - 5; iStat++) {
                    //        long rej;
                    //        if (long.TryParse(values[5 + iStat], out rej) && rej > -1) {
                    //            Survey[id].StationsEvaluation.StationsEvaluations.Add(new StationEvaluation(iStat, KnappSettings.SurveyBase));
                    //            Survey[id].StationsEvaluation[iStat].Rejected = rej;
                    //            Survey[id].StationsEvaluation[iStat].Samples = samples;
                    //        }
                    //    }
                    //}
                }
            }
            if (Survey != null)
                Survey.UpdateRatios();
        }

        public void SaveSurvey(string filepath, string separator, bool complete) {
            if (Survey == null) return;
            try {
                using (var file = new StreamWriter(filepath)) {
                    if (complete) WriteSurveyHeader(file, separator);
                    foreach (var vs in Survey.VialSurveyCollection) {
                        var line = vs.VialId + separator;
                        line += vs.CustomerEvaluation.ToString(CultureInfo.InvariantCulture) + separator;
                        if (!complete) {
                            file.WriteLine(line);
                            continue;
                        }
                        line += vs.MachineEvaluation.ToString(CultureInfo.InvariantCulture) + separator;
                        line += vs.Rejected.ToString(CultureInfo.InvariantCulture) + separator;
                        line += vs.Samples.ToString(CultureInfo.InvariantCulture) + separator;
                        //pier: TODO --> AGGIUNGERE VALUTAZIONI SINGOLE STAZIONI
                        //if (vs.StationsEvaluation != null) {
                        //    List<StationEvaluation> orderedList = vs.StationsEvaluation.StationsEvaluations.OrderBy(o => o.Id).ToList();
                        //    int currId = 0;
                        //    foreach (StationEvaluation statEval in orderedList) {
                        //        while (currId < statEval.Id) {
                        //            line += "-1" + separator;
                        //            currId++;
                        //        }
                        //        line += statEval.Rejected.ToString() + separator;
                        //    }
                        //}
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "KnappManager.SaveSurvey", "Error while saving knapp. Error: " + ex.Message);
            }
        }

        static void WriteSurveyHeader(StreamWriter file, string separator) {

            if (file == null) return;
            var line = "Vial ID" + separator +
                "Customer Evaluation" + separator +
                "Machine Evaluation" + separator +
                "Rejected" + separator +
                "Samples" + separator +
                "Single Stations Rejections" + separator;
            file.WriteLine(line);
        }
    }

    public class SurveyContainer {

        public Survey Survey { get; set; }
    }

    public class Survey {

        public VialSurvey this[uint id] {
            get {
                return VialSurveyCollection.Find(vs => vs.VialId == id);
            }
            set {
                var currVialSurvey = VialSurveyCollection.Find(vs => vs.VialId == id);
                if (currVialSurvey == null)
                    Add(value);
                //else
                //    currVialSurvey = value;
            }
        }
        int _base;
        public int Base {
            get {
                return _base;
            }
            set {
                _base = value;
                foreach (var vs in VialSurveyCollection) {
                    vs.Base = _base;
                }
            }
        }

        readonly KnappSettings _knappSettings;
        public List<VialSurvey> VialSurveyCollection { get; set; }
        //int roundsNum;
        //public int RoundsNum {
        //    get {
        //        if (_knappSettings != null) roundsNum = _knappSettings.RoundsNum;
        //        else roundsNum = - 1;
        //        return roundsNum;
        //    }
        //    set {
        //    }
        //}
        //public int Inspections4vialNum { get; protected set; }
        //public int NumberOfSpindles { get; protected set; }
        public double Machine2HumanRatio { get; set; }
        public double Human2MachineRatio { get; set; }
        public KnappHistogram KnappHistogram { get; set; }

        [XmlIgnore]
        public uint LowVialId { get; set; }
        [XmlIgnore]
        public uint HighVialId { get; set; }
        //public Survey(int _base) {

        //    Base = _base;
        //}

        public Survey() {

            VialSurveyCollection = new List<VialSurvey>();
            KnappHistogram = new KnappHistogram();
        }

        public Survey(KnappSettings knappSettings)
            : this() {

            _knappSettings = knappSettings;
            Base = _knappSettings.SurveyBase;
        }

        public void Add(VialSurvey vs) {

            VialSurveyCollection.Add(vs);
            //this = this.OrderBy(o => o.VialId).ToList();
            VialSurveyCollection.Sort((a, b) => a.VialId.CompareTo(b.VialId));
        }

        public override string ToString() {
            var xmlSer = new XmlSerializer(typeof(Survey));
            var writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            var xmlStr = writer.ToString();
            writer.Close();
            return xmlStr;
        }

        public void SaveXml(string filePath) {

            var writer = new StreamWriter(filePath);
            //XmlAttributeOverrides rules = new XmlAttributeOverrides();
            //if (this.Nodes != null && this.Nodes.Count > 0) {
            //    rules.Add(typeof(Recipe), "Nodes", new XmlAttributes() { XmlIgnore = false });
            //    rules.Add(typeof(Recipe), "Cams", new XmlAttributes() { XmlIgnore = true });
            //    rules.Add(typeof(Parameter), "Id", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute("Id") });
            //    rules.Add(typeof(Parameter), "IsVisible", new XmlAttributes() { XmlIgnore = true });
            //    rules.Add(typeof(Parameter), "IsEditable", new XmlAttributes() { XmlIgnore = true });
            //    rules.Add(typeof(Parameter), "Label", new XmlAttributes() { XmlIgnore = true });
            //    rules.Add(typeof(Parameter), "Value", new XmlAttributes() { XmlText = new XmlTextAttribute(typeof(string)) });
            //    rules.Add(typeof(Parameter), "Group", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Group") });
            //    rules.Add(typeof(Parameter), "ParamId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("ParamId") });
            //}
            var xmlSer = new XmlSerializer(typeof(Survey));//, rules);
            xmlSer.Serialize(writer, this);
            writer.Close();
        }

        public void ResetVialSurvey(uint vialId) {
            VialSurvey vialSurvey = this[vialId];
            if (vialSurvey == null) {
                vialSurvey = new VialSurvey(vialId, Base);
                //this.Add(vialSurvey);
                this[vialSurvey.VialId] = vialSurvey;
            }
            vialSurvey.Samples = vialSurvey.Rejected = 0;
            foreach (StationEvaluation se in vialSurvey.StationsEvaluation.StationsEvaluations) {
                se.VialRejection.Clear();
                se.Rejected = se.Samples = 0;
            }
        }

        public bool NewSample(int nodeId, int stationId, InspectionResults inspRes) {

            VialSurvey vialSurvey = this[inspRes.VialId];
            if (vialSurvey == null) {
                vialSurvey = new VialSurvey(inspRes.VialId, Base); 
                //this.Add(vialSurvey);
                this[vialSurvey.VialId] = vialSurvey;
            }
            if (_knappSettings.RoundsNum > 0 &&
                vialSurvey.Samples >= _knappSettings.RoundsNum) {
                return KnappCompleted();
            }
            //StationEvaluation currStatEvaluation = 
            bool vialInspCompleted;
            bool vialInspRejected;
            Log.Line(LogLevels.Debug, "VIAL " + inspRes.VialId.ToString(CultureInfo.InvariantCulture), "");
            KnappStationSettings stationKnappSettings = _knappSettings.KnappStationsSettings.SingleOrDefault(kss => kss.IdStation == stationId && kss.IdNode == nodeId);
            vialSurvey.StationsEvaluation.NewSample(stationKnappSettings, CalcNumObservationExpected(inspRes.VialId), inspRes, Base, out vialInspCompleted, out vialInspRejected);
            if (vialInspCompleted) {
                vialSurvey.Samples++;
                if (vialInspRejected) vialSurvey.Rejected++;
                UpdateRatios();
            }
            return KnappCompleted();
        }

        int CalcNumObservationExpected(uint vialId) {
            //return _knappSettings.Inspections4vialNum;
            return _knappSettings.KnappStationsSettings.Count(kss => kss.EnableKnapp && ((vialId + kss.StationSpindleInitialPosition) % _knappSettings.SpindleIncrement == 0));
        }

        bool KnappCompleted() {

            var knappCompleted = true;
            if (_knappSettings.RoundsNum > 0) {
                for (var i = LowVialId; i <= HighVialId; i++) {
                    if (this[i].Samples >= _knappSettings.RoundsNum) continue;
                    knappCompleted = false;
                    break;
                }
            }
            else
                knappCompleted = false;
            return knappCompleted;
        }

        public void UpdateRatios() {

            double machineRejOnHumanRej = 0;    //fiale scartate da macchina con valutazione umana > Machine2HumanHighThPerCent
            double humanRejOnHumanRej = 0;      //fiale scartate da uomo con valutazione umana > Machine2HumanHighThPerCent
            double humanRejOnMachineRej = 0;    //fiale scartate da macchina con valutazione umana > Machine2HumanHighThPerCent
            double machineRejOnMachineRej = 0;  //fiale scartate da uomo con valutazione umana > Machine2HumanHighThPerCent
            foreach (VialSurvey vs in VialSurveyCollection) {
                if (vs.CustomerEvaluation < 0 || vs.MachineEvaluation < 0)
                    continue;
                if (vs.CustomerEvaluation * 100F / Base >= _knappSettings.RatioHighThreshold) {
                    humanRejOnHumanRej += vs.CustomerEvaluation;
                    //if (vs.MachineEvaluation * 100F / Base >= _knappSettings.RatioHighThreshold)
                    machineRejOnHumanRej += vs.MachineEvaluation;
                }
                if (vs.MachineEvaluation * 100F / Base >= _knappSettings.RatioHighThreshold) {
                    machineRejOnMachineRej += vs.MachineEvaluation;
                    //if (vs.CustomerEvaluation * 100F / Base >= _knappSettings.RatioHighThreshold)
                    humanRejOnMachineRej += vs.CustomerEvaluation;
                }
            }
            Machine2HumanRatio = (humanRejOnHumanRej > 0) ? machineRejOnHumanRej / humanRejOnHumanRej * 100F : -1F;
            Human2MachineRatio = (machineRejOnMachineRej > 0) ? humanRejOnMachineRej / machineRejOnMachineRej * 100F : -1F;
        }
    }

    public class VialSurvey {

        public uint VialId { get; set; }
        public double CustomerEvaluation { get; set; }
        public long Rejected { get; set; }
        public long Samples { get; set; }
        double _evaluationPerCent;
        public double EvaluationPerCent {
            get {
                if (Samples == 0) _evaluationPerCent = -1F;
                _evaluationPerCent = Rejected / (double)Samples * 100F;
                return _evaluationPerCent;
            }
        }
        double _machineEvaluation;
        public double MachineEvaluation {
            get {
                if (Samples == 0) _machineEvaluation = -1F;
                else _machineEvaluation = Rejected / (double)Samples * Base;
                return _machineEvaluation;
            }
            set {
                _machineEvaluation = value;
            }
        }

        int _base;
        public int Base {
            get {
                return _base;
            }
            set {
                _base = value;
                foreach (var se in StationsEvaluation.StationsEvaluations) {
                    se.Base = _base;
                }
            }
        }
        public StationsEvaluationCollection StationsEvaluation { get; set; }
        //public int Inspections4vialNum { get; protected set; }

        public VialSurvey() {

            StationsEvaluation = new StationsEvaluationCollection();
            CustomerEvaluation = -1F;
        }

        public VialSurvey(uint vialId, int _base)
            : this() {

            VialId = vialId;
            Base = _base;
            //Inspections4vialNum = inspections4vialNum;
        }
    }

    public class VialSurveyEventArgs : EventArgs {

        public VialSurvey VialSurvey { get; protected set; }

        public VialSurveyEventArgs(VialSurvey vialSurvey) {

            VialSurvey = vialSurvey;
        }
    }

    public class StationsEvaluationCollection {

        public List<StationEvaluation> StationsEvaluations { get; set; }

        public StationsEvaluationCollection() {

            StationsEvaluations = new List<StationEvaluation>();
        }

        public StationEvaluation GetStationEvaluation(int idNode, int idStation) {

            return StationsEvaluations.Find(se => se.IdNode == idNode && se.IdStation == idStation);
        }

        //public new StationEvaluation this[int id] {
        //    get {
        //        return StationsEvaluations.Find(se => se.Id == id);
        //    }
        //}

        public void NewSample(KnappStationSettings kss, int numObservationsExpected, InspectionResults inspRes, int _base, out bool completed, out bool rejected) {

            //ora ci sono le fiale da ignorare per singola stazione
            kss.StationVialsToIgnoreRemained--;
            if (kss.StationVialsToIgnoreRemained >= 0) {
                Log.Line(LogLevels.Debug, "StationsEvaluationCollection.NewSample", "NODE {0} STATION {1} - Risultati da ignorare per singola stazione: {2}", kss.IdNode, kss.IdStation, kss.StationVialsToIgnoreRemained);
                completed = false;
                rejected = false;
                return;
            }
            StationEvaluation statEvaluation = GetStationEvaluation(kss.IdNode, kss.IdStation);
            if (statEvaluation == null) {
                statEvaluation = new StationEvaluation(kss.IdNode, kss.IdStation, _base);
                StationsEvaluations.Add(statEvaluation);
            }
            statEvaluation.Samples++;
            if (inspRes.IsReject)
                statEvaluation.Rejected++;
            statEvaluation.VialRejection.Add(statEvaluation.Samples, inspRes.IsReject);
            //Log.Line(LogLevels.Error, "10", "{0}: ADDED KEY = " + statEvaluation.Samples.ToString(), stationId);
            //return statEvaluation;
            completed = !(StationsEvaluations.Count < numObservationsExpected);
            rejected = false;
            foreach (StationEvaluation statEval in StationsEvaluations) {
                if (statEval.VialRejection.ContainsKey(statEvaluation.Samples)) {
                    rejected |= statEval.VialRejection[statEvaluation.Samples];
                    //break;
                }
                else
                    completed = false;
            }
            //foreach (StationEvaluation statEval in this) {
            //    if (!rejected) {
            //        if (statEval.VialRejection.ContainsKey(statEvaluation.Samples)) {
            //            Log.Line(LogLevels.Error, "0A", "{0}: " + statEval.VialRejection[statEvaluation.Samples].ToString() + "\n", statEval.Id);
            //        }
            //        else {
            //            Log.Line(LogLevels.Error, "0B", "{0}: ", statEval.Id);
            //        }
            //        Log.Line(LogLevels.Error, "1", "{0}: VIAL REJS COUNT = " + statEval.VialRejection.Count.ToString(), statEval.Id);
            //    }
            //}
            if (completed) {
                foreach (StationEvaluation statEval in StationsEvaluations) {
                    statEval.VialRejection.Remove(statEvaluation.Samples);
                    //Log.Line(LogLevels.Error, "11", "{0}: REMOVED KEY = " + statEvaluation.Samples.ToString(), statEval.Id);
                }
            }


            //return statEvaluation.Samples;
        }
    }

    public class StationEvaluation {

        public int IdNode { get; set; }
        public int IdStation { get; set; }
        public long Rejected { get; set; }
        public long Samples { get; set; }
        public double EvaluationPerCent {
            get {
                if (Samples == 0 || Base == 0) return Evaluation;
                return (Evaluation / Base * 100F);
            }
        }
        double _evaluation;
        public double Evaluation {
            get {
                if (Samples == 0) _evaluation = -1F;
                else _evaluation = Rejected / (double)Samples * Base;
                return _evaluation;
            }
        }
        public int Base { get; set; }
        [XmlIgnore]
        public Dictionary<long, bool> VialRejection { get; set; }

        public StationEvaluation() {

            VialRejection = new Dictionary<long, bool>();
        }

        public StationEvaluation(int idNode, int idStat, int _base)
            : this() {

            IdNode = idNode;
            IdStation = idStat;
            Base = _base;
        }
    }

    public class KnappHistogram {

        public SerializableSortedDictionary<int, int> Fqv { get; set; }   //istogramma knapp manuale
        public SerializableSortedDictionary<int, int> Fqa { get; set; }   //istogramma knapp macchina

        public void CreateHistogram(Survey survey) {

            Fqv = new SerializableSortedDictionary<int, int>();  //istogramma knapp manuale
            Fqa = new SerializableSortedDictionary<int, int>();  //istogramma knapp macchina
            for (var i = 0; i <= survey.Base; i++) {
                Fqv.Add(i, 0);
                Fqa.Add(i, 0);
            }

            // se tutte le valutazioni manuali sono assenti creo solo l'istogramma macchina
            var createBothHist = survey.VialSurveyCollection.Any(vs => vs.CustomerEvaluation > -1);

            foreach (var vs in survey.VialSurveyCollection) {
                var customerEvaluation = (int)Math.Round(vs.CustomerEvaluation);
                var machineEvaluation = (int)Math.Round(vs.MachineEvaluation);

                if (customerEvaluation > -1) {
                    if (createBothHist && machineEvaluation > -1) {
                        if (!Fqv.ContainsKey(customerEvaluation))
                            Fqv.Add(customerEvaluation, 0);
                        Fqv[customerEvaluation]++;
                    }
                }
                if (customerEvaluation > -1 || !createBothHist) {
                    if (machineEvaluation > -1) {
                        if (!Fqa.ContainsKey(machineEvaluation))
                            Fqa.Add(machineEvaluation, 0);
                        Fqa[machineEvaluation]++;
                    }
                }
            }
        }

        public override string ToString() {
            var xns = new XmlSerializerNamespaces();
            xns.Add(String.Empty, String.Empty);
            var xmlSer = new XmlSerializer(typeof(KnappHistogram));
            var writer = new StringWriter();
            xmlSer.Serialize(writer, this, xns);
            var xmlStr = writer.ToString();
            writer.Close();
            return xmlStr;
        }

        public void SaveXml(string filePath) {

            var writer = new StreamWriter(filePath);
            var xmlSer = new XmlSerializer(typeof(KnappHistogram));
            xmlSer.Serialize(writer, this);
            writer.Close();
        }
    }

    [XmlRoot("dictionary")]
    public class SerializableSortedDictionary<TKey, TValue>
        : SortedDictionary<TKey, TValue>, IXmlSerializable {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader) {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
                reader.ReadStartElement("Item");
                reader.ReadStartElement("Key");
                var key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("Value");
                var value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (var key in Keys) {
                writer.WriteStartElement("Item");

                writer.WriteStartElement("Key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("Value");
                var value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
