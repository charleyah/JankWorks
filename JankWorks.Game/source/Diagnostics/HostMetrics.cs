using System;

namespace JankWorks.Game.Diagnostics
{
    public sealed class HostMetrics
    {
        public MetricCounter[] TickMetricCounters { get; internal set; }

        public MetricCounter[] AsyncTickMetricCounters { get; internal set; }

        public int TicksPerSecond { get; set; }

        public HostMetrics()
        {
            this.TicksPerSecond = 0;
            this.TickMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncTickMetricCounters = Array.Empty<MetricCounter>();
        }
    }
}
