using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ExactaEasyCore.TrendingTool
{
    public class TrendConfigParSaved : ICloneable
    {
        public string RecipeName { get; set; }
        public int NodeId { get; set; }
        public int StationId { get; set; }
        public int ToolIndex { get; set; }
        public int ParameterIndex { get; set; }
        //public string ParameterName { get; set; }
        public string StationName { get; set; }
        public string ToolName { get; set; }
        public string ParameterName { get; set; }


        //private constructor for xml serialization
        private TrendConfigParSaved() { }

        public TrendConfigParSaved(string recipeName, int nodeId, int stationId, int toolIndex, int parameterIndex, string stationName, string toolName, string parameterName)
        {
            RecipeName = recipeName;
            NodeId = nodeId;
            StationId = stationId;
            ToolIndex = toolIndex;
            ParameterIndex = parameterIndex;
            StationName = stationName;
            ToolName = toolName;
            ParameterName = parameterName;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parSaved"></param>
        /// <returns>0=Not eqaul   1=Equal all   2=Equal only RecipeName, NodeId, StationId, ToolIndex, ParameterIndex</returns>
        public int IsEqualTo(TrendConfigParSaved parSaved)
        {
            if (parSaved == null)
                return 0;
            if (RecipeName == parSaved.RecipeName && NodeId == parSaved.NodeId && StationId == parSaved.StationId && ToolIndex == parSaved.ToolIndex && 
                ParameterIndex == parSaved.ParameterIndex && StationName == parSaved.StationName && ToolName == parSaved.ToolName && ParameterName == parSaved.ParameterName)
                return 1;
            else if (RecipeName == parSaved.RecipeName && NodeId == parSaved.NodeId && StationId == parSaved.StationId && ToolIndex == parSaved.ToolIndex &&
                ParameterIndex == parSaved.ParameterIndex && (StationName != parSaved.StationName || ToolName == parSaved.ToolName || ParameterName == parSaved.ParameterName))
                return 2;
            else
                return 0;
        }


        public object Clone()
        {
            return new TrendConfigParSaved(RecipeName, NodeId, StationId, ToolIndex, ParameterIndex, StationName, ToolName, ParameterName);
        }


        public string ToXML()
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(GetType());
                serializer.Serialize(sw, this);
                return sw.ToString();
            }
        }


        public static TrendConfigParSaved FromXML(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TrendConfigParSaved));
                return (TrendConfigParSaved)serializer.Deserialize(sr);
            }
        }
    }
}
