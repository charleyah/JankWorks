using System;
using System.Threading.Tasks;

using JankWorks.Core;

using JankWorks.Game.Diagnostics;
using JankWorks.Game.Configuration;
using JankWorks.Game.Hosting.Messaging;

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

        public bool IsLocal => !this.IsRemote;

        public abstract bool IsRemote { get; }

        public abstract bool IsConnected { get; }

        public abstract bool IsHostLoaded { get; }

        public abstract Dispatcher Dispatcher { get; }

        public abstract HostMetrics Metrics { get; }

        public Settings Settings { get; init; }

        protected Application Application { get; init; }

        protected Host(Application application, Settings settings)
        {
            this.Application = application;
            this.Settings = settings;
        }

        public abstract Task DisposeAsync();
    }        
}