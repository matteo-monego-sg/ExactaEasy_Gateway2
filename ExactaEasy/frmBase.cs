using ExactaEasyEng;
using System;
//using OnScreenKeyboard;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace ExactaEasy
{
    public partial class frmBase : Form {

        static ResourceManager uiStrings = null;
        //UnactivableForm osk;
        //NumericKeyboardCtrl osk1 = new NumericKeyboardCtrl();

        public static string CultureCode { get; internal set; }
        public static bool RestartForm { get; set; }

        public static bool ForceWindowHidden { get; set; }
        public static bool ChangeLanguage { get; set; }
        public static bool SendRecipeReq { get; set; }

        public static ResourceManager UIStrings {
            get {
                if (uiStrings == null) {
                    uiStrings = new ResourceManager("ExactaEasy.UIStrings", Assembly.GetExecutingAssembly());
                }
                return uiStrings;
            }
        }

        public frmBase() 
        {
            ExactaEasyEng.AppContext currCtx = AppEngine.Current.CurrentContext;

            var currentCulture = currCtx.CultureCode is null ? string.Empty : currCtx.CultureCode.ToLower();

            if (!string.IsNullOrEmpty(currCtx.CultureCode) &&
                (currentCulture == "en" || currentCulture == "de" ||
                currentCulture == "es" || currentCulture == "it" ||
                currentCulture == "ja" || currentCulture == "ru" ||
                currentCulture == "fr" || currentCulture == "sv" ||
                currentCulture == "pt" || currentCulture == "sk" ||
                currentCulture == "nl" || currentCulture == "zh" ||
                currentCulture == "da" || currentCulture == "hu" ||
                currentCulture == "cs" || currentCulture == "ko"))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(currCtx.CultureCode);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currCtx.CultureCode);
                CultureCode = currCtx.CultureCode;
            }
            else {
                MessageBox.Show("WRONG LANGUAGE OR NOT SUPPORTED LANGUAGE, PLEASE CONTACT SYSTEM ADMINISTRATOR", "LANGUAGE CODE ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitializeComponent();

            //osk = new NumericKeyboard();
            //((NumericKeyboard)osk).ShowBorders = false;

            //osk1.Visible = false;
           // Controls.Add(osk1);
        }

        protected override void OnControlAdded(ControlEventArgs e) {

            Debug.Print(e.Control.Name);
            addOskEvents(e.Control);
            base.OnControlAdded(e);
        }

        void addOskEvents(Control control) {

            if (control is Panel) {
                Panel ctrl = (Panel)control;
                ctrl.ControlAdded += new ControlEventHandler(ctrl_ControlAdded);
            }
            if (control is DataGridView) {
                DataGridView ctrl = (DataGridView)control;
                ctrl.CellBeginEdit += new DataGridViewCellCancelEventHandler(ctrl_CellBeginEdit);
                ctrl.CellEndEdit += new DataGridViewCellEventHandler(ctrl_CellEndEdit);
            }
            if (control is System.Windows.Forms.NumericUpDown) {
                System.Windows.Forms.NumericUpDown ctrl = (System.Windows.Forms.NumericUpDown)control;
                ctrl.GotFocus += new EventHandler(ctrl_GotFocus);
                ctrl.LostFocus += new EventHandler(ctrl_LostFocus);
            }
            if (control is System.Windows.Forms.TextBox) {
                System.Windows.Forms.TextBox ctrl = (System.Windows.Forms.TextBox)control;
                ctrl.GotFocus += new EventHandler(ctrl_GotFocus);
                ctrl.LostFocus += new EventHandler(ctrl_LostFocus);
            }
            if (control is System.Windows.Forms.NumericTextBox) {
                System.Windows.Forms.NumericTextBox ctrl = (System.Windows.Forms.NumericTextBox)control;
                ctrl.GotFocus += new EventHandler(ctrl_GotFocus);
                ctrl.LostFocus += new EventHandler(ctrl_LostFocus);
                ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
            }
            foreach (Control c in control.Controls)
                addOskEvents(c);
        }

        void ctrl_KeyDown(object sender, KeyEventArgs e) {

            if (sender is NumericTextBox) {
                System.Windows.Forms.NumericTextBox ctrl = (System.Windows.Forms.NumericTextBox)sender;
                if (e.KeyData == Keys.Escape)
                    ctrl.Value = (decimal)ctrl.Tag;

                if (e.KeyData == Keys.Escape || e.KeyData == Keys.Enter) {
                    ctrl.Parent.Focus();
                    hideOSK();
                }
            }
        }

        void ctrl_ControlAdded(object sender, ControlEventArgs e) {

            addOskEvents(e.Control);
        }

        void ctrl_CellEndEdit(object sender, DataGridViewCellEventArgs e) {

            hideOSK();
        }

        void ctrl_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {

            if (!(((DataGridView)sender)[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell) &&
                !(((DataGridView)sender)[e.ColumnIndex, e.RowIndex] is DataGridViewCheckBoxCell) && !e.Cancel)
                showOSK();
        }

        void ctrl_LostFocus(object sender, EventArgs e) {

            hideOSK();
        }

        void ctrl_GotFocus(object sender, EventArgs e) {

            if (sender is NumericTextBox) {
                System.Windows.Forms.NumericTextBox ctrl = (System.Windows.Forms.NumericTextBox)sender;
                ctrl.Tag = ctrl.Value;
            }

            showOSK();
        }

        public void showOSK() {

            //osk.Show();
            //osk.Location = new Point(this.Left + 495,this.Top + 178);

            //osk1.Location = new Point(495, 178);
            //osk1.Visible = true;
            //osk1.BringToFront();
        }

        public void hideOSK() {

            //osk.Hide();
            //osk1.Visible = false;
        }

        //protected override void SetVisibleCore(bool value) {

        //    if (ForceWindowHidden)
        //        base.SetVisibleCore(false);
        //    else
        //        base.SetVisibleCore(value);
        //}
    }

}
