using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class NodeRecipe {

        public int Id { get; set; }
        public string Description { get; set; }
        public List<FrameGrabberRecipe> FrameGrabbers { get; set; }
        public List<StationRecipe> Stations { get; set; }
        public string RawRecipe { get; set; }

        public NodeRecipe() {

            FrameGrabbers = new List<FrameGrabberRecipe>();
            Stations = new List<StationRecipe>();
            RawRecipe = "";
        }

        public NodeRecipe Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            NodeRecipe newObj = new NodeRecipe();
            newObj.Id = Id;
            newObj.Description = Description;
            if (FrameGrabbers != null) {
                newObj.FrameGrabbers = new List<FrameGrabberRecipe>();
                foreach (FrameGrabberRecipe fgr in FrameGrabbers) {
                    newObj.FrameGrabbers.Add(fgr.Clone(paramDictionary, cultureCode));
                }
            }
            if (Stations != null) {
                newObj.Stations = new List<StationRecipe>();
                foreach (StationRecipe sr in Stations) {
                    newObj.Stations.Add(sr.Clone(paramDictionary, cultureCode));
                }
            }
            newObj.RawRecipe = RawRecipe;
            return newObj;
        }

        public bool Compare(NodeRecipe nodeToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (nodeToCompare == null) {
                nodeToCompare = new NodeRecipe();
                nodeToCompare.Id = Id;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = nodeToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (FrameGrabbers != null) {
                foreach (FrameGrabberRecipe fgr in FrameGrabbers) {
                    FrameGrabberRecipe fgrToCompare = nodeToCompare.FrameGrabbers.Find((FrameGrabberRecipe fg) => { return fg.BoardId == fgr.BoardId; });
                    ris = ris | fgr.Compare(fgrToCompare, cultureCode, position + " - Frame Grabber " + fgr.BoardId.ToString(), paramDiffList);
                }
            }
            if (Stations != null) {
                foreach (StationRecipe sr in Stations) {
                    StationRecipe srToCompare = nodeToCompare.Stations.Find((StationRecipe s) => { return s.Id == sr.Id; });
                    ris = ris | sr.Compare(srToCompare, cultureCode, position + " - " + sr.Description + "(" + (sr.Id + 1) + ")", paramDiffList);
                }
            }
            return ris;
        }
    }

    public class NodeRecipeEventArgs : EventArgs {

        public NodeRecipe NodeRecipe { get; set; }
        public bool ToSave { get; set; }

        public NodeRecipeEventArgs(NodeRecipe nodeRecipe, bool toSave) {

            NodeRecipe = nodeRecipe;
            ToSave = toSave;
        }
    }

}
