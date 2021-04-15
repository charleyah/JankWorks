using System;
using System.Numerics;
using System.Text;
using System.IO;

using JankWorks.Drivers.OpenGL.Graphics;

using JankWorks.Graphics;

using static OpenGL.Constants;
using static OpenGL.Functions;

namespace JankWorks.Drivers.OpenGL
{
    sealed class GLGraphicsDevice : GraphicsDevice
    {
        public override RGBA ClearColour
        {
            get => this.clearColour;
            set
            {
                var vecColour = (Vector4)value;
                glClearColor(vecColour.X, vecColour.Y, vecColour.Z, vecColour.W);
                this.clearColour = value;
            }
        }

        public override Rectangle Viewport
        {
            get => this.viewport;
            set
            {
                glViewport(value.Position.X, value.Position.Y, value.Size.X, value.Size.Y);
                this.viewport = value;
            }
        }

        public override int MaxTextureUnits
        {
            get
            {
                var maxTextures = 0;
                unsafe { glGetIntegerv(GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS, &maxTextures); }
                return maxTextures;
            }
        }

        private Rectangle viewport;
        private RGBA clearColour;

        public GLGraphicsDevice(SurfaceSettings settings, IRenderTarget renderTarget) : base(renderTarget)
        {
            this.Viewport = settings.Viewport;
            this.ClearColour = settings.ClearColour;
        }

        public override void Clear() => glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT | GL_ACCUM_BUFFER_BIT);

        public override void CopyToTexture(Texture2D texture) => throw new NotImplementedException();
        
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

            return new GLShader(program, this);
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

            return new GLShader(program, this);
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

            return new GLShader(program, this);
        }

        public override Texture2D CreateTexture2D() => new GLTexture2D();

        public override Surface CreateSurface(SurfaceSettings settings) => throw new NotImplementedException();

        public override VertexBuffer<T> CreateVertexBuffer<T>() => new GLVertexBuffer<T>();

        public override VertexLayout CreateVertexLayout() => new GLVertexLayout();

        public override IndexBuffer CreateIndexBuffer() => new GLIndexBuffer();

        public override bool IsShaderFormatSupported(ShaderFormat format) => format == ShaderFormat.GLSL;

        public override void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count)
        {
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            glDrawArrays(primitive.GetGLPrimitive(), offset, count);
            program.UnBind();
        }

        public override void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount)
        {
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawArraysInstanced(primitive.GetGLPrimitive(), offset, count, instanceCount); }
            program.UnBind();
        }
        public override void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count)
        {
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawElements(primitive.GetGLPrimitive(), count, GL_UNSIGNED_INT, (void*)0); }
            program.UnBind();
        }

        public override void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount)
        {
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawElementsInstanced(primitive.GetGLPrimitive(), count, GL_UNSIGNED_INT, (void*)0, instanceCount); }
            program.UnBind();
        }
    }
}
