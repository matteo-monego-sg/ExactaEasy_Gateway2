using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPAMI.Util.Logger;
using ExactaEasyCore;

namespace ExactaEasyEng {

    public class ParameterCollection<T> : List<T>, IParameterCollection where T : IParameter, new() {

        ParameterInfoCollection _paramDictionary;
        string _cultureCode;

        public ParameterCollection(ParameterInfoCollection paramDictionary, string cultureCode) {

            _paramDictionary = paramDictionary;
            _cultureCode = cultureCode;
    }

        public IParameter this[string id] {
            get {
                return this.Find(p => p.Id == id);
            }
            set {
                int pos = this.IndexOf((T)this[id]); 
                RemoveAt(pos);
                Insert(pos, (T)value);
            }
        }

        public void Add(string id, string value) {

            T newParam = new T() { Id = id, Value = value };
            try {
                newParam.PopulateParameterInfo(_paramDictionary, _cultureCode);
                //this.Add(newParam);
            }
            catch {
                Log.Line(LogLevels.Error, "ParameterCollection.Add", "Error while adding parameter to list");
                //throw;
            }
            finally {
                this.Add(newParam);
            }
        } 

        public IParameterCollection Clone() {

            ParameterCollection<T> newColl = new ParameterCollection<T>(_paramDictionary, _cultureCode);
            foreach (IParameter param in this) {
                T newParam = new T();
                newParam.Clone(param);
                newColl.Add(newParam);
            }
            return newColl;
        }

        public void CopyParametersInfo(IParameterCollection sourceParamCollection) {

            try {
                foreach (IParameter par in this) {
                    IParameter pInfoSrc = sourceParamCollection[par.Id];
                    if (pInfoSrc != null) {
                        par.MinValue = pInfoSrc.MinValue;
                        par.MaxValue = pInfoSrc.MaxValue;
                        par.IsEditable = pInfoSrc.IsEditable;
                        par.IsVisible = pInfoSrc.IsVisible;
                        par.Label = null;
                        if (pInfoSrc.AdmittedValues != null) {
                            par.AdmittedValues = new List<string>();
                            foreach (string val in pInfoSrc.AdmittedValues)
                                par.AdmittedValues.Add(val);
                        }
                    }
                }
            }
            catch (Exception) {
                throw;
            }

        }

        public void PopulateParametersInfo() {

            foreach (IParameter par in this)
                par.PopulateParameterInfo(_paramDictionary, _cultureCode);
        }

        public void Replace(IParameterCollection paramCollection) {

            foreach (T param in paramCollection) {
                if (this[param.Id] != null)
                    this[param.Id] = param;
            }
        }





    }
}
