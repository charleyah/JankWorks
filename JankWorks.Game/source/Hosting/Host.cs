using System;
using System.Threading.Tasks;

using JankWorks.Core;

using JankWorks.Game.Local;
using JankWorks.Game.Diagnostics;
using JankWorks.Game.Configuration;

namespace JankWorks.Game.Hosting
{   
    public abstract class Host : Disposable
    {
        protected struct NewHostSceneRequest
        {
            public int SceneName;
            public HostScene Scene;
            public object InitState;
        }

        public abstract bool IsRemote { get; }

        public abstract bool IsConnected { get; }

        public abstract bool IsHostLoaded { get; }

        public float TicksPerSecond { get; protected set; }

        public TimeSpan Lag { get; protected set; }

        public Settings Settings { get; init; }

        protected Application Application { get; init; }

        protected Host(Application application, Settings settings)
        {
            this.Application = application;
            this.Settings = settings;
        }

        public abstract MetricCounter[] GetMetrics();

        public abstract void Connect();

        public abstract void NotifyClientLoaded();

        public abstract Task DisposeAsync();
    }

    public abstract class ClientHost : Host
    {
        protected ClientHost(Application application, Settings settings) : base(application, settings) { }

        public abstract void UnloadScene();

        public abstract void LoadScene(HostScene scene, object initState);

        public abstract Task RunAsync(Client client);

        public abstract Task RunAsync(Client client, int scene, object initState = null);

        public abstract void Run(Client client, int scene, object initState = null);
    }

    public abstract class LocalHost : ClientHost
    {
        protected LocalHost(Application application) : base(application, application.GetHostSettings()) { }     
    }

    public abstract class RemoteHost : ClientHost
    {
        protected RemoteHost(Application application) : base(application, application.GetClientSettings()) { }
    }     
}