using System;


using JankWorks.Core;
using JankWorks.Game.Configuration;
using JankWorks.Game.Local;

namespace JankWorks.Game.Hosting
{   
    public abstract class Host : Disposable
    {
        protected struct NewHostSceneRequest
        {
            public string? SceneName;
            public HostScene? Scene;
            public object? InitState;
        }

        public abstract bool IsRemote { get; }

        public abstract bool IsConnected { get; }

        public abstract bool IsHostLoaded { get; }

        public float TicksPerSecond { get; protected set; }

        public TimeSpan Lag { get; protected set; }

        public Settings Settings { get; init; }

        protected Application Application { get; init; }

        protected Host(Application application)
        {
            this.Application = application;
            this.Settings = application.Settings;
        }
       
        public abstract void Connect();
        public abstract void NotifyClientLoaded();
    }

    public abstract class LocalHost : Host
    {
        protected LocalHost(Application application) : base(application) { }
        public abstract void LoadScene(HostScene scene, object? initState = null);

        public abstract void Start(string scene, object? initState = null);

        public abstract void Run(string scene, object? initState = null);              
    }

    public abstract class RemoteHost : Host
    {
        protected RemoteHost(Application application) : base(application) { }

        public abstract void LoadScene(string scene, object? initState = null);
    }     
}
