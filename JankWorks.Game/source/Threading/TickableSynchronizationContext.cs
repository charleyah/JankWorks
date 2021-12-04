using System;
using System.Threading;

namespace JankWorks.Game.Threading
{
    sealed class TickableSynchronizationContext : QueueSynchronizationContext, ITickable
    {
        private readonly ITickable tickable;

        public TickableSynchronizationContext(ITickable tickable)
        {
            this.tickable = tickable;
        }

        public string GetName() => this.tickable.GetName();

        public void Tick(ulong tick, TimeSpan delta)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);
                this.tickable.Tick(tick, delta);
                this.Yield();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
    }
}