using System;

namespace JankWorks.Util
{
    public sealed class ArrayReadWriteBuffer<T> : ArrayWriteBuffer<T>, IReadBuffer<T> where T : unmanaged
    {
        public int ReadPosition 
        {
            get => this.cursor; 
            set
            {
                if(value > this.WritePosition)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.cursor = value;
            }
        }

        public override int Length => this.WritePosition - this.ReadPosition;

        private int cursor;

        public ArrayReadWriteBuffer() : this(16) { }

        public ArrayReadWriteBuffer(int capacity) : base(capacity)
        {
            this.cursor = 0;
        }

        public T? Read()
        {
            if (this.Length > 0)
            {
                return this.buffer[this.cursor++];
            }
            else
            {
                return null;
            }
        }

        public override void Clear(int offset, int length)
        {
            base.Clear(offset, length);
            this.ReadPosition = 0;
        }

        public override void Compact()
        {
            this.CompactWithoutResize();
            base.Compact();
        }

        public void CompactWithoutResize()
        {
            if(this.Length == 0)
            {
                this.WritePosition = 0;
            }
            else if (this.ReadPosition > 0 && this.WritePosition > 0)
            {
                var span = this.GetSpan();
                span.CopyTo(this.GetBufferSpan());
                this.WritePosition -= this.ReadPosition;
            }

            this.ReadPosition = 0;
        }

        public int Read(Span<T> destination)
        {
            var read = 0;
            var length = this.Length;

            if(length > 0 && !destination.IsEmpty)
            {
                length = Math.Min(destination.Length, length);

                this.GetSpan().Slice(0, length).CopyTo(destination);
                this.cursor += length;
                read = length;
            }

            return read;
        }

        public override Span<T> GetSpan() => new Span<T>(this.buffer, this.ReadPosition, this.Length);

        public void Swap(ArrayReadWriteBuffer<T> other)
        {
            var otherbuffer = other.buffer;
            var ow = other.WritePosition;
            other.buffer = this.buffer;           
            other.ReadPosition = 0;
            other.WritePosition = this.WritePosition;

            this.buffer = otherbuffer;
            this.ReadPosition = 0;
            this.WritePosition = ow;
        }        
    }
}