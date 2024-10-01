using ExactaEasyCore;
using System;
using System.Collections.Generic;

namespace DisplayManager
{

    interface IAssistant {
        void BindToCamera(Camera camera);
        void UnbindCurrentCamera();
        event EventHandler<LearningEventArgs> AppliedLearning;
    }

    public class LearningEventArgs : EventArgs {
        //public AssistantsEnum Learning;
        public Camera Camera;
        public List<FamIdParameter> ParamToEdit;

        public LearningEventArgs(Camera camera, List<FamIdParameter> paramToEdit) {
            Camera = camera;
            //Learning = learning;
            ParamToEdit = paramToEdit;
        }
    }

    public class FamIdParameter {
        public ParameterTypeEnum family;
        public string id;
        public int section;
        public FamIdParameter(ParameterTypeEnum _family, string _id, int _section) {
            family = _family;
            id = _id;
            section = _section;
        }
        public FamIdParameter(ParameterTypeEnum _family, string _id) {
            family = _family;
            id = _id;
            section = 0;
        }
    }

    public enum AssistantsEnum {
        Strobo,
        Normalization,
        VialAxis,
        CheckVialAxis,
        ResetTriggerEnc,
    }
}
