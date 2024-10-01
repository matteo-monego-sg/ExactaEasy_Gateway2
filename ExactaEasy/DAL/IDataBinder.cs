using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ExactaEasyEng;

namespace ExactaEasy.DAL {

    public interface IDataBinder {

        int TotalRowCount { get; }
        DataColumnCollection Columns { get; }
        DataRowCollection Rows { get; }
        UserLevelEnum UserLevel { get; set; }

        void GetData(int startRow, int numberOfRows);
        string[] GetCellValues(string columnName, int startRow, int row);
        void UpdateData();
        void UpdateData(int rowIndex);
        bool ValidatingData(int rowIndex, string newValue);
        bool EditableData(int rowIndex);
        bool ChangedData(int rowIndex);
        bool VerifyValue(int rowIndex);
        string GetValueType(int rowIndex);
        string[] GetParamInfo(int rowIndex);
        bool ValidatingIncrementData(int rowIndex, int sign, ref string newValue);
    }
}
