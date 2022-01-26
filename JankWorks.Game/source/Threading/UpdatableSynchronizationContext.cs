using System;
using System.Threading;

namespace JankWorks.Game.Threading
{
    sealed class UpdatableSynchronizationContext : QueueSynchronizationContext, IUpdatable
    {
        private readonly IUpdatable updatable;
        private readonly IntervalBehaviour interval;

        public UpdatableSynchronizationContext(IUpdatable updatable)
        {
            this.updatable = updatable;
            this.interval = updatable.UpdateInterval;
        }

        public string GetName() => this.updatable.GetName();

        public void Update(GameTime time)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);

                switch(this.interval)
                {
                    case IntervalBehaviour.Asynchronous:

                        if(this.Pending)
                        {
                            this.Yield();
                        }
                        else
                        {
                            this.updatable.Update(time);
                        }

                        break;

                    case IntervalBehaviour.Synchronous:
                        this.updatable.Update(time);
                        this.Join();
                        break;

                    case IntervalBehaviour.Overlapped:
                        this.Yield();
                        this.updatable.Update(time);                        
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
    }
}
