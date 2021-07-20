using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;

using JankWorks.Core;
using JankWorks.Drivers;

namespace JankWorks.Graphics
{
    public abstract class Font : Disposable
    {
        public virtual uint FontSize { get; set; }

        public abstract IEnumerator<Glyph> GetGlyphs();
        public abstract Glyph GetGlyph(char character);
        public abstract GlyphBitmap GetGlyphBitmap(char character);

        public static Font LoadFromStream(Stream stream, FontFormat format) => DriverConfiguration.Drivers.fontApi.LoadFontFromStream(stream, format);
    }

    public enum FontFormat
    {
        TrueType,
        OpenType
    }

    public struct Glyph
    {
        public Vector2 Size;
        public Vector2 Bearing;
        public uint Advance;
        public char Value;
    }

    public readonly ref struct GlyphBitmap
    {       
        public readonly ReadOnlySpan<byte> Pixels;

        public readonly Vector2 Size;

        public readonly PixelFormat Format;

        public GlyphBitmap(ReadOnlySpan<byte> pixels, Vector2 size, PixelFormat format)
        {
            this.Pixels = pixels;
            this.Size = size;
            this.Format = format;
        }
    }        
}
