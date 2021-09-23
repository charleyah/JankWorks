using System;
using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLSpriteRenderer : SpriteRenderer
    {
        private struct RendererState
        {
            public Matrix4x4 projection;
            public Matrix4x4 view;
            public DrawState? drawState;
            public bool drawing;

            public void Setup()
            {
                this.projection = Matrix4x4.Identity;
                this.view = Matrix4x4.Identity;
                this.drawState = null;
                this.drawing = false;
            }
        }
               
        private readonly struct Batch
        {
            public readonly Texture2D texture;
            public readonly int offset;
            public readonly int count;

            public Batch(Texture2D texture, int offset, int count)
            {
                this.texture = texture;
                this.offset = offset;
                this.count = count;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Vertex
        {
            public readonly Vector2 position;
            public readonly Vector2 texcoord;
            public readonly Vector4 colour;

            public Vertex(Vector2 position, Vector2 texcoord, Vector4 colour)
            {
                this.position = position;
                this.texcoord = texcoord;
                this.colour = colour;
            }           
        }

        public override DrawOrder Order { get; set; }

        public override Camera Camera { get; set; }

        private const int dataSize = 42;
        private const int verticesPerSprite = 6;       

        private Vertex[] vertices;
        private int vertexCount;

        private Batch[] batches;
        private int batchCount;

        private GLBuffer<Vertex> vertexBuffer;

        private RendererState state;

        private GLShader program;
        private VertexLayout layout;

        public GLSpriteRenderer(GraphicsDevice device, Camera camera, DrawOrder order)
        {
            this.Camera = camera;
            this.Order = order;

            this.vertices = new Vertex[dataSize];
            this.vertexCount = 0;

            this.batches = new Batch[dataSize];
            this.batchCount = 0;

            this.SetupBuffers();
            this.SetupLayout(device);
            this.SetupShaderProgram(device);

            this.program.SetVertexData(this.vertexBuffer, this.layout);

            this.state.Setup();            
        }

        private void SetupBuffers()
        {
            this.vertexBuffer.Generate();
            this.vertexBuffer.Write(GL_ARRAY_BUFFER, BufferUsage.Dynamic, this.vertices);           
        }

        private void SetupLayout(GraphicsDevice device)
        {
            var layout = device.CreateVertexLayout();

            var attribute = new VertexAttribute();

            attribute.Index = 0;
            attribute.Offset = 0;
            attribute.Stride = Marshal.SizeOf<Vertex>();
            attribute.Format = VertexAttributeFormat.Vector2f;            
            attribute.Usage = VertexAttributeUsage.Position;
            layout.SetAttribute(attribute);


            attribute.Index = 1;
            attribute.Offset = Marshal.SizeOf<Vector2>();
            attribute.Stride = Marshal.SizeOf<Vertex>();
            attribute.Format = VertexAttributeFormat.Vector2f;
            attribute.Usage = VertexAttributeUsage.TextureCoordinate;
            layout.SetAttribute(attribute);

            attribute.Index = 2;
            attribute.Offset = Marshal.SizeOf<Vector2>() * 2;
            attribute.Stride = Marshal.SizeOf<Vertex>();
            attribute.Format = VertexAttributeFormat.Vector4f;
            attribute.Usage = VertexAttributeUsage.Colour;
            layout.SetAttribute(attribute);

            this.layout = layout;
        }

        private void SetupShaderProgram(GraphicsDevice device)
        {
            var asm = typeof(GLSpriteRenderer).Assembly;
            var vertpath = $"JankWorks.Drivers.OpenGL.source.Graphics.{nameof(GLSpriteRenderer)}.vert.glsl";
            var fragpath = $"JankWorks.Drivers.OpenGL.source.Graphics.{nameof(GLSpriteRenderer)}.frag.glsl";
            this.program = (GLShader)device.CreateShader(ShaderFormat.GLSL, asm.GetManifestResourceStream(vertpath), asm.GetManifestResourceStream(fragpath));
        }

        public override void Reserve(int spriteCount)
        {
            ref var rstate = ref this.state;
            if (rstate.drawing) { throw new InvalidOperationException(); }

            var requestedVerticesCount = verticesPerSprite * spriteCount;

            if(requestedVerticesCount > this.vertices.Length)
            {
                var diff = requestedVerticesCount % dataSize;
                var newSize = (diff == 0) ? requestedVerticesCount : (dataSize - diff) + requestedVerticesCount;

                Array.Resize(ref this.vertices, newSize);
            }
        }

        public override void BeginDraw()
        {
            ref var rstate = ref this.state;

            if (rstate.drawing) { throw new InvalidOperationException(); }

            rstate.projection = this.Camera.GetProjection();
            rstate.view = this.Camera.GetView();
            rstate.drawState = null;

            this.ClearData();
            rstate.drawing = true;
        }

        public override void BeginDraw(DrawState state)
        {
            ref var rstate = ref this.state;

            if (rstate.drawing) { throw new InvalidOperationException(); }

            rstate.projection = this.Camera.GetProjection();
            rstate.view = this.Camera.GetView();
            rstate.drawState = state;

            this.ClearData();
            rstate.drawing = true;
        }

        private void ClearData()
        {
            // always clear batches due to texture reference
            Array.Clear(this.batches, 0, this.batches.Length); 
            this.batchCount = 0;

            // vertices is just values so we don't need to clear
            this.vertexCount = 0;
        }

        public override void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin, float rotation, RGBA colour, Bounds textureBounds)
        {
            ref var rstate = ref this.state;

            if(!rstate.drawing)
            {
                throw new InvalidOperationException();
            }
            else if (texture == null)
            {
                throw new NullReferenceException();
            }

            var vecColour = (Vector4)colour;
            var radians = MathF.PI / 180f * rotation;

            var model = Matrix4x4.Identity;            
            model = model * Matrix4x4.CreateScale(new Vector3(size, 0));            
            model = model * Matrix4x4.CreateTranslation(-new Vector3(size * origin, 0));
            model = model * Matrix4x4.CreateRotationZ(radians);
            model = model * Matrix4x4.CreateTranslation(new Vector3(position, 0));            
                                 

            var mvp = model * rstate.view * rstate.projection;

            var tl = new Vertex(Vector2.Transform(new Vector2(0, 0), mvp), textureBounds.TopLeft, vecColour);
            var tr = new Vertex(Vector2.Transform(new Vector2(1, 0), mvp), textureBounds.TopRight, vecColour);
            var bl = new Vertex(Vector2.Transform(new Vector2(0, 1), mvp), textureBounds.BottomLeft, vecColour);
            var br = new Vertex(Vector2.Transform(new Vector2(1, 1), mvp), textureBounds.BottomRight, vecColour);

            this.Queue(tl, tr, bl, br, texture);
        }

        private void Queue(Vertex tl, Vertex tr, Vertex bl, Vertex br, Texture2D texture)
        {
            if((this.vertices.Length - this.vertexCount) < verticesPerSprite)
            {
                Array.Resize(ref this.vertices, this.vertices.Length + dataSize);
            }

            var offset = vertexCount;

            /*
            quad draw order
            tl, tr, bl, bl, tr, br
            */

            this.vertices[vertexCount++] = tl;
            this.vertices[vertexCount++] = tr;
            this.vertices[vertexCount++] = bl;
            this.vertices[vertexCount++] = bl;
            this.vertices[vertexCount++] = tr;
            this.vertices[vertexCount++] = br;

            var batchUpperBound = this.batchCount - 1;
            Batch batch = (this.batchCount == 0) ? new Batch() : this.batches[batchUpperBound];

            if(object.ReferenceEquals(batch.texture, texture))
            {
                batch = new Batch(batch.texture, batch.offset, batch.count + verticesPerSprite);                
                this.batches[batchUpperBound] = batch;
            }
            else
            {
                batch = new Batch(texture, offset, verticesPerSprite);

                if(this.batchCount == this.batches.Length)
                {
                    Array.Resize(ref this.batches, this.batches.Length + dataSize);
                }

                this.batches[batchCount++] = batch;
            }
        }

        public override void EndDraw(Surface surface)
        {
            ref var rstate = ref this.state;
            if (!rstate.drawing) { throw new InvalidOperationException(); }

            if(this.Order == DrawOrder.Texture)
            {
                Array.Sort(this.batches, GLSpriteRenderer.BatchCompareByTexture);
                // Improvement possible, reorder vertex data to remove duplicate texture batches
            }

            this.Flush();
            this.DrawToSurface(surface);
            rstate.drawing = false;
        }        

        public override bool ReDraw(Surface surface)
        {
            ref var rstate = ref this.state;
            if (rstate.drawing) { throw new InvalidOperationException(); }

            var canReDraw = this.batchCount > 0 && rstate.projection.Equals(this.Camera.GetProjection()) && rstate.view.Equals(this.Camera.GetView());

            if (canReDraw)
            {
                this.DrawToSurface(surface);
            }

            return canReDraw;
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
            ref var rstate = ref this.state;
            var drawState = rstate.drawState;

            var batchesDrawn = 0;
            int batchDirection;
            int currentBatch;
            Texture2D currentTexture = null;

            if (this.Order == DrawOrder.Reversed)
            {
                batchDirection = -1;
                currentBatch = this.batchCount - 1;
            }
            else
            {
                batchDirection = 1;
                currentBatch = 0;
            }

            do
            {
                ref readonly var batch = ref this.batches[currentBatch];

                if (!object.ReferenceEquals(currentTexture, batch.texture))
                {
                    currentTexture = batch.texture;
                    this.program.SetUniform("Texture", currentTexture, 0);
                }

                if (drawState != null)
                {
                    surface.DrawPrimitives(this.program, DrawPrimitiveType.Triangles, batch.offset, batch.count, drawState.Value);
                }
                else
                {
                    surface.DrawPrimitives(this.program, DrawPrimitiveType.Triangles, batch.offset, batch.count);
                }
                currentBatch += batchDirection;
            }
            while (++batchesDrawn < this.batchCount);
        }

        protected override void Dispose(bool finalising)
        {
            this.program.Dispose();
            this.layout.Dispose();
            this.vertexBuffer.Delete();
        
            base.Dispose(finalising);
        }

        private static int BatchCompareByTexture(Batch left, Batch right)
        {
            var leftTexture = (GLTexture2D)left.texture;
            var rightTexture = (GLTexture2D)right.texture;

            return leftTexture.Id.CompareTo(rightTexture.Id);
        }
    }
}