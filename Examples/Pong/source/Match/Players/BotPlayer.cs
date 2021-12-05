using System;

using JankWorks.Game;
using JankWorks.Graphics;

using Pong.Match.Physics;

namespace Pong.Match.Players
{
    sealed class BotPlayer : ITickable
    {
        public byte PlayerNumber { get; init; }

        private Ball ball;
        private PhysicsSystem physics;
        private PlayerSystem players;

        private float targetY;
        private float resetY;

        public BotPlayer(byte number, PhysicsSystem physics, PlayerSystem players, Ball ball, Bounds bounds)
        {
            this.PlayerNumber = number;
            this.physics = physics;
            this.ball = ball;
            this.players = players;
            this.resetY = bounds.Size.Y / 2;
            this.targetY = this.resetY;
        }

        public void Tick(ulong tick, TimeSpan delta)
        {
            ref var ballPhysics = ref this.physics.GetComponent(this.ball.PhysicsComponentId);
            ref var botPhysics = ref this.players.GetPlayerPhysicsComponent(this.PlayerNumber);

            if(this.IsBallInOurCourt(in ballPhysics, in botPhysics, out var distance))
            {
                var speedX = distance / ballPhysics.velocity.X;

                if(speedX < 0)
                {
                    speedX = -speedX;
                }

                this.targetY = ballPhysics.position.Y + (ballPhysics.velocity.Y * speedX);
            }

            this.Move(ref botPhysics);
        }

        private void Move(ref PhysicsComponent botPhysics)
        {
            var diffY = botPhysics.position.Y - targetY;

            if(diffY < 0)
            {
                diffY = -diffY;
            }

            if (diffY <= this.players.PlayerVelocity)
            {
                botPhysics.velocity.Y = 0;
            }
            else if (botPhysics.position.Y < this.targetY)
            {
                botPhysics.velocity.Y = this.players.PlayerVelocity;
            }
            else if (botPhysics.position.Y > this.targetY)
            {
                botPhysics.velocity.Y = -this.players.PlayerVelocity;
            }            
        }

        private bool IsBallInOurCourt(in PhysicsComponent ball, in PhysicsComponent bot, out float distance)
        {
            if(this.PlayerNumber == 0)
            {
                distance = bot.position.X - ball.position.X;
                return ball.velocity.X < 0;
            }
            else
            {                
                distance = ball.position.X - bot.position.X;
                return ball.velocity.X > 0;
            }
        }
    }
}