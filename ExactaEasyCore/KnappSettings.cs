using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ExactaEasyCore {
    public class KnappSettings {

        public bool DisplayKnappPage { get; set; }
        public int SurveyBase { get; set; }
        public int VialOffset { get; set; }
        public string KnappFolder { get; set; }
        public bool OfflineKnapp { get; set; }
        public int MachineOffset { get; set; }
        public int SpindleIncrement { get; set; }
        //public int Inspections4vialNum { get; set; }
        public double RatioHighThreshold { get; set; }
        public int RoundsNum { get; set; }
        public int VialsToIgnoreAtStartup { get; set; }
        public bool MarkBrokenVials { get; set; }
        public BindingList<KnappStationSettings> KnappStationsSettings { get; set; }

        [XmlIgnore]
        public string KnappLabel { get; set; }
        [XmlIgnore]
        public int NumberOfSpindles { get; set; }
    }

    public class KnappStationSettings {

        public int IdNode { get; set; }
        public int IdStation { get; set; }
        public bool EnableKnapp { get; set; }
        public int StationSpindleInitialPosition { get; set; }
        public int StationVialsToIgnore { get; set; }
        protected int divisor;
        public int Divisor {
            get {
                return Math.Max(1, divisor);
            }
            set {
                divisor = value;
            }
        }

        [XmlIgnore]
        public long OfflineSpindleCurrentStationIncrement { get; set; }
        [XmlIgnore]
        public int StationVialsToIgnoreRemained { get; set; }
    }
}
