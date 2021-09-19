using System;

namespace JankWorks.Util
{
    public sealed class CircularArrayBuffer<T> : Buffer<T> where T : unmanaged
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
       
        public CircularArrayBuffer(int capacity)
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

        public override Span<T> GetBufferSpan() => new Span<T>(this.buffer);

        public override Span<T> GetSpan() => new Span<T>(this.buffer, 0, this.cursor);

        public override void Write(T value)
        {
            var cursor = this.cursor;
            this.buffer[cursor++] = value;
            this.cursor = (cursor >= this.Capacity) ? 0 : cursor;
        }

        public override void Write(ReadOnlySpan<T> values)
        {
            var cursor = this.cursor;
            var bufferSize = this.buffer.Length;

            for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
            {
                this.buffer[(cursor++) % bufferSize] = values[valueIndex];
            }
            this.cursor = cursor;
        }
    }
}