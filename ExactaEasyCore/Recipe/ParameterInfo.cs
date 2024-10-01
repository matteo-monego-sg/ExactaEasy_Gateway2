using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace ExactaEasyCore {

    [Flags]
    public enum ParameterTypeEnum {

        Acquisition = 0x01,
        Digitizer = 0x02,
        Elaboration = 0x04,
        RecipeAdvanced = 0x08,
        Strobo = 0x10,
        ROI = 0x20,
        Machine = 0x40,
        All = 0x7F
    }

    public class ParameterInfoCollection : List<ParameterInfo> {

        public static ParameterInfoCollection LoadFromFile(string filePath) {

            ParameterInfoCollection paramColl = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                XmlSerializer xmlSer = new XmlSerializer(typeof(ParameterInfoCollection));
                paramColl = (ParameterInfoCollection)xmlSer.Deserialize(reader);
            }
            return paramColl;
        }

        public ParameterInfo this[string id] {
            get {
                return this.Find(p => p.Id == id);
            }
        }

        public void SaveFile(string filePath) {

            XmlSerializer xmlSer = new XmlSerializer(typeof(ParameterInfoCollection));

            StreamWriter writer = new StreamWriter(filePath);
            xmlSer.Serialize(writer, this);
            writer.Close();
        }

    }

    public class ParameterInfo {

        public string Id { get; set; }
        string _valueType = String.Empty;
        public string ValueType
        {
            get { return _valueType; }
            set
            {
                if (value == "" || value == "string" || value == "decimal" || value == "bool" || value == "int" || value == "double")
                    _valueType = value;
                else
                    throw new ArgumentException("Value type not valid! Use 'string' or 'decimal'");
            }
        }
        public ParameterInfoLocalizedCollection LocalizedInfo { get; set; }
        public ParameterDescriptionLocalizedCollection LocalizedDescription { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
    }

    public class ParameterInfoLocalizedCollection : List<ParameterInfoLocalized> {

        public ParameterInfoLocalized this[string cultureCode] {
            get {
                return this.Find(pl => pl.CultureCode == cultureCode);
            }
        }
    }

    public class ParameterDescriptionLocalizedCollection : List<ParameterDescriptionLocalized>
    {

        public ParameterDescriptionLocalized this[string cultureCode]
        {
            get
            {
                return this.Find(pl => pl.CultureCode == cultureCode);
            }
        }
    }

    public class LocalizedValuesCollection : List<LocalizedValue> {
        
        public LocalizedValuesCollection this[string cultureCode] {
            get {
                LocalizedValuesCollection valCol = new LocalizedValuesCollection();
                foreach (LocalizedValue val in this)
                    if (val.CultureCode == cultureCode)
                        valCol.Add(val);
                return valCol;
            }
        }
    }

    public class LocalizedValue : LocalizedString { }

    public class ParameterInfoLocalized : LocalizedString { }

    public class ParameterDescriptionLocalized : LocalizedString { }

    public class LocalizedString {

        public string CultureCode { get; set; }
        public string Label { get; set; }
    }

}
