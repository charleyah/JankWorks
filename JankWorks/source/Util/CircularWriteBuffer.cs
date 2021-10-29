using System;

namespace JankWorks.Util
{
    public sealed class CircularWriteBuffer<T> : IBuffer<T>, IWriteBuffer<T> where T : unmanaged
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

        public int Capacity => this.buffer.Length;

        public int Length => this.WritePosition;

        private T[] buffer;
        private int cursor;
       
        public CircularWriteBuffer(int capacity)
        {
            this.buffer = new T[capacity];
            this.cursor = 0;
        }

        public void Clear() => this.Clear(0, this.buffer.Length);

        public void Clear(int offset, int length)
        {
            Array.Clear(this.buffer, offset, length);
            this.cursor = 0;
        }

        public Span<T> GetBufferSpan() => new Span<T>(this.buffer);

        public Span<T> GetSpan() => this.GetWritten();

        public Span<T> GetWritten() => new Span<T>(this.buffer, 0, this.cursor);

        public void Write(T value)
        {
            var cursor = this.cursor;
            this.buffer[cursor++] = value;
            this.cursor = (cursor >= this.Capacity) ? 0 : cursor;
        }

        public void Write(ReadOnlySpan<T> values)
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