using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ExactaEasyCore;

namespace ExactaEasyEng {

    public class RecipeTemplate {

        public string TemplateVersion { get; set; }
        public ParameterCollection<Parameter> AcquisitionParameters { get; set; }
        public ParameterCollection<Parameter> DigitizerParameters { get; set; }
        public ParameterCollection<Parameter> RecipeSimpleParameters { get; set; }
        public ParameterCollection<Parameter> RecipeAdvancedParameters { get; set; }
        public List<ParameterCollection<Parameter>> ROIParameters { get; set; }
        public ParameterCollection<Parameter> MachineParameters { get; set; }
        public ParameterCollection<Parameter> StroboParameters { get; set; }

        public static RecipeTemplate LoadFromFile(string filePath) {

            RecipeTemplate newTemplateRecipe = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                newTemplateRecipe = buildTemplate(reader);
            }
            return newTemplateRecipe;
        }

        public static RecipeTemplate LoadFromXml(string xmlString) {

            RecipeTemplate newTemplateRecipe = null;
            using (StringReader reader = new StringReader(xmlString)) {
                newTemplateRecipe = buildTemplate(reader);
            }
            return newTemplateRecipe;
        }

        static RecipeTemplate buildTemplate(TextReader reader) {

            XmlSerializer xmlSer = new XmlSerializer(typeof(RecipeTemplate));
            RecipeTemplate newTemplateRecipe = null;

            newTemplateRecipe = (RecipeTemplate)xmlSer.Deserialize(reader);
            //foreach (Cam cam in newRecipe.Cams) {
            //    if (cam.AcquisitionParameters["vialFormatDesc"] == null) {
            //        int index = cam.AcquisitionParameters.FindIndex(c => c.Id == "vialFormatNum");

            //        if (index++ > 0) {
            //            cam.AcquisitionParameters.Insert(index, new AcquisitionParameter() { Id = "vialFormatDesc", Value = "---" });
            //        }
            //    }
            //}
            return newTemplateRecipe;

        }

        public void SaveXml(string filePath) {

            StreamWriter writer = new StreamWriter(filePath);
            XmlSerializer xmlSer = new XmlSerializer(typeof(RecipeTemplate));
            xmlSer.Serialize(writer, this);
            writer.Close();
        }
    }
}
