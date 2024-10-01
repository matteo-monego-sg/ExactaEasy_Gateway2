using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPAMI.Util.Logger;

namespace ExactaEasyCore {

    public enum TaskStatus {
        Unknown,
        Running,
        Failed,
        Completed,
    }

    public interface IObservable {
        event EventHandler<TaskStatusEventArgs> TaskStatusUpdate;
    }

    

    public class TaskStatusEventArgs : EventArgs {

        public string TaskID { get; private set; }
        public TaskStatus TaskStatus { get; private set; }
        //public List<string> PropertyNames { get; private set; }
        public string AdditionalInfo { get; private set; }

        public TaskStatusEventArgs(string id, TaskStatus taskStatus, /*List<string> propertyNames=null, */string additionalInfo="") {
            TaskID = id;
            TaskStatus = taskStatus;
            //PropertyNames = propertyNames;
            AdditionalInfo = additionalInfo;
        }
    }
}
