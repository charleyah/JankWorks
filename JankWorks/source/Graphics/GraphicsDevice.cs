using System;
using System.IO;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;

namespace JankWorks.Graphics
{
    public record GraphicsDeviceInfo
    (
        string Name,
        string Driver,
        GraphicsApi DriverApi,
        int MaxMultiSamples,
        int MaxTextureUnits
    );

    public abstract class GraphicsDevice : Surface
    {
        public abstract GraphicsDeviceInfo Info { get; } 

        protected IRenderTarget RenderTarget { get; private set; }

        protected GraphicsDevice(IRenderTarget renderTarget, DrawState defaultDrawState) : base(defaultDrawState)
        {
            this.RenderTarget = renderTarget;
            this.RenderTarget.OnResize += (viewport) => this.Viewport = viewport;
        }

        public virtual bool Activate(TimeSpan timeout)
        {
            this.RenderTarget.Activate();
            return true;
        }
        public virtual void Activate() => this.RenderTarget.Activate();

        public virtual void Deactivate() => this.RenderTarget.Deactivate();

        public override void Display() => this.RenderTarget.Render();


        public abstract TextureSurface CreateTextureSurface(SurfaceSettings settings);

        public abstract VertexBuffer<T> CreateVertexBuffer<T>() where T : unmanaged;

        public abstract VertexLayout CreateVertexLayout();

        public abstract IndexBuffer CreateIndexBuffer();
       

        public abstract bool IsShaderFormatSupported(ShaderFormat format);       
        
        public abstract Shader CreateShader(ShaderFormat format, Stream vertex, Stream fragment, Stream geometry = null);

        public abstract Shader CreateShader(ShaderFormat format, ReadOnlySpan<byte> vertex, ReadOnlySpan<byte> fragment, ReadOnlySpan<byte> geometry = default);

        public abstract Shader CreateShader(ShaderFormat format, string vertex, string fragment, string geometry = null);


        public abstract Texture2D CreateTexture2D(Vector2i size, PixelFormat format);

        public virtual Texture2D CreateTexture2D(Stream stream, ImageFormat format, TextureFilter filter = TextureFilter.Linear, TextureWrap wrap = TextureWrap.Clamp)
        {
            using var image = Image.Load(stream, format);

            var tex = this.CreateTexture2D(image.Size, PixelFormat.RGBA);
            tex.Filter = filter;
            tex.Wrap = wrap;
            image.CopyTo(tex);
            return tex;
        }


        public virtual SpriteRenderer CreateSpriteRenderer(Camera camera) => this.CreateSpriteRenderer(camera, SpriteRenderer.DrawOrder.Sequential);

        public abstract SpriteRenderer CreateSpriteRenderer(Camera camera, SpriteRenderer.DrawOrder drawOrder);

        public abstract TextRenderer CreateTextRenderer(Camera camera, Font font);


        public static GraphicsDevice Create(SurfaceSettings settings, IRenderTarget renderTarget)
        {
            return DriverConfiguration.Drivers.graphicsApi.CreateGraphicsDevice(settings, renderTarget);
        }
    }
}