using System;

namespace JankWorks.Core
{
    public sealed class Signal : Subject<Action>
    {
        public Signal() : base() { }

        public void Notify()
        {
            lock (this)
            {
                this.PrepareInvocation();
                this.ClearSubscribers();
                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i]();
                }
                this.ClearInvocations();
            }
        }
    }

    public sealed class Signal<T1> : Subject<Action<T1>>
    {
        public Signal() : base() { }

        public void Notify(T1 arg)
        {
            lock (this)
            {
                this.PrepareInvocation();
                this.ClearSubscribers();
                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg);
                }
                this.ClearInvocations();
            }
        }

    }

    public sealed class Signal<T1, T2> : Subject<Action<T1, T2>>
    {
        public Signal() : base() { }

        public void Notify(T1 arg1, T2 arg2)
        {
            lock (this)
            {
                this.PrepareInvocation();
                this.ClearSubscribers();
                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg1, arg2);
                }
                this.ClearInvocations();
            }
        }
    }

    public sealed class Signal<T1, T2, T3> : Subject<Action<T1, T2, T3>>
    {
        public Signal() : base() { }

        public void Notify(T1 arg1, T2 arg2, T3 arg3)
        {
            lock (this)
            {
                this.PrepareInvocation();
                this.ClearSubscribers();
                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg1, arg2, arg3);
                }
                this.ClearInvocations();
            }
        }
    }

    public sealed class Signal<T1, T2, T3, T4> : Subject<Action<T1, T2, T3, T4>>
    {
        public Signal() : base() { }

        public void Notify(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            lock (this)
            {
                this.PrepareInvocation();
                this.ClearSubscribers();
                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg1, arg2, arg3, arg4);
                }
                this.ClearInvocations();
            }
        }
    }
}
