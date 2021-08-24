using System;

using JankWorks.Core;



namespace JankWorks.Graphics
{
    public abstract class Texture2D : Disposable
    {
        public TextureWrap Wrap { get; set; }
        public TextureFilter Filter { get; set; }

        public Vector2i Size { get; protected set; }
        public PixelFormat Format { get; protected set; }

        protected Texture2D(Vector2i size, PixelFormat format)
        {
            this.Size = size;
            this.Format = format;
        }

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

        public abstract void CopyTo(Span<byte> pixels, PixelFormat format);
        public abstract void CopyTo(Span<RGBA> pixels);
        public abstract void CopyTo(Span<ABGR> pixels);
        public abstract void CopyTo(Span<ARGB> pixels);
        public abstract void CopyTo(Span<BGRA> pixels);
        public abstract void CopyTo(Span<RGBA32> pixels);
        public abstract void CopyTo(Span<ARGB32> pixels);
    }

    public enum PixelFormat
    {
        GrayScale,
        RGB,
        RGBA
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
