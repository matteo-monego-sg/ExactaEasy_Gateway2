using System;
using System.Collections.Generic;
using ExactaEasyCore;

namespace DisplayManager {

    public class InspectionResults {
        public int NodeId;
        public int InspectionId;
        public uint SpindleId;
        public uint VialId;     // 0 = good
        public bool IsActive;
        public bool IsReject;
        public ushort RejectionCause;
        public ToolResultsCollection ResToolCollection;

        public InspectionResults(int nodeId, int inspectionId, uint spindleId, uint vialId, bool isActive, bool isReject, ushort rejectionCause, ToolResultsCollection resToolCollection) {

            NodeId = nodeId;
            InspectionId = inspectionId;
            SpindleId = spindleId;
            VialId = vialId;
            IsActive = isActive;
            IsReject = isReject;
            RejectionCause = rejectionCause;
            ResToolCollection = resToolCollection;
        }
    }

    public class ToolResults {
        public int ToolId;
        public bool IsActive;
        public bool IsReject;
        public bool IsDisplayed;
        public MeasureResultsCollection ResMeasureCollection;

        public ToolResults(int toolId, bool isActive, bool isReject, bool isDisplayed, MeasureResultsCollection resMeasureCollection) {
            
            ToolId = toolId;
            IsActive = isActive;
            IsReject = isReject;
            IsDisplayed = isDisplayed;
            ResMeasureCollection = resMeasureCollection;
        }
    }

    public class ToolResultsCollection : List<ToolResults> {

        public new ToolResults this[int id] {
            get {
                return Find(tr => tr.ToolId == id);
            }
        }
    }

    public class MeasureResults {
               
        public int MeasureId;
        public string MeasureName;
        public string MeasureUnit;
        public bool IsOk;
        public bool IsUsed;
        public MeasureTypeEnum MeasureType;
        public string MeasureValue;
        public int MeasureCount;

        public MeasureResults(int measureId, string measureName, string measureUnit, bool isOk, bool isUsed, MeasureTypeEnum measureType, string measureValue, int measureCount) {

            MeasureId = measureId;
            MeasureName = measureName;
            MeasureUnit = measureUnit;
            IsOk = isOk;
            IsUsed = isUsed;
            MeasureType = measureType;
            MeasureValue = measureValue;
            MeasureCount = measureCount;
        }
    }

    public class MeasureResultsCollection : List<MeasureResults> {

        public new MeasureResults this[int id] {
            get {
                return Find(mr => mr.MeasureId == id);
            }
        }
    }

    public class CollectorInfo {

        public int NodeId;
        public int InspectionId;
        public int ImagesSaved;

        public CollectorInfo(int nodeId, int inspectionId, int imagesSaved) {

            NodeId = nodeId;
            InspectionId = inspectionId;
            ImagesSaved = imagesSaved;
        }
    }

    public class MeasuresAvailableEventArgs : EventArgs {

        public InspectionResults InspectionResults { get; private set; }

        public MeasuresAvailableEventArgs(InspectionResults inspectionResults) {

            InspectionResults = inspectionResults;
        }
    }
}
