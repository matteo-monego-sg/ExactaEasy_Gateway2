using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace DictionaryEditorUI {
    public partial class DictionaryEditor : UserControl {

        DataTable dictDataTable;
        int infoColumnCount, descColumnCount;

        ParameterInfoCollection currParamInfoCollection;
        public ParameterInfoCollection CurrParamInfoCollection {
            set {
                currParamInfoCollection = value;
                buildDictDataTable();
                populateDictDataTable();
            }
            get {
                retrieveDictDataTable();
                return currParamInfoCollection;
            }
        }

        public DictionaryEditor() {

            InitializeComponent();

        }

        public void AddColumn() {
            //dgvDict.Col
        }

        public void PasteClipboard() {

            try {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = dgvDict.CurrentCell.RowIndex;
                int iCol = dgvDict.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines) {
                    if (iRow < dgvDict.RowCount && line.Length > 0) {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i) {
                            if (iCol + i < this.dgvDict.ColumnCount) {
                                oCell = dgvDict[iCol + i, iRow];
                                if (!oCell.ReadOnly) {
                                    if (oCell.Value.ToString() != sCells[i]) {
                                        oCell.Value = Convert.ChangeType(sCells[i],
                                                              oCell.ValueType);
                                        //oCell.Style.BackColor = Color.Tomato;
                                    }
                                    else
                                        iFail++;
                                    //only traps a fail if the data has changed 
                                    //and you are pasting into a read only cell
                                }
                            }
                            else { break; }
                        }
                        iRow++;
                    }
                    else { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due" +
                                        " to read only column setting", iFail));
                }
            }
            catch (FormatException) {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        void buildDictDataTable() {

            infoColumnCount = descColumnCount = 0;
            //datatable
            dictDataTable = new DataTable();
            if (currParamInfoCollection != null && currParamInfoCollection.Count > 0) {
                ParameterInfo pi = currParamInfoCollection[0];
                dictDataTable.Columns.Add("Id", typeof(string));
                dictDataTable.Columns.Add("Type", typeof(string));
                foreach (ParameterInfoLocalized pil in pi.LocalizedInfo) {
                    dictDataTable.Columns.Add("Info " + pil.CultureCode, typeof(string));
                    infoColumnCount++;
                }
                foreach (ParameterDescriptionLocalized pdl in pi.LocalizedDescription) {
                    dictDataTable.Columns.Add("Description " + pdl.CultureCode, typeof(string));
                    descColumnCount++;
                }
            }
            //datagridview
            foreach (DataGridViewColumn dgvCol in dgvDict.Columns) {
                dgvCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            //dictDataTable.Columns.Add("ModuleName", typeof(string));
            //dictDataTable.Columns.Add("Version", typeof(string));
            //dictDataTable.Columns["ModuleName"].DataPropertyName = "ModuleName";
            //dictDataTable.Columns["Version"].DataPropertyName = "Version";
            //dictDataTable.Columns["ModuleName"].HeaderText = frmBase.UIStrings.GetString("ModuleName");
            //dictDataTable.Columns["Version"].HeaderText = frmBase.UIStrings.GetString("Version");
        }

        void populateDictDataTable() {

            if (currParamInfoCollection != null) {
                dictDataTable.Clear();

                foreach (ParameterInfo pi in currParamInfoCollection) {
                    List<string> newLine = new List<string>();
                    newLine.Add(pi.Id);
                    newLine.Add(pi.ValueType);
                    int colOffset = newLine.Count;
                    for (int i = 0; i < infoColumnCount; i++) {
                        bool found = false;
                        string colName = dictDataTable.Columns[colOffset + i].ColumnName;
                        //if (!colName.StartsWith("Info "))
                        //    continue;
                        foreach (ParameterInfoLocalized pil in pi.LocalizedInfo) {
                            if (colName.EndsWith(pil.CultureCode)) {
                                found = true;
                                newLine.Add(pil.Label);
                            }
                        }
                        if (!found)
                            newLine.Add("");
                    }
                    colOffset += infoColumnCount;
                    for (int i = 0; i < descColumnCount; i++) {
                        bool found = false;
                        string colName = dictDataTable.Columns[colOffset + i].ColumnName;
                        //if (!colName.StartsWith("Info "))
                        //    continue;
                        foreach (ParameterDescriptionLocalized pdl in pi.LocalizedDescription) {
                            if (colName.EndsWith(pdl.CultureCode)) {
                                found = true;
                                newLine.Add(pdl.Label);
                            }
                        }
                        if (!found)
                            newLine.Add("");
                    }
                    //foreach (ParameterInfoLocalized pil in pi.LocalizedInfo)
                    //    newLine.Add(pil.Label);
                    //foreach (ParameterDescriptionLocalized pdl in pi.LocalizedDescription)
                    //    newLine.Add(pdl.Label);
                    dictDataTable.LoadDataRow(newLine.ToArray<object>(), true);
                }
                dgvDict.DataSource = dictDataTable;
                //dgvDict.Sort(dgvDict.Columns["ModuleName"], ListSortDirection.Ascending);
            }
        }

        void retrieveDictDataTable() {

            if (dictDataTable != null) {
                ParameterInfoCollection newParamInfoCollection = new ParameterInfoCollection();
                foreach (DataRow dr in dictDataTable.AsEnumerable()) {
                    if (dr.RowState == DataRowState.Deleted)
                        continue;
                    ParameterInfo newParamInfo = new ParameterInfo();
                    for (int i = 0; i < dictDataTable.Columns.Count; i++) {
                        if (dictDataTable.Columns[i].ColumnName == "Id")
                            newParamInfo.Id = dr[i].ToString();
                        if (dictDataTable.Columns[i].ColumnName == "Type")
                            newParamInfo.ValueType = dr[i].ToString();
                        if (dictDataTable.Columns[i].ColumnName.StartsWith("Info ")) {
                            if (newParamInfo.LocalizedInfo == null)
                                newParamInfo.LocalizedInfo = new ParameterInfoLocalizedCollection();
                            ParameterInfoLocalized pil = new ParameterInfoLocalized();
                            pil.CultureCode = dictDataTable.Columns[i].ColumnName.Replace("Info ", "");
                            pil.Label = dr[i].ToString();
                            newParamInfo.LocalizedInfo.Add(pil);
                        }
                        if (dictDataTable.Columns[i].ColumnName.StartsWith("Description ")) {
                            if (newParamInfo.LocalizedDescription == null)
                                newParamInfo.LocalizedDescription = new ParameterDescriptionLocalizedCollection();
                            ParameterDescriptionLocalized pdl = new ParameterDescriptionLocalized();
                            pdl.CultureCode = dictDataTable.Columns[i].ColumnName.Replace("Description ", "");
                            pdl.Label = dr[i].ToString();
                            newParamInfo.LocalizedDescription.Add(pdl);
                        }
                    }
                    if (newParamInfo != null)
                        newParamInfoCollection.Add(newParamInfo);
                }
                currParamInfoCollection = newParamInfoCollection;
            }
        }
    }
}
