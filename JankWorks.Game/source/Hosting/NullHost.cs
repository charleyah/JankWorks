using System;
using System.Threading.Tasks;

using JankWorks.Game.Diagnostics;
using JankWorks.Game.Local;

namespace JankWorks.Game.Hosting
{
    public sealed class NullHost : ClientHost
    {
        public override bool IsRemote => true;

        public override bool IsConnected => true;

        public override bool IsHostLoaded => true;

        public NullHost(Application app) : base(app, app.GetClientSettings()) { }

        public override Task DisposeAsync() => Task.CompletedTask;

        public override MetricCounter[] GetMetrics() => Array.Empty<MetricCounter>();

        public override void UnloadScene() { }

        public override void LoadScene(HostScene scene, object initState) { }

        public override void SynchroniseClientUpdate() { }

        public override void Start(Client client) { }

        public override Task RunAsync(Client client) => Task.CompletedTask;

        public override Task RunAsync(Client client, int scene, object initState = null) => Task.CompletedTask;        

        public override void Run(Client client, int scene, object initState = null) { }        

        public override void Start(Client client, int scene, object initState = null) { }        
    }
}