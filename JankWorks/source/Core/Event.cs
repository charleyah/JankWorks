using System;

namespace JankWorks.Core
{
    public sealed class Event : Subject<Action>
    {
        public Event() : base() { }

        public void Notify()
        {
            lock (this)
            {
                this.PrepareInvocation();

                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i]();
                }
                this.ClearInvocations();
            }
        }

        public void CancelNotify() => this.ClearInvocations();
    }

    public sealed class Event<T1> : Subject<Action<T1>>
    {
        public Event() : base() { }

        public void Notify(T1 arg)
        {
            lock (this)
            {
                this.PrepareInvocation();

                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg);
                }
                this.ClearInvocations();
            }
        }

        public void CancelNotify() => this.ClearInvocations();
    }

    public sealed class Event<T1, T2> : Subject<Action<T1, T2>>
    {
        public Event() : base() { }

        public void Notify(T1 arg1, T2 arg2)
        {
            lock (this)
            {
                this.PrepareInvocation();

                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg1, arg2);
                }
                this.ClearInvocations();
            }
        }

        public void CancelNotify() => this.ClearInvocations();
    }

    public sealed class Event<T1, T2, T3> : Subject<Action<T1, T2, T3>>
    {
        public Event() : base() { }

        public void Notify(T1 arg1, T2 arg2, T3 arg3)
        {
            lock (this)
            {
                this.PrepareInvocation();

                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg1, arg2, arg3);
                }
                this.ClearInvocations();
            }
        }

        public void CancelNotify() => this.ClearInvocations();
    }

    public sealed class Event<T1, T2, T3, T4> : Subject<Action<T1, T2, T3, T4>>
    {
        public Event() : base() { }

        public void Notify(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            lock (this)
            {
                this.PrepareInvocation();

                for (int i = 0; i < this.invocations.Count; i++)
                {
                    this.invocations[i](arg1, arg2, arg3, arg4);
                }
                this.ClearInvocations();
            }
        }

        public void CancelNotify() => this.ClearInvocations();
    }
}
