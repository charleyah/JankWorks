using System;
using System.Numerics;

using JankWorks.Game;
using JankWorks.Game.Assets;
using JankWorks.Game.Hosting.Messaging;
using JankWorks.Graphics;
using JankWorks.Util;

namespace Pong.Match.Physics
{
    sealed class PhysicsRenderer : IUpdatable, IRenderable
    {
        private Camera camera;
        private ShapeRenderer renderer;                
        private ArrayWriteBuffer<PhysicsComponent> components;

        private IMessageChannel<PhysicsEvent> events;

        public PhysicsRenderer(IMessageChannel<PhysicsEvent> events)
        {
            this.events = events;
            this.components = new ArrayWriteBuffer<PhysicsComponent>(8);
        }

        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets)
        {
            var viewportSize = device.Viewport.Size;
            this.camera = new OrthoCamera(viewportSize);
            this.renderer = device.CreateShapeRenderer(this.camera);
        }


        public void Update(TimeSpan delta)
        {
            PhysicsEvent msg;
            
            while(this.events.Pending)
            {
                msg = this.events.Receive().Value;
                this.ProcessEvent(in msg);
            }
        }

        private void ProcessEvent(in PhysicsEvent e)
        {
            switch(e.type)
            {
                case PhysicsEvent.Type.Data:
                    ref var com = ref this.components[e.componentId];
                    com = e.data;
                    break;

                case PhysicsEvent.Type.DataCount:
                    this.components.Reserve(e.componentCount);
                    this.components.WritePosition = e.componentCount;
                    break;
            }
        }

        public void Render(Surface surface, Frame frame)
        {
            var totalComponents = this.components.GetSpan();

            this.renderer.BeginDraw();
            for(int i = 0; i < totalComponents.Length; i++)
            {
                var com = totalComponents[i];
                var pos = Vector2.Lerp(com.position, com.position + com.velocity, (float)frame.Interpolation);
                this.renderer.DrawRectangle(com.size, pos, com.origin, 0f, com.colour);
            }
            this.renderer.EndDraw(surface);
        }

        public void DisposeGraphicsResources(GraphicsDevice device)
        {
            this.renderer.Dispose();
        }
    }
}