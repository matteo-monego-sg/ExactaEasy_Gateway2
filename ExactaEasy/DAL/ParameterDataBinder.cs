using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExactaEasyEng;
using System.Data;
using System.Globalization;
using ExactaEasyCore;

namespace ExactaEasy.DAL {
    public class ParametersDataBinder<T> : IDataBinder where T : Parameter {

        class ViewParameter {

            public T Parameter;
            public T ActualParameter;
        }

        DataTable dt;
        List<ViewParameter> view_parameters;
        int _userLevel;
        int _startRow;

        public int TotalRowCount {
            get {
                return view_parameters.Count;
            }
        }

        public DataColumnCollection Columns {
            get {
                return dt.Columns;
            }
        }

        public DataRowCollection Rows {
            get {
                return dt.Rows;
            }
        }

        public UserLevelEnum UserLevel {
            get {
                return (UserLevelEnum)_userLevel;
            }
            set {
                _userLevel = (int)value;
            }
        }

        public void GetData(int startRow, int numberOfRows) {

            int maxRowCount = view_parameters.Count;
            dt.Clear();
            for (int i = startRow; i < Math.Min(startRow + numberOfRows, maxRowCount); i++) {
                ViewParameter param = view_parameters[i];
                /*if (typeof(T) == typeof(DigitizerParameter))
                {
                    dt.LoadDataRow(new object[] { 
                    param.Parameter.Id, 
                    param.Parameter.Label, 
                    param.Parameter.GetValue(), 
                    param.ActualParameter == null ? false : param.ActualParameter.GetValue(), 
                    param.Parameter.IsEditable,
                    param.Parameter.MinValue,
                    param.Parameter.MaxValue
                    }, true);
                }
                else
                {*/
                decimal tempDecValue;
                string paramValue = param.Parameter.GetValue().ToString();
                string paramActualValue = (param.ActualParameter == null || param.ActualParameter.GetValue() == null) ? "" : param.ActualParameter.GetValue().ToString();
                if (param.Parameter != null && (param.Parameter.ValueType == "decimal" || param.Parameter.ValueType == "double")) {
                    tempDecValue = (decimal)param.Parameter.GetValue();
                    paramValue = tempDecValue.ToString("N" + param.Parameter.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                }
                if (param.ActualParameter != null && (param.ActualParameter.ValueType == "decimal" || param.ActualParameter.ValueType == "double")) {
                    tempDecValue = (decimal)param.ActualParameter.GetValue();
                    paramActualValue = tempDecValue.ToString("N" + param.ActualParameter.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                }

                dt.LoadDataRow(new object[] { 
                    param.Parameter.Id, 
                    param.Parameter.ExportName, 
                    paramValue,
                    paramActualValue, 
                    //param.Parameter.IsEditable,
                    param.Parameter.MinValue,
                    param.Parameter.MaxValue
                    }, true);
                //}
            }
            _startRow = startRow;
        }

        public string[] GetCellValues(string columnName, int startRow, int row) {

            string[] values = null;
            if (columnName == "Value") {
                int i = startRow + row;
                ViewParameter param = view_parameters[i];
                if (param.Parameter.AdmittedValues != null) {
                    values = new string[param.Parameter.AdmittedValues.Count];
                    for (int j = 0; j < values.Length; j++)
                        values[j] = param.Parameter.AdmittedValues[j];
                }
                if (param.Parameter.ValueType == "bool") {
                    values = new string[2];
                    values[0] = false.ToString();
                    values[1] = true.ToString();
                }
            }
            return values;
        }

        public void UpdateData() {

            for (int i = 0; i < dt.Rows.Count; i++) {
                UpdateData(i);
            }
        }

        public void UpdateData(int rowIndex) {

            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count) {
                if (view_parameters[_startRow + rowIndex].Parameter.ValueType == "decimal" ||
                    view_parameters[_startRow + rowIndex].Parameter.ValueType == "int" ||
                    view_parameters[_startRow + rowIndex].Parameter.ValueType == "double")
                    view_parameters[_startRow + rowIndex].Parameter.Value = (Convert.ToDouble(dt.Rows[rowIndex]["Value"], CultureInfo.InvariantCulture)).ToString("N" + view_parameters[_startRow + rowIndex].Parameter.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                else if (view_parameters[_startRow + rowIndex].Parameter.ValueType == "bool") {
                    view_parameters[_startRow + rowIndex].Parameter.Value = (Convert.ToBoolean(dt.Rows[rowIndex]["Value"], CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture).Replace(",", "");
                }
                else {
                    view_parameters[_startRow + rowIndex].Parameter.Value = dt.Rows[rowIndex]["Value"].ToString();
                }
            }
        }

        public bool ValidatingData(int rowIndex, string newValue) {

            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count)
                return view_parameters[_startRow + rowIndex].Parameter.VerifyValue(newValue);
            else
                return false;
        }

        public bool ValidatingIncrementData(int rowIndex, int sign, ref string newValue) {

            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count) {
                return (view_parameters[_startRow + rowIndex].Parameter.IncrementValue(sign, ref newValue));
            }
            else
                return false;
        }

        public bool ChangedData(int rowIndex) {

            bool res = false;
            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count && view_parameters[_startRow + rowIndex].ActualParameter != null) {
                res = view_parameters[_startRow + rowIndex].ActualParameter.Value != view_parameters[_startRow + rowIndex].Parameter.Value;
            }
            return res;
        }

        public bool VerifyValue(int rowIndex) {

            bool res = true;
            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count && view_parameters[_startRow + rowIndex].ActualParameter != null) {
                res = view_parameters[_startRow + rowIndex].Parameter.VerifyValue();
            }
            return res;
        }

        public string GetValueType(int rowIndex) {

            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count) {
                if (view_parameters[_startRow + rowIndex].ActualParameter != null) {
                    return view_parameters[_startRow + rowIndex].ActualParameter.ValueType;
                }
            }
            return "";
        }

        public string[] GetParamInfo(int rowIndex) {

            string[] infos = new string[8];
            if (view_parameters[_startRow + rowIndex].Parameter != null) {

                string um = " ";
                if (string.IsNullOrEmpty(view_parameters[_startRow + rowIndex].Parameter.MeasureUnit) == false) 
                    um = " " + view_parameters[_startRow + rowIndex].Parameter.MeasureUnit + " ";

                string description = "<EMPTY>";
                if (string.IsNullOrEmpty(view_parameters[_startRow + rowIndex].Parameter.Description) == false) {
                    description =  view_parameters[_startRow + rowIndex].Parameter.Description.Substring(0, Math.Min(1024, view_parameters[_startRow + rowIndex].Parameter.Description.Length));
                }
                
                infos[0] = view_parameters[_startRow + rowIndex].Parameter.ExportName;
                infos[1] = "Real name" + ": " + view_parameters[_startRow + rowIndex].Parameter.Id + um/* + "Type" + ": " + _parameters[_startRow + rowIndex].Parameter.ValueType*/; //tradurre
                infos[2] = "Range";              //tradurre
                infos[3] = "[" + view_parameters[_startRow + rowIndex].Parameter.MinValue + ", " + view_parameters[_startRow + rowIndex].Parameter.MaxValue + "]";              //tradurre
                infos[4] = "Increment";     //tradurre
                infos[5] =  view_parameters[_startRow + rowIndex].Parameter.Increment.ToString();
                infos[6] = "Description"; //tradurre
                infos[7] = description;
            }
            return infos;
        }

        public bool EditableData(int rowIndex) {

            if (_startRow + rowIndex >= 0 && _startRow + rowIndex < view_parameters.Count) {
                //return _parameters[_startRow + rowIndex].ActualParameter != null && _parameters[_startRow + rowIndex].Parameter.IsEditable <= _userLevel;
                return (view_parameters[_startRow + rowIndex].Parameter != null);
            }
            else {
                return false;
            }
        }

        public ParametersDataBinder(List<Parameter> parameters, List<Parameter> actualParameters, UserLevelEnum userLevel) {

            //List<ParameterInfo> paramInfo;
            dt = new DataTable();
            dt.Columns.Add("PropertyId", typeof(string));
            dt.Columns.Add("PropertyDescription", typeof(string));
            /*if (typeof(T) == typeof(DigitizerParameter)) {
                dt.Columns.Add("Value", typeof(bool));
                dt.Columns.Add("ActualValue", typeof(bool));
            }
            else {*/
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("ActualValue", typeof(string));
            //}
            //dt.Columns.Add("Editable", typeof(bool));
            dt.Columns.Add("MinValue", typeof(string));
            dt.Columns.Add("MaxValue", typeof(string));

            dt.Columns["PropertyId"].ReadOnly = true;
            dt.Columns["PropertyDescription"].ReadOnly = true;
            dt.Columns["ActualValue"].ReadOnly = true;
            _userLevel = (int)userLevel;


            // Carica solo le righe visibili
            view_parameters = new List<ViewParameter>();
            //for (int i = 0; i < parameters.Count; i++) {
            //    T param = parameters[i];
            //    //if (param.IsVisible <= _userLevel) {
            //    if (param.IsVisible > 0) {
            //        T aParam = null;
            //        if (actualParameters != null)
            //            aParam = actualParameters.FindById<T>(param.Id);
            //        if (aParam != null)
            //            _parameters.Add(new ViewParameter() { Parameter = param, ActualParameter = aParam });
            //        //}
            //    }
            //}
            if (actualParameters != null) {
                for (int i = 0; i < actualParameters.Count; i++) {
                    Parameter actual_param = actualParameters[i];
                    //if (param.IsVisible <= _userLevel) {
                    if (actual_param.IsVisible > 0) {
                        Parameter param = null;
                        if (parameters != null)
                            param = parameters.FindById<Parameter>(actual_param.Id);
                        if (param != null) {
                            view_parameters.Add(new ViewParameter() { Parameter = param as T, ActualParameter = actual_param as T});
                        }
                    }
                }
            }
        }
    }
}
