
namespace JankWorks.Util
{
    public abstract class DynamicBuffer<T> : Buffer<T> where T : unmanaged
    {
        public int Length => this.Position;

        public abstract void Reserve(int count);

        public abstract void Compact();
    }
}