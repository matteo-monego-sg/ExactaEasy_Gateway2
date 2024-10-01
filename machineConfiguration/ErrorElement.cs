using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration
{
    public class ErrorElement
    {
        MenuEnum _Menu;
        public MenuEnum Menu { get => _Menu; }

        string _Subject;
        public string Subject { get => _Subject; }

        List<int> _PosIndex = new List<int>();
        public List<int> PosIndex { get => _PosIndex; }

        string _Message;
        public string Message { get => _Message; }


        public ErrorElement(MenuEnum menu, string sub, string mex, params int[] indexces)
        {
            _Menu = menu;
            _PosIndex = indexces.ToList();
            _Subject = sub;
            _Message = mex;

            if (indexces.Length > 0)
            {
                if (_Subject.EndsWith(" ") == false)
                    _Subject += " [";
                else
                    _Subject += "[";

                for (int i = 0; i < indexces.Length; i++)
                {
                    if (i == indexces.Length - 1)
                    {
                        _Subject += indexces[i].ToString() + "]";
                    }
                    else
                    {
                        _Subject += indexces[i].ToString() + ", ";
                    }
                }
            }
        }

        public ErrorElement(MenuEnum menu, string sub, string mex, List<int> indexces)
        {
            _Menu = menu;
            _PosIndex = indexces;
            _Subject = sub;
            _Message = mex;

            if(indexces.Count > 0)
            {
                if (_Subject.EndsWith(" ") == false)
                    _Subject += " [";
                else
                    _Subject += "[";

                for (int i = 0; i < indexces.Count; i++)
                {
                    if (i == indexces.Count - 1)
                    {
                        _Subject += indexces[i].ToString() + "]";
                    }
                    else
                    {
                        _Subject += indexces[i].ToString() + ", ";
                    }
                }
            }
        }

        public ErrorElement(MenuEnum menu, string sub, string mex)
        {
            _Menu = menu;
            _PosIndex = null;
            _Subject = sub;
            _Message = mex;
        }


        //for only displays
        public ErrorElement(MenuEnum menu, string sub, string mex, int indexces, string displayId, string col, string row)
        {
            _Menu = menu;
            _PosIndex.Add(indexces);
            _Subject = sub;
            _Message = mex;

            if(_Subject.EndsWith(" ") == false)
            {
                _Subject += " ";
            }

            _Subject += $"[{displayId}][{col};{row}]";
        }
    }
}
