using System;
using System.Threading;

namespace JankWorks.Game.Threading
{
    sealed class UpdatableSynchronizationContext : QueueSynchronizationContext, IUpdatable
    {
        private readonly IUpdatable updatable;
        private readonly IntervalBehavior interval;

        public UpdatableSynchronizationContext(IUpdatable updatable)
        {
            this.updatable = updatable;
            this.interval = updatable.UpdateInterval;
        }

        public string GetName() => this.updatable.GetName();

        public void Update(TimeSpan delta)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);

                switch(this.interval)
                {
                    case IntervalBehavior.Asynchronous:

                        if(this.Pending)
                        {
                            this.Yield();
                        }
                        else
                        {
                            this.updatable.Update(delta);
                        }

                        break;

                    case IntervalBehavior.Synchronous:
                        this.updatable.Update(delta);
                        this.Join();
                        break;

                    case IntervalBehavior.Overlapped:
                        this.Yield();
                        this.updatable.Update(delta);                        
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
    }
}
