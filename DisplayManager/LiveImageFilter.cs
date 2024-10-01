using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAMI.Util.Logger;

namespace DisplayManager
{
    public class LiveImageFilter
    {
        //const
        Station _station;
        LiveImageFilterMode _mode;
        LiveImageFilterFrequency _frequency;
        Dictionary<LiveImageFilterMode, int> _dicCounter;

        //public prop
        public Station Station
        {
            get => _station;
        }

        public LiveImageFilterMode Mode
        {
            get => _mode;
            set
            {
                if (_mode == value)
                    return;

                _mode = value;
                Log.Line(LogLevels.Pass, $"LiveImageFilter.set_Mode", $"Mode enum changed to '{value}'({(int)value}) at station '{_station.Description}'");
            }
        }

        public LiveImageFilterFrequency Frequency
        {
            get => _frequency;
            set
            {
                if (_frequency == value)
                    return;

                _frequency = value;
                Log.Line(LogLevels.Pass, $"LiveImageFilter.set_Frequency", $"Frequency enum changed to '{value}'({(int)value}) at station '{_station.Description}'");
            }
        }

        public int FrequencyInt
        {
            get => (int)_frequency;
        }

        public int Counter
        {
            get => _dicCounter[_mode];
        }

        
        public LiveImageFilter(Station st)
        {
            if (st == null)
                throw new ArgumentException($"st cannot be null");

            _station = st;
            _mode = LiveImageFilterMode.All; //defualt
            _frequency = LiveImageFilterFrequency.One; //defualt
            _dicCounter = new Dictionary<LiveImageFilterMode, int>();

            foreach (LiveImageFilterMode mode in Enum.GetValues(typeof(LiveImageFilterMode)))
                _dicCounter.Add(mode, 0);
        }


        //methods
        public int IncrementCounter()
        {
            _dicCounter[_mode]++;
            return _dicCounter[_mode];
        }

        public void ResetCounter()
        {
            _dicCounter[_mode] = 0;
            Log.Line(LogLevels.Debug, $"LiveImageFilter.ResetCounter", $"Resetting of counter enum '{_mode}'({(int)_mode}) at station '{_station.Description}'");
        }
    }




    public enum LiveImageFilterMode
    {
        All = 1,
        Good = 15,
        Reject = 20,
        Light = 21,
        TechReject = 30,
    }


    public enum LiveImageFilterFrequency : int
    {
        One = 1,
        Two = 2,
        Five = 5,
        Ten = 10,
        Twenty = 20,
        Fifty = 50,
    }
}
