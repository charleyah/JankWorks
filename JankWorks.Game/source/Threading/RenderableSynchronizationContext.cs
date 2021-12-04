using System;
using System.Threading;

using JankWorks.Graphics;

using JankWorks.Game.Assets;

namespace JankWorks.Game.Threading
{
    sealed class RenderableSynchronizationContext : QueueSynchronizationContext, IRenderable
    {
        private readonly IRenderable renderable;

        public RenderableSynchronizationContext(IRenderable renderable)
        {
            this.renderable = renderable;
        }

        public string GetName() => this.renderable.GetName();

        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets) => this.renderable.InitialiseGraphicsResources(device, assets);

        public void DisposeGraphicsResources(GraphicsDevice device) => this.renderable.DisposeGraphicsResources(device);

        public void Render(Surface surface, Frame frame)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);
                this.renderable.Render(surface, frame);
                this.Yield();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
    }
}