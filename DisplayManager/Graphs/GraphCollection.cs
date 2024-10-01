using System.Collections.Generic;

namespace DisplayManager
{
    public class GraphCollection : List<Graph>
    {
        public Graph this[string graphName]
        {
            get
            {
                return this.Find(g => { return g.Name == graphName; });
            }
        }
    }
}
