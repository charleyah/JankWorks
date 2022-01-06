using System;

namespace JankWorks.Core
{
    public abstract class Disposable : IDisposable
    {
        public bool Disposed { get; protected internal set; }        

        public void Dispose()
        {
            if (!this.Disposed) 
            { 
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this.Disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing) { }

        ~Disposable()
        {
            if(!this.Disposed)
            {
                this.Dispose(false);
            }
        }
    }
}
