using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ExactaEasyCore.TrendingTool
{
    public class TrendConfigFileSaved
    {
        public string RecipeName { get; set; }
        public string StationName { get; set; }
        public string ToolName { get; set; }
        public string ParameterName { get; set; }
        public int BatchId { get; set; }
        public string FilePath { get; set; }


        //private constructor for xml serialization
        private TrendConfigFileSaved() { }

        public TrendConfigFileSaved(string recipeName, string stationName, string toolName, string parameterName, int batchId, string filePath)
        {
            RecipeName = recipeName;
            StationName = stationName;
            ToolName = toolName;
            ParameterName = parameterName;
            BatchId = batchId;
            FilePath = filePath;
        }



        public bool IsEqualTo(TrendConfigFileSaved fileSaved)
        {
            if (fileSaved == null)
                return false;
            if (RecipeName != fileSaved.RecipeName)
                return false;
            if (StationName != fileSaved.StationName)
                return false;
            if (ToolName != fileSaved.ToolName)
                return false;
            if (ParameterName != fileSaved.ParameterName)
                return false;
            if (BatchId != fileSaved.BatchId)
                return false;
            if (FilePath != fileSaved.FilePath)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{RecipeName};{StationName};{ToolName};{ParameterName};{BatchId};{FilePath};";
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

        public static TrendConfigFileSaved FromXML(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TrendConfigFileSaved));
                return (TrendConfigFileSaved)serializer.Deserialize(sr);
            }
        }
    }
}
