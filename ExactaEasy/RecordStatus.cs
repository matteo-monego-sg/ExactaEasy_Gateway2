using DisplayManager;
using ExactaEasyEng.Utilities;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExactaEasy
{
    public partial class RecordStatus : UserControl, IRecordStatus {
        public RecordStatus() {
            InitializeComponent();
        }

        public ProgressBar BufferBar {
            get { 
                return pbRec; 
            }
        }

        public TextBox BufferStatus {
            get {
                return tbRecBufferValue;
            }
        }

        public void Refresh(bool recording, double bufferSizePerCent) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated) {
                this.Invoke(new MethodInvoker(() => Refresh(recording, bufferSizePerCent)));
            }
            else {
                int perCentValue = (recording == true) ? Math.Min((int)(bufferSizePerCent + 0.5), 100) : 0;
                int pbColor = 1;
                if (perCentValue > 33)
                    pbColor = 3;
                if (perCentValue > 66)
                    pbColor = 2;
                BufferBar.Value = perCentValue;
                ModifyProgressBarColor.SetState(BufferBar, pbColor);
                if (BufferStatus != null)
                    BufferStatus.Text = perCentValue + "%";
                tbRecBuffer.BackColor = (recording == true) ? Color.Red : System.Drawing.SystemColors.ControlLight;
                this.Refresh();
            }
        }
    }

    internal static class ModifyProgressBarColor {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state) {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
