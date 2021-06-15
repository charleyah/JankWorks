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
                Size = windowSettings.VideoMode.Viewport.Size
            };

            using var window = Window.Create(windowSettings);
            using var device = GraphicsDevice.Create(surfaceSettings, window);

            surfaceSettings.ClearColour = Colour.Blue;
            using var framebuffer = device.CreateCanvas(surfaceSettings);
            framebuffer.ClearColour = Colour.Blue;
            

            Console.WriteLine(device.Info);

            
            using var buffer = device.CreateVertexBuffer<Vertex2>();
            buffer.Usage = BufferUsage.Static;

            Vertex2[] data = 
            {
                new Vertex2(new Vector2(0.5f, 0.5f), Colour.White, new Vector2(1.0f, 1.0f)),
                new Vertex2(new Vector2(0.5f, -0.5f), Colour.White, new Vector2(1.0f, 0.0f)),
                new Vertex2(new Vector2(-0.5f, -0.5f), Colour.White, new Vector2(0.0f, 0.0f)),
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




            using var triangle = new Triangle(device);
            

            shader.SetVertexData(buffer, layout, elements);
            shader.SetUniform("img", framebuffer.Texture, 0);

            shader.SetUniform("transform", Matrix4x4.Identity);
            shader.SetUniform("brightness", 1f);

            window.Show();



            while (window.IsOpen)
            {
                window.ProcessEvents();
                device.Clear();

                //shader.SetUniform("img", texture, 0);
                //device.DrawIndexedPrimitives(shader, DrawPrimitiveType.Triangles, 6);

                //framebuffer.Clear();
                
                //framebuffer.Display();
                triangle.Draw(device);

                //device.DrawIndexedPrimitives(shader, DrawPrimitiveType.Triangles, 6);

                device.Display();
            }
        }

        private const string TriangleVertexSource = @"
        #version 330 core
        layout(location = 0) in vec2 position;
        layout(location = 1) in vec3 vertcolour;
        
        out vec3 colour;

        void main()
        {
            colour = vertcolour;
            gl_Position = vec4(position, 0.0, 1.0);
        }
        ";

        private const string TriangleFragSource = @"
        #version 330 core
        
        out vec4 fragcolour;
        in vec3 colour;

        void main()
        {
            fragcolour = vec4(colour, 1.0);
        }
        ";




        class Triangle : IDisposable
        {
            private VertexBuffer<Vertex> buffer;
            private VertexLayout layout;
            private Shader shader;

            public Triangle(GraphicsDevice device)
            {
                this.buffer = device.CreateVertexBuffer<Vertex>();
                this.buffer.Usage = BufferUsage.Static;

                Vertex[] vertices =
                {
                    new Vertex(new Vector2(-0.5f, -0.5f), Colour.Green),
                    new Vertex(new Vector2(0.0f, 0.5f), Colour.Red),
                    new Vertex(new Vector2(0.5f, -0.5f), Colour.Blue),
                };

                this.buffer.Write(vertices);

                this.layout = device.CreateVertexLayout();

                var positionAttrib = new VertexAttribute()
                {
                    Format = VertexAttributeFormat.Vector2f,
                    Stride = Marshal.SizeOf<Vertex>(),
                    Offset = 0,
                    Index = 0,
                    Usage = VertexAttributeUsage.Position
                };

                var colourAttrib = new VertexAttribute()
                {
                    Format = VertexAttributeFormat.Vector3f,
                    Stride = Marshal.SizeOf<Vertex>(),
                    Offset = Marshal.SizeOf<Vector2>(),
                    Index = 1,
                    Usage = VertexAttributeUsage.Colour
                };

                this.layout.SetAttribute(positionAttrib);
                this.layout.SetAttribute(colourAttrib);

                this.shader = device.CreateShader(ShaderFormat.GLSL, TriangleVertexSource, TriangleFragSource);
                shader.SetVertexData(buffer, layout);
            }

            public void Dispose()
            {
                this.shader.Dispose();
                this.layout.Dispose();
                this.buffer.Dispose();
            }

            public void Draw(Surface surface) => surface.DrawPrimitives(this.shader, DrawPrimitiveType.Triangles, 0, 3);

            [StructLayout(LayoutKind.Sequential)]
            struct Vertex
            {
                public Vector2 Position;
                public Vector3 Colour;

                public Vertex(Vector2 position, RGBA colour) : this(position, (Vector3)colour) { }
                public Vertex(Vector2 position, Vector3 colour)
                {
                    this.Position = position;
                    this.Colour = colour;
                }
            }


        }
    }
}
