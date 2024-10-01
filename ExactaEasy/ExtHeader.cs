using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyEng;
using ExactaEasyEng.Utilities;

namespace ExactaEasy {

    public partial class ExtHeader : UserControl {

        delegate void SetStatusMessageDelegate(Label ctrl, string message);

        string _errorText = "";
        public string ErrorText {
            get {
                return lblRecipeName.Text;
            }
            set {
                if (_errorText != value) {
                    SetErrorText(value);
                    _errorText = value;
                }
            }
        }

        public ExtHeader() {

            InitializeComponent();
        }

        public void SetErrorText(string recipeName) {

            setLabelText(lblRecipeName, recipeName);
        }

        void setLabelText(Label ctrl, string message) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new SetStatusMessageDelegate(setLabelText), new object[] { ctrl, message });
            else 
            {
                if (ctrl is null)
                    return;
                ctrl.Text = message;
            }
        }
    }
}

