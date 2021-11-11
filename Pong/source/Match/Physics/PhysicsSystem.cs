using System;
using System.Numerics;

using JankWorks.Core;
using JankWorks.Graphics;
using JankWorks.Util;

using JankWorks.Game;
using JankWorks.Game.Hosting.Messaging;
using JankWorks.Game.Assets;

namespace Pong.Match.Physics
{
    sealed class PhysicsSystem : Disposable, ITickable
    {
        public event Action<int> OnCollision
        {
            add => this.OnCollisionHandler.Subscribe(value);
            remove => this.OnCollisionHandler.Unsubscribe(value);
        }

        public Event<int> OnCollisionHandler { get; init; }

        private IMessageChannel<PhysicsEvent> events;
        private ArrayWriteBuffer<PhysicsComponent> components;

        private Bounds area;
                
        public PhysicsSystem(IMessageChannel<PhysicsEvent> events, Bounds area)
        {
            this.OnCollisionHandler = new Event<int>();
            this.area = area;
            this.events = events;
            this.components = new ArrayWriteBuffer<PhysicsComponent>(8);
        }

        public int RequestComponent(PhysicsComponent inital)
        {
            var id = this.components.WritePosition;
            this.components.Write(inital);
            return id;
        }

        public void Broadcast()
        {
            var components = this.components.GetSpan();

            checked
            {
                var e = new PhysicsEvent()
                {
                    type = PhysicsEvent.Type.DataCount,
                    componentCount = (ushort)components.Length,                   
                };

                this.events.Send(e);
            }
            
            for (int i = 0; i < components.Length; i++)
            {
                this.BroadcastData(in components[i], i);
            }
        }

        public ref PhysicsComponent GetComponent(int id) => ref this.components[id];

        public void Tick(ulong tick, TimeSpan delta)
        {
            var components = this.components.GetSpan();

            for(int i = 0; i < components.Length; i++)
            {
                ref var com = ref components[i];

                if(com.velocity != Vector2.Zero)
                {
                    com.position += com.velocity;

                    this.CheckCollision(ref com, i);
                    this.CheckYBounds(ref com);
                }
                this.BroadcastData(in com, i);
            }
        }

        private void CheckCollision(ref PhysicsComponent com, int id)
        {
            var components = this.components.GetSpan();

            var source = com.GetBounds();

            for (int i = 0; i < components.Length; i++)
            {
                if(i == id)
                {
                    continue;
                }
                else
                {
                    if (components[i].GetBounds().Intersects(source))
                    {
                        com.velocity = new Vector2(-com.velocity.X, com.velocity.Y);
                        this.OnCollisionHandler.Notify(id);
                        break;
                    }
                }

            }
        }

        private void CheckYBounds(ref PhysicsComponent com)
        {
            var bounds = com.GetBounds();

            if(bounds.TopLeft.Y <= 0 || bounds.BottomRight.Y >= this.area.BottomRight.Y)
            {
                com.velocity = new Vector2(com.velocity.X, -com.velocity.Y);
            }
        }

        private void BroadcastData(in PhysicsComponent com, int id)
        {
            var physEvent = new PhysicsEvent()
            {
                type = PhysicsEvent.Type.Data,
                componentId = (ushort)id,
                data = com
            };

            this.events.Send(physEvent);
        }

        protected override void Dispose(bool finalising)
        {
            this.OnCollisionHandler.ClearSubscribers();             
            base.Dispose(finalising);
        }
    }
}