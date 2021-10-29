using System;

namespace JankWorks.Util
{
    public sealed class ArrayReadBuffer<T> : IDynamicBuffer<T>, IReadBuffer<T> where T : unmanaged
    {
        public ref T this[int index] => ref this.buffer[index];

        public int ReadPosition
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

        public int Length => this.Capacity - this.ReadPosition;

        public int Capacity { get; internal set; }

        internal T[] buffer;
        private int cursor;

        public ArrayReadBuffer(T[] buffer) : this(buffer, buffer.Length) { }

        public ArrayReadBuffer(T[] buffer, int count)
        {
            this.buffer = buffer;
            this.Capacity = count;
            this.ReadPosition = 0;
        }

        public void Clear() => this.Clear(0, this.Capacity);

        public void Clear(int offset, int length)
        {
            Array.Clear(this.buffer, offset, length);
            this.cursor = 0;
        }

        public void Compact() 
        {
            int length = this.Length;

            if (this.Capacity > length)
            {
                Array.Resize(ref this.buffer, length);
            }
        }

        public void Reserve(int count)
        {
            int required = this.cursor + count;
            int size = this.buffer.Length;

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

        public Span<T> GetBufferSpan() => new Span<T>(this.buffer, 0, this.Capacity);

        public Span<T> GetSpan() => new Span<T>(this.buffer, this.cursor, this.Length);

        public T? Read()
        {
            if(this.Length > 0)
            {
                return this.buffer[this.cursor++];
            }
            else
            {
                return null;                    
            }
        }

        public int Read(Span<T> destination)
        {
            var remaining = this.Length;

            if (remaining > 0 && !destination.IsEmpty)
            {
                if(destination.Length > remaining)
                {
                    this.GetSpan().CopyTo(destination);
                    this.cursor += remaining;
                }
                else
                {
                    this.GetSpan().Slice(0, destination.Length).CopyTo(destination);
                    this.cursor += destination.Length;
                }
            }

            return remaining;
        }
    }
}