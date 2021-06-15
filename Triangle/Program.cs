using System.Numerics;
using System.Runtime.InteropServices;

using JankWorks.Drivers;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Triangle
{
    class Program
    {
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

        static void Main(string[] args)
        {
            DriverConfiguration.Initialise("JankWorks.Glfw", "JankWorks.OpenGL");

            var windowsettings = WindowSettings.Default;

            var surfacesettings = new SurfaceSettings()
            {
                ClearColour = Colour.Black,
                Size = windowsettings.VideoMode.Viewport.Size
            };

            using var window = Window.Create(windowsettings);
            using var device = GraphicsDevice.Create(surfacesettings, window);



            using var buffer = device.CreateVertexBuffer<Vertex>();
            buffer.Usage = BufferUsage.Static;

            Vertex[] vertices =
            {
                new Vertex(new Vector2(-0.5f, -0.5f), Colour.Green),
                new Vertex(new Vector2(0.0f, 0.5f), Colour.Red),
                new Vertex(new Vector2(0.5f, -0.5f), Colour.Blue),
            };

            buffer.Write(vertices);



            using var layout = device.CreateVertexLayout();

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



            using var shader = device.CreateShader(ShaderFormat.GLSL, VertexSource, FragSource);
            shader.SetVertexData(buffer, layout);



            window.OnKeyPressed += (keypress) => 
            {
                if (keypress.Key == Key.Enter) 
                {
                    window.Close();
                }
            };



            window.Show();

            while(window.IsOpen)
            {
                window.ProcessEvents();

                device.Clear();

                device.DrawPrimitives(shader, DrawPrimitiveType.Triangles, 0, 3);

                device.Display();
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
