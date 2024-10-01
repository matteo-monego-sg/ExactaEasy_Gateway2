using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class CameraProvider {

        public string Name { get; set; }
        public string Type { get; set; }
        public List<String> ExternalDependencies { get; set; }
    }

    public class CameraProviderCollection : List<CameraProvider> {

        static CameraProviderCollection _allProviders = new CameraProviderCollection();

        public static CameraProvider GetProvider(string providerName) {

            return _allProviders[providerName];
        }

        public event EventHandler<CameraProviderAddedEventArgs> CameraProviderAdded;

        public CameraProvider this[string providerName] {
            get {
                return this.Find(p => { return p.Name == providerName; });
            }
        }

        public new void Add(CameraProvider cameraProvider) {

            base.Add(cameraProvider);
            if (_allProviders[cameraProvider.Name] == null)
                _allProviders.Add(cameraProvider);

            OnCameraProviderAdded(this, new CameraProviderAddedEventArgs(cameraProvider));
        }

        protected void OnCameraProviderAdded(object sender, CameraProviderAddedEventArgs e) {

            if (CameraProviderAdded != null)
                CameraProviderAdded(sender, e);
        }
    }

    public class CameraProviders {

        public string DefaultCameraProviderName { get; set; }

        CameraProviderCollection _providers = new CameraProviderCollection();
        public CameraProviderCollection Providers {
            get { return _providers; }
            set {
                _providers = value;
            }
        }
    }

    public class CameraProviderAddedEventArgs : EventArgs {

        public CameraProvider AddedCameraProvider { get; private set; }

        public CameraProviderAddedEventArgs(CameraProvider addedCameraProvider) {

            AddedCameraProvider = addedCameraProvider;
        }
    }
}
