using System;

namespace JankWorks.Core
{
    public abstract class Disposable : IDisposable
    {
        public bool Disposed { get; protected internal set; }        

        public void Dispose()
        {
            if (!this.Disposed) { this.Dispose(false); }
        }

        protected virtual void Dispose(bool finalising)
        {
            if(!finalising)
            {
                GC.SuppressFinalize(this);
                this.Disposed = true;
            }
        }

        ~Disposable()
        {
            if(!this.Disposed)
            {
                this.Dispose(true);
            }
        }
    }
}
