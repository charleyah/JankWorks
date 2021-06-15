using System;
using System.Numerics;

using JankWorks.Graphics;

using static OpenGL.Constants;
using static OpenGL.Functions;


namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLCanvas : Canvas
    {
        public override RGBA ClearColour
        {
            get => this.clearColour;
            set
            {
                var vecColour = (Vector4)value;

                glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
                glClearColor(vecColour.X, vecColour.Y, vecColour.Z, vecColour.W);
                this.clearColour = value;
            }
        }

        public override Rectangle Viewport
        {
            get => this.viewport;
            set
            {
                glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
                glViewport(value.Position.X, value.Position.Y, value.Size.X, value.Size.Y);
                this.viewport = value;
            }
        }

        public override Texture2D Texture => this.texture;

        private Rectangle viewport;
        private RGBA clearColour;

        private uint fbo;
        private uint rbo;

        private GLTexture2D texture;

        public GLCanvas(GLGraphicsDevice device, SurfaceSettings settings)
        {
            this.texture = new GLTexture2D();
            this.texture.Filter = TextureFilter.Linear;
            this.texture.Wrap = TextureWrap.Clamp;
            this.texture.SetPixels(settings.Size, ReadOnlySpan<RGBA>.Empty);

            unsafe
            {
                uint id = 0;
                glGenFramebuffers(1, &id);
                this.fbo = id;

                glGenRenderbuffers(1, &id);
                this.rbo = id;
            }


            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);            
            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, this.texture.Id, 0);

            glBindRenderbuffer(GL_RENDERBUFFER, this.rbo);
            glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH24_STENCIL8, settings.Size.X, settings.Size.Y);
            glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT, GL_RENDERBUFFER, this.rbo);

            if(glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
            {
                throw new Exception("framebuffer go oof");
            }

            this.Viewport = new Rectangle(new Vector2i(0, 0), settings.Size);

            glBindFramebuffer(GL_FRAMEBUFFER, 0);
            glBindRenderbuffer(GL_RENDERBUFFER, 0);
        }

        public override void Clear(ClearBitMask bits)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
            glClear(bits.GetGLClearBits());
        }

        public override void Display() => glBindFramebuffer(GL_FRAMEBUFFER, 0);
        

        public override void DrawPrimitives(Shader shader, DrawPrimitiveType primitive, int offset, int count)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            glDrawArrays(primitive.GetGLPrimitive(), offset, count);
            program.UnBind();
        }

        public override void DrawPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int offset, int count, int instanceCount)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawArraysInstanced(primitive.GetGLPrimitive(), offset, count, instanceCount); }
            program.UnBind();
        }
        public override void DrawIndexedPrimitives(Shader shader, DrawPrimitiveType primitive, int count)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawElements(primitive.GetGLPrimitive(), count, GL_UNSIGNED_INT, (void*)0); }
            program.UnBind();
        }

        public override void DrawIndexedPrimitivesInstanced(Shader shader, DrawPrimitiveType primitive, int count, int instanceCount)
        {
            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);
            var program = (GLShader)shader;
            program.Bind();
            program.BindTextures();
            unsafe { glDrawElementsInstanced(primitive.GetGLPrimitive(), count, GL_UNSIGNED_INT, (void*)0, instanceCount); }
            program.UnBind();
        }

        protected override void Dispose(bool finalising)
        {
            this.texture.Dispose();
            unsafe
            {
                uint id = this.rbo;
                glDeleteRenderbuffers(1, &id);
                id = this.fbo;
                glDeleteFramebuffers(1, &id);
            }
            base.Dispose(finalising);
        }
    }
}
