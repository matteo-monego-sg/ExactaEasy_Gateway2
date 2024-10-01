using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SPAMI.Util.Logger;

namespace ExactaEasyCore {
    public class TaskObserver {

        public static TaskObserver TheObserver { get; private set; }
        readonly TaskInfoCollection _tasks = new TaskInfoCollection();
        public TaskInfoCollection Tasks {
            get {
                return _tasks;
            }
        }
        public event EventHandler<TaskStatusEventArgs> DispatchObservation;

        static TaskObserver() {

            if (TheObserver == null)
                TheObserver = new TaskObserver();

        }

        public void AddObservable(IObservable observable) {
            observable.TaskStatusUpdate += observable_TaskStatusUpdate;

        }

        void observable_TaskStatusUpdate(object sender, TaskStatusEventArgs e) {
            Log.Line(LogLevels.Debug, "TaskObserver.observable_TaskStatusUpdate", e.TaskID.ToString(CultureInfo.InvariantCulture) + ": " + e.TaskStatus.ToString());
            /*if (e.PropertyNames != null) {
                foreach (string propName in e.PropertyNames) {
                    string propValue = sender.GetType().GetProperty(propName).GetValue(sender, null).ToString();
                    Log.Line(LogLevels.Debug, "TaskObserver.observable_TaskStatusUpdate", e.TaskID.ToString() + ": " + propName + " = " + propValue);
                }
            }*/
            Log.Line(LogLevels.Debug, "TaskObserver.observable_TaskStatusUpdate", e.TaskID.ToString(CultureInfo.InvariantCulture) + ": " + e.AdditionalInfo);
            if (Tasks.All(ti => ti.TaskId != e.TaskID))
                Tasks.Add(new TaskInfo((IObservable)sender, e.TaskID));
            Tasks[e.TaskID].TaskStatus = e.TaskStatus;
            Tasks[e.TaskID].AdditionalInfo = e.AdditionalInfo;
            OnDispatchObservation(sender, e);
        }

        protected void OnDispatchObservation(object sender, TaskStatusEventArgs e) {
            if (DispatchObservation != null) 
                DispatchObservation(sender, e);
        }
    }

    public class TaskInfoCollection : List<TaskInfo> {

        public TaskInfo this[string id] {
            get {
                return Find(ti => ti.TaskId == id);
            }
        }

        //public override bool Equals(object obj) {
        //    return base.Equals(obj);
        //}
    }

    public class TaskInfo {
        //public IObservable TaskObject { get; internal set; }
        public string TaskId { get; internal set; }
        public TaskStatus TaskStatus { get; internal set; }
        public string AdditionalInfo { get; internal set; }
        public IObservable Owner { get; private set; }

        public TaskInfo(IObservable owner, string taskId) {
            Owner = owner;
            TaskId = taskId;
            TaskStatus = TaskStatus.Unknown;
            AdditionalInfo = "";
        }
    }
}