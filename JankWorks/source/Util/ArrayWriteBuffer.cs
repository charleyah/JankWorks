using System;

namespace JankWorks.Util
{
    public class ArrayWriteBuffer<T> : IDynamicBuffer<T>, IWriteBuffer<T> where T : unmanaged
    {
        public ref T this[int index] => ref this.buffer[index];

        public int WritePosition
        {
            get => this.cursor;
            set
            {
                if (value < 0 || value >= this.Capacity)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    this.cursor = value;
                }
            }
        }

        public virtual int Length => this.WritePosition;

        public virtual int Capacity => this.buffer.Length;

        internal T[] buffer;
        private int cursor;

        public ArrayWriteBuffer() : this(16) { }

        public ArrayWriteBuffer(int capacity)
        {
            this.buffer = new T[capacity];
            this.cursor = 0;
        }

        public void Clear() => this.Clear(0, this.buffer.Length);

        public virtual void Clear(int offset, int length)
        {
            Array.Clear(this.buffer, offset, length);
            this.cursor = 0;
        }

        public virtual void Compact()
        {
            int length = this.WritePosition;

            if (this.Capacity > length)
            {
                Array.Resize(ref this.buffer, length);
            }
        }

        public virtual void Reserve(int count)
        {
            int required = this.cursor + count;
            int size = this.Capacity;

            if (required > 0 && required >= size)
            {
                if (size == 0)
                {
                    size = required;
                }
                else if (required > size && required % size == 0)
                {
                    size = required;
                }
                else
                {
                    size = (size - required % size) + required;
                }
                Array.Resize(ref this.buffer, size);
            }
        }

        public T[] GetBuffer() => this.buffer;

        public Span<T> GetBufferSpan() => new Span<T>(this.buffer);

        public virtual Span<T> GetSpan() => this.GetWritten();

        public Span<T> GetWritten() => new Span<T>(this.buffer, 0, this.cursor);

        public void Write(T value)
        {
            this.Reserve(1);
            this.buffer[this.cursor++] = value;
        }

        public void Write(ReadOnlySpan<T> values)
        {
            var length = values.Length;
            this.Reserve(length);

            var bufferSpace = new Span<T>(this.buffer, this.cursor, length);
            values.CopyTo(bufferSpace);

            this.cursor += length;
        }       
    }
}