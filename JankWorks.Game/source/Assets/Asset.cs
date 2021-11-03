using System;

namespace JankWorks.Game.Assets
{
    public readonly struct Asset
    {
        public readonly string Bundle;
        public readonly string Name;

        public Asset(string bundle, string name)
        {
            this.Bundle = bundle;
            this.Name = name;
        }
    }
}
