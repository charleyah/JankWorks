using System;

using JankWorks.Core;
using JankWorks.Drivers;

namespace JankWorks.Graphics
{
    public abstract class Surface : Disposable
    {
        public abstract Rectangle Viewport { get; set; }

        public abstract RGBA ClearColour { get; set; }

        public abstract void Clear();
        public abstract void Display();

        public abstract void CopyToTexture(Texture2D texture);

        public abstract void Activate();
        public abstract void Deactivate();
    }
}
