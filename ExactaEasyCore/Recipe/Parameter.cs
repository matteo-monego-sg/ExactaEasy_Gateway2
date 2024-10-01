using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using SPAMI.Util.Logger;

namespace ExactaEasyCore {

    public class Parameter : IParameter {

        NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent;
        CultureInfo culture = CultureInfo.InvariantCulture; // CultureInfo.GetCultureInfo("en-US");

        public string Id { get; set; }
        public string Label { get; set; }
        public string ExportName { get; set; }              //new 13/09/2017         
        string _value;
        public string Value {
            get {
                if (ValueType == "bool") {
                    if (_value == "1") _value = "True";
                    if (_value == "0") _value = "False";
                }
                return _value; 
            }
            set {
                if (ValueType == "bool") {
                    if (value == "1") value = "True";
                    if (value == "0") value = "False";
                }
                if (VerifyValue(value))
                    _value = value;
                else {
                    _value = value;         // DA VERIFICARE
                    //throw new InvalidOperationException(Id + " value " + Value + " not in [" + MinValue + "," + MaxValue + "]");
                }
            }
        }
        public string Description { get; set; }
        string _minValue;
        public string MinValue {
            get { return _minValue; }
            set {
                _minValue = value;
                //if (!VerifyValue())
                //    throw new ArgumentOutOfRangeException(Id + " value " + Value + " not in [" + MinValue + "," + MaxValue + "]");
            }
        }
        string _maxValue;
        public string MaxValue {
            get { return _maxValue; }
            set {
                _maxValue = value;
                //if (!VerifyValue())
                //    throw new ArgumentOutOfRangeException(Id + " value " + Value + " not in [" + MinValue + "," + MaxValue + "]");
            }
        }
        public int IsVisible { get; set; } 
        //public int IsEditable { get; set; }               //del 13/09/2017
        public double Increment { get; set; }               //new 13/09/2017
        public string MeasureUnit { get; set; }             //new 13/09/2017
        string _valueType;
        public string ValueType {
            get { return _valueType; }
            set {
                if (value == "" || value == "string" || value == "decimal" || value == "bool" || value == "int" || value == "double")
                    _valueType = value;
                else
                    throw new ArgumentException("Value type not valid! Use 'string' or 'decimal'");
            }
        }
        public List<string> AdmittedValues { get; set; }
        int _decimals;
        public int Decimals { 
            get { return _decimals; }
            set {
                int incrementDecimals = Math.Max(0, ((Increment - (int)Increment) + "").Length - 2);
                _decimals = Math.Max(incrementDecimals, value);
                if (ValueType == "int" || ValueType == "bool") {
                    _decimals = 0;
                }
            }
        }                   
        //new 13/09/2017
        public string Group { get; set; }
        public string ParamId { get; set; }
        public string ParentName { get; set; }

        public Parameter() {

            Id = string.Empty;
            Label = string.Empty;
            ExportName = string.Empty;
            Value = string.Empty;
            Description = string.Empty;
            MinValue = string.Empty;
            MaxValue = string.Empty;
            Increment = 0;
            MeasureUnit = string.Empty;
            ValueType = string.Empty;
            //AdmittedValues
            Decimals = 0;
            IsVisible = 0;
            Group = string.Empty;
            ParamId = string.Empty;
            ParentName = string.Empty;
        }

        public Parameter(Parameter other) {

            Id = other.Id;
            Label = other.Label;
            ExportName = other.ExportName;
            ValueType = other.ValueType;
            Value = other.Value;
            Description = other.Description;
            MinValue = other.MinValue;
            MaxValue = other.MaxValue;
            Increment = other.Increment;
            MeasureUnit = other.MeasureUnit;
            AdmittedValues = other.AdmittedValues;
            Decimals = other.Decimals;
            IsVisible = other.IsVisible;
            Group = other.Group;
            ParamId = other.ParamId;
            ParentName = other.ParentName;
        }

        public IParameter Clone() {

            Parameter newObj = new Parameter();
            newObj.Clone(this);
            return newObj;
        }

        public void Clone(IParameter source) {

            Id = source.Id;
            Label = source.Label;
            ExportName = source.ExportName;
            Description = source.Description;
            ValueType = source.ValueType;
            try {
                Value = source.Value;
            }
            catch (InvalidOperationException ex) {
                Log.Line(LogLevels.Warning, "Parameter.Clone", "Error: " + ex.Message);
            }
            IsVisible = source.IsVisible;
            //IsEditable = source.IsEditable;
            Group = source.Group;
            ParamId = source.ParamId;
            ParentName = source.ParentName;
            MinValue = source.MinValue;
            MaxValue = source.MaxValue;
            Increment = source.Increment;
            MeasureUnit = source.MeasureUnit;
            AdmittedValues = source.AdmittedValues;
            Decimals = source.Decimals;
        }

        public bool Compare(IParameter paramToCompare, out ParameterDiff paramDiff) {

            paramDiff = null;
            bool ris = false;
            if (string.IsNullOrEmpty(paramToCompare.Value) && string.IsNullOrEmpty(Value)) {
                return ris;
            }
            if (paramToCompare.Value != Value) {
                paramDiff = new ParameterDiff();
                paramDiff.ParameterId = Id;
                paramDiff.ParameterLabel = (string.IsNullOrEmpty(Label) == true) ? Id : Label;
                paramDiff.ParameterLocLabel = (string.IsNullOrEmpty(ExportName) == true) ? Id : ExportName;
                paramDiff.ComparedValue = paramToCompare.Value;
                paramDiff.CurrentValue = Value;
                paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
                ris = true;
            }
            if (string.IsNullOrEmpty(paramToCompare.Label) && string.IsNullOrEmpty(Label)) {
                return ris;
            }
            // SCOMMENTARE SE SI VUOLE TRACCIARE I CAMBI DI DISPLAY NAME
            //if (paramToCompare.Label != Label) {
            //    paramDiff = new ParameterDiff();
            //    paramDiff.ParameterId = Id;
            //    paramDiff.ParameterLabel = "Display Name";
            //    paramDiff.ComparedValue = paramToCompare.Label;
            //    paramDiff.CurrentValue = Label;
            //    paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
            //    ris = true;
            //}
            // SCOMMENTARE SE SI VUOLE TRACCIARE I CAMBI DI ESPORTABILITA'
            //if (paramToCompare.IsEditable != IsEditable) {
            //    paramDiff = new ParameterDiff();
            //    paramDiff.ParameterId = Id;
            //    paramDiff.ParameterLabel = "Export";
            //    paramDiff.ComparedValue = paramToCompare.IsEditable.ToString();
            //    paramDiff.CurrentValue = IsEditable.ToString();
            //    paramDiff.DifferenceType = ParameterCompareDifferenceType.Modified;
            //    ris = true;
            //}
            return ris;
        }

        public virtual bool PopulateParameterInfo(ParameterInfoCollection pDict, string cultureCode) {

            if (Label == null || Label == "")
                Label = "$" + Id;       //valore di default (più facile da notare)
            Description = "!";      //valore di default (più facile da notare)
            if (pDict != null) {
                if (pDict[Id] == null) {
                    return false;
                    //ParameterInfo parInfo = new ParameterInfo();
                    //parInfo.Id = Id;
                    //parInfo.ValueType = ValueType;
                    //pDict.Add(parInfo);
                }
                ParameterInfo pInfo = pDict[Id];
                this.ValueType = pInfo.ValueType;
                if (pInfo.LocalizedInfo != null && pInfo.LocalizedInfo[cultureCode] != null) {
                    Label = pInfo.LocalizedInfo[cultureCode].Label;
                    ExportName = pInfo.LocalizedInfo[cultureCode].Label;
                }
                if (pInfo.LocalizedDescription != null && pInfo.LocalizedDescription[cultureCode] != null)
                    Description = pInfo.LocalizedDescription[cultureCode].Label;
                //Values = pInfo.Values[cultureCode];
                return true;
            }
            return false;
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
            if (value == "")
                value = MinValue;
            if (ValueType == "bool") {
                if (value == "1") value = "True";
                if (value == "0") value = "False";
            }
            decimal risDec = 0;
            bool risBool = false;
            if ((ValueType == String.Empty) || (ValueType == "string"))
                ris = Value;
            else if ((ValueType == "decimal" || ValueType == "int" || ValueType == "double") && Decimal.TryParse(value, style, culture, out risDec))
                ris = risDec;
            else if ((ValueType == "bool") && Boolean.TryParse(value, out risBool))
                ris = risBool;
            else {
                Log.Line(LogLevels.Debug, "Parameter.getValue", "getValue return null...");
            }
            return ris;
        }

        public bool VerifyValue() {

            return VerifyValue(Value);
        }

        public bool VerifyValue(string value) {

            if (String.IsNullOrEmpty(value) && (ValueType == "decimal" || ValueType == "bool" || ValueType == "int" || ValueType == "double"))
                return false;
            string decSymbol = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if ((ValueType == "decimal" || ValueType == "bool" || ValueType == "int" || ValueType == "double") && MinValue != null && MinValue != "" && MaxValue != null && MaxValue != "") {
                if (decSymbol != ".") {
                    value = value.Replace(decSymbol, ".");
                }
                object ret = getValue(value);
                if (ret != null) {
                    if (ValueType == "bool") {
                        try {
                            bool val = (bool)ret;//(ret as IConvertible) != null && (ret as IConvertible).ToBoolean(null);
                            return true;
                        }
                        catch {
                            return false;
                        }
                    }
                    else {
                        decimal val = (decimal)ret;
                        if (val >= (decimal)getValue(MinValue) && val <= (decimal)getValue(MaxValue))
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
            else
                return true;
        }

        public bool IncrementValue(int sign, ref string newValue) {

            if (ValueType != "double" && ValueType != "int")
                return false;
            double newDoubleValue;
            if (Double.TryParse(getValue(Value).ToString(), out newDoubleValue)) {
                newDoubleValue += sign * Increment;
                newValue = newDoubleValue.ToString("N" + Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                //string decSymbol = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                //if (decSymbol != ".") {
                //    newValue = newValue.ToString().Replace(".", decSymbol);
                //}
                return VerifyValue(newValue);
            }
            return false;
        }
    }
}
