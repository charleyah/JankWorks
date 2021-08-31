using System;

using JankWorks.Graphics;

using static OpenGL.Functions;
using static OpenGL.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    struct GLBuffer<T> where T : unmanaged
    {
        public uint BufferId;
        public int ElementCount;

        public void Generate()
        {
            uint id = 0;
            unsafe { glGenBuffers(1, &id); }
            this.BufferId = id;
        }

        public void Delete()
        {
            uint id = this.BufferId;
            unsafe { glDeleteBuffers(1, &id); }
        }

        public void Write(int target, BufferUsage usage, ReadOnlySpan<T> data)
        {
            try
            {
                glBindBuffer(target, this.BufferId);

                unsafe
                {
                    fixed (T* ptr = data)
                    {
                        glBufferData(target, (uint)(sizeof(T) * data.Length), ptr, usage.GetGLBufferUsage());
                    }
                    this.ElementCount = data.Length;
                }
            }
            finally
            {
                glBindBuffer(target, 0);
            }
        }

        public void Update(int target, BufferUsage usage, ReadOnlySpan<T> data, int offset)
        {
            var sliceUpperBound = offset + data.Length;

            if(offset < 0 || sliceUpperBound > this.ElementCount)
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                glBindBuffer(target, this.BufferId);

                unsafe
                {
                    fixed (T* ptr = data)
                    {
                        glBufferSubData(target, (uint)(sizeof(T) * offset), (uint)(sizeof(T) * data.Length), ptr);
                    }
                }
            }
            finally
            {
                glBindBuffer(target, 0);
            }
        }

        public void CopyTo(int target, Span<T> destination)
        {
            if (this.ElementCount == 0)
            {
                return;
            }

            try
            {
                glBindBuffer(target, this.BufferId);

                ReadOnlySpan<T> source;

                unsafe
                {
                    source = new ReadOnlySpan<T>(glMapBuffer(target, GL_READ_ONLY).ToPointer(), this.ElementCount);
                }

                source.CopyTo(destination);
            }
            finally
            {
                glUnmapBuffer(target);
                glBindBuffer(target, 0);
            }
        }

        public T[] Read(int target)
        {
            var data = new T[this.ElementCount];
            this.CopyTo(target, data);
            return data;
        }
    }
}
