using System;
using System.Diagnostics;

using System.Threading;
using System.Threading.Tasks;

using JankWorks.Game.Local;
using JankWorks.Game.Diagnostics;
using JankWorks.Game.Platform;

using JankWorks.Game.Hosting.Messaging;
using JankWorks.Game.Hosting.Messaging.Memory;

namespace JankWorks.Game.Hosting
{
    public sealed class OfflineHost : ClientHost
    {
        private HostScene scene;
        private Client client;

        private ulong tick;

        private volatile HostState state;

        private NewHostSceneRequest newHostSceneRequest;

        private HostParameters parameters;

        private Counter tickCounter;
        private HostMetrics metrics;
        private Dispatcher dispatcher;
        private Thread runner;

        public override bool IsRemote => false;

        public override bool IsConnected => true;

        public override bool IsHostLoaded => this.state == HostState.RunningScene;

        public override Dispatcher Dispatcher => this.dispatcher;

        public override HostMetrics Metrics => this.metrics;

        public OfflineHost(Application application) : base(application, application.GetClientSettings())
        {
            var parms = application.HostParameters;
            this.parameters = parms;
            this.tickCounter = new Counter(TimeSpan.FromSeconds(1));
            this.metrics = new HostMetrics();
            this.state = HostState.Constructed;

            this.dispatcher = new MemoryDispatcher(application);
            this.runner = new Thread(new ThreadStart(this.Run));            
        }
      
        public override Task RunAsync(Client client)
        {
            this.client = client;
            var task = new Task(() => this.runner.Join());
            this.runner.Start();
            return task;
        }

        public override Task RunAsync(Client client, int scene, object initState = null)
        {
            this.client = client;
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                SceneName = scene,
                InitState = initState
            };
            this.state = HostState.LoadingScene;

            var task = new Task(() => this.runner.Join());
            this.runner.Start();
            return task;
        }

        public override void Start(Client client)
        {
            this.client = client;
            this.runner.Start();
        }

        public override void Start(Client client, int scene, object initState = null)
        {
            this.runner = new Thread(new ThreadStart(() => this.Run(client, scene, initState)));
            this.runner.Start();
        }

        public override void Run(Client client, int scene, object initState = null)
        {
            this.client = client;
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                SceneName = scene,
                InitState = initState
            };
            this.state = HostState.LoadingScene;
            this.Run();
        }

        private void Run()
        {
            var hostThread = Thread.CurrentThread;
            hostThread.Name = $"{this.Application.Name} Host Thread";
            Threads.HostThread = hostThread;

            var timer = new Stopwatch();
            var tickTime = TimeSpan.FromMilliseconds((1f / this.parameters.TickRate) * 1000);

            timer.Start();

            var lag = TimeSpan.Zero;
            this.tick = 0;

            var lastrun = timer.Elapsed;

            HostState state;

            while (true)
            {
                state = this.state;

                switch (state)
                {
                    case HostState.LoadingScene:
                        timer.Stop();
                        this.LoadScene();
                        continue;
                    case HostState.UnloadingScene:
                        timer.Stop();
                        this.scene.SharedDispose(this, this.client);
                        this.dispatcher.ClearChannels();
                        this.scene = null;
                        this.state = HostState.Constructed;
                        continue;

                    case HostState.WaitingOnClients:

                        if (this.client.State == ClientState.WaitingOnHost)
                        {
                            this.tick = 0;
                            this.state = HostState.RunningScene;
                            timer.Restart();
                        }
                        else
                        {
                            Thread.Yield();
                        }
                        continue;

                    case HostState.Constructed:
                        Thread.Yield();
                        continue;

                    case HostState.BeginShutdown:
                        base.Dispose(false);
                        this.state = HostState.Shutdown;
                        return;
                    case HostState.Shutdown:
                        return;
                }

                var now = timer.Elapsed;
                var since = now - lastrun;
                lag += since;

                if (lag >= tickTime)
                {

                    do
                    {
                        var delta = (lag > tickTime) ? tickTime : lag;

                        lock (this)
                        {
                            this.scene.Tick(this.tick++, delta);
                        }

                        lag -= tickTime;

                        this.metrics.TicksPerSecond = this.tickCounter.Frequency;
                        this.tickCounter.Count();
                    }
                    while (lag >= tickTime);
                }
                else
                {
                    var remaining = tickTime - lag;
                    if (remaining > TimeSpan.Zero)
                    {
                        PlatformApi.Instance.Sleep(remaining);
                    }
                }

                lastrun = now;
            }
        }

        public override void SynchroniseClientUpdate() 
        {
            lock (this)
            {
                this.dispatcher.Synchronise();
            }            
        }

        public override void UnloadScene()
        {
            if(this.state == HostState.RunningScene)
            {
                this.state = HostState.UnloadingScene;
            }
        }

        private void LoadScene()
        {
            this.scene = this.newHostSceneRequest.Scene;

            this.scene.SharedInitialise(this, this.client);            
            this.scene.SharedInitialised(this.newHostSceneRequest.InitState);

            this.metrics.TickMetricCounters = this.scene.TickMetricCounters;
            this.metrics.AsyncTickMetricCounters = this.scene.AsyncTickMetricCounters;

            this.newHostSceneRequest = default;            
            this.state = HostState.WaitingOnClients;
        }

        public override void LoadScene(HostScene scene, object initState = null)
        {
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                Scene = scene,
                InitState = initState
            };
            Thread.MemoryBarrier();
            this.state = HostState.LoadingScene;
        }

        protected override void Dispose(bool finalising)
        {
            this.dispatcher.Dispose();
            Threads.HostThread = null;

            this.state = HostState.Shutdown;
            base.Dispose(finalising);
        }

        public override Task DisposeAsync()
        {
            var task = new Task(() => this.runner.Join());

            if (this.state != HostState.BeginShutdown)
            {
                this.state = HostState.BeginShutdown;
            }
            return task;
        }
    }

    public enum HostState
    {
        Constructed = 0,

        BeginShutdown,

        Shutdown,

        RunningScene,

        LoadingScene,

        UnloadingScene,

        WaitingOnClients
    }
}