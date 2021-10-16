using System;
using JankWorks.Game.Assets;
using JankWorks.Graphics;

namespace JankWorks.Game.Diagnostics
{
    public abstract class MetricCounter : IComparable<MetricCounter>
    {
        public abstract string Name { get; }

        public TimeSpan Elpased { get; protected set; }

        protected DateTime Started { get; private set; }

        internal void Start() => this.Started = DateTime.Now;

        internal virtual void End() => this.Elpased = DateTime.Now - this.Started;

        int IComparable<MetricCounter>.CompareTo(MetricCounter other) => this.Elpased.CompareTo(other.Elpased);
    }

    internal sealed class TickableMetricCounter : MetricCounter, ITickable
    {
        public override string Name => this.tickable.GetType().Name;

        private ITickable tickable;

        public TickableMetricCounter(ITickable tickable)
        {
            this.tickable = tickable;
        }

        void ITickable.Tick(ulong tick, TimeSpan delta)
        {
            this.Start();
            tickable.Tick(tick, delta);
            this.End();
        }
    }

    internal sealed class UpdatableMetricCounter : MetricCounter, IUpdatable
    {
        public override string Name => this.updatable.GetType().Name;

        private IUpdatable updatable;

        public UpdatableMetricCounter(IUpdatable updatable)
        {
            this.updatable = updatable;
        }

        void IUpdatable.Update(TimeSpan delta)
        {
            this.Start();
            this.updatable.Update(delta);
            this.End();
        }
    }

    internal sealed class RenderableMetricCounter : MetricCounter, IRenderable
    {
        public override string Name => this.renderable.GetType().Name;

        private IRenderable renderable;

        public RenderableMetricCounter(IRenderable renderable)
        {
            this.renderable = renderable;
        }

        void IRenderable.Render(Surface surface, Frame frame)
        {
            this.Start();
            this.renderable.Render(surface, frame);
            this.End();
        }

        void IGraphicsResource.InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets)
        {
            this.renderable.InitialiseGraphicsResources(device, assets);
        }

        void IGraphicsResource.DisposeGraphicsResources(GraphicsDevice device)
        {
            this.renderable.DisposeGraphicsResources(device);
        }
    }


    internal abstract class AsyncMetricCounter : MetricCounter
    {
        protected Action<object> callbackHandler;
        private TimeSpan lastElpased;

        protected AsyncMetricCounter()
        {
            this.callbackHandler = (o) => this.End();
        }

        internal override void End() => this.lastElpased = DateTime.Now - this.Started;

        protected void UpdateElpased() => this.Elpased = this.lastElpased;        
    }


    internal sealed class AsyncTickableMetricCounter : AsyncMetricCounter, IAsyncTickable
    {
        public override string Name => this.tickable.GetType().Name;

        private IAsyncTickable tickable;

        public AsyncTickableMetricCounter(IAsyncTickable tickable)
        {
            this.tickable = tickable;
        }

        public void BeginTick(ulong tick, TimeSpan delta)
        {
            this.BeginTick(tick, delta, this.callbackHandler);
        }

        public void BeginTick(ulong tick, TimeSpan delta, Action<IAsyncTickable> callback)
        {
            this.Start();
            this.tickable.BeginTick(tick, delta, callback);
        }

        public void EndTick(ulong tick, TimeSpan delta)
        {
            this.tickable.EndTick(tick, delta);
            this.UpdateElpased();
        }
    }

    internal sealed class AsyncUpdatableMetricCounter : AsyncMetricCounter, IAsyncUpdatable
    {
        public override string Name => this.updatable.GetType().Name;

        private IAsyncUpdatable updatable;

        public AsyncUpdatableMetricCounter(IAsyncUpdatable updatable)
        {
            this.updatable = updatable;
        }

        public void BeginUpdate(TimeSpan delta)
        {
            this.BeginUpdate(delta, this.callbackHandler);
        }

        public void BeginUpdate(TimeSpan delta, Action<IAsyncUpdatable> callback)
        {
            this.Start();
            this.updatable.BeginUpdate(delta, callback);
        }

        public void EndUpdate(TimeSpan delta)
        {
            this.updatable.EndUpdate(delta);
            this.UpdateElpased();
        }
    }

    internal sealed class AsyncRenderableMetricCounter : AsyncMetricCounter, IAsyncRenderable
    {
        public override string Name => this.renderable.GetType().Name;

        private IAsyncRenderable renderable;

        public AsyncRenderableMetricCounter(IAsyncRenderable renderable)
        {
            this.renderable = renderable;
        }

        public void BeginRender(Surface surface, Frame frame)
        {
            this.BeginRender(surface, frame, this.callbackHandler);
        }

        public void BeginRender(Surface surface, Frame frame, Action<IAsyncRenderable> callback)
        {
            this.Start();
            this.renderable.BeginRender(surface, frame, callback);
        }

        public void EndRender(Surface surface, Frame frame)
        {
            this.renderable.EndRender(surface, frame);
            this.UpdateElpased();
        }

        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets)
        {
            this.renderable.InitialiseGraphicsResources(device, assets);
        }

        public void DisposeGraphicsResources(GraphicsDevice device)
        {
            this.renderable.DisposeGraphicsResources(device);
        }
    }
}