using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;

namespace ExactaEasyEng {

    public class Parameter : IParameter {

        NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");

        public string Id { get; set; }
        public string Label { get; set; }
        [XmlIgnore]
        public string Description { get; set; }
        string _valueType = String.Empty;
        [XmlIgnore]
        public string ValueType {
            get { return _valueType; }
            set {
                if (value == "" || value == "string" || value == "decimal")
                    _valueType = value;
                else
                    throw new ArgumentException("Value type not valid! Use 'string' or 'decimal'");
            }
        }
        string _value;
        public string Value {
            get { return _value; }
            set {
                if (VerifyValue(value))
                    _value = value;
                else
                    throw new InvalidOperationException(Id + " value " + Value + " not in [" + MinValue + "," + MaxValue + "]");
            }
        }

        string _minValue;
        public string MinValue {
            get { return _minValue; }
            set {
                _minValue = value;
                if (!VerifyValue())
                    throw new ArgumentOutOfRangeException(Id + " value " + Value + " not in [" + MinValue + "," + MaxValue + "]");
            }
        }

        string _maxValue;
        public string MaxValue {
            get { return _maxValue; }
            set {
                _maxValue = value;
                if (!VerifyValue())
                    throw new ArgumentOutOfRangeException(Id + " value " + Value + " not in [" + MinValue + "," + MaxValue + "]");
            }
        }

        public int IsVisible { get; set; }
        public int IsEditable { get; set; }
        public List<string> AdmittedValues { get; set; }

        public IParameter Clone() {

            Parameter newObj = new Parameter();
            newObj.Clone(this);
            return newObj;
        }

        public virtual void PopulateParameterInfo(ParameterInfoCollection pDict, string cultureCode) {

            if (Label == null || Label == "")
                Label = "$" + Id;       //valore di default (più facile da notare)
            Description = "!";      //valore di default (più facile da notare)
            if (pDict != null) {
                if (pDict[Id] == null) {
                    ParameterInfo parInfo = new ParameterInfo();
                    parInfo.Id = Id;
                    parInfo.ValueType = ValueType;
                    pDict.Add(parInfo);
                }
                ParameterInfo pInfo = pDict[Id];
                this.ValueType = pInfo.ValueType;
                if (pInfo.LocalizedInfo != null && pInfo.LocalizedInfo[cultureCode] != null)
                    Label = pInfo.LocalizedInfo[cultureCode].Label;
                if (pInfo.LocalizedDescription != null && pInfo.LocalizedDescription[cultureCode] != null)
                    Description = pInfo.LocalizedDescription[cultureCode].Label;
                //Values = pInfo.Values[cultureCode];
            }
        }

        public object GetValue() {

            return getValue(Value);
            //object ris = null;
            //decimal risDec = 0;
            //if ((ValueType == String.Empty) || (ValueType == "string"))
            //    ris = Value;
            //else if ((ValueType == "decimal") && Decimal.TryParse(Value, style, culture, out risDec))
            //    ris = risDec;

            //return ris;
        }

        object getValue(string value) {

            object ris = null;
            decimal risDec = 0;
            bool risBool = false;
            if ((ValueType == String.Empty) || (ValueType == "string"))
                ris = Value;
            else if ((ValueType == "decimal") && Decimal.TryParse(value, style, culture, out risDec))
                ris = risDec;
            else if ((ValueType == "bool") && Boolean.TryParse(value, out risBool))
                ris = risBool;

            return ris;
        }

        public bool VerifyValue() {

            return VerifyValue(Value);
        }

        public bool VerifyValue(string value) {

            if (value == null || (value == "" && (ValueType == "decimal" && ValueType == "bool")))
                return false;
            string decSymbol = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (decSymbol != ".") {
                value = value.Replace(decSymbol, ".");
            }
            if (ValueType == "decimal" && MinValue != null && MinValue != "" && MaxValue != null && MaxValue != "") {
                decimal val = (decimal)getValue(value);
                if (val >= (decimal)getValue(MinValue) && val <= (decimal)getValue(MaxValue))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public void Clone(IParameter source) {

            Id = source.Id;
            Label = source.Label;
            ValueType = source.ValueType;
            Value = source.Value;
            IsVisible = source.IsVisible;
            IsEditable = source.IsEditable;
        }

    }
}
