using System;

namespace JankWorks.Game.Diagnostics
{
    public sealed class HostMetrics
    {
        public MetricCounter[] Counters { get; set; }

        public int TicksPerSecond { get; set; }

        public HostMetrics()
        {
            this.TicksPerSecond = 0;
            this.Counters = Array.Empty<MetricCounter>();
        }
    }
}
