# JankWorks

JankWorks is a low level multimedia wrapper and framework suitable for game and game engine development. Currently in-development and not ready for use.

| Project            | Description                                   |
| ------------------ | --------------------------------------------- |
| JankWorks          | Multimedia API abstraction                    |
| JankWorks.Game     | Game framework API                            |
| JankWorks.FreeType | FreeType driver that implements font loading  |
| JankWorks.Glfw     | Glfw driver that implements window management |
| JankWorks.OpenGL   | OpenGL driver that implements graphics API    |
| JankWorks.OpenAL   | OpenAL driver that implements audio API       |
| JankWorks.DotNet   | .NET driver that implements Image loading     |

JankWorks is built with C# 9.0 and only supports .NET 5 onwards.

## Lets Draw A Triangle

Just like the first tutorials for using any low level graphics API like DirectX, OpenGL or Vulkan... lets draw a triangle.

### Initialising JankWorks

JankWorks is a decoupled library that requires implementations called drivers to be loaded before being used. This is done via the `DriverConfiguration` class.

```csharp
DriverConfiguration.Initialise("JankWorks.Glfw", "JankWorks.OpenGL");
```

Here we are specifying assemblies to load that include drivers for JankWorks. As we're going to draw a triangle we need to load windowing and graphics that are provided by Glfw and OpenGL respectively. If part of JankWorks is used that has no loaded driver, then `DriverUnitialisedException` is thrown.

### Creating Window and Graphics Device

With Glfw and OpenGL drivers loaded, we can proceed to create a window to display our triangle and a graphics device to draw it.

```csharp
var windowsettings = WindowSettings.Default;

var surfacesettings = new SurfaceSettings()
{
    ClearColour = Colour.Black,
    Size = windowsettings.VideoMode.Viewport.Size
};

using var window = Window.Create(windowsettings);
using var device = GraphicsDevice.Create(surfacesettings, window);
```

We start with specifying the window settings, which include information such as window size, what style it is (e.g windowed, borderless or full screen) and other things like VSync and cursor visibility. Selecting Default will make a borderless full screen window on the primary monitor.

Next is the surface settings, that describe information about the surface we'll draw on. We set the clear colour to black and the size to that of the video mode viewport specified by the window settings.

We finish with creating the window and then the graphics device, specifying the window as its render target.

### The Basic Steps

To draw a triangle, we need to provide the graphics device with three major components, vertex buffer that stores position and colour data, a vertex layout which describes the memory layout of the vertex data and how its passed to the last component, a shader program that is then executed by the GPU to draw the vertex data.

### Vertex Data

To make data accessible to a shader program on the GPU, we need to store it in a vertex buffer. We want to include the screen positions of each corner of the triangle and what colour they should be. We can represent each corner with a struct like the following.

```csharp
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
```

The `StructLayout` attribute is required to prevent the compiler from potentially rearranging the order of Position and Colour in memory. We'll be telling the GPU how to read this struct later, so its important its members are in an order we expect.

lets next create a buffer to store the position and colour data.

```csharp
using var buffer = device.CreateVertexBuffer<Vertex>();
buffer.Usage = BufferUsage.Static;

Vertex[] vertices =
{
    new Vertex(new Vector2(-0.5f, -0.5f), Colour.Green), // bottom left
    new Vertex(new Vector2(0.0f, 0.5f), Colour.Red), // top
    new Vertex(new Vector2(0.5f, -0.5f), Colour.Blue), // bottom right
};

buffer.Write(vertices);
```

After creating the buffer, we set the usage property which is a hint to the GPU on how this buffer is intended to be used and because the data isn't changing, we set it to static. Next we create our 3 points of the triangle and write them to the buffer. By default the centre of the screen will be X,Y position of 0,0 so we specify the corners relative to the centre of the screen.

### Vertex Layout

We now have a buffer with our positioning and colour data in it. But in order for a shader program to make use of it, we need to define the memory layout and how to access it.

```csharp
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
```

The shader program will need to read two values from each vertex in the buffer, position and colour separately. To do this we create a `VertexAttribute` to describe each one.

- `Format` - defines the data type the shader should expect, this will match the type corresponding to the data member in the `Vertex` structure (e.g Position is `Vector2` which is two floats so the format is `Vector2f`). 
- `Stride` - defines how many bytes are between each vertex in the buffer. We set this to the byte size of the `Vertex` structure.
- `Offset` - defines where in the vertex memory the value is. For Position this is zero as its the first value and for Colour we set it to the size of Position's data type as Colour proceeds it.
- `Index` - specifies the location the shader will use to access the value.
- `Usage` - Hint to the shader of what the value is being used for.

### Shader Program

Now that we have vertex data described and in a buffer, we can now create a shader program to draw it. There are different kinds of shaders that make up a shader program. We'll be concerning ourselves with two kinds of shader, vertex and fragment shaders. The pipeline starts with vertex shaders processing vertex data and coordinate information followed by the fragment shader that handles pixel colouring, there is also a third shader called geometry shader but we don't require one to draw a triangle. As we're using OpenGL for our graphics driver, we'll be using GLSL for writing shader programs.

```csharp
using var shader = device.CreateShader(ShaderFormat.GLSL, VertexSource, FragSource);
```

#### Vertex Shader

```glsl
#version 330 core
layout(location = 0) in vec2 position;
layout(location = 1) in vec3 vertcolour;

out vec3 colour;

void main()
{
	colour = vertcolour;
	gl_Position = vec4(position, 0.0, 1.0);
}
```

The vertex shader, like all shaders have in and out parameters along with some special purpose ones. Both `position` and `vertcolour` are in parameters that come from our vertex buffer we created earlier. The `layout(location = 0)` statement corresponds to the index we specified when creating the vertex attribute for position and colour respectively. We require to set the coordinate for the current vertex to draw which is done by setting the gl_Position variable to the position parameter passed from the vertex buffer. Lastly, we specify an out parameter called colour and set that to the colour passed from the vertex buffer. This out parameter is passed down the graphics pipeline to the fragment shader.

#### Fragment Shader

```glsl
#version 330 core
        
out vec4 fragcolour;
in vec3 colour;

void main()
{
	fragcolour = vec4(colour, 1.0);
}
```

The fragment shader is responsible for setting pixel colour data and so we simply want to set it to the colour passed in from the vertex shader that read it from the vertex buffer. Unlike the vertex shader, the fragment shader doesn't have a special variable to assign the colour value. Its passed down the pipeline to the next step so we simply assign the colour value to an out parameter. 

#### Binding It All Together

Now that we've got our shader program created, we now need to bind it with the vertex buffer and vertex layout that's done as follows

```c#
shader.SetVertexData(buffer, layout);
```

Our shader program is now ready to be used by the graphics device.

### One Last Thing

When we finally get to see our triangle its going to be on a full screen window, so lets make sure we have a way of closing the window.

```csharp
window.OnKeyPressed += (keypress) => 
{
    if (keypress.Key == Key.Enter) 
    {
        window.Close();
    }
};
```

The window will now close by pressing the enter key.

### Show Me The Pixels

Its time to finally display our triangle.

```csharp
window.Show();

while(window.IsOpen)
{
    window.ProcessEvents();

    device.Clear();

    device.DrawPrimitives(shader, DrawPrimitiveType.Triangles, 0, 3);

    device.Display();
}
```

We start by making our window visible, then we go into a loop for handling application events and drawing. In our loop we first process application events and then through the graphics device clear the surface/canvas. Next we execute our shader program by calling draw command on it by the graphics device, we specify we're drawing triangles and the number of vertices which is 3, one for each corner. Finally we render to the window via the display command.

![](https://raw.githubusercontent.com/DangerRoss/JankWorks/main/Triangle/result.png)

You can view the full source code [here](https://github.com/DangerRoss/JankWorks/blob/main/Triangle/Program.cs)



