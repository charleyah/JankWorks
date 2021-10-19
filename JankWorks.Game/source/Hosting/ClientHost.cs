using System.Threading.Tasks;

using JankWorks.Game.Configuration;
using JankWorks.Game.Local;

namespace JankWorks.Game.Hosting
{
    public abstract class ClientHost : Host
    {
        protected ClientHost(Application application, Settings settings) : base(application, settings) { }

        public abstract void UnloadScene();

        public abstract void LoadScene(HostScene scene, object initState);

        public abstract void SynchroniseClientUpdate();

        public abstract void Start(Client client);

        public abstract Task RunAsync(Client client);

        public abstract Task RunAsync(Client client, int scene, object initState = null);

        public abstract void Run(Client client, int scene, object initState = null);

        public abstract void Start(Client client, int scene, object initState = null);
    }
}
