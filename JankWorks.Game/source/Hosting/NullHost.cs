using System;
using System.Threading.Tasks;

using JankWorks.Game.Diagnostics;

namespace JankWorks.Game.Hosting
{
    public sealed class NullHost : Host
    {
        public override bool IsRemote => true;

        public override bool IsConnected => true;

        public override bool IsHostLoaded => true;

        public NullHost(Application app) : base(app, app.GetClientSettings()) { }

        public override Task DisposeAsync() => Task.CompletedTask;

        public override MetricCounter[] GetMetrics() => Array.Empty<MetricCounter>();
    }
}