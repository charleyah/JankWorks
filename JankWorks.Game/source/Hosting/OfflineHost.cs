using System;
using System.Diagnostics;

using System.Threading;
using System.Threading.Tasks;

using JankWorks.Game.Platform;

namespace JankWorks.Game.Hosting
{
    public sealed class OfflineHost : LocalHost
    {
        private struct LocalClientState
        {
            public bool Connected;
            public bool Loaded;
        }

        private HostScene scene;

        private ulong tick;

        private volatile HostState state;

        private LocalClientState localClient;

        private NewHostSceneRequest newHostSceneRequest;

        private HostParameters parameters;

        private Task runner;

        public OfflineHost(Application application) : base(application)
        {
            var parms = application.HostParameters;
            this.parameters = parms;

            this.runner = new Task(this.Run, TaskCreationOptions.LongRunning);            
        }

        public override bool IsRemote => false;

        public override bool IsConnected => this.localClient.Connected;

        public override bool IsHostLoaded => this.state == HostState.RunningScene;

        public override void Connect() => this.localClient.Connected = true;


        public override void NotifyClientLoaded() => this.localClient.Loaded = true;


        private void Run()
        {
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

                switch(state)
                {
                    case HostState.LoadingScene:
                        timer.Stop();
                        this.LoadScene();
                        continue;


                    case HostState.WaitingOnClients:

                        if(this.localClient.Loaded)
                        {
                            this.scene.Initialised();
                            this.tick = 0;
                            this.state = HostState.RunningScene;
                            timer.Start();
                        }
                        continue;

                    case HostState.Constructed:
                        Thread.Yield();
                        continue;

                    case HostState.BeginShutdown:
                        this.UnloadScene();
                        this.state = HostState.Shutdown;
                        base.Dispose(false);
                        return;
                    case HostState.Shutdown:                       
                        return;
                }

                var now = timer.Elapsed;
                var since = now - lastrun;
                lag += since;
                this.Lag = lag;

                if(lag >= tickTime)
                {
                    do
                    {
                        var delta = (lag > tickTime) ? tickTime : lag;
                        this.scene.Tick(this.tick++, delta);
                        lag -= tickTime;
                        this.TicksPerSecond = Convert.ToSingle(Math.Round(1000 / delta.TotalMilliseconds, 0));
                    }
                    while (lag >= tickTime);
                }
                else
                {
                    var remaining = tickTime - lag;
                    if(remaining > TimeSpan.Zero)
                    {
                        PlatformApi.Instance.Sleep(remaining);
                    }
                }

                lastrun = now;
            }
        }

        private void UnloadScene()
        {
            if (this.scene != null)
            {
                this.scene.PreDispose();
                this.scene.HostDispose(this);
                this.scene.Dispose(this.Application);
            }
        }

        private void LoadScene()
        {
            this.UnloadScene();

            this.scene = this.newHostSceneRequest.Scene ?? this.Application.RegisteredScenes[this.newHostSceneRequest.SceneName]();
            this.scene.PreInitialise(this.newHostSceneRequest.InitState);
            this.scene.Initialise(this.Application, this.AssetManager);
            this.scene.HostInitialise(this, this.AssetManager);
            this.scene.HostInitialised(this.newHostSceneRequest.InitState);

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
            this.localClient.Loaded = false;
            this.state = HostState.LoadingScene;
        }

        public override Task RunAsync()
        {
            this.localClient.Loaded = false;
            this.localClient.Connected = false;
            this.state = HostState.Constructed;
            this.runner.Start();
            return this.runner;
        }

        public override Task RunAsync(int scene, object initState = null)
        {
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                SceneName = scene,
                InitState = initState
            };
            this.localClient.Loaded = false;
            this.state = HostState.LoadingScene;
            this.runner.Start();
            return this.runner;
        }

        public override void Run(int scene, object initState = null)
        {
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                SceneName = scene,
                InitState = initState
            };
            this.state = HostState.LoadingScene;
            this.runner.RunSynchronously();
        }

        protected override void Dispose(bool finalising)
        {
            this.UnloadScene();
            this.state = HostState.Shutdown;
            base.Dispose(finalising);
        }

        public override Task DisposeAsync()
        {
            if (this.state != HostState.BeginShutdown)
            {
                this.state = HostState.BeginShutdown;
            }
            return this.runner;
        }
    }

    public enum HostState
    {
        Constructed = 0,

        BeginShutdown,

        Shutdown,

        RunningScene,

        LoadingScene,

        WaitingOnClients,

    }
}