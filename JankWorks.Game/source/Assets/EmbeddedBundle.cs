using System;
using System.Reflection;
using System.IO;

namespace JankWorks.Game.Assets
{
    public sealed class EmbeddedBundle : IAssetBundle
    {
        private Assembly assembly;
        public EmbeddedBundle(Assembly assembly)
        {
            this.assembly = assembly;
        }
        public Stream GetAsset(string name) => this.assembly.GetManifestResourceStream(name) ?? throw new ApplicationException($"Missing asset {name}");
    }
}
