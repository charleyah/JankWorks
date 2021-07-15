using System;
using System.IO;

namespace JankWorks.Game.Assets
{
    public sealed class DirectoryBundle : IAssetBundle
    {
        private DirectoryInfo dir;

        public DirectoryBundle(string path) : this(new DirectoryInfo(path)) { }

        public DirectoryBundle(DirectoryInfo dir)
        {
            if(!dir.Exists)
            {
                throw new ApplicationException($"Missing directory {dir.FullName}");
            }
            this.dir = dir;
        }

        public Stream GetAsset(string name)
        {
            var path = Path.Combine(this.dir.FullName, name);

            return File.Exists(path) ? new FileStream(path, FileMode.Open, FileAccess.Read) : throw new ApplicationException($"Missing asset {path}");
        }
    }
}
