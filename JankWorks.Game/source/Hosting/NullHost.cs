using System;
using System.Threading.Tasks;

using JankWorks.Game.Hosting.Messaging;
using JankWorks.Game.Diagnostics;
using JankWorks.Game.Local;

namespace JankWorks.Game.Hosting
{
    public sealed class NullHost : ClientHost
    {
        public override HostState State => throw new NotSupportedException();

        public override bool IsRemote => true;        

        public override bool IsConnected => true;

        public override bool IsHostLoaded => true;

        public override Dispatcher Dispatcher => throw new NotSupportedException();

        public override HostMetrics Metrics => this.emptyMetrics;
        

        private readonly HostMetrics emptyMetrics;

        public NullHost(Application app) : base(app, app.GetClientSettings()) 
        {
            this.emptyMetrics = new HostMetrics();
        }

        public override Task DisposeAsync() => Task.CompletedTask;

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