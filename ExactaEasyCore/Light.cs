namespace ExactaEasyCore {

    public class Light {

        public int Id { get; set; }
        public ParameterCollection<Parameter> StroboParameters { get; set; }

        public Light() {

            StroboParameters = new ParameterCollection<Parameter>();
        }

        public Light Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            Light newLight = new Light();
            newLight.Id = Id;
            newLight.StroboParameters = (ParameterCollection<Parameter>)StroboParameters.Clone(paramDictionary, cultureCode);
            return newLight;
        }
    }
}
