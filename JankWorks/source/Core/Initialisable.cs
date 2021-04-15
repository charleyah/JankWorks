using System;

namespace JankWorks.Core
{
    public abstract class Initialisable : Disposable
    {
        public bool Initialised => !this.Disposed;

        protected Initialisable() : base() 
        {
            GC.SuppressFinalize(this);
            this.Disposed = true;
        }

        public void Initialise()
        {
            if(!this.Initialised)
            {
                this.Initialise(false);
            }
        }

        protected virtual void Initialise(bool reinitialising) 
        {
            GC.ReRegisterForFinalize(this);
        }

        public void Reinitialise()
        {
            if(this.Initialised)
            {
                this.Dispose();
                this.Initialise(true);
            }
        }
    }
}
