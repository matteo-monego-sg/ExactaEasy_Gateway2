using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class StationProvider {

        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class StationProviderCollection : List<StationProvider> {

        static StationProviderCollection _allProviders = new StationProviderCollection();

        public static StationProvider GetProvider(string providerName) {
            
            return _allProviders[providerName];
        }

        public event EventHandler<StationProviderAddedEventArgs> StationProviderAdded;

        public StationProvider this[string providerName] {
            get {
                return this.Find(p => { return p.Name == providerName; });
            }
        }

        public new void Add(StationProvider StationProvider) {

            base.Add(StationProvider);
            if (_allProviders[StationProvider.Name] == null)
                _allProviders.Add(StationProvider);

            OnStationProviderAdded(this, new StationProviderAddedEventArgs(StationProvider));
        }

        protected void OnStationProviderAdded(object sender, StationProviderAddedEventArgs e) {

            if (StationProviderAdded != null)
                StationProviderAdded(sender, e);
        }
    }

    public class StationProviders {

        public string DefaultStationProviderName { get; set; }

        StationProviderCollection _providers = new StationProviderCollection();
        public StationProviderCollection Providers {
            get { return _providers; }
            set {
                _providers = value;
            }
        }
    }

    public class StationProviderAddedEventArgs : EventArgs {

        public StationProvider AddedStationProvider { get; private set; }

        public StationProviderAddedEventArgs(StationProvider addedStationProvider) {

            AddedStationProvider = addedStationProvider;
        }
    }
}
