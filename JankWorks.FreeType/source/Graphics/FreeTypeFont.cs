using System;
using System.Collections.Generic;
using System.Numerics;

using JankWorks.Graphics;

using JankWorks.Drivers.FreeType.Native;

using static JankWorks.Drivers.FreeType.Native.Functions;
using static JankWorks.Drivers.FreeType.Native.Constants;
using System.Collections;

namespace JankWorks.Drivers.FreeType.Graphics
{
    public sealed class FreeTypeFont : Font
    {
        public override uint FontSize 
        { 
            get => base.FontSize; 
            set
            {
                FT_Set_Pixel_Sizes(this.face, 0, value);
                base.FontSize = value;
            }                
        }

        private IDisposable source;
        private FT_Face face;
        private char? current;

        public FreeTypeFont(FT_Face face, IDisposable source)
        {
            this.face = face;
            this.source = source;
            this.current = null;
            this.FontSize = 12;
        }

        private void LoadCharacter(char character)
        {
            if (this.current != character)
            {
                var error = FT_Load_Char(this.face, character, FT_LOAD_RENDER);                

                if (error != FT_Error.FT_Err_Ok)
                {
                    throw new ApplicationException(error.ToString());
                }
                current = character;
            }
        }

        public override Glyph GetGlyph(char character)
        {
            this.LoadCharacter(character);
                                           
            unsafe
            {
                var ftglyph = this.face.Rec->glyph.Rec;

                return new Glyph()
                {
                    Size = new Vector2(ftglyph->bitmap.width, ftglyph->bitmap.rows),
                    Bearing = new Vector2(ftglyph->bitmap_left, ftglyph->bitmap_top),
                    Advance = (uint)ftglyph->advance.x,
                    Value = character                                 
                };
            }           
        }

        public override GlyphBitmap GetGlyphBitmap(char character)
        {
            this.LoadCharacter(character);

            unsafe
            {
                ref var bitmap = ref this.face.Rec->glyph.Rec->bitmap;

                var format = bitmap.pixel_mode switch
                {
                    FT_Pixel_Mode.FT_PIXEL_MODE_GRAY => PixelFormat.GrayScale8,
                    FT_Pixel_Mode.FT_PIXEL_MODE_LCD => PixelFormat.RGB24,
                    _ => throw new NotSupportedException()
                };

                var bufferSize = (int)bitmap.rows * bitmap.width;

                return new GlyphBitmap(new ReadOnlySpan<byte>(bitmap.buffer, bufferSize), new Vector2(bitmap.width, bitmap.rows), format);
            }
        }

        public override IEnumerator<Glyph> GetGlyphs() => new GlyphIterator(this);

        protected override void Dispose(bool finalising)
        {
            FT_Done_Face(this.face);
            this.source.Dispose();
            base.Dispose(finalising);
        }

        private sealed class GlyphIterator : IEnumerator<Glyph>
        {
            public Glyph Current => this.glyph;
            object IEnumerator.Current => this.glyph;

            private Glyph glyph;
            private uint glyphIndex;
            private bool readFirst;
            private FreeTypeFont font;

            public GlyphIterator(FreeTypeFont font)
            {
                this.font = font;
                this.readFirst = false;
            }

            public bool MoveNext()
            {
                if(!readFirst)
                {
                    var ch = FT_Get_First_Char(this.font.face, ref glyphIndex);

                    var foundGlyph = glyphIndex != 0;

                    if (foundGlyph)
                    {
                        this.glyph = this.font.GetGlyph((char)ch);
                    }
                    this.readFirst = true;
                    return foundGlyph;
                }
                else
                {
                    var ch = FT_Get_Next_Char(this.font.face, glyph.Value, ref glyphIndex);

                    var foundGlyph = glyphIndex != 0;

                    if (foundGlyph)
                    {
                        this.glyph = this.font.GetGlyph((char)ch);
                    }
                    return foundGlyph;
                }
            }

            public void Reset() => throw new NotImplementedException();

            public void Dispose() { }
        }
    }
}
