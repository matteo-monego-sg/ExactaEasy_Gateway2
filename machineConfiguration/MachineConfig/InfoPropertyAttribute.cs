using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration.MachineConfig
{
    public enum InfoPropertyAttributeTypeData
    {
        STRING,
        INT,
        DOUBLE,
        BOOL
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class InfoPropertyAttribute : Attribute
    {
        string _title = null; //default null
        public string Title
        {
            get => _title;
        }

        bool _isEditable = true; //default true
        public bool IsEditable
        {
            get => _isEditable;
        }

        InfoPropertyAttributeTypeData _TypeData = InfoPropertyAttributeTypeData.STRING; //default string
        public InfoPropertyAttributeTypeData TypeData
        {
            get => _TypeData;
        }

        public InfoPropertyAttribute(string title, bool isEditable, InfoPropertyAttributeTypeData typedata)
        {
            _title = title;
            _isEditable = isEditable;
            _TypeData = typedata;
        }
    }
}
