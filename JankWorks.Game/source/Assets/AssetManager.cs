using System;
using System.IO;
using System.Collections.Generic;

using JankWorks.Core;

namespace JankWorks.Game.Assets
{
    public sealed class AssetManager : Disposable
    {
        private Dictionary<string, IAssetBundle> bundles;

        private List<IDisposable> disposableBundles;

        public AssetManager()
        {
            this.bundles = new Dictionary<string, IAssetBundle>();
            this.disposableBundles = new List<IDisposable>(8);
        }

        public IAssetBundle this[string name] => this.GetBundle(name);

        public void RegisterBundle(IAssetBundle bundle, string name)
        {
            if(!this.bundles.TryAdd(name, bundle))
            {
                throw new ArgumentException();
            }
            else if(bundle is IDisposable disposableBundle)
            {
                this.disposableBundles.Add(disposableBundle);
            }
        }

        public IAssetBundle GetBundle(string name)
        {
            if(this.bundles.TryGetValue(name, out var bundle))
            {
                return bundle;
            }
            else
            {
                throw new ApplicationException($"Missing asset bundle {name}");
            }
        }

        public Stream GetAsset(string bundle, string asset)
        {
            var b = this.GetBundle(bundle);

            if(b != null)
            {
                return b.GetAsset(asset);
            }
            else
            {
                throw new ApplicationException($"Missing asset bundle {bundle}");
            }
        }

        protected override void Dispose(bool finalising)
        {
            foreach(var disposableBundle in this.disposableBundles)
            {
                disposableBundle.Dispose();
            }
            this.disposableBundles.Clear();
            base.Dispose(finalising);
        }
    }
}
