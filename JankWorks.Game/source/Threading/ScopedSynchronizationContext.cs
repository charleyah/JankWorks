using System;
using System.Threading;

namespace JankWorks.Game.Threading
{
    sealed class ScopedSynchronizationContext : QueueSynchronizationContext, IDisposable
    {
        private readonly bool capture;

        public ScopedSynchronizationContext(bool capture)
        {
            this.capture = capture;
            if (capture)
            {
                SynchronizationContext.SetSynchronizationContext(this);
            }            
        }

        public override SynchronizationContext CreateCopy() => new ScopedSynchronizationContext(false);

        public void Dispose()
        {
            this.Join();

            if(this.capture)
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }           
        }    
    }
}