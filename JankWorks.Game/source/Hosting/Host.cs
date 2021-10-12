using System;
using System.Threading.Tasks;

using JankWorks.Core;
using JankWorks.Game.Configuration;
using JankWorks.Game.Assets;

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

        protected AssetManager AssetManager { get; init; }

        protected Host(Application application, Settings settings)
        {
            this.Application = application;
            this.Settings = settings;
            this.AssetManager = application.RegisterAssetManager();
        }
       
        public abstract void Connect();

        public abstract void NotifyClientLoaded();

        public abstract Task DisposeAsync();
    }

    public abstract class LocalHost : Host
    {
        protected LocalHost(Application application) : base(application, application.GetHostSettings()) { }

        public abstract void LoadScene(HostScene scene, object initState = null);

        public abstract Task RunAsync();

        public abstract Task RunAsync(int scene, object initState = null);

        public abstract void Run(int scene, object initState = null);              
    }

    public abstract class RemoteHost : Host
    {
        protected RemoteHost(Application application) : base(application, application.GetClientSettings()) { }

        public abstract void LoadScene(int scene, object initState = null);
    }     
}