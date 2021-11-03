using System;
using System.Numerics;
using System.Runtime;

using JankWorks.Graphics;
using JankWorks.Util;

using JankWorks.Game.Hosting;
using JankWorks.Game.Local;
using JankWorks.Game.Assets;

namespace JankWorks.Game.Diagnostics
{
    public sealed class PerformanceInfo : IUpdatable, IRenderable
    {
        public Vector2 Position { get; set; }

        public RGBA Colour { get; set; }

        public int MaxDisplayCounters { get; set; }

        public bool ShowMemoryInfo { get; set; }

        private Client client;
        private Host host;

        private ArrayWriteBuffer<char> textBuffer;
        private TextRenderer renderer;
        
        private Asset fontAsset;
        private uint fontSize;

        public PerformanceInfo(Client client, Host host, Asset fontAsset, uint fontSize)
        {
            this.client = client;
            this.host = host;
            this.fontAsset = fontAsset;
            this.fontSize = fontSize;
            this.Colour = JankWorks.Graphics.Colour.White;
            this.Position = new Vector2(4, 2);
            this.textBuffer = new ArrayWriteBuffer<char>();
        }

        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets)
        {
            using var fontSource = assets.GetAsset(this.fontAsset);
            using var font = Font.LoadFromStream(fontSource, this.fontAsset.Name.EndsWith(".ttf") ? FontFormat.TrueType : FontFormat.OpenType);
            font.FontSize = this.fontSize;
            this.renderer = device.CreateTextRenderer(new OrthoCamera(device.Viewport.Size), font);
        }

        public void Update(TimeSpan delta)
        {
            this.textBuffer.WritePosition = 0;

            this.UpdateHostMetrics();

            this.UpdateClientMetrics();

            this.UpdateMemoryMetrics();

            this.UpdateMetricCounters();
        }

        private void UpdateHostMetrics()
        {
            if (this.host != null && !(this.host is NullHost))
            {
                var hostmetrics = this.host.Metrics;

                this.textBuffer.Write("TPS: ");
                this.textBuffer.WriteInt(hostmetrics.TicksPerSecond);
                this.textBuffer.Write('\n');
            }            
        }

        private void UpdateClientMetrics()
        {
            var clientMetrics = this.client.Metrics;

            this.textBuffer.Write("UPS ");
            this.textBuffer.WriteInt(clientMetrics.UpdatesPerSecond);
            this.textBuffer.Write('\n');

            this.textBuffer.Write("FPS ");
            this.textBuffer.WriteInt(clientMetrics.FramesPerSecond);
            this.textBuffer.Write('\n');
        }

        private void UpdateMemoryMetrics()
        {
            if(this.ShowMemoryInfo)
            {
                var info = GC.GetGCMemoryInfo();

                this.textBuffer.Write("\nManaged Memory\n");
                this.textBuffer.Write("Heap ");
                this.textBuffer.WriteLong(info.HeapSizeBytes);
                this.textBuffer.Write("B\n");


                this.textBuffer.Write("Fragmented ");
                this.textBuffer.WriteLong(info.FragmentedBytes);
                this.textBuffer.Write("B\n");

                string latency = GCSettings.LatencyMode switch
                {
                    GCLatencyMode.Batch => "Batch",
                    GCLatencyMode.Interactive => "Interactive",
                    GCLatencyMode.LowLatency => "LowLatency",
                    GCLatencyMode.SustainedLowLatency => "SustainedLowLatency",
                    GCLatencyMode.NoGCRegion => "NoGCRegion",
                    _ => throw new NotImplementedException()
                };

                this.textBuffer.Write("GC Mode ");
                this.textBuffer.Write(latency);
                this.textBuffer.Write('\n');

                for (int gen = 0; gen <= GC.MaxGeneration; gen++)
                {
                    this.textBuffer.Write("Gen");
                    this.textBuffer.WriteInt(gen);
                    this.textBuffer.Write(' ');
                    this.textBuffer.WriteInt(GC.CollectionCount(gen));
                    this.textBuffer.Write('\n');
                }
            }
        }

        private void UpdateMetricCounters()
        {
            var maxcounters = this.MaxDisplayCounters;

            if(maxcounters > 0)
            {
                if (this.host != null)
                {
                    var hostMetrics = this.host.Metrics;

                    this.PrintCounters("Tickable", hostMetrics.TickMetricCounters, maxcounters);
                    this.PrintCounters("AsyncUpdatable", hostMetrics.AsyncTickMetricCounters, maxcounters);
                }

                var clientMetrics = this.client.Metrics;

                this.PrintCounters("Updatable", clientMetrics.UpdatableMetricCounters, maxcounters);
                this.PrintCounters("AsyncUpdatable", clientMetrics.AsyncUpdatableMetricCounters, maxcounters);
                this.PrintCounters("Renderable", clientMetrics.RenderableMetricCounters, maxcounters);
                this.PrintCounters("AsyncRenderable", clientMetrics.AsyncRenderableMetricCounters, maxcounters);
            }            
        }

        private void PrintCounters(string header, MetricCounter[] counters, int maxcounters)
        {
            var max = Math.Min(counters.Length, maxcounters);

            if(max > 0)
            {
                this.textBuffer.Write('\n');
                this.textBuffer.Write(header);
                this.textBuffer.Write('\n');

                for (int i = 0; i < max; i++)
                {
                    var counter = counters[i];

                    this.textBuffer.Write(counter.Name);
                    this.textBuffer.Write(' ');
                    this.textBuffer.WriteDouble(counter.Elpased.TotalMilliseconds, 2);
                    this.textBuffer.Write("ms\n");
                }
            }
        }

        public void Render(Surface surface, Frame frame)
        {
            this.renderer.BeginDraw();
            this.renderer.Draw(this.textBuffer.GetSpan(), this.Position, this.Colour);
            this.renderer.EndDraw(surface);
        }

        public void DisposeGraphicsResources(GraphicsDevice device)
        {
            this.renderer.Dispose();
        }
    }
}