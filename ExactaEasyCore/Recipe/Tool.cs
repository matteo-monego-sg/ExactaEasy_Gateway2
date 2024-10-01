using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExactaEasyCore {

    public class Tool {

        public int Id { get; set; }
        public bool Active { get; set; }
        public string Label { get; set; }
        //private string _label;
        //public string Label {
        //    get {
        //        if (ToolParameters != null) {
        //            _label = (string.IsNullOrEmpty(ToolParameters["Label"].Value) == true) ? "" : ToolParameters["Label"].Value;
        //        }
        //        return _label;
        //    }
        //    set {
        //        if (ToolParameters["Label"] == null) {
        //            ToolParameters.Add(new Parameter {
        //                Id = "Label",
        //                Value = value,
        //            });
        //        }
        //        _label = value;
        //    }
        //}
        //public string ExportName { get; set; }
        //public string TypeName { get; set; }
        public ParameterCollection<Parameter> ToolParameters { get; set; }
        public List<Shape> Shapes { get; set; }
        public ParameterCollection<ToolOutput> ToolOutputs { get; set; }
        public List<OutputCondition> ToolOutputsConditions { get; set; }

        // collection di appoggio per import/export parametri
        private ParameterCollection<Parameter> _toolParMerge = new ParameterCollection<Parameter>();
        public ParameterCollection<Parameter> ToolParMerge {
            get {
                _toolParMerge.Clear();
                //if (ToolParameters["Active"] == null) {
                //    Parameter activePar = new Parameter();
                //    activePar.Id = "Active";
                //    activePar.Value = Active.ToString();
                //    _toolParMerge.Add(activePar);
                //}
                _toolParMerge.AddRange(ToolParameters);
                _toolParMerge.AddRange(ToolOutputs);
                return _toolParMerge;
            }
        }

        public Tool() {

            ToolParameters = new ParameterCollection<Parameter>();
            Shapes = new List<Shape>();
            ToolOutputs = new ParameterCollection<ToolOutput>();
            ToolOutputsConditions = new List<OutputCondition>();
        }

        public Tool Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            Tool newTool = new Tool();
            newTool.Id = Id;
            newTool.Label = Label;
            newTool.Active = Active;
            //newTool.ExportName = ExportName;
            //newTool.TypeName = TypeName;
            if (ToolParameters != null) {
                newTool.ToolParameters = new ParameterCollection<Parameter>();
                for (int i = 0; i < ToolParameters.Count; i++)
                    newTool.ToolParameters.Add((Parameter)ToolParameters[i].Clone());
            }
            if (Shapes != null) {
                newTool.Shapes = new List<Shape>();
                for (int i = 0; i < Shapes.Count; i++)
                    newTool.Shapes.Add(Shapes[i].Clone());
            }
            if (ToolOutputs != null) {
                newTool.ToolOutputs = new ParameterCollection<ToolOutput>();
                for (int i = 0; i < ToolOutputs.Count; i++)
                    newTool.ToolOutputs.Add(ToolOutputs[i].Clone());
            }
            if (ToolOutputsConditions != null) {
                newTool.ToolOutputsConditions = new List<OutputCondition>();
                for (int i = 0; i < ToolOutputsConditions.Count; i++)
                    newTool.ToolOutputsConditions.Add(ToolOutputsConditions[i].Clone());
            }
            return newTool;
        }

        public bool Compare(Tool toolToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (toolToCompare == null) {
                toolToCompare = new Tool();
                toolToCompare.Id = Id;
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = toolToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            List<ParameterDiff> _paramDiffList = null;
            if (ToolParameters != null) {
                ris = ris | ToolParameters.Compare(toolToCompare.ToolParameters, cultureCode, position, out _paramDiffList);
                if (_paramDiffList != null)
                    paramDiffList.AddRange(_paramDiffList);
            }
            if (Shapes != null) {
                for (int iS = 0; iS < Shapes.Count; iS++) {
                    if (iS > toolToCompare.Shapes.Count - 1) {
                        Shape newSh = new Shape();
                        newSh.Id = Shapes[iS].Id;
                        toolToCompare.Shapes.Add(newSh);
                    }
                    ris = ris | Shapes[iS].Compare(toolToCompare.Shapes[iS], cultureCode, position + " Shape " + Shapes[iS].Id, paramDiffList);
                }
            }
            if (ToolOutputs != null) {
                for (int iTO = 0; iTO < ToolOutputs.Count; iTO++) {
                    if (iTO > toolToCompare.ToolOutputs.Count - 1) {
                        ToolOutput newTO = new ToolOutput();
                        newTO.ParamId = ToolOutputs[iTO].ParamId;
                        toolToCompare.ToolOutputs.Add(newTO);
                    }
                    ris = ris | ToolOutputs[iTO].Compare(toolToCompare.ToolOutputs[iTO], cultureCode, position + " Output \"" + ToolOutputs[iTO].Label + "\"" + "(" + (ToolOutputs[iTO].ParamId + 1) + ")", paramDiffList);
                }
            }
            if (ToolOutputsConditions != null) {
                for (int iTOC = 0; iTOC < ToolOutputsConditions.Count; iTOC++) {
                    if (iTOC > toolToCompare.ToolOutputsConditions.Count - 1) {
                        OutputCondition newOC = new OutputCondition();
                        newOC.Id = ToolOutputsConditions[iTOC].Id;
                        toolToCompare.ToolOutputsConditions.Add(newOC);
                    }
                    ris = ris | ToolOutputsConditions[iTOC].Compare(toolToCompare.ToolOutputsConditions[iTOC], cultureCode, position + " Condition \"" + ToolOutputsConditions[iTOC].Name + "\"", paramDiffList);
                }
            }
            return ris;
        }
    }


    //pier: solo per uso interno NO A RICETTA
    public enum MeasureTypeEnum {
        BOOL = 0,
        INT = 1,
        DOUBLE = 2,
        STRING = 3,
        UNKNOWN
    }

    public class ToolOutput : Parameter {

        //public string Id { get; set; }                  //edit 02/10/2017 (da int a string)
        //public string Label { get; set; }
        //public string MeasureUnit { get; set; }
        public bool IsUsed { get; set; }
        //public int IsEditable { get; set; }           //del 13/09/2017
        //public MeasureTypeEnum Type { get; set; }
        //public string Value { get; set; }
        public bool EnableGraph { get; set; }
        //public string SectionId { get; set; }           //new 13/09/2017 
        //public string SubSectionId { get; set; }        //new 13/09/2017 
        //public string Group { get; set; }               //new 13/09/2017 
        //public string ParamId { get; set; }                //new 13/09/2017
        //public string ExportName { get; set; }          //new 13/09/2017
        //public string Description { get; set; }         //new 13/09/2017
        //public string MinValue { get; set; }            //new 13/09/2017
        //public string MaxValue { get; set; }            //new 13/09/2017
        //public double Increment { get; set; }           //new 13/09/2017
        //public int Decimals { get; set; }               //new 13/09/2017
        //public int IsVisible { get; set; }             //new 13/09/2017

        public ToolOutput()
            : base() {

            IsUsed = false;
            EnableGraph = false;
        }

        public ToolOutput(Parameter par)
            : base(par) {

            this.IsUsed = false;
            this.EnableGraph = false;
        }

        public ToolOutput(ToolOutput other)
            : base(other) {

            this.IsUsed = other.IsUsed;
            this.EnableGraph = other.EnableGraph;
        }

        public ToolOutput(string paramId, string id, string label, string mu, bool used, int visible, string valueType, string value/*, OutputCondition condition*/, bool enableGraph)
            : this() {

            ParamId = paramId;
            Id = id;
            Label = label;
            MeasureUnit = mu;
            IsUsed = used;
            IsVisible = visible;
            ValueType = valueType;
            Value = value;
            //Condition = condition;
            EnableGraph = enableGraph;

        }

        public new ToolOutput Clone() {

            ToolOutput newToolOutput = new ToolOutput(this);
            return newToolOutput;
        }

        public bool Compare(ToolOutput toolOutputToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (toolOutputToCompare == null) {
                toolOutputToCompare = new ToolOutput();
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = toolOutputToCompare.ParamId.ToString();
                paramDiff.CurrentValue = ParamId.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (toolOutputToCompare.Label != Label) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Name";
                paramDiff.ParameterLabel = "Name";
                paramDiff.ParameterLocLabel = "Name";
                paramDiff.ComparedValue = toolOutputToCompare.Label + " " + toolOutputToCompare.MeasureUnit;
                paramDiff.CurrentValue = Label + " " + MeasureUnit;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            // SCOMMENTARE SE SI VUOLE TRACCIARE I CAMBI DI UNITA' DI MISURA
            //if (toolOutputToCompare.MeasureUnit != MeasureUnit) {
            //    ParameterDiff paramDiff = new ParameterDiff();
            //    paramDiff.ParameterId = Id.ToString();
            //    paramDiff.ParameterLabel = "MeasureUnit";
            //    paramDiff.ComparedValue = toolOutputToCompare.MeasureUnit;
            //    paramDiff.CurrentValue = MeasureUnit;
            //    paramDiff.ParameterPosition = position;
            //    paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
            //    paramDiffList.Add(paramDiff);
            //    ris = true;
            //}
            if (toolOutputToCompare.IsUsed != IsUsed) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "IsUsed";
                paramDiff.ParameterLabel = "IsUsed";
                paramDiff.ParameterLocLabel = "IsUsed";
                paramDiff.ComparedValue = (toolOutputToCompare.IsUsed == false) ? "0" : "1";
                paramDiff.CurrentValue = (IsUsed == false) ? "0" : "1";
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            // SCOMMENTARE SE SI VUOLE TRACCIARE I CAMBI DI ESPORTABILITA'
            //if (toolOutputToCompare.IsEditable != IsEditable) {
            //    ParameterDiff paramDiff = new ParameterDiff();
            //    paramDiff.ParameterId = Id.ToString();
            //    paramDiff.ParameterLabel = "Export";
            //    paramDiff.ComparedValue = toolOutputToCompare.IsEditable.ToString();
            //    paramDiff.CurrentValue = IsEditable.ToString();
            //    paramDiff.ParameterPosition = position;
            //    paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
            //    paramDiffList.Add(paramDiff);
            //    ris = true;
            //}
            // SCOMMENTARE SE SI VUOLE TRACCIARE IL CAMBIO DI TIPOLOGIA DI OUTPUT
            //if (toolOutputToCompare.Type != Type) {
            //    ParameterDiff paramDiff = new ParameterDiff();
            //    paramDiff.ParameterId = Id.ToString();
            //    paramDiff.ParameterLabel = "Type";
            //    paramDiff.ComparedValue = ((int)toolOutputToCompare.Type).ToString();
            //    paramDiff.CurrentValue = ((int)Type).ToString();
            //    paramDiff.ParameterPosition = position;
            //    paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
            //    paramDiffList.Add(paramDiff);
            //    ris = true;
            //}
            if (toolOutputToCompare.Value != Value) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Limit";
                paramDiff.ParameterLabel = "Limit";
                paramDiff.ParameterLocLabel = "Limit";
                paramDiff.ComparedValue = toolOutputToCompare.Value;
                paramDiff.CurrentValue = Value;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            //if (Condition != null) {
            //    OutputCondition conditionToCompare = toolOutputToCompare.Condition;
            //    ris = ris | Condition.Compare(conditionToCompare, cultureCode, position + " Condition " + Condition.Id, paramDiffList);
            //}
            return ris;
        }
    }

    public class OutputCondition {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public OutputCondition() {
        }

        public OutputCondition(int id, string name, string value) {

            Id = id;
            Name = name;
            Value = value;
        }

        public OutputCondition Clone() {

            OutputCondition newCondition = new OutputCondition();
            newCondition.Id = Id;
            newCondition.Name = Name;
            newCondition.Value = Value;
            return newCondition;
        }

        public bool Compare(OutputCondition conditionToCompare, string cultureCode, string position, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (conditionToCompare == null) {
                conditionToCompare = new OutputCondition();
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "PRESENCE";
                paramDiff.ParameterLabel = "PRESENCE";
                paramDiff.ParameterLocLabel = "PRESENCE";
                paramDiff.ComparedValue = conditionToCompare.Id.ToString();
                paramDiff.CurrentValue = Id.ToString();
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (conditionToCompare.Name != Name) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Name";
                paramDiff.ParameterLabel = "Name";
                paramDiff.ParameterLocLabel = "Name";
                paramDiff.ComparedValue = conditionToCompare.Name;
                paramDiff.CurrentValue = Name;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            if (conditionToCompare.Value != Value) {
                ParameterDiff paramDiff = new ParameterDiff();
                paramDiff.ParameterId = "Checked";
                paramDiff.ParameterLabel = "Checked";
                paramDiff.ParameterLocLabel = "Checked";
                paramDiff.ComparedValue = conditionToCompare.Value;
                paramDiff.CurrentValue = Value;
                paramDiff.ParameterPosition = position;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                paramDiffList.Add(paramDiff);
                ris = true;
            }
            return ris;
        }
    }
}

