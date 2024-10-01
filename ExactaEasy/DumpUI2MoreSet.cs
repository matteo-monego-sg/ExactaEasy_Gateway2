using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using System.IO;
using System.Threading;
using SPAMI.Util.Logger;
using System.Xml.Linq;
using GretelClients;

namespace ExactaEasy
{
    public partial class DumpUI2MoreSet : Form
    {
        StationDumpSettings2 _sds;
        List<KeyValuePair<StationDumpSamplings2, string>> _dicSamplingKV;
        List<KeyValuePair<StationDumpPatternTypes2, string>> _dicPatternTypeGood;
        List<KeyValuePair<StationDumpPatternTypes2, string>> _dicPatternTypeOnReject;

        public DumpUI2MoreSet(StationDumpSettings2 sds)
        {
            InitializeComponent();

            _sds = sds;
            if (_sds == null)
                throw new ArgumentException("sds cannot be null");

            _dicSamplingKV = new List<KeyValuePair<StationDumpSamplings2, string>>();
            foreach (StationDumpSamplings2 val in Enum.GetValues(typeof(StationDumpSamplings2)))
                _dicSamplingKV.Add(new KeyValuePair<StationDumpSamplings2, string>(val, val.ToString()));

            _dicPatternTypeGood = new List<KeyValuePair<StationDumpPatternTypes2, string>>();
            _dicPatternTypeOnReject = new List<KeyValuePair<StationDumpPatternTypes2, string>>();
            foreach (StationDumpPatternTypes2 val in Enum.GetValues(typeof(StationDumpPatternTypes2)))
            {
                _dicPatternTypeGood.Add(new KeyValuePair<StationDumpPatternTypes2, string>(val, val.ToString()));
                _dicPatternTypeOnReject.Add(new KeyValuePair<StationDumpPatternTypes2, string>(val, val.ToString()));
            }
        }


        private void DumpUI2MoreSet_Load(object sender, EventArgs e)
        {
            //translations
            labelSampling.Text = frmBase.UIStrings.GetString("Sampling").ToUpper();
            labelTools.Text = frmBase.UIStrings.GetString("Tools").ToUpper();
            labelPatternSet.Text = frmBase.UIStrings.GetString("OnPattern").ToUpper();
            labelGood.Text = frmBase.UIStrings.GetString("Good").ToUpper();
            labelOnReject.Text = frmBase.UIStrings.GetString("OnReject").ToUpper();
            labelGoodSave.Text = labelOnRejetSave.Text = frmBase.UIStrings.GetString("Save").ToUpper();
            labelGoodEvery.Text = labelOnRejectEvery.Text = frmBase.UIStrings.GetString("Every").ToUpper();

            SetUI();
            //events
            cbSampling.SelectedValueChanged += cb_changedValue;
            cbGood.SelectedValueChanged += cb_changedValue;
            cbOnReject.SelectedValueChanged += cb_changedValue;
            numGoodSave.ValueChanged += num_changedValue;
            numGoodEvery.ValueChanged += num_changedValue;
            numOnRejectSave.ValueChanged += num_changedValue;
            numOnRejectEvery.ValueChanged += num_changedValue;
            chTools.SelectedValueChanged += ChTools_SelectedValueChanged;
        }


        void SetUI()
        {
            //sampling
            cbSampling.DisplayMember = "Value";
            cbSampling.ValueMember = "Key";
            cbSampling.DataSource = _dicSamplingKV;
            cbSampling.SelectedValue = _sds.Sampling;
            //pattern
            if (_sds.Condition == StationDumpConditions2.OnPattern)
            {
                panelOnPattern.Enabled = true;
                //good
                cbGood.DisplayMember = "Value";
                cbGood.ValueMember = "Key";
                cbGood.DataSource = _dicPatternTypeGood;
                cbGood.SelectedValue = _sds.ConditionOnGood.Type;
                numGoodSave.Value = _sds.ConditionOnGood.ToSave;
                numGoodEvery.Value = _sds.ConditionOnGood.Every;
                //reject
                cbOnReject.DisplayMember = "Value";
                cbOnReject.ValueMember = "Key";
                cbOnReject.DataSource = _dicPatternTypeOnReject;
                cbOnReject.SelectedValue = _sds.ConditionOnReject.Type;
                numOnRejectSave.Value = _sds.ConditionOnReject.ToSave;
                numOnRejectEvery.Value = _sds.ConditionOnReject.Every;

                SetUINum();
            }
            else
                panelOnPattern.Enabled = false;
            //tools
            if(_sds.SaveOnTool != null)
            {
                chTools.Items.Clear();
                chTools.Enabled = true;
                string txtBase = frmBase.UIStrings.GetString("Tool").ToUpper();
                for (int i = 0; i < _sds.SaveOnTool.Length; i++)
                    chTools.Items.Add(new ItemCheck(i, $"{txtBase}_{i + 1}"), _sds.SaveOnTool[i]);
            }
            else
            {
                chTools.Items.Clear();
                chTools.Enabled = false;
            }
        }

        void SetUINum()
        {
            numGoodSave.Enabled = numGoodEvery.Enabled = _sds.ConditionOnGood.Type == StationDumpPatternTypes2.EveryOnceIn ? true : false;
            numOnRejectSave.Enabled = numOnRejectEvery.Enabled = _sds.ConditionOnReject.Type == StationDumpPatternTypes2.EveryOnceIn ? true : false;
        }


        private void cb_changedValue(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            //sampling
            if (cb == cbSampling)
                _sds.Sampling = (StationDumpSamplings2)cb.SelectedValue;
            //good
            if (cb == cbGood)
                _sds.ConditionOnGood.Type = (StationDumpPatternTypes2)cb.SelectedValue;
            //on reject
            if (cb == cbOnReject)
                _sds.ConditionOnReject.Type = (StationDumpPatternTypes2)cb.SelectedValue;

            SetUINum();
        }

        private void num_changedValue(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            //good save
            if (num == numGoodSave)
                _sds.ConditionOnGood.ToSave = (int)num.Value;
            //good every
            if (num == numGoodEvery)
                _sds.ConditionOnGood.Every = (int)num.Value;
            //on reject save
            if (num == numOnRejectSave)
                _sds.ConditionOnReject.ToSave = (int)num.Value;
            //on reject save
            if (num == numOnRejectEvery)
                _sds.ConditionOnReject.Every = (int)num.Value;
        }


        private void ChTools_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            ItemCheck itm = (ItemCheck)chTools.SelectedItem;
            if (clb.CheckedItems.Contains(clb.SelectedItem))
                _sds.SaveOnTool[itm.Index] = true;
            else
                _sds.SaveOnTool[itm.Index] = false;
        }



        class ItemCheck
        {
            public int Index { get; set; }
            public string Text { get; set; }

            public ItemCheck(int index, string text)
            {
                Index = index;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
