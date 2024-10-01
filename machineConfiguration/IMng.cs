using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration
{
    public interface IMng
    {
        bool AllowAdd { get; }
        bool AllowDelete { get; }
        bool AllowMoveUpDown { get; }
        bool AllowDeleteAll { get; }
        bool AllowCreateFromStations { get; }
        bool AllowAutoCompleteId { get; }
        bool AllowAddSetStationNamesToNode { get; }
        bool AllowRemoveStationNamesToNode { get; }
        bool IsDisplay { get; }
        int TypeDisplaySelected { get; set; }
        event EventHandler<IMng> RefreshControlCommands;

        void SetUI();
        void SendCommand(string comm);
        void SelectItemsInError(ErrorElement err);
    }
}
