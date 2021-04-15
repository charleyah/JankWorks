using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Drivers;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Sample
{
    class Application
    {
        static Application()
        {
            DriverConfiguration.Initialise("JankWorks.Glfw", "JankWorks.OpenGL", "JankWorks.DotNet");
        }

        static Stream GetEmbeddedResource(string name)
        {
            var asm = typeof(Application).Assembly;
            return asm.GetManifestResourceStream("Sample." + name);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex2
        {
            public Vector2 Position;
            public Vector3 Colour;
            public Vector2 TextureCoords;

            public Vertex2(Vector2 position, RGBA colour, Vector2 texcoords) : this(position, (Vector3)colour, texcoords) { }
            public Vertex2(Vector2 position, Vector3 colour, Vector2 texcoords)
            {
                this.Position = position;
                this.Colour = colour;
                this.TextureCoords = texcoords;
            }
        }

        static void Main(string[] args)
        {
            var windowSettings = WindowSettings.Default;
            windowSettings.VideoMode = new VideoMode(1024, 768, 32, 60);
            windowSettings.Style = WindowStyle.Windowed;

            var surfaceSettings = new SurfaceSettings()
            {
                ClearColour = Colour.Black,
                Viewport = windowSettings.VideoMode.Viewport
            };

            using var window = Window.Create(windowSettings);
            using var device = GraphicsDevice.Create(surfaceSettings, window);

            Console.WriteLine(device.Info);

            
            using var buffer = device.CreateVertexBuffer<Vertex2>();
            buffer.Usage = BufferUsage.Static;

            var brcolour = Colour.Blend(Colour.White, Colour.Red, 0.6f);
            var trcolour = Colour.Blend(Colour.Yellow, Colour.Red, 0.4f);

            Vertex2[] data = 
            {
                new Vertex2(new Vector2(0.5f, 0.5f), trcolour, new Vector2(1.0f, 1.0f)),
                new Vertex2(new Vector2(0.5f, -0.5f), Colour.White, new Vector2(1.0f, 0.0f)),
                new Vertex2(new Vector2(-0.5f, -0.5f), brcolour, new Vector2(0.0f, 0.0f)),
                new Vertex2(new Vector2(-0.5f, 0.5f), Colour.White, new Vector2(0.0f, 1.0f))
            };

            buffer.Write(data);



            
            using var layout = device.CreateVertexLayout();

            var posAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Index = 0,
                Offset = 0,
                Stride = Marshal.SizeOf<Vertex2>(),
                Usage = VertexAttributeUsage.Position
            };

            var colourAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector4f,
                Index = 1,
                Offset = Marshal.SizeOf<Vector2>(),
                Stride = Marshal.SizeOf<Vertex2>(),
                Usage = VertexAttributeUsage.Colour
            };

            var texAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Index = 2,
                Offset = Marshal.SizeOf<Vector2>() + Marshal.SizeOf<Vector3>(),
                Stride = Marshal.SizeOf<Vertex2>(),
                Usage = VertexAttributeUsage.TextureCoordinate
            };

            layout.SetAttribute(posAttrib);
            layout.SetAttribute(colourAttrib);
            layout.SetAttribute(texAttrib);




            using var elements = device.CreateIndexBuffer();
            elements.Usage = BufferUsage.Static;

            uint[] indexes = 
            { 
                0, 1, 3,
                1, 2, 3
            };

            elements.Write(indexes);




            using var texture = Image.LoadTexture(device, GetEmbeddedResource("punchy.png"), ImageFormat.PNG);

            using var shader = device.CreateShader(ShaderFormat.GLSL, GetEmbeddedResource("vert.glsl"), GetEmbeddedResource("frag.glsl"));

            shader.SetVertexData(buffer, layout, elements);
            shader.SetUniform("img", texture, 0);

            shader.SetUniform("transform", Matrix4x4.Identity);
            shader.SetUniform("brightness", 1f);

            window.Show();

            while (window.IsOpen)
            {
                window.ProcessEvents();
                device.Clear();

                device.DrawIndexedPrimitives(shader, DrawPrimitiveType.Triangles, 6);
                device.Display();
            }
        }
    }
}
