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
        public override string Name => this.tickable.GetName();

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
        public override string Name => this.updatable.GetName();

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
        public override string Name => this.renderable.GetName();

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


    internal abstract class ParallelMetricCounter : MetricCounter
    {
        protected Action<object> callbackHandler;
        private TimeSpan lastElpased;

        protected ParallelMetricCounter()
        {
            this.callbackHandler = (o) => this.End();
        }

        internal override void End() => this.lastElpased = DateTime.Now - this.Started;

        protected void UpdateElpased() => this.Elpased = this.lastElpased;        
    }


    internal sealed class ParallelTickableMetricCounter : ParallelMetricCounter, IParallelTickable
    {
        public override string Name => this.tickable.GetName();

        private IParallelTickable tickable;

        public ParallelTickableMetricCounter(IParallelTickable tickable)
        {
            this.tickable = tickable;
        }

        public void ForkTick(ulong tick, TimeSpan delta)
        {
            this.ForkTick(tick, delta, this.callbackHandler);
        }

        public void ForkTick(ulong tick, TimeSpan delta, Action<IParallelTickable> callback)
        {
            this.Start();
            this.tickable.ForkTick(tick, delta, callback);
        }

        public void JoinTick(ulong tick, TimeSpan delta)
        {
            this.tickable.JoinTick(tick, delta);
            this.UpdateElpased();
        }
    }

    internal sealed class ParallelUpdatableMetricCounter : ParallelMetricCounter, IParallelUpdatable
    {
        public override string Name => this.updatable.GetName();

        private IParallelUpdatable updatable;

        public ParallelUpdatableMetricCounter(IParallelUpdatable updatable)
        {
            this.updatable = updatable;
        }

        public void ForkUpdate(TimeSpan delta)
        {
            this.ForkUpdate(delta, this.callbackHandler);
        }

        public void ForkUpdate(TimeSpan delta, Action<IParallelUpdatable> callback)
        {
            this.Start();
            this.updatable.ForkUpdate(delta, callback);
        }

        public void JoinUpdate(TimeSpan delta)
        {
            this.updatable.JoinUpdate(delta);
            this.UpdateElpased();
        }
    }

    internal sealed class ParallelRenderableMetricCounter : ParallelMetricCounter, IParallelRenderable
    {
        public override string Name => this.renderable.GetName();

        private IParallelRenderable renderable;

        public ParallelRenderableMetricCounter(IParallelRenderable renderable)
        {
            this.renderable = renderable;
        }

        public void ForkRender(Surface surface, Frame frame)
        {
            this.ForkRender(surface, frame, this.callbackHandler);
        }

        public void ForkRender(Surface surface, Frame frame, Action<IParallelRenderable> callback)
        {
            this.Start();
            this.renderable.ForkRender(surface, frame, callback);
        }

        public void JoinRender(Surface surface, Frame frame)
        {
            this.renderable.JoinRender(surface, frame);
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