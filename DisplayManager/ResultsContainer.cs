using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Concurrent;
using ExactaEasyCore;
using ExactaEasyEng;
using SPAMI.Util.Logger;
using System.Globalization;
using ExactaEasyCore.TrendingTool;

namespace DisplayManager {

    public abstract class MeasuresContainer {

        public string Name { get; private set; }
        public StationCollection SourceStations { get; private set; }
        //public StatisticCollection<MeasureId> MeasureStatistics { get; protected set; }
        public Recipe CurrRecipe { get; set; }
        public bool Recording { get; private set; }
        public double BufferSizePerCent { get; private set; }

        protected MeasuresContainer(string name, StationCollection sourceStations/*, VisionSystemConfig visionSystemConfig*/) {

            Name = name;
            SourceStations = sourceStations;
            BufferSizePerCent = 0;
            foreach (var station in sourceStations) {
                station.MeasuresAvailable += station_MeasuresAvailable;
                station.RecBufferState += station_RecBufferState;
            }
        }

        bool disposed = false;
        public virtual void Dispose() {

            disposed = true;
            foreach (var station in SourceStations) {
                station.MeasuresAvailable -= station_MeasuresAvailable;
                station.RecBufferState -= station_RecBufferState;
            }
        }

        void station_MeasuresAvailable(object sender, MeasuresAvailableEventArgs e) {

            try {
                if (!disposed)
                    PushMeasures((IStation)sender, e);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "MeasuresContainer.station_MeasuresAvailable", "Error: " + ex.Message);
            }
        }

        void station_RecBufferState(object sender, RecordStateEventArgs e) {

            try {
                if (!disposed) {
                    Recording = e.Recording;
                    BufferSizePerCent = e.BufferSizePerCent;
                }
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "MeasuresContainer.station_RecBufferState", "Error: " + ex.Message);
            }
        }

        public virtual void PushMeasures(IStation currentStation, MeasuresAvailableEventArgs newMeasure) {

            //InspectionResults inspectionRes = newMeasure.InspectionResults;
            //foreach (var toolRes in inspectionRes.ResToolCollection)
            //{
            //    foreach (var measureRes in toolRes.ResMeasureCollection)
            //    {
            //        if (!measureRes.IsUsed || measureRes.MeasureValue.Length == 0)
            //        {
            //            continue;
            //        }
            //        var measureId = new MeasureId(currentStation.NodeId, currentStation.IdStation, toolRes.ToolId, measureRes.MeasureId, currentStation);
            //        MeasureStatistics.Update(measureId, measureRes);
            //    }
            //}
        }

        public virtual void ResetStatistics() {

            //MeasureStatistics.Clear();
        }

        protected virtual string TruncStringValue(string value, int decimalPlaces) {

            //System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
            var trunc = value.LastIndexOf(".", StringComparison.Ordinal) + decimalPlaces;
            if (trunc > 1)
                value = value.Substring(0, Math.Min(trunc + 1, value.Length));
            return value;
        }
    }

    public class MeasuresContainerCollection : List<MeasuresContainer> {

        public MeasuresContainer this[string containerName] {
            get {
                return this.Find(mc => { return mc.Name == containerName; });
            }
        }
    }

    public class MeasuresFile : MeasuresContainer {
        readonly string _folder;
        readonly string _separator;
        FileStream _resultsFileStream;   //pier: uso filestream per avere più opzioni sul file (in particolare sullo share mode)
        readonly UnicodeEncoding _uniEncoding = new UnicodeEncoding();

        public MeasuresFile(string name, string folder, string separator, IStation station/*, VisionSystemConfig visionSystemConfig*/)
            : base(name, new StationCollection { station }/*, visionSystemConfig*/) {

            _folder = folder;
            _separator = separator;
        }

        public MeasuresFile(string name, string folder, string separator, StationCollection sourceStations/*, VisionSystemConfig visionSystemConfig*/)
            : base(name, sourceStations/*, visionSystemConfig*/) {

            _folder = folder;
            _separator = separator;
        }

        void Init(IStation currentStation) {

            if (!Directory.Exists(_folder))
                throw new Exception("Path does not exists");
            //resultsFileStream = new FileStream(folder + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + "Station_" + currentStation.IdStation.ToString("d2") + ".csv", );
            _resultsFileStream = new FileStream(_folder + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + "Node_" + currentStation.NodeId + "_Station_" + currentStation.IdStation.ToString("d2") + ".csv", FileMode.Append, FileAccess.Write, FileShare.None);
            //resultsStream = new StreamWriter(folder + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + "Station_" + currentStation.IdStation.ToString("d2") + ".csv");
            //resultsStream.AutoFlush = true;
            string header =
                //UIStrings.GetString("NodeId") + separator +
                VisionSystemManager.UIStrings.GetString("SpindleId") + _separator +
                VisionSystemManager.UIStrings.GetString("VialId") + _separator +
                VisionSystemManager.UIStrings.GetString("Description") + _separator +
                //UIStrings.GetString("ToolId") + separator +
                //UIStrings.GetString("MeasureId") + separator +
                VisionSystemManager.UIStrings.GetString("Measure") + _separator +
                VisionSystemManager.UIStrings.GetString("Used") + _separator +
                VisionSystemManager.UIStrings.GetString("Value") + _separator +
                VisionSystemManager.UIStrings.GetString("UM") + _separator +
                VisionSystemManager.UIStrings.GetString("Mean") + _separator +
                VisionSystemManager.UIStrings.GetString("StdDev") + _separator +
                VisionSystemManager.UIStrings.GetString("SampleSize") + _separator +
                VisionSystemManager.UIStrings.GetString("RejectedPerCent") + _separator + "\r\n";

            _resultsFileStream.Write(_uniEncoding.GetBytes(header), 0, _uniEncoding.GetByteCount(header));
        }

        public override void Dispose() {

            if (_resultsFileStream != null) {
                _resultsFileStream.Flush();
                _resultsFileStream.Close();
            }
            base.Dispose();
        }

        public override void PushMeasures(IStation currentStation, MeasuresAvailableEventArgs newMeasures) {

            base.PushMeasures(currentStation, newMeasures);
            if (currentStation.DumpResultsEnabled) {
                if (_resultsFileStream == null) {
                    Init(currentStation);
                }
                string newLine = "";
                InspectionResults inspectionRes = newMeasures.InspectionResults;
                foreach (ToolResults toolRes in inspectionRes.ResToolCollection) {
                    foreach (MeasureResults measureRes in toolRes.ResMeasureCollection) {
                        //var measureId = new MeasureId(currentStation.NodeId, currentStation.IdStation, toolRes.ToolId, measureRes.MeasureId, currentStation);
                        //Statistic<MeasureId> ms = MeasureStatistics[measureId];
                        //newLine += inspectionRes.SpindleId + _separator +
                        //    inspectionRes.VialId + _separator;
                        //newLine += measureId.Description + _separator +
                        //    measureRes.MeasureName + _separator +
                        //    measureRes.IsUsed + _separator +
                        //    measureRes.MeasureValue + _separator +
                        //    measureRes.MeasureUnit + _separator;
                        //if (ms != null && measureRes.IsUsed) {
                        //    newLine += ms.currentMean.ToString(CultureInfo.InvariantCulture) + _separator +
                        //        ms.currentStdDev.ToString(CultureInfo.InvariantCulture) + _separator +
                        //        ms.SamplesNum + _separator +
                        //        ms.RejectPercent + _separator;
                        //}
                        newLine += "\r\n";
                    }
                }
                //newLine =
                //    currentStation.IdStation.ToString() + separator +
                //    newMeasure.ToolLabel + separator +
                //    newMeasure.MeasureLabel + separator +
                //    newMeasure.Value.ToString() + separator +
                //    newMeasure.RejectCause.ToString();
                if (_resultsFileStream != null && newLine.Length > 0) {
                    //resultsFileStream.Write(newLines);
                    _resultsFileStream.Write(_uniEncoding.GetBytes(newLine), 0, _uniEncoding.GetByteCount(newLine));
                }
            }
            else {
                if (_resultsFileStream != null) {
                    _resultsFileStream.Flush();
                    _resultsFileStream.Close();
                    _resultsFileStream = null;
                }
            }
        }
    }

    //public class GridViewMeasuresDisplay : MeasuresContainer {
    //    readonly DataGridView _dataGrid;
    //    public BindingList<Statistic<MeasureId>> MeasureStatisticsUi { get; protected set; }

    //    public GridViewMeasuresDisplay(string name, DataGridView dataGrid, StationCollection sourceStations/*, VisionSystemConfig visionSystemConfig*/)
    //        : base(name, sourceStations/*, visionSystemConfig*/) {

    //        _dataGrid = dataGrid;
    //        InitializeDataGrid();
    //        //if (dataGrid.Rows.Count < rowStartIndex + rowCount) {
    //        //    throw new Exception("DataGridView has wrong number of rows");
    //        //}
    //        //DataGridViewRowCollection toolRows = (DataGridViewRowCollection)dataGrid.Rows
    //        //    .Cast<DataGridViewRow>()
    //        //    .Where(r => ((r.Index >= rowStartIndex) && (r.Index < rowStartIndex + rowCount)));
    //    }

    //    int _referenceIdColumn;
    //    void InitializeDataGrid() {

    //        if (_dataGrid.InvokeRequired) {
    //            _dataGrid.Invoke(new MethodInvoker(InitializeDataGrid));
    //        }
    //        else {
    //            _dataGrid.DataSource = MeasureStatisticsUi;
    //            _dataGrid.AutoGenerateColumns = false;
    //            _dataGrid.MultiSelect = false;
    //            _dataGrid.ReadOnly = true;
    //            _dataGrid.AllowUserToAddRows = false;
    //            _dataGrid.AllowUserToDeleteRows = false;
    //            //dataGrid.ScrollBars = ScrollBars.Both;

    //            var idColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "ID",
    //                HeaderText = @"ID HIDDEN",
    //                Visible = false,
    //                SortMode = DataGridViewColumnSortMode.Automatic
    //            };
    //            _dataGrid.Columns.Add(idColumn);

    //            var idLabelColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "IDDesc",
    //                HeaderText = @"ID",
    //                MinimumWidth = 100,
    //                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
    //            };
    //            _dataGrid.Columns.Add(idLabelColumn);
    //            _referenceIdColumn = _dataGrid.Columns.Count - 1;

    //            var nameColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "Name",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("Measure"),
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
    //            };
    //            _dataGrid.Columns.Add(nameColumn);

    //            var usedColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "Used",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("Used"),
    //                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
    //                Width = 50,
    //                Visible = false,
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
    //            };
    //            _dataGrid.Columns.Add(usedColumn);

    //            var valueColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "CurrentValue",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("Value"),
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
    //            };
    //            _dataGrid.Columns.Add(valueColumn);

    //            var umColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "UM",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("UM"),
    //                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
    //                Width = 50,
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
    //            };
    //            _dataGrid.Columns.Add(umColumn);

    //            var samplesNumColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "SamplesNum",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("SampleSize"),
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
    //            };
    //            _dataGrid.Columns.Add(samplesNumColumn);

    //            var currentMeanColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "CurrentMean",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("Mean"),
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
    //            };
    //            _dataGrid.Columns.Add(currentMeanColumn);

    //            var currentStdDevColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "CurrentStdDev",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("StdDev"),
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
    //            };
    //            _dataGrid.Columns.Add(currentStdDevColumn);

    //            var rejectCauseColumn = new DataGridViewTextBoxColumn {
    //                DataPropertyName = "RejectPercent",
    //                HeaderText = VisionSystemManager.UIStrings.GetString("RejectedPerCent"),
    //                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
    //            };
    //            _dataGrid.Columns.Add(rejectCauseColumn);

    //            //dataGrid.Refresh();
    //            foreach (DataGridViewColumn col in _dataGrid.Columns) {
    //                col.Frozen = false;
    //                //col.SortMode = DataGridViewColumnSortMode.NotSortable;
    //            }
    //        }
    //    }

    //    public override void ResetStatistics() {

    //        if (_dataGrid == null) return;
    //        if (_dataGrid.InvokeRequired) {
    //            _dataGrid.Invoke(new MethodInvoker(ResetStatistics));
    //        }
    //        else {
    //            MeasureStatisticsUi.Clear();
    //            RefreshDataGrid();
    //            base.ResetStatistics();
    //        }
    //    }
    //    void RefreshDataGrid() {
    //        if (_dataGrid == null) return;
    //        if (_dataGrid.InvokeRequired) {
    //            _dataGrid.Invoke(new MethodInvoker(RefreshDataGrid));
    //        }
    //        else {
    //            var rowOffset = _dataGrid.FirstDisplayedScrollingRowIndex;
    //            var colOffset = _dataGrid.FirstDisplayedScrollingColumnIndex;
    //            var selectedRow = -1;
    //            var selectedCol = -1;
    //            if (_dataGrid.CurrentCell != null)
    //                selectedRow = _dataGrid.CurrentCell.RowIndex;
    //            if (_dataGrid.CurrentCell != null)
    //                selectedCol = _dataGrid.CurrentCell.ColumnIndex;
    //            _dataGrid.DataSource = MeasureStatisticsUi;
    //            if (rowOffset > -1) {
    //                _dataGrid.FirstDisplayedScrollingRowIndex = rowOffset;
    //            }
    //            if (colOffset > -1) {
    //                _dataGrid.FirstDisplayedScrollingColumnIndex = colOffset;
    //            }
    //            if (selectedRow > -1) {
    //                _dataGrid.Rows[selectedRow].Selected = true;
    //                _dataGrid.CurrentCell = _dataGrid.Rows[selectedRow].Cells[_referenceIdColumn];
    //            }
    //            if (selectedCol > -1) {
    //                _dataGrid.Columns[selectedCol].Selected = true;
    //            }
    //        }
    //    }


    //    public override void PushMeasures(IStation currentStation, MeasuresAvailableEventArgs newMeasures) {

    //        base.PushMeasures(currentStation, newMeasures);
    //        MeasureStatisticsUi = new BindingList<Statistic<MeasureId>>(MeasureStatistics.OrderBy(o => o.ID).ToList());
    //        RefreshDataGrid();
    //    }
    //}

    public class GridViewInspectionDisplay : MeasuresContainer {

        NodeRecipe _nodeRecipe;
        Recipe _recipe;
        readonly IResultsGrid _resGrid;
        DataGridViewTextBoxColumn _featureColumn;
        DataGridViewTextBoxColumn _limitColumn;
        DataGridViewTextBoxColumn _resultColumn;
        DataGridViewImageColumn _trendDataSaveColumn; //images
        DataGridViewImageColumn _trendConfigParSavedColumn; //always not visible, use cell value for TrendConfigParSaved instance, use cell tag for [int] btnType
        StationRecipe _stationRecipe;
        readonly IStation _sourceStation;
        readonly List<RejectionCause> _rejectionCauses;
        //bool redrawOnResults = true;
        int prevToolsActiveCount = 0;
        readonly IRecordStatus _recStatus;
        Bitmap _bitmapEmpty;
        Bitmap _bitmapSaveUncheck;
        Bitmap _bitmapSaveUncheckCrossed;
        Bitmap _bitmapSaveCheck;
        Bitmap _bitmapSaveCheckCrossed;
        Bitmap _bitmapSaveError;
        Bitmap _bitmapSaveErrorCrossed;
        bool _isBusy = false;

        public new Recipe CurrRecipe {
            get {
                return _recipe;
            }
            set {
                _recipe = value;
                if (_recipe != null) {
                    _nodeRecipe = _recipe.Nodes.Find(nn => nn.Id == _sourceStation.NodeId);
                    if (_nodeRecipe != null /*&& _sourceStation.IdStation < _nodeRecipe.Stations.Count*/) {
                        _stationRecipe = _nodeRecipe.Stations.Find(ss => ss.Id == _sourceStation.IdStation);
                    }
                }
                addDataGridRows();
            }
        }

        public GridViewInspectionDisplay(string name, IResultsGrid resGrid, IStation srcStation/*, VisionSystemConfig visionSystemConfig*/, List<RejectionCause> rejCauses, IRecordStatus recStatus)
            : base(name, new StationCollection { srcStation }/*, visionSystemConfig*/) {

            _resGrid = resGrid;
            InitializeDataGrid();
            _sourceStation = srcStation;
            _rejectionCauses = rejCauses;
            _recStatus = recStatus;
            _bitmapEmpty = new Bitmap(64, 64);
            _bitmapSaveUncheck = ((Bitmap[])_resGrid.DataGrid.Tag)[0];
            _bitmapSaveUncheckCrossed = ((Bitmap[])_resGrid.DataGrid.Tag)[1];
            _bitmapSaveCheck = ((Bitmap[])_resGrid.DataGrid.Tag)[2];
            _bitmapSaveCheckCrossed = ((Bitmap[])_resGrid.DataGrid.Tag)[3];
            _bitmapSaveError = ((Bitmap[])_resGrid.DataGrid.Tag)[4];
            _bitmapSaveErrorCrossed = ((Bitmap[])_resGrid.DataGrid.Tag)[5];

            _resGrid.DataGrid.CellContentClick += DataGrid_CellContentClick;

            SetVisibleTrendDataSaveColumn();
        }

        public override void Dispose() {

            _resGrid.DataGrid.Dispose();
            base.Dispose();
        }

        void addDataGridRows() {
            if (_resGrid == null) return;
            if (_resGrid.InvokeRequired)
                _resGrid.Invoke(new MethodInvoker(addDataGridRows), null);
            else {
                string cultureCode = AppEngine.Current.CurrentContext.CultureCode;
                ParameterInfoCollection pDict = AppEngine.Current.ParametersInfo;
                _resGrid.DataGrid.Rows.Clear();
                if (_resGrid.DataGrid.Rows.Count <= 0) _resGrid.DataGrid.Rows.Add();
                _resGrid.DataGrid.Rows[0].Cells[_featureColumn.Name].Value = VisionSystemManager.UIStrings.GetString("Spindle");
                var rowIndex = 1;
                if (_stationRecipe == null) return;
                foreach (var tool in _stationRecipe.Tools) {
                    //bool isActive;
                    //if (!bool.TryParse(findToolParamById(tool, "Active"), out isActive) || isActive == false)
                    //    continue;
                    if (tool.Active == false)
                        continue;
                    while (_resGrid.DataGrid.Rows.Count <= rowIndex) _resGrid.DataGrid.Rows.Add();
                    var s = VisionSystemManager.UIStrings.GetString("Tool") + " " + (tool.Id + 1).ToString("d2");
                    if (s != null)
                        _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value = s.ToUpper();
                    _resGrid.DataGrid.Rows[rowIndex].Cells[_limitColumn.Name].Value = tool.Label;//findToolParamById(tool, "Label");
                    _resGrid.DataGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                    rowIndex++;
                    if (tool.ToolOutputs != null) {
                        for (int iToolOut = 0; iToolOut < tool.ToolOutputs.Count; iToolOut++) { // -1 per spindle id
                            while (_resGrid.DataGrid.Rows.Count <= rowIndex) _resGrid.DataGrid.Rows.Add();
                            var toolOutput = tool.ToolOutputs[iToolOut];
                            string to_key = toolOutput.Label.Replace("$", "");
                            bool isLocalized = (pDict[to_key] != null) && (pDict[to_key].LocalizedInfo != null) && (pDict[to_key].LocalizedInfo[cultureCode] != null) && (string.IsNullOrEmpty(pDict[to_key].LocalizedInfo[cultureCode].Label) == false);
                            string toolOutputName = (isLocalized == false) ? toolOutput.Label : pDict[to_key].LocalizedInfo[cultureCode].Label;
                            _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value = toolOutputName + " " + toolOutput.MeasureUnit;
                            _resGrid.DataGrid.Rows[rowIndex].Cells[_limitColumn.Name].Value = toolOutput.Value;
                            _resGrid.DataGrid.Rows[rowIndex].Cells[_resultColumn.Name].Value = "---";
                            _resGrid.DataGrid.Rows[rowIndex].Cells[_trendConfigParSavedColumn.Name].Value = new TrendConfigParSaved(
                                AppEngine.Current.CurrentContext.ActiveRecipe.RecipeName,
                                _sourceStation.NodeId,
                                _sourceStation.IdStation,
                                tool.Id,
                                iToolOut,
                                _stationRecipe.Description,
                                tool.Label,
                                _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value.ToString());
                            rowIndex++;
                        }
                    }
                }
                _resGrid.DataGrid.Refresh();
                //redrawOnResults = true;
                SetVisibleTrendDataSaveColumn();
            }
        }

        void InitializeDataGrid() {

            if (_resGrid.InvokeRequired) {
                _resGrid.Invoke(new MethodInvoker(InitializeDataGrid), null);
            }
            else {
                //dataGrid.DataSource = MeasureStatisticsUI;
                //resGrid.DataGrid
                _resGrid.DataGrid.AutoGenerateColumns = false;
                _resGrid.DataGrid.MultiSelect = false;
                _resGrid.DataGrid.ReadOnly = true;
                _resGrid.DataGrid.AllowUserToAddRows = false;
                _resGrid.DataGrid.AllowUserToDeleteRows = false;
                _resGrid.DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //dataGrid.ScrollBars = ScrollBars.Both;

                _featureColumn = new DataGridViewTextBoxColumn {
                    Name = "Feature",
                    HeaderText = VisionSystemManager.UIStrings.GetString("Feature"),
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    Width = 50
                };
                //featureColumn.DataPropertyName = "";
                _resGrid.DataGrid.Columns.Add(_featureColumn);

                _limitColumn = new DataGridViewTextBoxColumn {
                    Name = "Limit",
                    HeaderText = VisionSystemManager.UIStrings.GetString("Limit"),
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    Width = 100
                };
                //limitColumn.DataPropertyName = "";
                _resGrid.DataGrid.Columns.Add(_limitColumn);

                _resultColumn = new DataGridViewTextBoxColumn {
                    Name = "Result",
                    HeaderText = VisionSystemManager.UIStrings.GetString("Result"),
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    Width = 150
                };
                //resultColumn.DataPropertyName = "";
                _resGrid.DataGrid.Columns.Add(_resultColumn);

                _trendDataSaveColumn = new DataGridViewImageColumn()
                {
                    Name = "TrendDataSave",
                    HeaderText = "",
                    Visible = false,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    Width = 40,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                };
                _resGrid.DataGrid.Columns.Add(_trendDataSaveColumn);

                _trendConfigParSavedColumn = new DataGridViewImageColumn()
                {
                    Name = "TrendConfigParSavedColumn",
                    HeaderText = "",
                    Visible = false,
                };
                _resGrid.DataGrid.Columns.Add(_trendConfigParSavedColumn);

                //dataGrid.Refresh();
                foreach (DataGridViewColumn col in _resGrid.DataGrid.Columns) {
                    col.Frozen = false;
                    col.HeaderCell.Style.BackColor = Color.Black;
                    col.HeaderCell.Style.ForeColor = Color.White;
                    col.DefaultCellStyle.Alignment = col != _trendDataSaveColumn ? DataGridViewContentAlignment.MiddleLeft : DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    col.ReadOnly = true;
                }
            }
        }

        void RefreshDataGrid(InspectionResults inspRes) 
        {
            if (_resGrid == null) return;
            if (_resGrid.InvokeRequired) {
                _resGrid.Invoke((MethodInvoker)(() => RefreshDataGrid(inspRes)), null);
            }
            else {
                //CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
                string cultureCode = AppEngine.Current.CurrentContext.CultureCode;
                ParameterInfoCollection pDict = AppEngine.Current.ParametersInfo;
                if (!inspRes.IsReject) {
                    _resGrid.Header.ForeColor = Color.Green;
                    var s = VisionSystemManager.UIStrings.GetString("Good");
                    if (s != null)
                        _resGrid.Header.Text = s.ToUpper();
                }
                else {
                    _resGrid.Header.ForeColor = Color.Red;
                    var rejCause = _rejectionCauses.Find(rc => rc.NumericValue == inspRes.RejectionCause);
                    if (rejCause == null ||
                        rejCause.StringValue == "" ||
                        VisionSystemManager.UIStrings.GetString(rejCause.StringValue) == "") {
                        var s = VisionSystemManager.UIStrings.GetString("Bad");
                        if (s != null)
                            _resGrid.Header.Text = s.ToUpper();
                    }
                    else {
                        var s1 = VisionSystemManager.UIStrings.GetString(rejCause.StringValue);
                        if (s1 != null)
                            _resGrid.Header.Text = s1.ToUpper();
                    }
                }
                if (redrawRequired(inspRes))
                    _resGrid.DataGrid.Rows.Clear();
                if (_resGrid.DataGrid.Rows.Count <= 0) _resGrid.DataGrid.Rows.Add();
                _resGrid.DataGrid.Rows[0].Cells[_featureColumn.Name].Value = VisionSystemManager.UIStrings.GetString("Spindle");
                _resGrid.DataGrid.Rows[0].Cells[_limitColumn.Name].Value = inspRes.SpindleId;
                // ora la parte in cui serve la ricetta
                if (_stationRecipe == null) return;
                var rowIndex = 1;
                prevToolsActiveCount = 0;
                foreach (var tool in _stationRecipe.Tools) {
                    var toolRes = inspRes.ResToolCollection[tool.Id];
                    //bool isActive;
                    //if (!bool.TryParse(findToolParamById(tool, "Active"), out isActive) || isActive == false ||
                    //    !toolRes.IsActive) continue;
                    if (tool.Active == false)
                        continue;
                    prevToolsActiveCount++;
                    // intestazione tool
                    while (_resGrid.DataGrid.Rows.Count <= rowIndex) _resGrid.DataGrid.Rows.Add();
                    var s = VisionSystemManager.UIStrings.GetString("Tool") + " " + (tool.Id + 1).ToString("d2");
                    if (s != null)
                        _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value = s.ToUpper();
                    _resGrid.DataGrid.Rows[rowIndex].Cells[_limitColumn.Name].Value = tool.Label;//findToolParamById(tool, "Label");
                    _resGrid.DataGrid.Rows[rowIndex].DefaultCellStyle.BackColor = toolRes.IsReject ? Color.Red : Color.LightBlue;
                    rowIndex++;
                    // misure
                    for (int iToolOut = 0; iToolOut < tool.ToolOutputs.Count; iToolOut++) {     // -1 per spindle id
                        while (_resGrid.DataGrid.Rows.Count <= rowIndex)
                        {
                            _resGrid.DataGrid.Rows.Add();
                            /*ParameterName set after*/
                            _resGrid.DataGrid.Rows[_resGrid.DataGrid.RowCount - 1].Cells[_trendConfigParSavedColumn.Name].Value = new TrendConfigParSaved(
                                AppEngine.Current.CurrentContext.ActiveRecipe.RecipeName,
                                _sourceStation.NodeId,
                                _sourceStation.IdStation,
                                tool.Id,
                                iToolOut,
                                _stationRecipe.Description,
                                tool.Label,
                                null);
                        }
                        var toolOutput = tool.ToolOutputs[iToolOut];
                        var measRes = toolRes.ResMeasureCollection[iToolOut];
                        if (measRes == null) {
                            Log.Line(LogLevels.Warning, "GridViewInspectionDisplay.RefreshDataGrid", "Recipe not complete. Check it");
                            continue;
                        }
                        string to_key = toolOutput.Label.Replace("$", "");
                        bool isLocalized = (pDict[to_key] != null) && (pDict[to_key].LocalizedInfo != null) &&
                            (pDict[to_key].LocalizedInfo[cultureCode] != null) && 
                            (string.IsNullOrEmpty(pDict[to_key].LocalizedInfo[cultureCode].Label) == false);
                        string toolOutputName = (isLocalized == false) ? toolOutput.Label : pDict[to_key].LocalizedInfo[cultureCode].Label;
                        _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value = toolOutputName + " " + toolOutput.MeasureUnit;
                        _resGrid.DataGrid.Rows[rowIndex].Cells[_limitColumn.Name].Value = toolOutput.Value;

                        TrendConfigParSaved parSaved = (TrendConfigParSaved)_resGrid.DataGrid[_trendConfigParSavedColumn.Name, rowIndex].Value;
                        parSaved.ParameterName = _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value.ToString();

                        switch (measRes.MeasureType) {
                            case MeasureTypeEnum.DOUBLE:
                                _resGrid.DataGrid.Rows[rowIndex].Cells[_resultColumn.Name].Value = TruncStringValue(measRes.MeasureValue, 4);
                                break;
                            //case MeasureTypeEnum.INT:
                            //case MeasureTypeEnum.BOOL:
                            //case MeasureTypeEnum.STRING:
                            default:
                                _resGrid.DataGrid.Rows[rowIndex].Cells[_resultColumn.Name].Value = measRes.MeasureValue;
                                break;
                        }
                        //resGrid.DataGrid.Rows[rowIndex].Cells[resultColumn.Name].Value = measRes.MeasureValue;

                        //trend data
                        if(AppEngine.Current.TrendTool.IsReady && measRes.MeasureType != MeasureTypeEnum.STRING) //ignore type string
                        {
                            foreach (TrendConfigParSaved par in AppEngine.Current.TrendTool.Config.ParsSaved)
                            {
                                if (par.IsEqualTo(parSaved) == 1)
                                {
                                    string value = _resGrid.DataGrid.Rows[rowIndex].Cells[_resultColumn.Name].Value.ToString();
                                    if (measRes.MeasureType == MeasureTypeEnum.BOOL)
                                        value = value.ToLower() == "false" ? "0" : "1";
                                    else if (measRes.MeasureType == MeasureTypeEnum.DOUBLE)
                                        value = double.Parse(value).ToString(System.Globalization.CultureInfo.InvariantCulture); //invariant culture
                                    AppEngine.Current.TrendTool.InsertValue(par, AppEngine.Current.CurrentContext.CurrentBatchId, value);
                                }
                            }
                        }

                        var defFont = _resGrid.DataGrid.Font;
                        _resGrid.DataGrid.Rows[rowIndex].DefaultCellStyle.Font = ((measRes.IsUsed == true) && (measRes.IsOk == false)) ? new Font(defFont.FontFamily, defFont.Size, FontStyle.Bold, GraphicsUnit.Point, 0) : new Font(defFont.FontFamily, defFont.Size, FontStyle.Regular, GraphicsUnit.Point, 0);
                        rowIndex++;
                    }
                }
                //redrawOnResults = false;
                if (_recStatus != null) {
                    _recStatus.Refresh(Recording, BufferSizePerCent);
                }
                //SetVisibleTrendDataSaveColumn();
            }
        }

        bool redrawRequired(InspectionResults inspRes) {

            if (_stationRecipe == null || _stationRecipe.Tools == null) return false;

            int toolsActiveCount = 0;
            foreach (var tool in _stationRecipe.Tools) {
                var toolRes = inspRes.ResToolCollection[tool.Id];
                //bool isActive;
                //if (!bool.TryParse(findToolParamById(tool, "Active"), out isActive) || isActive == false ||
                //    !toolRes.IsActive) continue;
                if (tool.Active == false)
                    continue;
                toolsActiveCount++;
            }

            if (prevToolsActiveCount != toolsActiveCount)
                return true;

            return false;
        }

        //string findToolParamById(Tool tool, string id) {

        //    Parameter param = tool.ToolParameters[id] as Parameter;
        //    if (param != null) {
        //        return param.Value;
        //    }
        //    return "";
        //}

        public override void PushMeasures(IStation currentStation, MeasuresAvailableEventArgs newMeasures) {

            RefreshDataGrid(newMeasures.InspectionResults);
        }

        void SetVisibleTrendDataSaveColumn()
        {
            if (_resGrid == null)
                return;
            bool visibilitycol = (int)AppEngine.Current.CurrentContext.UserLevel >= 9 && AppEngine.Current.TrendTool.IsReady ? true : false;
            if (_trendDataSaveColumn.Visible != visibilitycol)
                _trendDataSaveColumn.Visible = visibilitycol;

            if (visibilitycol)
            {
                //find simultaneos
                int countSimulataneos = 0;
                if(AppEngine.Current.CurrentContext.ActiveRecipe != null)
                {
                    foreach (TrendConfigParSaved par in AppEngine.Current.TrendTool.Config.ParsSaved)
                        if (par.RecipeName == AppEngine.Current.CurrentContext.ActiveRecipe.RecipeName)
                            countSimulataneos++;
                }

                for(int i = 0; i < _resGrid.DataGrid.RowCount; i++)
                {
                    DataGridViewRow dgvr = _resGrid.DataGrid.Rows[i];
                    int btnType = 1; /* Default is 1, if match found set to 2 or 3*/
                    if (dgvr.Cells[_trendConfigParSavedColumn.Name].Value != null)
                    {
                        foreach (TrendConfigParSaved par in AppEngine.Current.TrendTool.Config.ParsSaved)
                        {
                            TrendConfigParSaved parDgv = (TrendConfigParSaved)dgvr.Cells[_trendConfigParSavedColumn.Name].Value;
                            int equal = par.IsEqualTo(parDgv);
                            if (equal == 1)
                                btnType = 2;
                            else if (equal == 2)
                                btnType = 3; //error if found ids and indexcies equals but with different names
                        }
                    }
                    else
                        btnType = 0;

                    //check max simultaneos
                    bool maxCount = false;
                    if (countSimulataneos >= AppEngine.Current.MachineConfiguration.MaxSimultaneosSavesTrendData)
                        maxCount = true;

                    //set crossed
                    if (btnType == 1 && (AppEngine.Current.CurrentContext.MachineMode == MachineModeEnum.Running || maxCount))
                        btnType = 4;
                    if (btnType == 2 && (AppEngine.Current.CurrentContext.MachineMode == MachineModeEnum.Running))
                        btnType = 5;
                    if (btnType == 3 && (AppEngine.Current.CurrentContext.MachineMode == MachineModeEnum.Running))
                        btnType = 6;

                    //set buttons
                    if(btnType == 0)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapEmpty)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapEmpty;
                    }
                    if (btnType == 1)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapSaveUncheck)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapSaveUncheck;
                    }
                    if (btnType == 2)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapSaveCheck)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapSaveCheck;
                    }
                    if (btnType == 3)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapSaveError)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapSaveError;
                    }
                    if (btnType == 4)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapSaveUncheckCrossed)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapSaveUncheckCrossed;
                    }
                    if (btnType == 5)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapSaveCheckCrossed)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapSaveCheckCrossed;
                    }
                    if (btnType == 6)
                    {
                        if (dgvr.Cells[_trendDataSaveColumn.Index].Value != _bitmapSaveErrorCrossed)
                            dgvr.Cells[_trendDataSaveColumn.Index].Value = _bitmapSaveErrorCrossed;
                    }

                    //set tag cell
                    dgvr.Cells[_trendConfigParSavedColumn.Name].Tag = btnType;
                }
            }
        }


        private void DataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (AppEngine.Current.TrendTool.IsReady == false)
                return;
            if (_isBusy)
                return;

            if (AppEngine.Current.CurrentContext.MachineMode == MachineModeEnum.Running) //change only when the machine is not running
                return;

            _isBusy = true;

            if (e.RowIndex >= 0 && e.ColumnIndex == _trendDataSaveColumn.Index && _resGrid.DataGrid[_trendConfigParSavedColumn.Name, e.RowIndex].Value != null)
            {
                int btnType = (int)_resGrid.DataGrid[_trendConfigParSavedColumn.Name, e.RowIndex].Tag;
                TrendConfigParSaved parDgv = (TrendConfigParSaved)_resGrid.DataGrid[_trendConfigParSavedColumn.Name, e.RowIndex].Value;

                if (btnType == 1) //uncheck --add
                {
                    AppEngine.Current.TrendTool.Config.ParsSaved.Add(parDgv);
                    AppEngine.Current.TrendTool.Config.Save();
                }
                else if(btnType == 2) //checked --remove equal all
                {
                    foreach (TrendConfigParSaved parSaved in AppEngine.Current.TrendTool.Config.ParsSaved)
                    {
                        if (parDgv.IsEqualTo(parSaved) == 1)
                        {
                            AppEngine.Current.TrendTool.Config.ParsSaved.Remove(parSaved);
                            AppEngine.Current.TrendTool.Config.Save();
                            break;//important
                        }
                    }
                }
                else if (btnType == 3) //error --remove equal partial (2)
                {
                    foreach (TrendConfigParSaved parSaved in AppEngine.Current.TrendTool.Config.ParsSaved)
                    {
                        if (parDgv.IsEqualTo(parSaved) == 2)
                        {
                            AppEngine.Current.TrendTool.Config.ParsSaved.Remove(parSaved);
                            AppEngine.Current.TrendTool.Config.Save();
                            break;//important
                        }
                    }
                }

                SetVisibleTrendDataSaveColumn();
            }

            _isBusy = false;
        }
    }

    public class MeasuresDB : MeasuresContainer {

        NodeRecipe _nodeRecipe;
        Recipe _recipe;
        StationRecipe _stationRecipe;
        readonly IStation _sourceStation;
        //readonly List<RejectionCause> _rejectionCauses;
        DBConnection _db;
        public event EventHandler DBfilled;
        public int DatabaseVialId { get; set; }

        public new Recipe CurrRecipe {
            get {
                return _recipe;
            }
            set {
                _recipe = value;
                if (_recipe != null) {
                    _nodeRecipe = _recipe.Nodes.Find(nn => nn.Id == _sourceStation.NodeId);
                    if (_nodeRecipe != null /*&& _sourceStation.IdStation < _nodeRecipe.Stations.Count*/) {
                        _stationRecipe = _nodeRecipe.Stations.Find(ss => ss.Id == _sourceStation.IdStation);
                    }
                }
            }
        }

        public MeasuresDB(string name, IStation srcStation, DBConnection db)
            : base(name, new StationCollection { srcStation }/*, visionSystemConfig*/) {

            _sourceStation = srcStation;
            //_rejectionCauses = rejCauses;
            _db = db;
            DatabaseVialId = 1;
        }


        string findToolParamById(Tool tool, string id) {

            Parameter param = tool.ToolParameters.Find(pp => pp.Id == id);
            if (param != null) {
                return param.Value;
            }
            return "";
        }

        public override void PushMeasures(IStation currentStation, MeasuresAvailableEventArgs newMeasures) {

            lock (_db.SqlConnection) {
                if (_db.Ready)
                    DBInspectionResults(currentStation, newMeasures.InspectionResults);
            }
        }

        protected virtual void OnDBFilled(object sender, EventArgs e) {

            if (DBfilled != null) DBfilled(sender, e);
        }

        public void DBInspectionResults(IStation currentStation, InspectionResults inspRes) {

            if (_stationRecipe == null) return;
            //tool
            foreach (var tool in _stationRecipe.Tools) {
                var toolRes = inspRes.ResToolCollection[tool.Id];
                bool isActive;
                if (!bool.TryParse(findToolParamById(tool, "Active"), out isActive) || isActive == false ||
                    !toolRes.IsActive) {
                    _db.FillData(DatabaseVialId, currentStation.IdStation, inspRes.IsReject, tool.Id, tool.ToolParameters[1].Value, tool.ToolParameters[2].Value, toolRes.IsActive, false, "", Convert.ToString(0.0), Convert.ToString(0.0));
                    continue;
                }
                // misure
                for (int iToolOut = 0; iToolOut < tool.ToolOutputs.Count; iToolOut++) {
                    var toolOutput = tool.ToolOutputs[iToolOut];
                    var measRes = toolRes.ResMeasureCollection[iToolOut];

                    //        _resGrid.DataGrid.Rows[rowIndex].Cells[_featureColumn.Name].Value = toolOutput.Name + " " + toolOutput.MeasureUnit;
                    //        _resGrid.DataGrid.Rows[rowIndex].Cells[_limitColumn.Name].Value = toolOutput.Value;
                    string result = "";
                    switch (measRes.MeasureType) {
                        case MeasureTypeEnum.DOUBLE:
                            result = TruncStringValue(measRes.MeasureValue, 4);
                            break;
                        //case MeasureTypeEnum.INT:
                        //case MeasureTypeEnum.BOOL:
                        //case MeasureTypeEnum.STRING:
                        default:
                            result = measRes.MeasureValue;
                            break;
                    }
                    _db.FillData(DatabaseVialId, currentStation.IdStation, inspRes.IsReject, tool.Id, tool.ToolParameters[1].Value, tool.ToolParameters[2].Value, toolRes.IsActive, toolRes.IsReject, toolOutput.Label, toolOutput.Value, result);
                }
            }
            OnDBFilled(currentStation, EventArgs.Empty);
        }
    }

    public class Results2Scada : MeasuresContainer {

        ConcurrentQueue<InspectionResults> resQueue = new ConcurrentQueue<InspectionResults>();
        Thread senderThread;
        int _queueCapacity;
        bool exit = false;
        AutoResetEvent resEnqueued = new AutoResetEvent(false);
        List<StationSetting> _statSettings;
        static int instanceCounter;

        public Results2Scada(string name, StationCollection stations, List<StationSetting> statSettings, int queueCapacity)
            : base(name, stations) {

            resQueue.Clear();
            _queueCapacity = queueCapacity;
            _statSettings = statSettings;
            if (instanceCounter == 0) {
                senderThread = new Thread(new ThreadStart(scadaSenderThread));
                senderThread.Name = "SCADA Results Sender Thread";
                senderThread.IsBackground = true;
                senderThread.Start();
            }
            Interlocked.Increment(ref instanceCounter);
        }

        public override void Dispose() {

            base.Dispose();
            Interlocked.Decrement(ref instanceCounter);
            if (instanceCounter == 0) {
                exit = true;
                if (senderThread != null) {
                    if (senderThread.IsAlive)
                        senderThread.Join(4000);
                    senderThread.Interrupt();
                    senderThread.Abort();
                }
            }
        }

        public override void PushMeasures(IStation currentStation, MeasuresAvailableEventArgs newMeasures) {

            if (_queueCapacity <= resQueue.Count) {
                Log.Line(LogLevels.Warning, "Results2Scada.PushMeasures", "SCADA Results Queue full, cannot enqueue further results");
                return;
            }
            if (CurrRecipe == null) {
                Log.Line(LogLevels.Warning, "Results2Scada.PushMeasures", "SCADA Results request a valid Recipe");
                return;
            }
            resQueue.Enqueue(newMeasures.InspectionResults);
            Log.Line(LogLevels.Debug, "Results2Scada.PushMeasures", "Enqueued measures for scada. Queue size: " + resQueue.Count);
            resEnqueued.Set();
        }

        void scadaSenderThread() {

            while (!exit) {
                bool signaled = resEnqueued.WaitOne(1000);
                if (!signaled || resQueue.Count <= 0) continue;
                InspectionResults inspRes;
                string stringToSend = "";
                int dequeueCounter = 0;
                while (resQueue.TryDequeue(out inspRes)) {
                    if ((CurrRecipe.Nodes == null) || (inspRes.NodeId >= CurrRecipe.Nodes.Count) ||
                        (CurrRecipe.Nodes[inspRes.NodeId].Stations == null) || (inspRes.InspectionId >= CurrRecipe.Nodes[inspRes.NodeId].Stations.Count)) {
                        Log.Line(LogLevels.Warning, "Results2Scada.scadaSenderThread", "Recipe incorrect");
                        continue;
                    }
                    StationRecipe currStationRecipe = CurrRecipe.Nodes[inspRes.NodeId].Stations[inspRes.InspectionId];
                    StationSetting currStatSettings = _statSettings.Find(ss => ss.Id == inspRes.InspectionId);
                    if (currStatSettings.ToolResultsToStoreCollection == null ||
                        currStatSettings.ToolResultsToStoreCollection.Count == 0)
                        continue;
                    //stringToSend += inspRes.InspectionId + ";";
                    foreach (ToolResults resTool in inspRes.ResToolCollection) {
                        bool found = false;
                        foreach (int id in currStatSettings.ToolResultsToStoreCollection) {
                            if (id == resTool.ToolId)
                                found = true;
                        }
                        if (!found)
                            continue;
                        //stringToSend += resTool.ToolId + ";";

                        foreach (MeasureResults measRes in resTool.ResMeasureCollection) {
                            string limit = "N/A";
                            string value = "N/A";
                            //if (measRes.IsUsed && (resTool.ToolId < currStationRecipe.Tools.Count) && (measRes.MeasureId < currStationRecipe.Tools[resTool.ToolId].ToolOutputs.Count)) {
                            if ((resTool.ToolId < currStationRecipe.Tools.Count) && (measRes.MeasureId < currStationRecipe.Tools[resTool.ToolId].ToolOutputs.Count)) {
                                limit = currStationRecipe.Tools[resTool.ToolId].ToolOutputs[measRes.MeasureId].Value;
                            }
                            if (measRes.IsUsed)
                                value = measRes.MeasureValue;
                            stringToSend += inspRes.NodeId + ";" + inspRes.InspectionId + ";" + resTool.ToolId + ";" + measRes.MeasureId + ";" + limit + ";" + value + ";";
                        }
                    }
                    dequeueCounter++;
                    //stringToSend += "\r\n";
                }
                if (dequeueCounter > 0) {
                    Log.Line(LogLevels.Debug, "Results2Scada.scadaSenderThread", "Dequeued {0} measures for scada", dequeueCounter);
                    AppEngine.Current.SendResults(stringToSend);
                }
            }
            Log.Line(LogLevels.Debug, "Results2Scada.scadaSenderThread", senderThread.Name + ": Exiting ...");
        }

    }

    internal static class ConcurrentQueueExtensions {

        public static void Clear<T>(this ConcurrentQueue<T> queue) {
            T item;
            while (queue.TryDequeue(out item)) {
                // do nothing
            }
        }
    }

}
