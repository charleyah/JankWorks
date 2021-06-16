using System;
using System.Numerics;
using System.Runtime.InteropServices;

using JankWorks.Core;
using JankWorks.Interface;
using JankWorks.Graphics;
using JankWorks.Drivers;


namespace Tests
{
    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        struct TexVertex
        {
            public Vector2 Position;
            public Vector2 TextCoord;
            public TexVertex(Vector2 position, Vector2 textCoord)
            {
                this.Position = position;
                this.TextCoord = textCoord;
            }
        }

        static Program()
        {
            DriverConfiguration.Initialise("JankWorks.Glfw", "JankWorks.OpenGL");
        }

        static void Main(string[] args)
        {
            var windowsettings = WindowSettings.Default;

            var surfacesettings = new SurfaceSettings()
            {
                ClearColour = Colour.Black,
                Size = windowsettings.VideoMode.Viewport.Size
            };

            using var window = Window.Create(windowsettings);
            using var device = GraphicsDevice.Create(surfacesettings, window);

            using var triangle = new Triangle(device);

            using var shader = GetTextureRenderShader(device, out var quad, out var layout, out var indexes);

            using var canvas = device.CreateCanvas(surfacesettings);

            shader.SetUniform("rtexture", canvas.Texture, 0);

            window.OnKeyPressed += (keypress) =>
            {
                if (keypress.Key == Key.Enter)
                {
                    window.Close();
                }
            };

            window.Show();

            device.Clear();

            while (window.IsOpen)
            {
                window.ProcessEvents();



                canvas.Clear();
                triangle.Draw(canvas);
                canvas.Display();

                device.Clear();

                device.DrawIndexedPrimitives(shader, DrawPrimitiveType.Triangles, 6);

                device.Display();
            }
        }

        static Shader GetTextureRenderShader(GraphicsDevice device, out VertexBuffer<TexVertex> quad, out VertexLayout layout, out IndexBuffer indexes)
        {
            quad = device.CreateVertexBuffer<TexVertex>();
            quad.Usage = BufferUsage.Static;
            TexVertex[] quadData =
            {
                new TexVertex(new Vector2(1f,  1f), new Vector2(1f, 1f)),
                new TexVertex(new Vector2(1f, -1f), new Vector2(1f, 0f)),
                new TexVertex(new Vector2(-1f, -1f), new Vector2(0f, 0f)),
                new TexVertex(new Vector2(-1f,  1f), new Vector2(0f, 1f)),

            };
            quad.Write(quadData);


            layout = device.CreateVertexLayout();

            var posAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Index = 0,
                Offset = 0,
                Stride = Marshal.SizeOf<TexVertex>(),
                Usage = VertexAttributeUsage.Position
            };

            var texAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Index = 1,
                Offset = Marshal.SizeOf<Vector2>(),
                Stride = Marshal.SizeOf<TexVertex>(),
                Usage = VertexAttributeUsage.TextureCoordinate
            };

            layout.SetAttribute(posAttrib);
            layout.SetAttribute(texAttrib);

            indexes = device.CreateIndexBuffer();
            indexes.Usage = BufferUsage.Static;

            uint[] indexValues =
            {
                0, 1, 3,
                1, 2, 3
            };

            indexes.Write(indexValues);



            const string VertexSource = @"
            #version 330 core
            layout(location = 0) in vec2 position;
            layout(location = 1) in vec2 texpos;
        
            out vec2 texcoord;

            void main()
            {
                texcoord = texpos;
                gl_Position = vec4(position, 0.0, 1.0);
            }
            ";

            const string FragSource = @"
            #version 330 core

            in vec2 texcoord;
            out vec4 fragcolour;

            uniform sampler2D rtexture;

            void main()
            {
                fragcolour = texture(rtexture, texcoord);
            }
            ";

            var shader = device.CreateShader(ShaderFormat.GLSL, VertexSource, FragSource);
            shader.SetVertexData(quad, layout, indexes);
            return shader;
        }
    }





    sealed class Triangle : Disposable
    {
        private VertexBuffer<Vertex> buffer;
        private VertexLayout layout;
        private Shader shader;

        public Triangle(GraphicsDevice device)
        {
            this.buffer = CreateBufferData(device);
            this.layout = CreateLayout(device);
            this.shader = CreateShader(device);
            this.shader.SetVertexData(this.buffer, this.layout);
        }

        private VertexBuffer<Vertex> CreateBufferData(GraphicsDevice device)
        {
            var buffer = device.CreateVertexBuffer<Vertex>();
            buffer.Usage = BufferUsage.Static;

            Vertex[] vertices =
            {
                new Vertex(new Vector2(-0.5f, -0.5f), Colour.Green),
                new Vertex(new Vector2(0.0f, 0.5f), Colour.Red),
                new Vertex(new Vector2(0.5f, -0.5f), Colour.Blue),
            };

            buffer.Write(vertices);

            return buffer;
        }

        private VertexLayout CreateLayout(GraphicsDevice device)
        {
            var layout = device.CreateVertexLayout();

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

            layout.SetAttribute(positionAttrib);
            layout.SetAttribute(colourAttrib);

            return layout;
        }

        private Shader CreateShader(GraphicsDevice device)
        {
            var shader = device.CreateShader(ShaderFormat.GLSL, VertexSource, FragSource);
            return shader;
        }
            

        public void Draw(Surface surface)
        {
            surface.DrawPrimitives(this.shader, DrawPrimitiveType.Triangles, 0, 3);
        }

        protected override void Dispose(bool finalising)
        {
            this.shader.Dispose();
            this.layout.Dispose();
            this.buffer.Dispose();
            base.Dispose(finalising);
        }

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

        private const string VertexSource = @"
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

        private const string FragSource = @"
        #version 330 core
        
        out vec4 fragcolour;
        in vec3 colour;

        void main()
        {
            fragcolour = vec4(colour, 1.0);
        }
        ";
    }
}
