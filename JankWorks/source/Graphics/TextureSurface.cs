using System;

namespace JankWorks.Graphics
{
    public abstract class TextureSurface : Surface
    {
        public abstract Texture2D Texture { get; }

        protected TextureSurface(DrawState defaultDrawState) : base(defaultDrawState) { }
    }
}
