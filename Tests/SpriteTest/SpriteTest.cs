using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

#pragma warning disable CS8618

namespace Tests.SpriteTest
{
    class SpriteTest : Test
    {
        private Texture2D texture;
        
        private VertexLayout quadlayout;
        private VertexBuffer<Vertex2> quad;
        private IndexBuffer quadIndexes;

        private Shader program;

        private Matrix4x4 model;
        private Matrix4x4 view;
        private Matrix4x4 projection;

        private Vector2 spriteSize;
        private Vector2 spritePositionOrigin;
        private Vector2 spritePosition; 

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            var viewport = graphics.Viewport;

            this.spriteSize = new Vector2(256);
            this.spritePositionOrigin = new Vector2(0.5f);
            this.spritePosition = (Vector2)viewport.Size / 2;

            this.projection = Matrix4x4.CreateOrthographicOffCenter(0, viewport.Size.X, viewport.Size.Y, 0, -1, 1);

            this.view = Matrix4x4.CreateTranslation(new Vector3(0));

            this.model = Matrix4x4.CreateScale(new Vector3(spriteSize, 0)) * Matrix4x4.CreateTranslation(new Vector3(spritePosition - (this.spriteSize * this.spritePositionOrigin), 0));


            this.texture = graphics.CreateTexture2D(GetEmbeddedStream("SpriteTest.punchy_512.png"), ImageFormat.PNG);

            this.quadlayout = graphics.CreateVertexLayout();
            var positionAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Stride = Marshal.SizeOf<Vertex2>(),
                Offset = 0,
                Index = 0,
                Usage = VertexAttributeUsage.Position
            };
            this.quadlayout.SetAttribute(positionAttrib);

            var texAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Stride = Marshal.SizeOf<Vertex2>(),
                Offset = Marshal.SizeOf<Vector2>() + Marshal.SizeOf<Vector3>(),
                Index = 1,
                Usage = VertexAttributeUsage.TextureCoordinate
            };
            this.quadlayout.SetAttribute(texAttrib);



            this.quad = graphics.CreateVertexBuffer<Vertex2>();
            Vertex2[] quadData =
            {
                new Vertex2(new Vector2(0f,  0f), Colour.White, new Vector2(0f, 0f)),
                new Vertex2(new Vector2(1f, 0f), Colour.White, new Vector2(1f, 0f)),
                new Vertex2(new Vector2(0f, 1f), Colour.White, new Vector2(0f, 1f)),
                new Vertex2(new Vector2(1f,  1f), Colour.White, new Vector2(1f, 1f)),

            };
            this.quad.Write(quadData);



            this.quadIndexes = graphics.CreateIndexBuffer();
            uint[] indexValues =
            {
                0, 1, 2,
                2, 3, 1
            };
            this.quadIndexes.Write(indexValues);



            this.program = graphics.CreateShader(ShaderFormat.GLSL, GetEmbeddedStream("SpriteTest.vert.glsl"), GetEmbeddedStream("SpriteTest.frag.glsl"));

            this.program.SetVertexData(this.quad, this.quadlayout, this.quadIndexes);

            this.program.SetUniform("image", this.texture, 0);
            this.program.SetUniform("model", this.model);
            this.program.SetUniform("view", this.view);
            this.program.SetUniform("projection", this.projection);
        }

        public override void Draw(GraphicsDevice device)
        {
            device.DrawIndexedPrimitives(this.program, DrawPrimitiveType.Triangles, 6);
        }

        public override void Dispose(GraphicsDevice device, AudioDevice audio, Window window)
        {
            this.program.Dispose();
            this.quadIndexes.Dispose();
            this.quadlayout.Dispose();
            this.quad.Dispose();
            this.texture.Dispose();
        }
    }
}