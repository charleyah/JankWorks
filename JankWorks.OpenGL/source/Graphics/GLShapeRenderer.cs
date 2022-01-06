using System;
using System.Numerics;
using System.Runtime.InteropServices;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLShapeRenderer : ShapeRenderer
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector2 position;
            public Vector4 fillColour;

            public Vertex(Vector2 position, Vector4 fillColour)
            {
                this.position = position;
                this.fillColour = fillColour;
            }
        }

        public override Camera Camera { get; set; }

        private RendererState state;

        private Vertex[] vertices;
        private int vertexCount;

        private GLBuffer<Vertex> vertexBuffer;
        private VertexLayout layout;
        private GLShader program;

        const int dataSize = 172;

        public GLShapeRenderer(GraphicsDevice device, Camera camera)
        {
            this.state.Setup();
            this.Camera = camera;
            this.vertices = new Vertex[dataSize];
            this.SetupGrahpicsResources(device);
            
        }

        private void SetupGrahpicsResources(GraphicsDevice device)
        {
            this.vertexBuffer.Generate();

            this.layout = device.CreateVertexLayout();

            var attribute = new VertexAttribute();

            attribute.Index = 0;
            attribute.Offset = 0;
            attribute.Stride = Marshal.SizeOf<Vertex>();
            attribute.Format = VertexAttributeFormat.Vector2f;
            attribute.Usage = VertexAttributeUsage.Position;
            this.layout.SetAttribute(attribute);

            attribute.Index = 1;
            attribute.Offset = Marshal.SizeOf<Vector2>();
            attribute.Stride = Marshal.SizeOf<Vertex>();
            attribute.Format = VertexAttributeFormat.Vector4f;
            attribute.Usage = VertexAttributeUsage.Colour;
            this.layout.SetAttribute(attribute);

            var asm = typeof(GLShapeRenderer).Assembly;
            var vertpath = $"{nameof(GLShapeRenderer)}.vert.glsl";
            var fragpath = $"{nameof(GLShapeRenderer)}.frag.glsl";
            this.program = (GLShader)device.CreateShader(ShaderFormat.GLSL, asm.GetManifestResourceStream(vertpath), asm.GetManifestResourceStream(fragpath));

            this.program.SetVertexData(this.vertexBuffer, this.layout);
        }


        public override void Reserve(int vertices)
        {
            var remaining = this.vertices.Length - this.vertexCount;

            if (vertices > remaining)
            {
                vertices += this.vertices.Length;
                var diff = vertices % dataSize;
                var newSize = (diff == 0) ? vertices : (dataSize - diff) + vertices;

                Array.Resize(ref this.vertices, newSize);
            }
        }

        public override void Clear() => this.vertexCount = 0;

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
                
        public override void DrawLine(Vector2 start, Vector2 end, RGBA colour, float thickness)
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangle(Vector2 size, Vector2 position, Vector2 origin, float rotation, RGBA fillcolour)
        {
            ref readonly var rstate = ref this.state;

            if (!rstate.drawing)
            {
                throw new InvalidOperationException();
            }

            var vecColour = (Vector4)fillcolour;
            var radians = MathF.PI / 180f * rotation;

            var model = Matrix4x4.Identity;
            model = model * Matrix4x4.CreateScale(new Vector3(size, 0));
            model = model * Matrix4x4.CreateTranslation(-new Vector3(size * origin, 0));
            model = model * Matrix4x4.CreateRotationZ(radians);
            model = model * Matrix4x4.CreateTranslation(new Vector3(position, 0));

            var mvp = model * rstate.view * rstate.projection;

            var tl = new Vertex(Vector2.Transform(new Vector2(0, 0), mvp), vecColour);
            var tr = new Vertex(Vector2.Transform(new Vector2(1, 0), mvp), vecColour);
            var bl = new Vertex(Vector2.Transform(new Vector2(0, 1), mvp), vecColour);
            var br = new Vertex(Vector2.Transform(new Vector2(1, 1), mvp), vecColour);

            const int verticeCount = 6;

            this.Reserve(verticeCount);

            unsafe
            {
                fixed(Vertex* verticesPtr = this.vertices.AsSpan(this.vertexCount))
                {
                    verticesPtr[0] = tl;
                    verticesPtr[1] = tr;
                    verticesPtr[2] = bl;
                    verticesPtr[3] = bl;
                    verticesPtr[4] = tr;
                    verticesPtr[5] = br;
                }
            }

            this.vertexCount += verticeCount;
        }

        public override void DrawTriangle(Vector2 size, Vector2 position, Vector2 origin, float rotation, RGBA fillcolour)
        {
            ref readonly var rstate = ref this.state;

            if (!rstate.drawing)
            {
                throw new InvalidOperationException();
            }

            var vecColour = (Vector4)fillcolour;
            var radians = MathF.PI / 180f * rotation;

            var model = Matrix4x4.Identity;
            model = model * Matrix4x4.CreateScale(new Vector3(size, 0));
            model = model * Matrix4x4.CreateRotationZ(radians);
            model = model * Matrix4x4.CreateTranslation(new Vector3(position, 0));

            var mvp = model * rstate.view * rstate.projection;

            var t = new Vertex(Vector2.Transform(new Vector2(0, 1f), mvp), vecColour);
            var bl = new Vertex(Vector2.Transform(new Vector2(-1f, -1f), mvp), vecColour);            
            var br = new Vertex(Vector2.Transform(new Vector2(1f, -1f), mvp), vecColour);

            const int verticeCount = 3;

            this.Reserve(verticeCount);

            unsafe
            {
                fixed (Vertex* verticesPtr = this.vertices.AsSpan(this.vertexCount))
                {

                    verticesPtr[0] = bl;
                    verticesPtr[1] = t;
                    verticesPtr[2] = br;
                }
            }

            this.vertexCount += verticeCount;
        }

        public override void EndDraw(Surface surface)
        {
            this.state.EndDraw();
            this.Flush();
            this.DrawToSurface(surface);
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

            if (vertexCount > 0)
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

        protected override void Dispose(bool disposing)
        {
            this.program.Dispose();
            this.layout.Dispose();
            this.vertexBuffer.Delete();
            this.vertices = Array.Empty<Vertex>();            
            base.Dispose(disposing);
        }
    }
}