using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExactaEasyEng.AppDebug
{
    public class DebugSizeImagesResults
    {
        bool _isReset;

        int _nodeId;
        string _nodeName;
        int _stationId;
        string _stationName;
        int _counter;
        long _min;
        long _max;
        long _sum;
        long _average;


        //public prop
        public int NodeId => _nodeId;
        public string NodeName => _nodeName;
        public int StationId => _stationId;
        public string StationName => _stationName;
        public int Counter => _counter;
        public long Min => _min;
        public long Max => _max;
        public long Sum => _sum;
        public long Average => _average;


        public DebugSizeImagesResults(int nodeId, string nodeName, int stId, string stName)
        {
            _isReset = true;

            _nodeId = nodeId;
            _nodeName = nodeName;
            _stationId = stId;
            _stationName = stName;

            _counter = 0;
            _min = 0;
            _max = 0;
            _average = 0;
            _sum = 0;
        }


        public void AddSize(long size)
        {
            if(_isReset)
            {
                _min = long.MaxValue;
                _isReset = false;
            }

            _counter++;
            if (_min > size)
                _min = size;
            if (_max < size)
                _max = size;
            _sum += size;
            _average = _sum / _counter;
        }

        public void Reset()
        {
            _isReset = true;

            _counter = 0;
            _min = 0;
            _max = 0;
            _average = 0;
            _sum = 0;
        }
    }



    public class DebugSizeImagesResultsCollection : List<DebugSizeImagesResults>
    {
        public DebugSizeImagesResults this[int nodeId, int stationId]
        {
            get
            {
                foreach (DebugSizeImagesResults img in this)
                    if (img.NodeId == nodeId && img.StationId == stationId)
                        return img;

                return null;
            }
        }

        public void ResetAll()
        {
            foreach (DebugSizeImagesResults img in this)
                img.Reset();
        }
    }
}
