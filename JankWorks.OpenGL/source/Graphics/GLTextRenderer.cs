using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLTextRenderer : TextRenderer
    {
        private readonly struct TexturedGlyph
        {
            public readonly Glyph glyph;
            public readonly Bounds bounds;

            public TexturedGlyph(Glyph glyph, Bounds bounds)
            {
                this.glyph = glyph;
                this.bounds = bounds;
            }
        }

        public override Camera Camera { get; set; }

        private int lineSpacing;
        private int maxAdvance;
        private Texture2D fontTexture;
        private Dictionary<uint, TexturedGlyph> glyphs;

        private VertexLayout layout;
        private GLBuffer<Vertex2> vertexBuffer;
        private GLShader program;

        private const int dataSize = 128;
        private const int verticesPerChar = 6;

        private Vertex2[] vertices;
        private int vertexCount;

        private RendererState state;
        private RGBA currentDrawColour = Colour.White;
        private Func<char, int, RGBA> colourpicker;

        public GLTextRenderer(GLGraphicsDevice device, Font font, Camera camera)
        {
            this.colourpicker = (c, i) => this.currentDrawColour;

            this.state.Setup();

            this.vertexCount = 0;
            this.vertices = new Vertex2[dataSize];
            this.Camera = camera;
            this.glyphs = new Dictionary<uint, TexturedGlyph>();
                
            this.SetupGrahpicsResources(device);
            this.SetFont(font);                
        }

        private void SetupGrahpicsResources(GLGraphicsDevice device)
        {
            this.vertexBuffer.Generate();

            this.layout = device.CreateVertexLayout();

            var attribute = new VertexAttribute();

            attribute.Index = 0;
            attribute.Offset = 0;
            attribute.Stride = Marshal.SizeOf<Vertex2>();
            attribute.Format = VertexAttributeFormat.Vector2f;
            attribute.Usage = VertexAttributeUsage.Position;
            layout.SetAttribute(attribute);


            attribute.Index = 1;
            attribute.Offset = Marshal.SizeOf<Vector2>();
            attribute.Stride = Marshal.SizeOf<Vertex2>();
            attribute.Format = VertexAttributeFormat.Vector2f;
            attribute.Usage = VertexAttributeUsage.TextureCoordinate;
            layout.SetAttribute(attribute);

            attribute.Index = 2;
            attribute.Offset = Marshal.SizeOf<Vector2>() * 2;
            attribute.Stride = Marshal.SizeOf<Vertex2>();
            attribute.Format = VertexAttributeFormat.Vector4f;
            attribute.Usage = VertexAttributeUsage.Colour;
            layout.SetAttribute(attribute);

            var asm = typeof(GLTextRenderer).Assembly;
            var vertpath = $"{nameof(GLTextRenderer)}.vert.glsl";
            var fragpath = $"{nameof(GLTextRenderer)}.frag.glsl";
            this.program = (GLShader)device.CreateShader(ShaderFormat.GLSL, asm.GetManifestResourceStream(vertpath), asm.GetManifestResourceStream(fragpath));

            this.program.SetVertexData(this.vertexBuffer, this.layout);

            this.fontTexture = device.CreateTexture2D(new Vector2i(1024, 2048), PixelFormat.GrayScale);
            this.program.SetUniform("Texture", this.fontTexture, 0);
        }

        public override void SetFont(Font font)
        {
            const int padding = 10;
            const int charsPerRow = 100;
            int charsOnRow = 0;

            var size = Vector2i.Zero;

            this.lineSpacing = font.LineSpacing;
            this.maxAdvance = font.MaxAdvance;

            size.Y = this.lineSpacing + padding;
            int rowWidth = 0;

            foreach (var glyph in font)
            {
                var c = glyph.Value;

                if (IsCharDrawable(c))
                {
                    if (++charsOnRow == charsPerRow)
                    {
                        size.Y += this.lineSpacing + padding;
                        size.X = Math.Max(size.X, rowWidth);
                        rowWidth = 0;
                        charsOnRow = 0;
                    }
                    else
                    {
                        rowWidth += glyph.Size.X + padding;
                    }
                }
            }

            size.X = Math.Max(size.X, rowWidth);

            charsOnRow = 0;
            
            this.fontTexture.SetPixels(size, ReadOnlySpan<byte>.Empty, PixelFormat.GrayScale);

            this.glyphs.Clear();

            var pos = Vector2i.Zero;


            foreach (var glyph in font)
            {
                var c = glyph.Value;
                if (IsCharDrawable(c))
                {
                    var bitmap = font.GetGlyphBitmap(glyph.Value);

                    if (bitmap.Format != PixelFormat.GrayScale)
                    {
                        throw new NotSupportedException();
                    }

                    this.fontTexture.SetPixels(bitmap.Size, pos, bitmap.Pixels, PixelFormat.GrayScale);

                    Vector2 topleft = (Vector2)pos / (Vector2)size;
                    Vector2 bottomright = (Vector2)(pos + bitmap.Size) / (Vector2)size;

                    var bounds = new Bounds(topleft.X, topleft.Y, bottomright.X, bottomright.Y);
                    var texturedGlyph = new TexturedGlyph(glyph, bounds);
                    
                    if(++charsOnRow == charsPerRow)
                    {
                        pos.Y += this.lineSpacing + padding;
                        pos.X = 0;
                        charsOnRow = 0;
                    }
                    else
                    {
                        pos.X += bitmap.Size.X + padding;
                    }
                   
                    this.glyphs.Add(glyph.Value, texturedGlyph);
                }
            }
        }

        public override void Clear() => this.vertexCount = 0;      
        
        public override void Reserve(int charCount)
        {
            var requestedVerticesCount = verticesPerChar * charCount;
            var remaining = this.vertices.Length - this.vertexCount;

            if (requestedVerticesCount > remaining)
            {
                requestedVerticesCount += this.vertices.Length;
                var diff = requestedVerticesCount % dataSize;
                var newSize = (diff == 0) ? requestedVerticesCount : (dataSize - diff) + requestedVerticesCount;

                Array.Resize(ref this.vertices, newSize);
            }
        }

        public override void BeginDraw()
        {
            this.state.BeginDraw(this.Camera, null);
            this.Clear();
        }

        public override void BeginDraw(DrawState state)
        {
            this.state.BeginDraw(this.Camera, state);
            this.Clear();
        }

        public override bool ReDraw(Surface surface)
        {
            var canReDraw = this.vertexCount > 0 && this.state.CanReDraw(this.Camera);

            if (canReDraw)
            {
                this.DrawToSurface(surface);
            }

            return canReDraw;
        }

        public override Bounds Draw(ReadOnlySpan<char> text, Vector2 position, Vector2 origin, float rotation, RGBA colour)
        {
            this.currentDrawColour = colour;
            return this.Draw(text, position, origin, rotation, this.colourpicker);
        }

        public override Bounds Draw(ReadOnlySpan<char> text, Vector2 position, Vector2 origin, float rotation, Func<char, int, RGBA> colourpicker)
        {
            ref readonly var rstate = ref this.state;

            if (!rstate.drawing)
            {
                throw new InvalidOperationException();
            }
            else if (!text.IsEmpty && !text.IsWhiteSpace())
            {
                var drawableCharCount = CountDrawableChars(text);

                this.Reserve(drawableCharCount);

                var vertices = new Span<Vertex2>(this.vertices, this.vertexCount, drawableCharCount * verticesPerChar);
                var charsProcessed = 0;
                var glyphpos = Vector2.Zero;
                var textSize = new Vector2(0, this.lineSpacing);
                var line = (this.lineSpacing / 4) * 3;

                unsafe
                {
                    int currentCharIndex = 0;

                    fixed (char* textptr = text)
                    {
                        do
                        {
                            char currentChar = *(textptr + currentCharIndex);

                            var colour = (Vector4)colourpicker(currentChar, currentCharIndex);

                            if (currentChar == ' ')
                            {
                                glyphpos.X += this.maxAdvance;
                            }
                            else if (currentChar == '\t')
                            {
                                glyphpos.X += this.maxAdvance * 4;
                            }
                            else if (currentChar == '\n')
                            {
                                glyphpos.Y += this.lineSpacing;
                                textSize.X = Math.Max(textSize.X, glyphpos.X);
                                glyphpos.X = 0;
                            }
                            else if (IsCharDrawable(currentChar))
                            {
                                if(this.glyphs.TryGetValue(currentChar, out var texturedglyph))
                                {
                                    var glyph = texturedglyph.glyph;
                                    var glyhSize = (Vector2)glyph.Size;
                                    var glyphModel = Matrix4x4.CreateScale(new Vector3(glyhSize, 0));

                                    var h = new Vector2(0, line - glyph.Bearing.Y);
                                    var w = glyph.Advance.X - glyph.Bearing.X;

                                    glyphModel = glyphModel * Matrix4x4.CreateTranslation(new Vector3(glyphpos + h, 0));
                                    glyphpos.X += w;

                                    var tl = new Vertex2(Vector2.Transform(new Vector2(0, 0), glyphModel), texturedglyph.bounds.TopLeft, colour);
                                    var tr = new Vertex2(Vector2.Transform(new Vector2(1, 0), glyphModel), texturedglyph.bounds.TopRight, colour);
                                    var bl = new Vertex2(Vector2.Transform(new Vector2(0, 1), glyphModel), texturedglyph.bounds.BottomLeft, colour);
                                    var br = new Vertex2(Vector2.Transform(new Vector2(1, 1), glyphModel), texturedglyph.bounds.BottomRight, colour);

                                    fixed (Vertex2* glyhvertices = vertices.Slice(charsProcessed * verticesPerChar))
                                    {
                                        glyhvertices[0] = tl;
                                        glyhvertices[1] = tr;
                                        glyhvertices[2] = bl;
                                        glyhvertices[3] = bl;
                                        glyhvertices[4] = tr;
                                        glyhvertices[5] = br;
                                    }
                                    charsProcessed++;
                                }
                                else
                                {
                                    glyphpos.X += this.maxAdvance;
                                }                                                              
                            }
                        }
                        while (++currentCharIndex < text.Length);

                        textSize.X = Math.Max(textSize.X, glyphpos.X);
                        textSize.Y = textSize.Y + glyphpos.Y;
                    }

                    var bounds = new Bounds(position - (textSize * origin), textSize);

                    var radians = MathF.PI / 180f * rotation;

                    var textModel = Matrix4x4.CreateTranslation(-new Vector3(textSize * origin, 0));
                    textModel = textModel * Matrix4x4.CreateRotationZ(radians);
                    textModel = textModel * Matrix4x4.CreateTranslation(new Vector3(position, 0));

                    charsProcessed = 0;

                    var mvp = textModel * rstate.view * rstate.projection;

                    fixed (Vertex2* vertexptr = vertices)
                    {
                        do
                        {
                            var pos = (vertexptr + charsProcessed)->position;
                            (vertexptr + charsProcessed)->position = Vector2.Transform(pos, mvp);
                        }
                        while (++charsProcessed < vertices.Length);
                    }

                    this.vertexCount += vertices.Length;

                    return bounds;
                }
            }

            return default;
        }

        private static int CountDrawableChars(ReadOnlySpan<char> chars)
        {
            var drawable = 0;

            unsafe
            {
                int count = 0;
                var length = chars.Length;
                fixed (char* charsptr = chars)
                {                    
                    do
                    {
                        char currentChar = *(charsptr + count);
                        if (IsCharDrawable(currentChar))
                        {
                            drawable++;
                        }
                    }
                    while (++count < length);
                }
            }
            return drawable;
        }

        private void Flush()
        {
            var vertexCount = this.vertexCount;

            if (this.vertexBuffer.ElementCount < vertexCount)
            {
                this.vertexBuffer.Write(GL_ARRAY_BUFFER, BufferUsage.Dynamic, this.vertices);
            }
            else
            {
                this.vertexBuffer.Update(GL_ARRAY_BUFFER, BufferUsage.Dynamic, this.vertices.AsSpan(0, vertexCount), 0);
            }
        }

        private void DrawToSurface(Surface surface)
        {
            var vertexCount = this.vertexCount;

            if(vertexCount > 0)
            {
                ref readonly var rstate = ref this.state;

                if (rstate.drawState != null)
                {
                    var ds = rstate.drawState.Value;
                    surface.DrawPrimitives(this.program, DrawPrimitiveType.Triangles, 0, vertexCount, in ds);
                }
                else
                {
                    surface.DrawPrimitives(this.program, DrawPrimitiveType.Triangles, 0, vertexCount);
                }
            }
        }

        public override void EndDraw(Surface surface)
        {
            this.state.EndDraw();
            this.Flush();
            this.DrawToSurface(surface);
        }

        protected override void Dispose(bool finalising)
        {
            this.Clear();
            this.fontTexture.Dispose();
            this.program.Dispose();
            this.layout.Dispose();
            this.vertexBuffer.Delete();

            base.Dispose(finalising);
        }

        private static bool IsCharDrawable(char c) => !char.IsWhiteSpace(c);
    }
}