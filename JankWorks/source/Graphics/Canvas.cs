using System;

namespace JankWorks.Graphics
{
    public abstract class Canvas : Surface
    {
        public abstract Texture2D Texture { get; }
    }
}
