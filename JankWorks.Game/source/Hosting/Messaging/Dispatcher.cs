using JankWorks.Core;

namespace JankWorks.Game.Hosting.Messaging
{
    public abstract class Dispatcher : Disposable
    {
        protected Application Application { get; private set; }

        protected Dispatcher(Application application)
        {
            this.Application = application;
        }

        public abstract IMessageChannel<Message> GetMessageChannel<Message>(byte id, ChannelParameters parameters) where Message : unmanaged;

        public abstract void Synchronise();

        public abstract void ClearChannels();

        protected override void Dispose(bool disposing)
        {
            this.ClearChannels();
            base.Dispose(disposing);
        }
    }
}