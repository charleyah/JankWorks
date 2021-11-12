using System;
using System.Numerics;

using JankWorks.Graphics;
using JankWorks.Game;

using Pong.Match.Physics;

namespace Pong.Match
{
    sealed class Ball
    {
        public int PhysicsComponentId { get; init; }

        private Bounds area;
        private PhysicsSystem physics;

        private const float initialVelocityX = 8f;

        private float maxVelocityX;
        private Random rng;

        public Ball(PhysicsSystem physics, Bounds area, float maxVelocity)
        {
            this.maxVelocityX = maxVelocity;
            this.area = area;
            this.rng = new Random();

            var com = new PhysicsComponent()
            {
                colour = Pong.UI.Colours.Forground,
                size = new Vector2(25, 25),
                position = area.Size / 2,
                origin = new Vector2(0.5f)
            };
            
            this.PhysicsComponentId = physics.RequestComponent(com);
            this.physics = physics;
            this.physics.OnCollision += (id) => 
            { 
                if(id == this.PhysicsComponentId)
                {
                    ref var com = ref this.physics.GetComponent(this.PhysicsComponentId);

                    if(com.velocity.X > 0)
                    {
                        com.velocity.X = MathF.Min(com.velocity.X + 2f, maxVelocityX);

                    }
                    else if(com.velocity.X < 0)
                    {
                        com.velocity.X = MathF.Max(com.velocity.X - 2f, -maxVelocityX);
                    }
                }
            };
        }

        public void StartMoving()
        {
            ref var com = ref this.physics.GetComponent(this.PhysicsComponentId);

            var velocityY = this.rng.Next(1, 6);

            if(this.rng.Next(0, 100) > 50)
            {
                velocityY = -velocityY;
            }
                    

            com.velocity = new Vector2(-initialVelocityX, velocityY);
        }
    }
}
