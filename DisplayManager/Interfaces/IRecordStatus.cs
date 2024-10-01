using System.ComponentModel;
using System.Windows.Forms;

namespace DisplayManager {
    public interface IRecordStatus : ISynchronizeInvoke {
        ProgressBar BufferBar { get; }
        TextBox BufferStatus { get; }
        void Refresh(bool recording, double bufferSizePerCent);
    }
}
