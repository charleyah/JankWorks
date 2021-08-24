using System;

using JankWorks.Graphics;

using static OpenGL.Constants;
using static OpenGL.Functions;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLTexture2D : Texture2D
    {
        internal uint Id;

        private int GLSourceFormat => this.Format switch
        {
            PixelFormat.GrayScale => GL_RED,
            PixelFormat.RGB => GL_RGB,
            PixelFormat.RGBA => GL_RGBA,
            _ => throw new NotImplementedException()
        };

        public GLTexture2D(Vector2i size, PixelFormat format, TextureWrap warp, TextureFilter filter) : base(size, format)
        {
            this.Wrap = warp;
            this.Filter = filter;

            uint texid = 0;
            unsafe { glGenTextures(1, &texid); }
            this.Id = texid;

            this.SetPixels(this.Size, ReadOnlySpan<byte>.Empty, this.Format);
        }

        public GLTexture2D(Vector2i size, PixelFormat format) : base(size, format)
        {
            uint texid = 0;
            unsafe { glGenTextures(1, &texid); }
            this.Id = texid;
            this.SetPixels(this.Size, ReadOnlySpan<byte>.Empty, this.Format);
        }

        public GLTexture2D(uint id, Vector2i size, PixelFormat format) : base(size, format)
        {
            this.Id = id;
        }

        internal void Bind() => glBindTexture(GL_TEXTURE_2D, this.Id);

        internal void UnBind() => glBindTexture(GL_TEXTURE_2D, 0);


        public override void SetPixels(Vector2i size, ReadOnlySpan<byte> pixels, PixelFormat format)
        {
            this.Size = size;
            switch(format)
            {
                case PixelFormat.RGB:

                    unsafe
                    {
                        this.Bind();
                        fixed (byte* ptr = pixels)
                        {
                            glTexImage2D(GL_TEXTURE_2D, 0, this.GLSourceFormat, size.X, size.Y, 0, GL_RGB, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                    }

                    return;

                case PixelFormat.GrayScale:

                    unsafe
                    {
                        this.Bind();
                        glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

                        fixed (byte* ptr = pixels)
                        {
                            glTexImage2D(GL_TEXTURE_2D, 0, this.GLSourceFormat, size.X, size.Y, 0, GL_RED, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                        glPixelStorei(GL_UNPACK_ALIGNMENT, 4);
                    }

                    return;

                case PixelFormat.RGBA:

                    unsafe
                    {
                        this.Bind();
                        fixed (byte* ptr = pixels)
                        {
                            glTexImage2D(GL_TEXTURE_2D, 0, this.GLSourceFormat, size.X, size.Y, 0, GL_RGBA, GL_UNSIGNED_BYTE, (IntPtr)ptr);
                        }
                        this.ApplyStates();
                        this.UnBind();
                    }

                    return;
            }
        }

        public override void SetPixels(Vector2i size, Vector2i position, ReadOnlySpan<byte> pixels, PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.RGB:

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

                case PixelFormat.GrayScale:

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



        public override void CopyTo(Span<byte> pixels, PixelFormat format)
        {
            switch(format)
            {
                case PixelFormat.GrayScale:


                    this.Bind();
                    glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

                    unsafe
                    {
                        fixed (byte* ptr = pixels)
                        {
                            glGetnTexImage(GL_TEXTURE_2D, 0, GL_RED, GL_UNSIGNED_BYTE, (uint)pixels.Length, (IntPtr)ptr);
                        }
                    }
                                            
                    glPixelStorei(GL_UNPACK_ALIGNMENT, 4);
                    this.UnBind();                                        
                    return;

                case PixelFormat.RGB:

                    unsafe
                    {
                        fixed (byte* ptr = pixels)
                        {
                            this.CopyTo((IntPtr)ptr, (uint)pixels.Length, GL_RGB, GL_UNSIGNED_BYTE);
                        }
                    }

                    return;

                case PixelFormat.RGBA:

                    unsafe
                    {
                        fixed (byte* ptr = pixels)
                        {
                            this.CopyTo((IntPtr)ptr, (uint)pixels.Length, GL_RGBA, GL_UNSIGNED_BYTE);
                        }
                    }

                    return;
            }
        }

        public override void CopyTo(Span<RGBA> pixels)
        {            
            unsafe
            {
                fixed (RGBA* ptr = pixels)
                {
                    this.CopyTo((IntPtr)ptr, (uint)(pixels.Length * sizeof(RGBA)), GL_RGBA, GL_UNSIGNED_BYTE);
                }
            }
        }

        public override void CopyTo(Span<ABGR> pixels)
        {
            unsafe
            {

                fixed (ABGR* ptr = pixels)
                {
                    var pixeltype = BitConverter.IsLittleEndian ? GL_UNSIGNED_INT_8_8_8_8 : GL_UNSIGNED_INT_8_8_8_8_REV;
                    this.CopyTo((IntPtr)ptr, (uint)(pixels.Length * sizeof(ABGR)), GL_RGBA, pixeltype);
                }
            }
        }

        public override void CopyTo(Span<ARGB> pixels)
        {
            unsafe
            {

                fixed (ARGB* ptr = pixels)
                {
                    var pixeltype = BitConverter.IsLittleEndian ? GL_UNSIGNED_INT_8_8_8_8 : GL_UNSIGNED_INT_8_8_8_8_REV;
                    this.CopyTo((IntPtr)ptr, (uint)(pixels.Length * sizeof(ARGB)), GL_BGRA, pixeltype);
                }
            }
        }

        public override void CopyTo(Span<BGRA> pixels)
        {
            unsafe
            {
                fixed (BGRA* ptr = pixels)
                {
                    this.CopyTo((IntPtr)ptr, (uint)(pixels.Length * sizeof(BGRA)), GL_BGRA, GL_UNSIGNED_BYTE);
                }
            }
        }

        public override void CopyTo(Span<RGBA32> pixels)
        {
            unsafe
            {
                fixed (RGBA32* ptr = pixels)
                {
                    this.CopyTo((IntPtr)ptr, (uint)(pixels.Length * sizeof(RGBA32)), GL_RGBA, GL_UNSIGNED_INT_8_8_8_8);
                }
            }
        }

        public override void CopyTo(Span<ARGB32> pixels)
        {
            unsafe
            {
                fixed (ARGB32* ptr = pixels)
                {
                    this.CopyTo((IntPtr)ptr, (uint)(pixels.Length * sizeof(ARGB32)), GL_RGBA, GL_UNSIGNED_INT_8_8_8_8_REV);
                }
            }
        }



        private void SetPixels(IntPtr ptr, Vector2i size, int pixelformat, int pixeltype)
        {
            try
            {
                this.Bind();
                glTexImage2D(GL_TEXTURE_2D, 0, this.GLSourceFormat, size.X, size.Y, 0, pixelformat, pixeltype, ptr);
                this.Size = size;
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

        private void CopyTo(IntPtr ptr, uint buffersize, int pixelformat, int pixeltype)
        {
            try
            {
                this.Bind();
                glGetnTexImage(GL_TEXTURE_2D, 0, pixelformat, pixeltype, buffersize, ptr);
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