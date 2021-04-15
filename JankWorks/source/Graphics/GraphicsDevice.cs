using System;
using System.IO;

using JankWorks.Drivers;

namespace JankWorks.Graphics
{
    public abstract class GraphicsDevice : Surface
    {
        protected IRenderTarget RenderTarget { get; private set; }

        protected GraphicsDevice(IRenderTarget renderTarget)
        {
            this.RenderTarget = renderTarget;
            this.RenderTarget.OnResize += (viewport) => this.Viewport = viewport;
        }

        public override void Activate() => this.RenderTarget.Activate();
        public override void Deactivate() => this.RenderTarget.Deactivate();
        public override void Display() => this.RenderTarget.Render();

        public abstract int MaxTextureUnits { get; }

        public abstract Surface CreateSurface(SurfaceSettings settings);
        public abstract VertexBuffer<T> CreateVertexBuffer<T>() where T : unmanaged;
        public abstract VertexLayout CreateVertexLayout();
        public abstract IndexBuffer CreateIndexBuffer();
       
        public abstract bool IsShaderFormatSupported(ShaderFormat format);        
        public abstract Shader CreateShader(ShaderFormat format, Stream vertex, Stream fragment, Stream geometry = null);
        public abstract Shader CreateShader(ShaderFormat format, ReadOnlySpan<byte> vertex, ReadOnlySpan<byte> fragment, ReadOnlySpan<byte> geometry = default);
        public abstract Shader CreateShader(ShaderFormat format, string vertex, string fragment, string geometry = null);

        public abstract Texture2D CreateTexture2D();

        public abstract void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count);
        public abstract void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount);
        public abstract void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count);
        public abstract void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount);

        public static GraphicsDevice Create(SurfaceSettings settings, IRenderTarget renderTarget)
        {
            return DriverConfiguration.Drivers.graphicsApi.CreateGraphicsDevice(settings, renderTarget);
        }
    }

    public enum DrawPrimitiveType
    {
        Points,
        Lines,
        LineStrip,
        LineLoop,

        Triangles,
        TriangleStrip
    }
}
