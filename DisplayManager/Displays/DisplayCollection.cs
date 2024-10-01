using System.Collections.Generic;

namespace DisplayManager
{
    public class DisplayCollection : List<Display>
    {

        public Display this[string displayName]
        {
            get
            {
                return Find(d => { return d.Name == displayName; });
            }
        }
    }
}
