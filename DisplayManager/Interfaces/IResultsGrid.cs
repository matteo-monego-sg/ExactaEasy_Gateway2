using System.ComponentModel;
using System.Windows.Forms;

namespace DisplayManager {
    public interface IResultsGrid : ISynchronizeInvoke {
        DataGridView DataGrid { get; }
        Label Header { get; }
    }
}
