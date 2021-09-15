using System;

using JankWorks.Core;
using JankWorks.Platform;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;

using JankWorks.Graphics;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.OpenGL.Driver))]

namespace JankWorks.Drivers.OpenGL
{
    public sealed class Driver : Disposable, IGraphicsDriver
    {
        public GraphicsApi GraphicsApi => GraphicsApi.OpenGL;

        private LibraryLoader loader;

        public Driver()
        {
            var env = SystemEnvironment.Current;

            this.loader = env.OS switch
            {
                SystemPlatform.Windows => env.LoadLibrary("opengl32.dll"),
                SystemPlatform.Linux => env.LoadLibrary("libGL.so"),
                _ => throw new NotImplementedException()
            };
        }

        public GraphicsDevice CreateGraphicsDevice(SurfaceSettings settings, IRenderTarget renderTarget)
        {
            renderTarget.Activate();
            global::OpenGL.Loader.Init(this.loader);
            return new GLGraphicsDevice(settings, renderTarget);
        }
        

        public bool IsShaderFormatSupported(ShaderFormat format) => format == ShaderFormat.GLSL;

        protected override void Dispose(bool finalising)
        {
            this.loader.Dispose();
            base.Dispose(finalising);
        }
    }
}
