using System;
using System.Collections.Concurrent;
using System.Threading;

namespace JankWorks.Game.Threading
{
    abstract class QueueSynchronizationContext : SynchronizationContext
    {
        public bool Pending => this.operationsInProgress > 0 || !this.awaitingTasks.IsEmpty;

        private ConcurrentQueue<Callback> awaitingTasks;
        private uint operationsInProgress;

        protected QueueSynchronizationContext()
        {
            this.awaitingTasks = new ConcurrentQueue<Callback>();
            this.operationsInProgress = 0;
        }

        public override void OperationStarted() => Interlocked.Increment(ref this.operationsInProgress);

        public override void OperationCompleted() => Interlocked.Decrement(ref this.operationsInProgress);

        public override void Post(SendOrPostCallback d, object state) => this.awaitingTasks.Enqueue(new Callback(d, state));

        public virtual void Yield()
        {
            while (this.awaitingTasks.TryDequeue(out var callback))
            {
                callback.Invoke();
            }
        }

        public virtual void Join() => SpinWait.SpinUntil(this.Waiter);
        

        private bool Waiter()
        {
            this.Yield();
            return this.operationsInProgress == 0 && this.awaitingTasks.IsEmpty;
        }
    }
}