using System;

namespace JankWorks.Game.Diagnostics
{
    public sealed class ClientMetrics
    {
        public MetricCounter[] UpdatableMetricCounters { get; internal set; }

        public MetricCounter[] ParallelUpdatableMetricCounters { get; internal set; }

        public MetricCounter[] RenderableMetricCounters { get; internal set; }

        public MetricCounter[] ParallelRenderableMetricCounters { get; internal set; }

        public int FramesPerSecond { get; set; }

        public int UpdatesPerSecond { get; set; }

        public ClientMetrics()
        {
            this.FramesPerSecond = 0;
            this.UpdatesPerSecond = 0;

            this.UpdatableMetricCounters = Array.Empty<MetricCounter>();
            this.ParallelUpdatableMetricCounters = Array.Empty<MetricCounter>();
            this.RenderableMetricCounters = Array.Empty<MetricCounter>();
            this.ParallelRenderableMetricCounters = Array.Empty<MetricCounter>();
        }
    }
}