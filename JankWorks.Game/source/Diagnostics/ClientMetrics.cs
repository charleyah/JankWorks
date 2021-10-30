using System;

namespace JankWorks.Game.Diagnostics
{
    public sealed class ClientMetrics
    {
        public MetricCounter[] Counters { get; set; }

        public int FramesPerSecond { get; set; }

        public int UpdatesPerSecond { get; set; }

        public ClientMetrics()
        {
            this.FramesPerSecond = 0;
            this.UpdatesPerSecond = 0;
            this.Counters = Array.Empty<MetricCounter>();
        }
    }
}