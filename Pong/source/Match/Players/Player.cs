using System;
using JankWorks.Game.Hosting.Messaging;

namespace Pong.Match.Players
{
    abstract class Player
    { 
        public byte Number { get; init; }

        protected IMessageChannel<PlayerEvent> Events { get; init; }

        public Player(byte number, IMessageChannel<PlayerEvent> events)
        {
            this.Number = number;
            this.Events = events;
        }

        protected void SubmitMovement(PlayerMovement movement)
        {
            var e = new PlayerEvent()
            {
                Movement = movement,
                PlayerNumber = this.Number
            };

            this.Events.Send(e);
        }
    }
}