using System;

namespace JankWorks.Util
{
    public sealed class DynamicArrayBuffer<T> : DynamicBuffer<T> where T : unmanaged
    {
        public override ref T this[int index] => ref this.buffer[index];

        public override int Position
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

        public override int Capacity => this.buffer.Length;

        private T[] buffer;
        private int cursor;

        public DynamicArrayBuffer() : this(16) { }

        public DynamicArrayBuffer(int capacity)
        {
            this.buffer = new T[capacity];
            this.cursor = 0;
        }

        public override void Clear() => this.Clear(0, this.buffer.Length);

        public override void Clear(int offset, int length)
        {
            Array.Clear(this.buffer, offset, length);
            this.cursor = 0;
        }

        public override void Compact()
        {
            int length = this.Length;

            if (this.Capacity > length)
            {
                Array.Resize(ref this.buffer, length);
            }
        }

        public override void Reserve(int count)
        {
            int required = this.cursor + count;
            int size = this.Capacity;

            if (required > 0 && required >= size)
            {
                if (size == 0)
                {
                    size = required;
                }
                else if (required % size == 0)
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

        public override Span<T> GetBufferSpan() => new Span<T>(this.buffer);

        public override Span<T> GetSpan() => new Span<T>(this.buffer, 0, this.cursor);

        public override void Write(T value)
        {
            this.Reserve(1);
            this.buffer[this.cursor++] = value;
        }

        public override void Write(ReadOnlySpan<T> values)
        {
            var length = values.Length;
            this.Reserve(length);

            var bufferSpace = new Span<T>(this.buffer, this.cursor, length);
            values.CopyTo(bufferSpace);

            this.cursor += length;
        }        
    }
}