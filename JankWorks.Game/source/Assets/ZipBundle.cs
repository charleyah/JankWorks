using System;
using System.IO;
using System.IO.Compression;

using JankWorks.Core;

namespace JankWorks.Game.Assets
{
    public sealed class ZipBundle : Disposable, IAssetBundle
    {
        private ZipArchive zip;

        public ZipBundle(string path)
        {
            this.zip = ZipFile.OpenRead(path);
        }

        public Stream GetAsset(string name)
        {
            var entry = this.zip.GetEntry(name);
            return entry != null ? entry.Open() : throw new ApplicationException($"Missing asset {name}");
        }

        protected override void Dispose(bool finalising)
        {
            this.zip.Dispose();
            base.Dispose(finalising);
        }
    }
}
