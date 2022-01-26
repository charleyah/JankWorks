using System;

namespace JankWorks.Game.Diagnostics
{
    public sealed class HostMetrics
    {
        public MetricCounter[] TickMetricCounters { get; internal set; }

        public MetricCounter[] ParallelTickMetricCounters { get; internal set; }

        public int TicksPerSecond { get; set; }

        public double TickLag { get; set; }

        public HostMetrics()
        {
            this.TicksPerSecond = 0;
            this.TickMetricCounters = Array.Empty<MetricCounter>();
            this.ParallelTickMetricCounters = Array.Empty<MetricCounter>();
        }
    }
}
