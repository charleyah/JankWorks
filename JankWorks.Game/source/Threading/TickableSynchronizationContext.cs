using System;
using System.Threading;

namespace JankWorks.Game.Threading
{
    sealed class TickableSynchronizationContext : QueueSynchronizationContext, ITickable
    {
        private readonly ITickable tickable;
        private readonly IntervalBehaviour interval;

        public TickableSynchronizationContext(ITickable tickable)
        {
            this.tickable = tickable;
            this.interval = tickable.TickInterval;
        }

        public string GetName() => this.tickable.GetName();

        public void Tick(ulong tick, GameTime time)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);

                switch (this.interval)
                {
                    case IntervalBehaviour.Asynchronous:

                        if (this.Pending)
                        {
                            this.Yield();
                        }
                        else
                        {
                            this.tickable.Tick(tick, time);
                        }

                        break;

                    case IntervalBehaviour.Synchronous:
                        this.tickable.Tick(tick, time);
                        this.Join();
                        break;

                    case IntervalBehaviour.Overlapped:
                        this.Yield();
                        this.tickable.Tick(tick, time);
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