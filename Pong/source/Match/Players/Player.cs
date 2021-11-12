using JankWorks.Game;
using JankWorks.Game.Hosting.Messaging;


namespace Pong.Match.Players
{
    abstract class Player : IDispatchable
    { 
        public byte Number { get; init; }

        protected IMessageChannel<PlayerEvent> Events { get; private set; }

        public Player(byte number)
        {
            this.Number = number;            
        }

        public void InitialiseChannels(Dispatcher dispatcher)
        {
            this.Events = dispatcher.GetMessageChannel<PlayerEvent>(PlayerEvent.Channel, new ChannelParameters()
            {
                Direction = IChannel.Direction.Up,
                MaxQueueSize = 16,
                Reliability = IChannel.Reliability.Reliable
            });
        }

        public void UpSynchronise() { }        

        public void DownSynchronise() { }

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