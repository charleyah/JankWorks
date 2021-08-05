using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.IO;
using System.Threading;


using JankWorks.Drivers.Graphics;
using JankWorks.Drivers.OpenGL.Graphics;

using JankWorks.Graphics;
using JankWorks.Util;

using static OpenGL.Constants;
using static OpenGL.Functions;

namespace JankWorks.Drivers.OpenGL
{
    sealed class GLGraphicsDevice : GraphicsDevice
    {
        public override RGBA ClearColour
        {
            get => (RGBA)this.clearColour;
            set => this.clearColour = (Vector4)value;            
        }

        public override Rectangle Viewport
        {
            get => this.viewport;
            set
            {
                glBindFramebuffer(GL_FRAMEBUFFER, 0);
                glViewport(value.Position.X, value.Position.Y, value.Size.X, value.Size.Y);
                this.viewport = value;
            }
        }

        public override GraphicsDeviceInfo Info => this.info;

        private Rectangle viewport;
        private Vector4 clearColour;

        private readonly GraphicsDeviceInfo info;

        public GLGraphicsDevice(SurfaceSettings settings, IRenderTarget renderTarget) : base(renderTarget, DrawState.Default)
        {
            this.Viewport = new Rectangle(new Vector2i(0, 0), settings.Size);
            this.ClearColour = settings.ClearColour;
            this.info = this.GetDeviceInfo();
        }

        private GraphicsDeviceInfo GetDeviceInfo()
        {
            string name = new CString(glGetString(GL_RENDERER));
            string driver = new CString(glGetString(GL_VERSION));

            var maxTextures = 0;
            var maxSamples = 0;

            unsafe 
            { 
                glGetIntegerv(GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS, &maxTextures);
                glGetIntegerv(GL_MAX_SAMPLES, &maxSamples);
            }
         
            return new GraphicsDeviceInfo(name, driver, GraphicsApi.OpenGL, maxSamples, maxTextures);
        }


        public override bool Activate(TimeSpan timeout)
        {
            var mutex = typeof(GLGraphicsDevice);

            if (Monitor.IsEntered(mutex))
            {
                return true;
            }
            else
            {
                var entered = Monitor.TryEnter(mutex, timeout);

                if (entered)
                {
                    base.Activate();
                }

                return entered;
            }
        }

        public override void Activate()
        {
            var mutex = typeof(GLGraphicsDevice);
            if (Monitor.IsEntered(mutex))
            {
                return;
            }
            else
            {
                Monitor.Enter(mutex);
                base.Activate();
            }
        }

        public override void Deactivate()
        {
            var mutex = typeof(GLGraphicsDevice);

            if (Monitor.IsEntered(mutex))
            {
                base.Deactivate();
                Monitor.Exit(mutex);
            }
            else
            {
                throw new SynchronizationLockException();
            }
        }


        public override void Clear(ClearBitMask bits)
        {            
            glBindFramebuffer(GL_FRAMEBUFFER, 0);

            var colour = this.clearColour;
            glClearColor(colour.X, colour.Y, colour.Z, colour.W);

            glClear(bits.GetGLClearBits());
        }
        
        public override Shader CreateShader(ShaderFormat format, Stream vertex, Stream fragment, Stream geometry = null)
        {
            if (!this.IsShaderFormatSupported(format))
            {
                throw new NotSupportedException();
            }

            var vertexId = GlslCompiler.CompileShader(vertex, GL_VERTEX_SHADER);
            var fragmentId = GlslCompiler.CompileShader(fragment, GL_FRAGMENT_SHADER);
            var geometryId = GlslCompiler.CompileShader(geometry, GL_GEOMETRY_SHADER);

            var program = GlslCompiler.LinkProgram(vertexId, fragmentId, geometryId, true);

            return new GLShader(program);
        }

        public override Shader CreateShader(ShaderFormat format, ReadOnlySpan<byte> vertex, ReadOnlySpan<byte> fragment, ReadOnlySpan<byte> geometry = default)
        {
            if (!this.IsShaderFormatSupported(format))
            {
                throw new NotSupportedException();
            }

            var vertexId = GlslCompiler.CompileShader(vertex, GL_VERTEX_SHADER);
            var fragmentId = GlslCompiler.CompileShader(fragment, GL_FRAGMENT_SHADER);
            var geometryId = GlslCompiler.CompileShader(geometry, GL_GEOMETRY_SHADER);

            var program = GlslCompiler.LinkProgram(vertexId, fragmentId, geometryId, true);

            return new GLShader(program);
        }

        public override Shader CreateShader(ShaderFormat format, string vertex, string fragment, string geometry = null)
        {
            if (!this.IsShaderFormatSupported(format))
            {
                throw new NotSupportedException();
            }

            var vertexId = GlslCompiler.CompileShader(Encoding.UTF8.GetBytes(vertex), GL_VERTEX_SHADER);
            var fragmentId = GlslCompiler.CompileShader(Encoding.UTF8.GetBytes(fragment), GL_FRAGMENT_SHADER);
            var geometryId = (geometry != null) ? GlslCompiler.CompileShader(Encoding.UTF8.GetBytes(geometry), GL_GEOMETRY_SHADER) : 0;

            var program = GlslCompiler.LinkProgram(vertexId, fragmentId, geometryId, true);

            return new GLShader(program);
        }

        public override Texture2D CreateTexture2D() => new GLTexture2D();

        public override Texture2D[] CreateTexture2Ds(int count)
        {
            var ids = new uint[count];
            unsafe
            {
                fixed(uint* idsptr = ids)
                {
                    glGenTextures(count, idsptr);
                }
            }
            return (from id in ids select new GLTexture2D(id)).ToArray();
        }

        public override TextureSurface CreateTextureSurface(SurfaceSettings settings) => new GLTextureSurface(settings);

        public override VertexBuffer<T> CreateVertexBuffer<T>() => new GLVertexBuffer<T>();

        public override VertexLayout CreateVertexLayout() => new GLVertexLayout();

        public override IndexBuffer CreateIndexBuffer() => new GLIndexBuffer();

        public override bool IsShaderFormatSupported(ShaderFormat format) => format == ShaderFormat.GLSL;

        public override void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            glDrawArrays(primitive.GetGLPrimitive(), offset, count);
            program.UnBind();
        }

        public override void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawArraysInstanced(primitive.GetGLPrimitive(), offset, count, instanceCount); }
            program.UnBind();
        }
        public override void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawElements(primitive.GetGLPrimitive(), count, GL_UNSIGNED_INT, (void*)0); }
            program.UnBind();
        }

        public override void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawElementsInstanced(primitive.GetGLPrimitive(), count, GL_UNSIGNED_INT, (void*)0, instanceCount); }
            program.UnBind();
        }

        protected override void ApplyDrawState(in DrawState drawState) => drawState.Process();
    }
}
