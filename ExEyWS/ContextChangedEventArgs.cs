using System;

namespace ExactaEasyEng
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextChangedEventArgs : EventArgs
    {

        public ContextChangesEnum ContextChanges { get; private set; }
        public bool ApplyParameters { get; private set; }

        public ContextChangedEventArgs(ContextChangesEnum contextChanges, bool applyParams)
        {
            ContextChanges = contextChanges;
            ApplyParameters = applyParams;
        }
    }
}
