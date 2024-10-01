using System.Collections.Generic;

namespace DisplayManager {

    public interface IResultId {

        List<int> IDs { get; }
        string Description { get; }
    }

    public class MeasureId : IResultId {

        public List<int> IDs { get; protected set; }

        public string Description {
            get;
            protected set;
        }

        public MeasureId() {
            IDs = new List<int>();
        }

        public MeasureId(int nodeId, int stationId, int toolId, int measureId, IStation station)
            : this() {

            IDs.Add(nodeId);
            IDs.Add(stationId);
            IDs.Add(toolId);
            IDs.Add(measureId);
            Description = station.Description + " Tool_" + toolId.ToString("d2") + " Measure_" + measureId.ToString("d2");

            //if (visionSystemConfig.NodesDefinition != null &&
            //    visionSystemConfig.NodesDefinition.NodeDefinitions.Find(nd => nd.Id == _nodeId) != null) {

            //    IDLabels[0] = visionSystemConfig.NodesDefinition.NodeDefinitions.Find(nd => nd.Id == _nodeId).NodeDescription;
            //    if (visionSystemConfig.StationDefinition != null &&
            //    visionSystemConfig.StationDefinition.Find(sd => sd.Id == _stationId) != null) {
            //        IDLabels[1] = visionSystemConfig.StationDefinition.Find(sd => sd.Id == _stationId).StationDescription;
            //    }
            //}
        }

        //public string ResolveIdToLabel(string separator) {

        //    string retLine = "";
        //    for (int i = 0; i < IDs.Count; i++) {
        //        if (IDLabels[i].Length > 0)
        //            retLine += IDLabels[i] + separator;
        //        else
        //            retLine += IDs[i].ToString() + separator;
        //    }
        //    return retLine;
        //}

        public override bool Equals(object obj) {
            if (!(obj is MeasureId)) return false;
            var other = (MeasureId)obj;
            //bool equal = true;
            for (int id = 0; id < IDs.Count; id++) {
                if (IDs[id] != other.IDs[id]) {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode() {
            unchecked {
                return (IDs != null ? IDs.GetHashCode() : 0);
            }
        }
    }
}
