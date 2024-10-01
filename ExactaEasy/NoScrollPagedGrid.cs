﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasy.DAL;
using System.Globalization;
using System.Threading;
using SPAMI.Util.Logger;

namespace ExactaEasy {

    public partial class NoScrollPagedGrid : UserControl {

        public event DataErrorEventHandler DataError;
        public event DataGridViewCellCancelEventHandler BeginEdit;
        public event DataGridViewCellCancelEventHandler EndEdit;

        IDataBinder dataSource;
        int currentPage = 0;
        int visibleRowsCount = 0;
        int pageCount = 0;
        bool isEditing = false;
        bool disablePrevNextBtn = false;
        DataGridViewDisableImageColumn plusColumn;
        DataGridViewDisableImageColumn minusColumn;
        DataTable logDataTable;
        //bool ROiTab = false;
        //int numberOfPageforROI = 0;
        //int numberOfRow = 8;

        //public event EventHandler ROIChange;

        //public int currentROI 
        //{
        //    get
        //    {
        //        return currentPage;
        //    }
        //}

        public IDataBinder DataSource {
            get {
                return dataSource;
            }
            set {
                dataSource = value;
                createColumns();
                checkDifferences();
            }
        }

        public string MinValueColumnName { get; set; }
        public string MaxValueColumnName { get; set; }

        public string Title {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public bool ShowMinMaxInfo {
            get { return tableLayoutPanelMinMax.Visible; }
            set { tableLayoutPanelMinMax.Visible = value; }
        }

        public bool ShowTitleBar {
            get { return lblTitle.Visible; }
            set { lblTitle.Visible = value; }
        }

        public bool ShowLogInfo {
            get { return pnlInspLog.Visible; }
            set { pnlInspLog.Visible = value; }
        }

        DataGridViewComboBoxCell defaultComboCell;

        public NoScrollPagedGrid() {
            InitializeComponent();

            dataGridView1.VirtualMode = true;
            dataGridView1.AllowUserToResizeRows = false;

            dataGridView1.SizeChanged += new EventHandler(dataGridView1_SizeChanged);
            dataGridView1.CellValueNeeded += new DataGridViewCellValueEventHandler(dataGridView1_CellValueNeeded);
            dataGridView1.CellValuePushed += new DataGridViewCellValueEventHandler(dataGridView1_CellValuePushed);
            dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
            //dataGridView1.CellEnter += new DataGridViewCellEventHandler(dataGridView1_CellEnter);
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellBeginEdit);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.CellParsing += new DataGridViewCellParsingEventHandler(dataGridView1_CellParsing);
            dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);

            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DataGridViewCellStyle hStyle = new DataGridViewCellStyle();
            hStyle.BackColor = Color.Gray;
            hStyle.ForeColor = Color.White;
            hStyle.Font = new Font("Nirmala UI", 12, FontStyle.Bold);
            DataGridViewCellStyle cStyle = new DataGridViewCellStyle();
            cStyle.BackColor = Color.FromArgb(240, 240, 240);
            cStyle.ForeColor = Color.Black;
            cStyle.Font = new Font("Nirmala UI", 12, FontStyle.Bold);
            DataGridViewCellStyle caStyle = new DataGridViewCellStyle();
            caStyle.BackColor = Color.LightGray;
            caStyle.ForeColor = Color.Black;
            caStyle.Font = new Font("Nirmala UI", 12, FontStyle.Bold);

            dataGridView1.ColumnHeadersDefaultCellStyle = hStyle;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.RowsDefaultCellStyle = cStyle;
            dataGridView1.AlternatingRowsDefaultCellStyle = caStyle;
            dataGridView1.EnableHeadersVisualStyles = false;

            defaultComboCell = new DataGridViewComboBoxCell();
            defaultComboCell.FlatStyle = FlatStyle.Flat;
            defaultComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            defaultComboCell.Style.Font = new Font("Nirmala UI", 12, FontStyle.Bold);

            plusColumn = new DataGridViewDisableImageColumn();
            plusColumn.Name = "PLUS";
            plusColumn.HeaderText = "";
            plusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            plusColumn.Image = global::ExactaEasy.Properties.Resources.list_add;
            plusColumn.Resizable = DataGridViewTriState.False;
            //plusColumn.FlatStyle = FlatStyle.System;
            //plusColumn.Text = "+";
            //plusColumn.UseColumnTextForButtonValue = true;
            plusColumn.Width = 38;
            plusColumn.DefaultCellStyle.Font = new Font("Nirmala UI", 12, FontStyle.Bold);
            plusColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            minusColumn = (DataGridViewDisableImageColumn)plusColumn.Clone();
            minusColumn.Name = "MINUS";
            minusColumn.Image = global::ExactaEasy.Properties.Resources.list_remove;


            logDataTable = new DataTable();
            logDataTable.Columns.Add("LogEntryTypeImage", typeof(Image));
            logDataTable.Columns.Add("EntryLevel", typeof(string));
            //logDataTable.Columns.Add("LogTime", typeof(DateTime));
            logDataTable.Columns.Add("EntryMessage", typeof(string));
            inspLogGridView.Columns["LogEntryTypeImage"].DataPropertyName = "LogEntryTypeImage";
            inspLogGridView.Columns["EntryLevel"].DataPropertyName = "EntryLevel";
            inspLogGridView.Columns["EntryMessage"].DataPropertyName = "EntryMessage";

            // stringhe
            //lblMinValue.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase((frmBase.UIStrings.GetString("MinValue")).ToLower());
            //lblMaxValue.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase((frmBase.UIStrings.GetString("MaxValue")).ToLower());
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {

            if (DataSource.ChangedData(e.RowIndex)) {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                //e.CellStyle.BackColor = Color.Red;
                e.FormattingApplied = true;
            }
            else {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
                e.FormattingApplied = true;
            }
            //if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Parameter") {
            //    var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //    string[] infos = DataSource.GetParamInfo(e.RowIndex);
            //    toolTipDesc.ToolTipTitle = infos[0];
            //    string text = "";
            //    for (int i = 1; i< infos.Length; i++) {
            //        text += infos[i] + "\n";
            //    }
            //    toolTipDesc.Show(text, dataGridView1, 10000);
            //    //cell.ToolTipText = "";
            //    //foreach (string info in infos) {
            //    //    cell.ToolTipText += info + "\n";
            //    //}

            //}
            //e.ColumnIndex = "";
        }

        private System.Diagnostics.Process _pOSK = null;

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {

            DataGridViewCellCancelEventArgs eArgs = new DataGridViewCellCancelEventArgs(e.ColumnIndex, e.RowIndex);
            OnEndEdit(this, eArgs);
            if (!eArgs.Cancel) {
                //lblError.Visible = false;
                isEditing = false;
                if (_pOSK != null) {
                    try {
                        _pOSK.Kill();
                    }
                    catch {
                    }
                    finally {
                        _pOSK.Dispose();
                        _pOSK = null;
                    }
                }
            }
            else {
                dataGridView1.BeginEdit(true);
            }
        }

        void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {

            OnBeginEdit(this, e);
            if (!e.Cancel) { // Se non è già stata richiesta la cancellazione dell'evento, prosegui
                if (DataSource.EditableData(e.RowIndex)) {
                    isEditing = true;
                    if ((dataGridView1[e.ColumnIndex, e.RowIndex].GetType() != typeof(DataGridViewComboBoxCell)) && (_pOSK == null)) {
                        _pOSK = System.Diagnostics.Process.Start("osk.exe");
                    }
                }
                else {
                    e.Cancel = true;
                }
            }
        }

        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {

            if (isEditing && !DataSource.ValidatingData(e.RowIndex, e.FormattedValue.ToString())) {
                OnDataError(this, new DataErrorEventArgs(e.FormattedValue + " is not a valid value!"));
                //lblError.Text = e.FormattedValue + " is not a valid value!";
                //lblError.Visible = true;
                e.Cancel = true;
            }
        }

        void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e) {

            double test;
            if (Double.TryParse(e.Value.ToString(), out test)) {
                string decSymbol = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (decSymbol != ".") {
                    e.Value = e.Value.ToString().Replace(".", decSymbol);
                }
            }
            e.ParsingApplied = true;
        }

        void OnDataError(object sender, DataErrorEventArgs e) {

            if (DataError != null)
                DataError(sender, e);
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {

            //e.Cancel = true;
        }

        void dataGridView1_SelectionChanged(object sender, EventArgs e) {

            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells.Count > 0) {
                //if (dataGridView1.CurrentRow.Cells[MinValueColumnName].Value != null)
                //lblValoreMin.Text = dataGridView1.CurrentRow.Cells[MinValueColumnName].Value.ToString();
                // if (dataGridView1.CurrentRow.Cells[MaxValueColumnName].Value != null)
                //lblValoreMax.Text = dataGridView1.CurrentRow.Cells[MaxValueColumnName].Value.ToString();
            }
        }

        public void HideColumn(string columnName) {

            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].Visible = false;
        }

        public void SetColumnCaption(string columnName, string caption) {

            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].HeaderText = caption;
        }

        public void SetColumnPercentageWidth(string columnName, int percWidth) {

            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].FillWeight = percWidth;
        }

        public void SetColumnForeColor(string columnName, Color color) {

            if (dataGridView1.Columns.Contains(columnName)) {
                dataGridView1.Columns[columnName].DefaultCellStyle.ForeColor = color;
                int colIndex = dataGridView1.Columns[columnName].Index;
                foreach (DataGridViewRow dgvr in dataGridView1.Rows) {
                    dgvr.Cells[colIndex].Style.ApplyStyle(dataGridView1.Columns[colIndex].DefaultCellStyle);
                }
            }
        }

        public void UpdateData() {

            if (DataSource != null)
                DataSource.UpdateData();
        }

        private void PagedGrid_Load(object sender, EventArgs e) {

            dataGridView1.ShowCellToolTips = false;
            toolTipDesc.AutomaticDelay = 0;
            toolTipDesc.OwnerDraw = true;
            toolTipDesc.ShowAlways = true;
            toolTipDesc.ToolTipTitle = "";
            toolTipDesc.UseAnimation = true;
            toolTipDesc.UseFading = false;
        }

        void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) {

            if ((dataSource.Rows.Count > 0 && e.RowIndex < dataSource.Rows.Count) &&
                (dataSource.Columns.Count > 0 && e.ColumnIndex < dataSource.Columns.Count))
                e.Value = dataSource.Rows[e.RowIndex][e.ColumnIndex].ToString();
        }

        void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e) {

            DataSource.Rows[e.RowIndex][e.ColumnIndex] = e.Value;
            DataSource.UpdateData(e.RowIndex);
        }

        void dataGridView1_SizeChanged(object sender, EventArgs e) {

            recalcGrid();
        }

        private void btnPrev_Click(object sender, EventArgs e) {

            if (currentPage > 0) {
                currentPage--;
                //int newcurrentPage = currentPage;
                //if (ROiTab) //AT/mod+ "Uso i pulsanti di display delle pagine sia per vedere le varie ROI, sia per caricarle"
                //    if (ROIChange != null) ROIChange(this, EventArgs.Empty);
                //currentPage = newcurrentPage;
                refreshData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e) {

            if (currentPage < pageCount - 1) {
                currentPage++;
                //int newcurrentPage = currentPage;
                //if (ROiTab) //AT/mod+ "Uso i pulsanti di display delle pagine sia per vedere le varie ROI, sia per caricarle"
                //    if (ROIChange != null) ROIChange(this, EventArgs.Empty);
                //currentPage = newcurrentPage;
                refreshData();
            }
        }

        void createColumns() {
            if (DataSource != null) {
                dataGridView1.SuspendLayout();
                dataGridView1.Columns.Clear();
                foreach (DataColumn col in DataSource.Columns) {
                    if (col.DataType == typeof(bool)) {
                        DataGridViewCheckBoxColumn boolCol = new DataGridViewCheckBoxColumn();
                        boolCol.Name = col.ColumnName;
                        boolCol.TrueValue = 1;
                        boolCol.FalseValue = 0;
                        boolCol.FlatStyle = FlatStyle.Flat;
                        dataGridView1.Columns.Add(boolCol);
                    }
                    else {
                        //if (col.ColumnName == "Value") {
                        //    dataGridView1.Columns.Add(minusColumn);
                        //}
                        dataGridView1.Columns.Add(col.ColumnName, col.Caption);
                        //if (col.ColumnName == "Value") {
                        //    dataGridView1.Columns.Add(plusColumn);
                        //}
                    }
                    if (col.ReadOnly)
                        dataGridView1.Columns[col.ColumnName].ReadOnly = true;
                    dataGridView1.Columns[col.ColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dataGridView1.Columns.Add(minusColumn);
                dataGridView1.Columns.Add(plusColumn);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                recalcGrid();
                dataGridView1.ResumeLayout();
            }
        }

        void checkDifferences() {
            if (DataSource != null) {
                
            }
        }

        int calcNumOfVisibleRows() {
            int rowNum = 0;
            rowNum = (int)Math.Floor((double)(dataGridView1.DisplayRectangle.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.RowTemplate.Height);
            return rowNum;
        }

        int calcNumOfPages(int rowsPerPage) {
            int pageCount = 0;
            if (DataSource != null) {
                //if (ROiTab) //AT/mod+ "Se sto usando le ROI mostro un numero di pagine pari al numero di ROI che ci sono in camera"
                //    pageCount = numberOfPageforROI;
                //else
                pageCount = (int)Math.Ceiling((double)DataSource.TotalRowCount / rowsPerPage);
                if (pageCount < 2) disablePrevNextBtn = true;
            }
            return pageCount;
        }

        void recalcGrid() {
            visibleRowsCount = calcNumOfVisibleRows();
            disablePrevNextBtn = false;
            pageCount = calcNumOfPages(visibleRowsCount);
            btnNext.Enabled = btnPrev.Enabled = !disablePrevNextBtn;    //PD041013: nascondo frecce se ho solo una pagina di parametri
            currentPage = 0;
            refreshData();
        }

        void refreshData() {

            if (dataGridView1.Columns.Count > 0) {
                DataSource.GetData(currentPage * visibleRowsCount, visibleRowsCount);
                dataGridView1.RowCount = DataSource.Rows.Count;
                List<string> editableRows = new List<string>();
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                    if (!col.ReadOnly && col.Visible)
                        editableRows.Add(col.Name);
                for (int i = 0; i < dataGridView1.RowCount; i++) {
                    foreach (string colName in editableRows) {
                        string[] cellValues = DataSource.GetCellValues(colName, currentPage * visibleRowsCount, i);
                        if (cellValues != null && cellValues.Length > 0) {
                            DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)defaultComboCell.Clone();
                            foreach (string val in cellValues)
                                comboCell.Items.Add(val);
                            dataGridView1[colName, i] = comboCell;
                            ((DataGridViewDisableImageCell)this.dataGridView1.Rows[i].Cells["PLUS"]).Hide = true;
                            ((DataGridViewDisableImageCell)this.dataGridView1.Rows[i].Cells["MINUS"]).Hide = true;
                        }
                        if ((cellValues == null || cellValues.Length == 0) && DataSource.GetValueType(i) == "string") {
                            ((DataGridViewDisableImageCell)this.dataGridView1.Rows[i].Cells["PLUS"]).Hide = true;
                            ((DataGridViewDisableImageCell)this.dataGridView1.Rows[i].Cells["MINUS"]).Hide = true;
                        }
                    }
                }
            }
            dataGridView1.Refresh();
        }

        protected virtual void OnBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {

            if (BeginEdit != null)
                BeginEdit(sender, e);
        }

        protected virtual void OnEndEdit(object sender, DataGridViewCellCancelEventArgs e) {

            if (EndEdit != null)
                EndEdit(sender, e);
        }

        private void toolTipDesc_Draw(object sender, DrawToolTipEventArgs e) {

            //SolidBrush backColor = new SolidBrush(SystemColors.Info);
            //SolidBrush foreColor = new SolidBrush(SystemColors.InfoText);
            //e.Graphics.FillRectangle(backColor, e.Bounds);
            //e.Graphics.DrawRectangle(Pens.Chocolate, new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1));
            //Font drawFont = dataGridView1.RowsDefaultCellStyle.Font;
            //e.Graphics.DrawString(this.toolTipDesc.ToolTipTitle + "\n" + e.ToolTipText, drawFont/*e.Font*/, foreColor, e.Bounds);

            e.DrawBackground();
            e.DrawBorder();
            Font regFont = new Font(dataGridView1.RowsDefaultCellStyle.Font, FontStyle.Regular);
            Font boldFont = new Font(dataGridView1.RowsDefaultCellStyle.Font, FontStyle.Bold);
            SolidBrush b = new SolidBrush(SystemColors.InfoText);
            string[] rows = e.ToolTipText.Split('\n');
            float[] rowHeight = new float[rows.Length];
            for (int r = 0; r < rows.Length; r++) {
                SizeF s = e.Graphics.MeasureString(rows[r], boldFont);
                if (s.Height > rowHeight[r])
                    rowHeight[r] = s.Height;
            }
            float y = (e.Bounds.Height - rowHeight.Sum()) / 2;
            float x = 3;
            for (int r = 0; r < rows.Length; r++) {
                Font currFont = (r==2 || r==4 || r>=6) ? boldFont : regFont;    //PIER: NUMERI MAGICI...
                e.Graphics.DrawString(rows[r], currFont, b, x, y);
                y += e.Graphics.MeasureString(rows[r], currFont).Height;
            }
            b.Dispose();
            regFont.Dispose();
            boldFont.Dispose();
        }

        private void toolTipDesc_Popup(object sender, PopupEventArgs e) {

            string[] rows = toolTipDesc.GetToolTip(e.AssociatedControl).Split('\n');
            int width = 0;
            int height = 0;
            Font boldFont = new Font(dataGridView1.RowsDefaultCellStyle.Font, FontStyle.Bold);
            using (Graphics g = Graphics.FromHwnd(this.Handle)) {

                float[] rowHeight = new float[rows.Length];
                float colWidth = 0;
                for (int r = 0; r < rows.Length; r++) {
                    SizeF s = g.MeasureString(rows[r], boldFont);
                    if (s.Height > rowHeight[r])
                        rowHeight[r] = s.Height;
                    if (s.Width > colWidth)
                        colWidth = s.Width;
                }
                width = (int)colWidth + 10;
                height = (int)rowHeight.Sum() + 10;
            }
            e.ToolTipSize = new Size(width, height);
            boldFont.Dispose();
        }

        private int cellColumnIndex = -1, cellRowIndex = -1;

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {

        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {

            if (e.ColumnIndex != cellColumnIndex && e.RowIndex != cellRowIndex) {
                toolTipDesc.Hide(this.dataGridView1);
            }
            cellColumnIndex = -1;
            cellRowIndex = -1;
        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e) {

            //toolTipDesc.Hide(this.dataGridView1);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e) {

            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1);
            if (dataGridView1[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell && validClick == true) {
                dataGridView1.BeginEdit(true);
                ((ComboBox)dataGridView1.EditingControl).DroppedDown = true;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

            if (e.ColumnIndex != cellColumnIndex || e.RowIndex != cellRowIndex) {

                toolTipDesc.Hide(dataGridView1);
                cellColumnIndex = e.ColumnIndex;
                cellRowIndex = e.RowIndex;

                if (cellColumnIndex >= 0 && cellRowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "PropertyDescription") {

                    Point mousePos = PointToClient(MousePosition);
                    string[] infos = DataSource.GetParamInfo(e.RowIndex);
                    toolTipDesc.ToolTipTitle = infos[0];
                    string text = "";
                    for (int i = 1; i < infos.Length; i++) {
                        text += infos[i] + "\n";
                    }
                    toolTipDesc.Show(text, dataGridView1, mousePos);
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

            if ((dataGridView1.Columns[e.ColumnIndex].Name == "PLUS" || dataGridView1.Columns[e.ColumnIndex].Name == "MINUS") &&
                (dataGridView1[e.ColumnIndex, e.RowIndex] is DataGridViewDisableImageCell) && (dataGridView1[e.ColumnIndex, e.RowIndex] as DataGridViewDisableImageCell).Hide == false) {
                //MessageBox.Show("\"" + dataGridView1.Columns[e.ColumnIndex].Name + "\" BUTTON CLICKED!!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //dataGridView1_CellBeginEdit(sender, new DataGridViewCellCancelEventArgs(dataGridView1.Columns["Value"].Index, e.RowIndex);
                //dataGridView1[dataGridView1.Columns["Value"].Index, e.RowIndex].Value = dataGridView1[dataGridView1.Columns["Value"].Index, e.RowIndex].Value + sign * 
                int valueColIndex = dataGridView1.Columns["Value"].Index;
                DataGridViewCellCancelEventArgs cancelEv = new DataGridViewCellCancelEventArgs(valueColIndex, e.RowIndex);
                OnBeginEdit(this, cancelEv);
                if (cancelEv.Cancel == false && DataSource.EditableData(e.RowIndex) == true) { // Se non è già stata richiesta la cancellazione dell'evento, prosegui
                    int sign = (dataGridView1.Columns[e.ColumnIndex].Name == "PLUS") ? 1 : -1;
                    string newValue = "";
                    if (DataSource.ValidatingIncrementData(e.RowIndex, sign, ref newValue) == true) {

                        DataSource.Rows[e.RowIndex][dataGridView1.Columns["Value"].Index] = newValue;
                        DataSource.UpdateData(e.RowIndex);
                    }
                    else {
                        OnDataError(this, new DataErrorEventArgs(newValue + " is not a valid value!"));
                    }
                }
                DataGridViewCellCancelEventArgs eArgs = new DataGridViewCellCancelEventArgs(valueColIndex, e.RowIndex);
                OnEndEdit(this, eArgs);
                //if (!eArgs.Cancel) {
                //    //lblError.Visible = false;
                //    isEditing = false;
                //}
                //else {
                //    dataGridView1.BeginEdit(true);
                //}
                dataGridView1.Refresh();
            }
        }
    }
}
