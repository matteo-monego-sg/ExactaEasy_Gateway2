using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using machineConfiguration.MachineConfig;

namespace machineConfiguration
{
    public class GeneralElement
    {
        //public int Id;
        public string Name;
        public string Value;
        public string Title;
        public bool IsManagedByProgram;
        public bool ValueIsEditable;

        public bool IsMovedUp;
        public bool IsMovedDown;

        public InfoPropertyAttributeTypeData TypeData;
    }
}
