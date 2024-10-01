using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using SPAMI.Util;
using System.Xml;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace ExactaEasyEng {
    public class Recipe {

        public static bool SerializeParameterGroup { get; set; }

        static Recipe() {

            SerializeParameterGroup = false;
        }

        public static Recipe LoadFromFile(string filePath, bool correct = true) {

            Recipe newRecipe = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                string xmlString = reader.ReadToEnd();
                newRecipe = LoadFromXml(xmlString, correct);
            }
            return newRecipe;
        }

        public static Recipe LoadFromXml(string xmlString, bool correct = true) {

            Recipe newRecipe = null;
            using (StringReader reader = new StringReader(xmlString)) {
                XmlAttributeOverrides xmlRules = new XmlAttributeOverrides();
                if (xmlString.Contains("<Nodes>")) {
                    xmlRulesAdder(xmlRules);
                }
                newRecipe = buildRecipe(reader, xmlRules, correct);
            }
            return newRecipe;
        }

        static void xmlRulesAdder(XmlAttributeOverrides xmlRules) {

            xmlRules.Add(typeof(Recipe), "Nodes", new XmlAttributes() { XmlIgnore = false });
            xmlRules.Add(typeof(Recipe), "Cams", new XmlAttributes() { XmlIgnore = true });
            xmlRules.Add(typeof(Parameter), "Id", new XmlAttributes() { XmlAttribute = new XmlAttributeAttribute("Id") });
            xmlRules.Add(typeof(Parameter), "IsVisible", new XmlAttributes() { XmlIgnore = true });
            //xmlRules.Add(typeof(Parameter), "IsEditable", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("IsEditable") });
            xmlRules.Add(typeof(Parameter), "Label", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Label") });
            xmlRules.Add(typeof(Parameter), "Value", new XmlAttributes() { XmlText = new XmlTextAttribute(typeof(string)) });
            xmlRules.Add(typeof(Parameter), "Group", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Group") });
            xmlRules.Add(typeof(Parameter), "ParamId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("ParamId") });
            //new rules 13/09/2017
            //xmlRules.Add(typeof(Parameter), "SectionId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("SectionId") });
            //xmlRules.Add(typeof(Parameter), "SubSectionId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("SubSectionId") });
            xmlRules.Add(typeof(Parameter), "ExportName", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("ExportName") });
            xmlRules.Add(typeof(Parameter), "Description", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Description") });
            xmlRules.Add(typeof(Parameter), "MinValue", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("MinValue") });
            xmlRules.Add(typeof(Parameter), "MaxValue", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("MaxValue") });
            xmlRules.Add(typeof(Parameter), "Increment", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Increment") });
            xmlRules.Add(typeof(Parameter), "MeasureUnit", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("MeasureUnit") });
            xmlRules.Add(typeof(Parameter), "ValueType", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("ValueType") });
            //xmlRules.Add(typeof(Parameter), "AdmittedValues", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("AdmittedValues") });
            xmlRules.Add(typeof(Parameter), "Decimals", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Decimals") });
            //new rules 13/09/2017 tool outputs
            //xmlRules.Add(typeof(ToolOutput), "SectionId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("SectionId") });
            //xmlRules.Add(typeof(ToolOutput), "SubSectionId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("SubSectionId") });
            xmlRules.Add(typeof(ToolOutput), "Group", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Group") });
            xmlRules.Add(typeof(ToolOutput), "ParamId", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("ParamId") });
            xmlRules.Add(typeof(ToolOutput), "ExportName", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("ExportName") });
            xmlRules.Add(typeof(ToolOutput), "Description", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Description") });
            xmlRules.Add(typeof(ToolOutput), "MinValue", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("MinValue") });
            xmlRules.Add(typeof(ToolOutput), "MaxValue", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("MaxValue") });
            xmlRules.Add(typeof(ToolOutput), "Increment", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Increment") });
            xmlRules.Add(typeof(ToolOutput), "Decimals", new XmlAttributes() { XmlIgnore = false, XmlAttribute = new XmlAttributeAttribute("Decimals") });

            xmlRules.Add(typeof(Tool), "ToolParMerge", new XmlAttributes() { XmlIgnore = true });
            //xmlRules.Add(typeof(NodeRecipe), "FrameGrabbers", new XmlAttributes() { XmlIgnore = false });
            //xmlRules.Add(typeof(NodeRecipe), "Stations", new XmlAttributes() { XmlIgnore = false });
            //xmlRules.Add(typeof(GretelNodeRecipe), "UACCIUWARIWA", new XmlAttributes() { XmlIgnore = true });
            //xmlRules.Add(typeof(GretelNodeRecipe), "FrameGrabbers", new XmlAttributes() { XmlIgnore = true });
            //xmlRules.Add(typeof(GretelNodeRecipe), "Stations", new XmlAttributes() { XmlIgnore = true });
        }

        static Recipe buildRecipe(TextReader reader, XmlAttributeOverrides xmlRules, bool correct) {

            XmlSerializer xmlSer = new XmlSerializer(typeof(Recipe), xmlRules);
            Recipe newRecipe = null;
            newRecipe = (Recipe)xmlSer.Deserialize(reader);
            if (newRecipe.Nodes != null && correct == true) {
                //if (newRecipe.Nodes.Count != AppEngine.Current.MachineConfiguration.NodeSettings.Count) {
                //Log.Line(LogLevels.Warning, "Recipe.buildRecipe", "Number of nodes mismatch (recipe nodes: {0}, configuration nodes: {1}). Correcting recipe...", newRecipe.Nodes.Count, AppEngine.Current.MachineConfiguration.NodeSettings.Count);
                try {
                    foreach (NodeSetting node in AppEngine.Current.MachineConfiguration.NodeSettings) {
                        NodeRecipe currNodeRecipe = newRecipe.Nodes.Find(nr => nr.Id == node.Id);
                        if (currNodeRecipe != null)
                            continue;
                        Log.Line(LogLevels.Warning, "Recipe.buildRecipe", "Configuration node ID {0} not present in recipe, correcting recipe...", node.Id);
                        currNodeRecipe = new NodeRecipe();
                        currNodeRecipe.Id = node.Id;
                        newRecipe.Nodes.Add(currNodeRecipe);
                        newRecipe.Nodes.Last().Stations.Add(new StationRecipe());
                        newRecipe.Nodes.Last().Stations.Last().Cameras.Add(new CameraRecipe());
                    }
                    for (int iR = newRecipe.Nodes.Count - 1; iR >= 0; iR--) {
                        NodeRecipe currNodeRecipe = newRecipe.Nodes[iR];
                        NodeSetting currNodeSetting = AppEngine.Current.MachineConfiguration.NodeSettings.Find(ns => ns.Id == currNodeRecipe.Id);
                        if (currNodeSetting != null)
                            continue;
                        Log.Line(LogLevels.Warning, "Recipe.buildRecipe", "Recipe node ID {0} not present in machine configuration, correcting recipe...", currNodeRecipe.Id);
                        newRecipe.Nodes.RemoveAt(iR);
                    }
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Warning, "Recipe.buildRecipe", "An error occurred while correcting recipe: " + ex.Message);
                }
                //}
            }
            if (newRecipe.Cams != null) {
                foreach (Cam cam in newRecipe.Cams) {
                    if (cam.AcquisitionParameters["vialFormatDesc"] == null) {
                        int index = cam.AcquisitionParameters.FindIndex(c => c.Id == "vialFormatNum");
                        if (index++ > 0) {
                            cam.AcquisitionParameters.Insert(index, new Parameter() { Id = "vialFormatDesc", Value = "---" });
                        }
                    }
                }
            }

            //check names ICS
            foreach(NodeRecipe nodeR in newRecipe.Nodes)
            {
                foreach(StationRecipe stR in nodeR.Stations)
                {
                    foreach(Tool tR in stR.Tools)
                    {
                        foreach(ToolOutput toR in tR.ToolOutputs)
                        {
                            string label = toR.Label;
                            if (string.IsNullOrEmpty(label) == false)
                            {
                                const string str1 = "{";
                                const string str2 = "}_";
                                if (label.StartsWith(str1) && label.Contains(str2))
                                {
                                    int start1 = label.IndexOf(str1);
                                    int start2 = label.IndexOf(str2);
                                    string rep = label.Substring(start1, start2 - start1 + str2.Length);
                                    toR.Label = label.Replace(rep, "");
                                }
                            }
                        }
                    }
                }
            }

            return newRecipe;
        }

        public string RecipeName { get; set; }
        public string RecipeVersion { get; set; }
        public List<NodeRecipe> Nodes { get; set; }
        public List<Cam> Cams { get; set; }

        public Recipe() {

            RecipeVersion = "1";
        }

        public Recipe Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            Recipe newRecipe = new Recipe();
            newRecipe.RecipeName = RecipeName;
            newRecipe.RecipeVersion = RecipeVersion;
            if (Nodes != null && Nodes.Count > 0) {
                newRecipe.Nodes = new List<NodeRecipe>();
                for (int i = 0; i < Nodes.Count; i++)
                    newRecipe.Nodes.Add(Nodes[i].Clone(paramDictionary, cultureCode));
            }
            else if (Cams != null && Cams.Count > 0) {
                newRecipe.Cams = new List<Cam>();
                for (int i = 0; i < Cams.Count; i++)
                    newRecipe.Cams.Add(Cams[i].Clone(paramDictionary, cultureCode));
            }
            return newRecipe;
        }

        public bool Compare(Recipe recipeToCompare, string cultureCode, List<ParameterDiff> paramDiffList) {

            bool ris = false;
            if (Nodes != null && Nodes.Count > 0) {
                if (recipeToCompare.Nodes == null)
                    throw new InvalidOperationException();
                foreach (NodeRecipe nr in Nodes) {
                    NodeRecipe nrToCompare = recipeToCompare.Nodes.Find((NodeRecipe n) => { return n.Id == nr.Id; });
                    string nodeLabel = (AppEngine.Current.MachineConfiguration.NodeSettings[nr.Id]!=null)
                        ? AppEngine.Current.MachineConfiguration.NodeSettings[nr.Id].NodeDescription + "(" + AppEngine.Current.MachineConfiguration.NodeSettings[nr.Id].IP4Address + ")" 
                        : "???";
                    ris = ris | nr.Compare(nrToCompare, cultureCode, nodeLabel, paramDiffList);
                }
            }
            else {
                if (recipeToCompare.Cams == null)
                    throw new InvalidOperationException();
                foreach (Cam c in Cams) {
                }
            }
            return ris;
        }

        public override string ToString() {

            string xmlStr = "";
            XmlAttributeOverrides xmlRules = new XmlAttributeOverrides();
            if (this.Nodes != null && this.Nodes.Count > 0) {
                xmlRulesAdder(xmlRules);
            }
            XmlSerializer xmlSer = new XmlSerializer(typeof(Recipe), xmlRules);
            StringWriter writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            xmlStr = writer.ToString();
            writer.Close();
            return xmlStr;
        }

        public void SaveXml(string filePath) {

            StreamWriter writer = new StreamWriter(filePath);
            XmlAttributeOverrides xmlRules = new XmlAttributeOverrides();
            if (this.Nodes != null && this.Nodes.Count > 0) {
                xmlRulesAdder(xmlRules);
            }
            XmlSerializer xmlSer = new XmlSerializer(typeof(Recipe), xmlRules);
            xmlSer.Serialize(writer, this);
            writer.Close();
        }
    }

    public class Node {

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Station {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    public class Cam {

        /*[XmlElement(Order = 0)]*/
        public int Id { get; set; }
        /*[XmlElement(Order = 1)]*/
        public bool Enabled { get; set; }
        /*[XmlElement(Order = 2)]*/
        public string Program { get; set; }
        /*[XmlElement(Order = 3)]*/
        public ParameterCollection<Parameter> AcquisitionParameters { get; set; }
        /*[XmlElement(Order = 4)]*/
        public ParameterCollection<Parameter> DigitizerParameters { get; set; }
        /*[XmlElement(Order = 5)]*/
        public ParameterCollection<Parameter> RecipeSimpleParameters { get; set; }
        /*[XmlElement(Order = 6)]*/
        public ParameterCollection<Parameter> RecipeAdvancedParameters { get; set; }
        /*[XmlElement(Order = 7)]*/
        public List<ParameterCollection<Parameter>> ROIParameters { get; set; }
        /*[XmlElement(Order = 8)]*/
        public ParameterCollection<Parameter> MachineParameters { get; set; }
        /*[XmlElement(Order = 9)]*/
        public ParameterCollection<Parameter> StroboParameters { get; set; }
        public List<Light> Lights { get; set; }

        [XmlIgnore()]
        public Parameter VialFormatNum {
            get {
                return (Parameter)AcquisitionParameters["vialFormatNum"];
            }
        }
        //[XmlIgnore()]
        //public Parameter VialFormatDesc {
        //    get {
        //        return (Parameter)AcquisitionParameters["vialFormatDesc"];
        //    }
        //}
        //[XmlIgnore()]
        //public Parameter SavedVialAxisFilename {
        //    get {
        //        return (Parameter)AcquisitionParameters["savedVialAxisFilename"];
        //    }
        //    set {
        //        AcquisitionParameters.Add("savedVialAxisFilename", value.GetValue().ToString());
        //    }
        //}

        [XmlIgnore()]
        public int StationId { get; set; }

        public Cam() {

            Lights = new List<Light>();
        }

        public Cam Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            Cam newObj = new Cam();
            newObj.Id = Id;
            newObj.Enabled = Enabled;
            newObj.Program = Program;
            newObj.AcquisitionParameters = (ParameterCollection<Parameter>)AcquisitionParameters.Clone(paramDictionary, cultureCode);
            newObj.DigitizerParameters = (ParameterCollection<Parameter>)DigitizerParameters.Clone(paramDictionary, cultureCode);
            newObj.RecipeSimpleParameters = (ParameterCollection<Parameter>)RecipeSimpleParameters.Clone(paramDictionary, cultureCode);
            newObj.RecipeAdvancedParameters = (ParameterCollection<Parameter>)RecipeAdvancedParameters.Clone(paramDictionary, cultureCode);
            newObj.ROIParameters = new List<ParameterCollection<Parameter>>();
            for (int ir = 0; ir < ROIParameters.Count; ir++)
                newObj.ROIParameters.Add((ParameterCollection<Parameter>)ROIParameters[ir].Clone(paramDictionary, cultureCode));
            if (StroboParameters != null)
                newObj.StroboParameters = (ParameterCollection<Parameter>)StroboParameters.Clone(paramDictionary, cultureCode);
            newObj.MachineParameters = (ParameterCollection<Parameter>)MachineParameters.Clone(paramDictionary, cultureCode);
            if (Lights != null) {
                for (int il = 0; il < Lights.Count; il++)
                    newObj.Lights.Add(Lights[il].Clone(paramDictionary, cultureCode));
            }
            return newObj;
        }
    }

    public class ROI {

        [XmlArray(ElementName = "ROIParameters")]
        public ParameterCollection<Parameter> ROIParameters { get; set; }
    }

    public static class ListExtension {

        public static T FindById<T>(this List<T> list, string id) where T : Parameter {

            return list.Find(p => p.Id == id);
        }

    }

}
