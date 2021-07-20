using System;

using JankWorks.Graphics;

using static OpenGL.Constants;
using static OpenGL.Functions;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLTexture2D : Texture2D
    {
        internal uint Id;

        public GLTexture2D()
        {
            uint texid = 0;
            unsafe { glGenTextures(1, &texid); }
            this.Id = texid;
        }

        public GLTexture2D(uint id)
        {
            this.Id = id;
        }

        internal void Bind() => glBindTexture(GL_TEXTURE_2D, this.Id);

        internal void UnBind() => glBindTexture(GL_TEXTURE_2D, 0);


        public override void SetPixels(Vector2i size, ReadOnlySpan<byte> pixels, PixelFormat format)
        {
            switch(format)
            {
                case PixelFormat.RGB24:

                    unsafe
                    {
                        this.Bind();
                        fixed (byte* ptr = pixels)
                        {
                            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, size.X, size.Y, 0, GL_RGB, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                    }

                    return;

                case PixelFormat.GrayScale8:

                    unsafe
                    {
                        this.Bind();
                        glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

                        fixed (byte* ptr = pixels)
                        {
                            glTexImage2D(GL_TEXTURE_2D, 0, GL_RED, size.X, size.Y, 0, GL_RED, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                        glPixelStorei(GL_UNPACK_ALIGNMENT, 4);
                    }

                    return;
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<byte> pixels, PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.RGB24:

                    unsafe
                    {
                        this.Bind();
                        fixed (byte* ptr = pixels)
                        {
                            glTexSubImage2D(GL_TEXTURE_2D, 0, position.X, position.Y, size.X, size.Y, GL_RGB, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                    }

                    return;

                case PixelFormat.GrayScale8:

                    unsafe
                    {
                        this.Bind();
                        glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

                        fixed (byte* ptr = pixels)
                        {
                            glTexSubImage2D(GL_TEXTURE_2D, 0, position.X, position.Y, size.X, size.Y, GL_RED, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                        glPixelStorei(GL_UNPACK_ALIGNMENT, 4);
                    }

                    return;
            }
        }

        public override void SetPixels(Vector2i size, ReadOnlySpan<RGBA> pixels)
        {
            unsafe
            {
                fixed (RGBA* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, GL_RGBA, GL_UNSIGNED_BYTE);
                }
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<RGBA> pixels)
        {
            unsafe
            {
                fixed (RGBA* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, position, GL_RGBA, GL_UNSIGNED_BYTE);
                }
            }
        }


        public override void SetPixels(Vector2i size, ReadOnlySpan<ABGR> pixels)
        {
            unsafe
            {
                fixed (ABGR* ptr = pixels)
                {
                    var pixeltype = BitConverter.IsLittleEndian ? GL_UNSIGNED_INT_8_8_8_8 : GL_UNSIGNED_INT_8_8_8_8_REV;
                    this.SetPixels((IntPtr)ptr, size, GL_RGBA, pixeltype);
                }
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<ABGR> pixels)
        {
            unsafe
            {
                fixed (ABGR* ptr = pixels)
                {
                    var pixeltype = BitConverter.IsLittleEndian ? GL_UNSIGNED_INT_8_8_8_8 : GL_UNSIGNED_INT_8_8_8_8_REV;
                    this.SetPixels((IntPtr)ptr, size, position, GL_RGBA, pixeltype);
                }
            }
        }


        public override void SetPixels(Vector2i size, ReadOnlySpan<ARGB> pixels)
        {
            unsafe
            {
                fixed (ARGB* ptr = pixels)
                {
                    var pixeltype = BitConverter.IsLittleEndian ? GL_UNSIGNED_INT_8_8_8_8 : GL_UNSIGNED_INT_8_8_8_8_REV;
                    this.SetPixels((IntPtr)ptr, size, GL_BGRA, pixeltype);
                }
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<ARGB> pixels)
        {
            unsafe
            {
                fixed (ARGB* ptr = pixels)
                {
                    var pixeltype = BitConverter.IsLittleEndian ? GL_UNSIGNED_INT_8_8_8_8 : GL_UNSIGNED_INT_8_8_8_8_REV;
                    this.SetPixels((IntPtr)ptr, size, position, GL_BGRA, pixeltype);
                }
            }
        }


        public override void SetPixels(Vector2i size, ReadOnlySpan<BGRA> pixels)
        {
            unsafe
            {
                fixed (BGRA* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, GL_BGRA, GL_UNSIGNED_BYTE);
                }
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<BGRA> pixels)
        {
            unsafe
            {
                fixed (BGRA* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, position, GL_BGRA, GL_UNSIGNED_BYTE);
                }
            }
        }


        public override void SetPixels(Vector2i size, ReadOnlySpan<RGBA32> pixels)
        {
            unsafe
            {
                fixed (RGBA32* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, GL_RGBA, GL_UNSIGNED_INT_8_8_8_8);
                }
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<RGBA32> pixels)
        {
            unsafe
            {
                fixed (RGBA32* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, position, GL_RGBA, GL_UNSIGNED_INT_8_8_8_8);
                }
            }
        }


        public override void SetPixels(Vector2i size, ReadOnlySpan<ARGB32> pixels)
        {
            unsafe
            {
                fixed (ARGB32* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, GL_BGRA, GL_UNSIGNED_INT_8_8_8_8_REV);
                }
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<ARGB32> pixels)
        {
            unsafe
            {
                fixed (ARGB32* ptr = pixels)
                {
                    this.SetPixels((IntPtr)ptr, size, position, GL_BGRA, GL_UNSIGNED_INT_8_8_8_8_REV);
                }
            }
        }



        private void SetPixels(IntPtr ptr, Vector2i size, int pixelformat, int pixeltype)
        {
            try
            {
                this.Bind();
                glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, size.X, size.Y, 0, pixelformat, pixeltype, ptr);              
                this.ApplyStates();
            }
            finally
            {
                this.UnBind();
            }
        }

        private void SetPixels(IntPtr ptr, Vector2i size, Vector2i position, int pixelformat, int pixeltype)
        {
            try
            {
                this.Bind();
                glTexSubImage2D(GL_TEXTURE_2D, 0, position.X, position.Y, size.X, size.Y, pixelformat, pixeltype, ptr);
                this.ApplyStates();
            }
            finally
            {
                this.UnBind();
            }
        }

        internal void ApplyStates()
        {
            switch (this.Wrap)
            {
                case TextureWrap.Repeat:
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
                    break;

                case TextureWrap.Clamp:
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
                    break;

                case TextureWrap.Edge:
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
                    break;
            }

            switch(this.Filter)
            {
                case TextureFilter.Nearest:
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);


                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
                    break;

                case TextureFilter.Linear:
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);


                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
                    break;
            }
            glGenerateMipmap(GL_TEXTURE_2D);
        }

        protected override void Dispose(bool finalising)
        {
            this.UnBind();
            unsafe
            {                
                var id = this.Id;
                glDeleteTextures(1, &id);
            }

            base.Dispose(finalising);
        }
    }
}
