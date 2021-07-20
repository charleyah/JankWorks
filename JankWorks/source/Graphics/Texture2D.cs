using System;

using JankWorks.Core;



namespace JankWorks.Graphics
{
    public abstract class Texture2D : Disposable
    {
        public TextureWrap Wrap { get; set; }
        public TextureFilter Filter { get; set; }

        public abstract void SetPixels(Vector2i size, ReadOnlySpan<byte> pixels, PixelFormat format);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<RGBA> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<ABGR> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<ARGB> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<BGRA> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<RGBA32> pixels);
        public abstract void SetPixels(Vector2i size, ReadOnlySpan<ARGB32> pixels);

        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<byte> pixels, PixelFormat format);
        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<RGBA> pixels);
        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<ABGR> pixels);
        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<ARGB> pixels);
        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<BGRA> pixels);
        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<RGBA32> pixels);
        public abstract void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<ARGB32> pixels);
    }

    public enum PixelFormat
    {
        GrayScale8,
        RGB24,
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
