using System.IO;

namespace JankWorks.Game.Assets
{
    public interface IAssetBundle
    {
        Stream GetAsset(string name);

        Stream this[string name] => this.GetAsset(name);
    }
}
