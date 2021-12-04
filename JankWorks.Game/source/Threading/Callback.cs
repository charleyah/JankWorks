using System.Threading;

namespace JankWorks.Game.Threading
{
    readonly struct Callback
    {
        public readonly SendOrPostCallback SendOrPostDelegate;

        public readonly object State;

        public Callback(SendOrPostCallback sendOrPostDelegate, object state)
        {
            this.SendOrPostDelegate = sendOrPostDelegate;
            this.State = state;
        }

        public void Invoke() => this.SendOrPostDelegate(this.State);
    }
}