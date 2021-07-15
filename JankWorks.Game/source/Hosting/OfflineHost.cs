using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Threading;
using System.Threading.Tasks;

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

        private volatile HostState state;

        private TimeSpan targetDelta;

        private LocalClientState localClient;

        private NewHostSceneRequest newHostSceneRequest;

        private Task runner;


#pragma warning disable CS8618
        public OfflineHost(Application application) : base(application)
        {
            var parms = application.HostParameters;

            this.targetDelta = TimeSpan.FromMilliseconds((1f / parms.TickRate) * 1000);

            this.runner = new Task(this.Run, TaskCreationOptions.LongRunning);            
        }
#pragma warning restore CS8618

        public override bool IsRemote => false;

        public override bool IsConnected => this.localClient.Connected;

        public override bool IsHostLoaded => throw new NotImplementedException();

        public override void Connect() => this.localClient.Connected = true;


        public override void NotifyClientLoaded() => this.localClient.Loaded = true;


        private void Run()
        {
            var timer = new Stopwatch();
            TimeSpan target = this.targetDelta;

            timer.Start();

            var lag = TimeSpan.Zero;     
            var tick = 0ul;

            TimeSpan lastrun = timer.Elapsed;

            while (this.state == HostState.RunningScene)
            {
                TimeSpan now = timer.Elapsed;
                TimeSpan since = now - lastrun;
                lag += since;
                this.Lag = lag;

                if(lag >= target)
                {
                    do
                    {
                        TimeSpan delta = (lag > target) ? target : lag;
                        this.scene.Tick(tick++, delta);
                        lag -= target;
                        this.TicksPerSecond = Convert.ToSingle(Math.Round(delta.TotalMilliseconds / 1000, 0));
                    }
                    while (lag >= target);
                }
                else
                {
                    Thread.Sleep(target - lag);
                }   
            }
        }

        private HostState CheckForStateChange()
        {
            throw new NotImplementedException();
        }

        public override void LoadScene(HostScene scene, object? initState = null)
        {
            throw new NotImplementedException();
        }

        public override void Start(string scene, object? initState = null)
        {
            throw new NotImplementedException();
        }

        public override void Run(string scene, object? initState = null)
        {
            throw new NotImplementedException();
        }
    }

    
         
    
    public enum HostState
    {
        Constructed = 0,

        RunningScene,

        BeginLoadingScene,

        LoadingScene,

        EndLoadingScene,

        WaitingOnClients,

        LoadFaliure
    }
}
