using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExactaEasyCore {

    public class NodeProvider {

        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class NodeProviderCollection : List<NodeProvider> {

        static NodeProviderCollection _allProviders = new NodeProviderCollection();

        public static NodeProvider GetProvider(string providerName) {
            
            return _allProviders[providerName];
        }

        public event EventHandler<NodeProviderAddedEventArgs> NodeProviderAdded;

        public NodeProvider this[string providerName] {
            get {
                return this.Find(p => { return p.Name == providerName; });
            }
        }

        public new void Add(NodeProvider NodeProvider) {

            base.Add(NodeProvider);
            if (_allProviders[NodeProvider.Name] == null)
                _allProviders.Add(NodeProvider);

            OnNodeProviderAdded(this, new NodeProviderAddedEventArgs(NodeProvider));
        }

        protected void OnNodeProviderAdded(object sender, NodeProviderAddedEventArgs e) {

            if (NodeProviderAdded != null)
                NodeProviderAdded(sender, e);
        }
    }

    public class NodeProviders {

        public string DefaultNodeProviderName { get; set; }

        NodeProviderCollection _providers = new NodeProviderCollection();
        public NodeProviderCollection Providers {
            get { return _providers; }
            set {
                _providers = value;
            }
        }
    }

    public class NodeProviderAddedEventArgs : EventArgs {

        public NodeProvider AddedNodeProvider { get; private set; }

        public NodeProviderAddedEventArgs(NodeProvider addedNodeProvider) {

            AddedNodeProvider = addedNodeProvider;
        }
    }
}
