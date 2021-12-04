using System;
using System.Threading;

namespace JankWorks.Game.Threading
{
    sealed class UpdatableSynchronizationContext : QueueSynchronizationContext, IUpdatable
    {
        private readonly IUpdatable updatable;

        public UpdatableSynchronizationContext(IUpdatable updatable)
        {
            this.updatable = updatable;
        }

        public string GetName() => this.updatable.GetName();

        public void Update(TimeSpan delta)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);
                this.updatable.Update(delta);
                this.Yield();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
    }
}
