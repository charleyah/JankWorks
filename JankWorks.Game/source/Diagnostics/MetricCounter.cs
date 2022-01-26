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

        void ITickable.Tick(ulong tick, GameTime time)
        {
            this.Start();
            tickable.Tick(tick, time);
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

        void IUpdatable.Update(GameTime time)
        {
            this.Start();
            this.updatable.Update(time);
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

        void IRenderable.Render(Surface surface, GameTime time)
        {
            this.Start();
            this.renderable.Render(surface, time);
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

        public void ForkTick(ulong tick, GameTime time)
        {
            this.ForkTick(tick, time, this.callbackHandler);
        }

        public void ForkTick(ulong tick, GameTime time, Action<IParallelTickable> callback)
        {
            this.Start();
            this.tickable.ForkTick(tick, time, callback);
        }

        public void JoinTick(ulong tick, GameTime time)
        {
            this.tickable.JoinTick(tick, time);
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

        public void ForkUpdate(GameTime time)
        {
            this.ForkUpdate(time, this.callbackHandler);
        }

        public void ForkUpdate(GameTime time, Action<IParallelUpdatable> callback)
        {
            this.Start();
            this.updatable.ForkUpdate(time, callback);
        }

        public void JoinUpdate(GameTime time)
        {
            this.updatable.JoinUpdate(time);
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

        public void ForkRender(Surface surface, GameTime time)
        {
            this.ForkRender(surface, time, this.callbackHandler);
        }

        public void ForkRender(Surface surface, GameTime time, Action<IParallelRenderable> callback)
        {
            this.Start();
            this.renderable.ForkRender(surface, time, callback);
        }

        public void JoinRender(Surface surface, GameTime time)
        {
            this.renderable.JoinRender(surface, time);
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