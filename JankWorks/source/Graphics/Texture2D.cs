using System;

using JankWorks.Core;



namespace JankWorks.Graphics
{
    public abstract class Texture2D : Disposable
    {
        public TextureWrap Wrap { get; set; }
        public TextureFilter Filter { get; set; }

        public abstract void SetPixels(Vector2i size, ReadOnlySpan<RGBA> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<ABGR> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<ARGB> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<BGRA> pixels);

        public abstract void SetPixels(Vector2i size, ReadOnlySpan<RGBA32> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<ARGB32> pixels);
    }

    public enum TextureWrap
    {
        Repeat,
        Clamp,
        Edge
    }

    public enum TextureFilter
    {
        Nearest,
        Linear
    }
}
