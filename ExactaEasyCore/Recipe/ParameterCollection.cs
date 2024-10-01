using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPAMI.Util.Logger;
using ExactaEasyCore;

namespace ExactaEasyCore {

    public class ParameterCollection<T> : List<T>, IParameterCollection where T : IParameter, new() {

        ParameterInfoCollection _paramDictionary;
        string _cultureCode;

        public ParameterCollection() { }

        public ParameterCollection(ParameterInfoCollection paramDictionary, string cultureCode) {

            _paramDictionary = paramDictionary;
            _cultureCode = cultureCode;
        }

        public IParameter this[string id] {
            get {
                return this.Find(p => (p.Id == id));
            }
            set {
                int pos = this.IndexOf((T)this[id]);
                RemoveAt(pos);
                Insert(pos, (T)value);
            }
        }

        public IParameter this[IParameter parameter] {
            get {
                return this.Find(p => ((p.Id == parameter.Id) && (p.ParamId == parameter.ParamId)));
            }
            set {
                int pos = this.IndexOf((T)this[parameter]);
                RemoveAt(pos);
                Insert(pos, (T)value);
            }
        }

        public bool Contains(object id) {
            return this.Find(p => p.Id == id.ToString()) != null;
        }

        public void Add(string id, string value) {

            T newParam = new T() { Id = id, Value = value };
            try {
                if (_paramDictionary != null && _cultureCode != null)
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

        //public IParameterCollection Clone() {

        //    ParameterCollection<T> newColl = new ParameterCollection<T>(_paramDictionary, _cultureCode);
        //    foreach (IParameter param in this) {
        //        T newParam = new T();
        //        newParam.Clone(param);
        //        newColl.Add(newParam);
        //    }
        //    return newColl;
        //}

        //pier: ho modificato così il Clone per far andare il tutto....
        public IParameterCollection Clone(ParameterInfoCollection paramDictionary, string cultureCode) {

            _paramDictionary = paramDictionary;
            _cultureCode = cultureCode;
            ParameterCollection<T> newColl = new ParameterCollection<T>(paramDictionary, cultureCode);
            foreach (IParameter param in this) {
                T newParam = new T();
                newParam.Clone(param);
                newColl.Add(newParam);
            }
            return newColl;
        }

        public bool Compare(IParameterCollection paramCollectionToCompare, string cultureCode, string position, out List<ParameterDiff> paramDiffList) {

            paramDiffList = new List<ParameterDiff>();
            bool ris = false;
            ParameterDiff paramDiff = null;
            foreach (Parameter p in paramCollectionToCompare) {
                if (p.Id == "")
                    continue;
                if (this[p] != null) {
                    IParameter currParam = this[p];
                    if (currParam.Compare(p, out paramDiff)) {
                        paramDiff.ParameterPosition = position;
                        paramDiffList.Add(paramDiff);
                        ris = true;
                    }
                }
                else {
                    // Parametro non presente nella ricetta corrente
                    paramDiff = new ParameterDiff();
                    paramDiff.ParameterId = p.Id;
                    paramDiff.ParameterLabel = (string.IsNullOrEmpty(p.Label) == true) ? p.Id : p.Label;
                    paramDiff.ParameterLocLabel = (string.IsNullOrEmpty(p.ExportName) == true) ? p.Id : p.ExportName;
                    paramDiff.ComparedValue = p.Value;
                    paramDiff.DifferenceType = ParameterCompareDifferenceType.Deleted;
                    paramDiff.ParameterPosition = position;
                    paramDiffList.Add(paramDiff);
                }
            }
            // Ricerca di eventuali parametri aggiunti
            foreach (IParameter p in this) {
                if (p.Id != "" && paramCollectionToCompare[p.Id] == null) {
                    paramDiff = new ParameterDiff();
                    paramDiff.ParameterId = p.Id;
                    paramDiff.ParameterLabel = (string.IsNullOrEmpty(p.Label) == true) ? p.Id : p.Label;
                    paramDiff.ParameterLocLabel = (string.IsNullOrEmpty(p.ExportName) == true) ? p.Id : p.ExportName;
                    paramDiff.CurrentValue = p.Value;
                    paramDiff.DifferenceType = ParameterCompareDifferenceType.Added;
                    paramDiff.ParameterPosition = position;
                    paramDiffList.Add(paramDiff);
                }
            }
            return ris;
        }

        //public void CopyParametersInfo(IParameterCollection sourceParamCollection) {

        //    try {
        //        foreach (IParameter par in this) {
        //            IParameter pInfoSrc = sourceParamCollection[par.Id];
        //            if (pInfoSrc != null) {
        //                par.MinValue = pInfoSrc.MinValue;
        //                par.MaxValue = pInfoSrc.MaxValue;
        //                //par.IsEditable = pInfoSrc.IsEditable;
        //                par.IsVisible = pInfoSrc.IsVisible;
        //                par.Label = null;
        //                if (pInfoSrc.AdmittedValues != null) {
        //                    par.AdmittedValues = new List<string>();
        //                    foreach (string val in pInfoSrc.AdmittedValues)
        //                        par.AdmittedValues.Add(val);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception) {
        //        throw;
        //    }

        //}

        public void PopulateParametersInfo() {

            foreach (IParameter par in this)
                par.PopulateParameterInfo(_paramDictionary, _cultureCode);
        }

        public void PopulateParametersInfo(IParameterCollection param_collection) {

            IParameterCollection paramCollection = param_collection.Clone(_paramDictionary, _cultureCode);
            foreach (IParameter par in this) {

                par.IsVisible = 0;
                //bool useDictionary = (_paramDictionary != null && _cultureCode != null && _paramDictionary.Contains() ? true : false;

                //if (useDictionary == true) {
                // gestione con dizionario (Eoptis)
                bool useDictionary = par.PopulateParameterInfo(_paramDictionary, _cultureCode);
                if (useDictionary == true) {
                    par.ExportName = par.Label;
                }
                //}

                IParameter compPar = paramCollection[par.Id];
                if (compPar != null) {
                    par.Group = compPar.Group;
                    if (useDictionary == false) {
                        // gestione senza dizionario (GRETEL)
                        par.ExportName = compPar.ExportName;
                        par.Label = compPar.Label;
                        par.Description = compPar.Description;
                    }
                    par.MinValue = compPar.MinValue;
                    par.MaxValue = compPar.MaxValue;
                    par.Increment = compPar.Increment;
                    par.MeasureUnit = compPar.MeasureUnit;
                    par.ValueType = compPar.ValueType;
                    par.AdmittedValues = compPar.AdmittedValues;
                    par.Decimals = compPar.Decimals;
                    //par.IsUsed = compPar.IsUsed;
                    par.IsVisible = compPar.IsVisible;
                    par.ParamId = compPar.ParamId;
                    paramCollection.Remove(compPar);
                }
            }
                //foreach (IParameter newPar in paramCollection) {
                //    (this as IParameterCollection).Add(newPar);
                //}
        }

        public void PopulateParametersInfo(ParameterInfoCollection paramDictionary, string cultureCode) {

            foreach (IParameter par in this)
                par.PopulateParameterInfo(paramDictionary, cultureCode);
        }

        public void Replace(IParameterCollection paramCollection) {

            foreach (T param in paramCollection) {
                if (this[param.Id] != null)
                    this[param.Id] = param;
            }
        }





    }
}
