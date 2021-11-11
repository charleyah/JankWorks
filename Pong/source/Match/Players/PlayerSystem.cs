using System;
using System.Numerics;

using JankWorks.Game;
using JankWorks.Game.Hosting.Messaging;

using Pong.Match.Physics;

namespace Pong.Match.Players
{
    class PlayerSystem : ITickable
    {
        public Vector2 PlayerSize { get; set; }

        public float PlayerVelocity { get; set; }
       
        private IMessageChannel<PlayerEvent> events;
        private PhysicsSystem physics;

        private PlayerEntity[] players;
       
        public PlayerSystem(IMessageChannel<PlayerEvent> events, PhysicsSystem physics, byte playerCount)
        {
            this.events = events;
            this.physics = physics;
            this.players = new PlayerEntity[playerCount];
            this.PlayerVelocity = 8f;
            this.PlayerSize = new Vector2(50, 100);
        }

        public void RegisterPlayer(byte number)
        {
            ref var player = ref this.players[number];

            if(!player.isActive)
            {
                var initial = new PhysicsComponent()
                {
                    colour = Pong.UI.Colours.Forground,
                    position = Vector2.Zero,
                    origin = Vector2.Zero,
                    size = new Vector2(50, 100),
                    velocity = Vector2.Zero
                };

                player = new PlayerEntity()
                {
                    physId = this.physics.RequestComponent(initial),
                    isActive = true                    
                };
            }
        }

        public void SetPlayerPosition(byte number, Vector2 position, Vector2 origin)
        {
            var player = this.players[number];
            ref var phys = ref this.physics.GetComponent(player.physId);
            phys.position = position;
            phys.origin = origin;
        }

        public ref PhysicsComponent GetPlayerPhysicsComponent(byte playerNumber)
        {
            var player = this.players[playerNumber];
            return ref this.physics.GetComponent(player.physId);
        }

        public void Tick(ulong tick, TimeSpan delta)
        {
            PlayerEvent e;

            while (this.events.Pending)
            {
                e = this.events.Receive().Value;
                this.ProcessEvent(in e);
            }
        }

        private void ProcessEvent(in PlayerEvent e)
        {
            if(e.PlayerNumber < this.players.Length)
            {
                var player = this.players[e.PlayerNumber];

                if(player.isActive)
                {
                    ref var phys = ref this.physics.GetComponent(player.physId);

                    switch (e.Movement)
                    {
                        case PlayerMovement.Still:
                            phys.velocity = Vector2.Zero;
                            break;

                        case PlayerMovement.Up:
                            phys.velocity = new Vector2(0, -this.PlayerVelocity);
                            break;

                        case PlayerMovement.Down:
                            phys.velocity = new Vector2(0, this.PlayerVelocity);
                            break;
                    }
                }                
            }
        }

        private struct PlayerEntity
        {
            public bool isActive;
            public int physId;
        }
    }
}