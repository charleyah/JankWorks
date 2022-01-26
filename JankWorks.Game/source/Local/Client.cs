using System;
using System.Diagnostics;
using System.Threading;

using JankWorks.Core;
using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

using JankWorks.Game.Diagnostics;
using JankWorks.Game.Configuration;
using JankWorks.Game.Hosting;
using JankWorks.Game.Platform;
using JankWorks.Game.Threading;

namespace JankWorks.Game.Local
{
    public sealed class Client : Disposable, IRunner<ClientState, ClientState>
    {
        private struct NewSceneRequest
        {
            public int Scene;
            public ClientHost Host;
            public object InitState;
        }

        public Settings Settings { get; private set; }

        public ClientConfgiuration Configuration { get; private set; }

        public ClientMetrics Metrics { get; private set; }

        public Rectangle Viewport
        {
            get => this.graphicsDevice.Viewport;
        }

        public ClientState State => this.state;

        Stopwatch IRunner<ClientState, ClientState>.Timer => this.timer;

        TimeSpan IRunner<ClientState, ClientState>.TotalElapsed { get; set; }
        TimeSpan IRunner<ClientState, ClientState>.Accumulated { get; set; }
        TimeSpan IRunner<ClientState, ClientState>.TargetElapsed => this.updateTime;

        long IRunner<ClientState, ClientState>.LastRunTick { get; set; }

        private Stopwatch timer;
        private TimeSpan updateTime;

        private Application application;

        private ClientHost host;

        private Window window;

        private ScopedSynchronizationContext inputContext;

        private GraphicsDevice graphicsDevice;

        private AudioDevice audioDevice;

        private LoadingScreen loadingScreen;

        private Scene scene;

        private volatile ClientState state;

        private NewSceneRequest newSceneRequest;

        private Counter upsCounter;
        private Counter fpsCounter;

        public Client(Application application, ClientHost host)
        {
            this.timer = new Stopwatch();

            this.inputContext = new ScopedSynchronizationContext(false);
            this.Metrics = new ClientMetrics();

            var second = TimeSpan.FromSeconds(1);
            this.upsCounter = new Counter(second);
            this.fpsCounter = new Counter(second);

            this.state = ClientState.Constructed;
            this.application = application;
            this.host = host;

            var parms = application.ClientParameters;

            var settings = application.GetClientSettings();

            var config = application.DefaultClientConfiguration;

            config.Load(settings);

            var updateRate = (config.Vsync) ? config.DisplayMode.RefreshRate : config.UpdateRate;
            this.updateTime = TimeSpan.FromMilliseconds((1f / updateRate) * 1000);

            this.Configuration = config;
            this.Settings = settings;
          
                        
            var winds = new WindowSettings()
            {
                Title = application.Name,
                Monitor = config.Monitor,
                ShowCursor = parms.ShowCursor,
                Style = config.WindowStyle,
                DisplayMode = config.DisplayMode,
                VSync = config.Vsync
            };

            var surfs = new SurfaceSettings()
            {
                ClearColour = parms.ClearColour,
                Size = config.DisplayMode.Viewport.Size
            };

            var drawState = new DrawState()
            {
                Blend = BlendMode.Alpha,
                DepthTest = DepthTestMode.None                 
            };

            this.window = Window.Create(winds);
            this.graphicsDevice = GraphicsDevice.Create(surfs, this.window);
            this.graphicsDevice.DefaultDrawState = drawState;
            this.audioDevice = AudioDevice.GetDefault();
        }

        private void LoadScene()
        {
            var changeState = this.newSceneRequest;
            int scene = changeState.Scene;
            var host = this.newSceneRequest.Host ?? throw new ApplicationException();
            object initstate = this.newSceneRequest.InitState;

            this.UnloadScene();

            if (host is NullHost nullhost)
            {
                this.LoadSceneWithNoHost(scene, nullhost, initstate);
            }
            else
            {
                this.LoadSceneWithHost(scene, host, initstate);
            }

            this.Metrics.UpdatableMetricCounters = this.scene.UpdatableMetricCounters;
            this.Metrics.ParallelUpdatableMetricCounters = this.scene.ParallelUpdatableMetricCounters;
            this.Metrics.RenderableMetricCounters = this.scene.RenderableMetricCounters;
            this.Metrics.ParallelRenderableMetricCounters = this.scene.ParallelRenderableMetricCounters;

            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            this.state = ClientState.EndLoadingScene;
        }

        private void UnloadScene()
        {
            var scene = this.scene;
            if (scene != null)
            {
                scene.UnsubscribeInputs(this.window);

                this.inputContext = new ScopedSynchronizationContext(false);
                
                scene.PreDispose();

                using var sync = new ScopedSynchronizationContext(true);
                               
                if(this.host is NullHost)
                {
                    scene.DisposeSoundResources(this.audioDevice);
                    sync.Join();

                    try
                    {
                        this.graphicsDevice.Activate();
                        scene.DisposeGraphicsResources(this.graphicsDevice);
                        sync.Join();
                    }
                    finally
                    {
                        this.graphicsDevice.Deactivate();
                    }

                    scene.ClientDispose(this);
                    sync.Join();
                }
                else
                {
                    SynchronizationContext.SetSynchronizationContext(null);
                    this.host.UnloadScene();
                    SynchronizationContext.SetSynchronizationContext(sync);

                    scene.DisposeSoundResources(this.audioDevice);
                    sync.Join();

                    try
                    {
                        this.graphicsDevice.Activate();
                        scene.DisposeGraphicsResources(this.graphicsDevice);
                        sync.Join();
                    }
                    finally
                    {
                        this.graphicsDevice.Deactivate();
                    }

                    scene.ClientDisposeAfterShared(this);
                    sync.Join();
                }

                scene.Dispose(this.application);
                sync.Join();
            }
        }

        private void LoadSceneWithNoHost(int scene, NullHost host, object initState)
        {            
            var sceneToLoad = this.application.RegisteredScenes[scene]();

            sceneToLoad.PreInitialise(initState);

            using (var sync = new ScopedSynchronizationContext(true))
            {                             
                sceneToLoad.Initialise(this.application, this.application.RegisterAssetManager());
                sync.Join();

                sceneToLoad.ClientInitialise(this);
                sync.Join();

                try
                {
                    this.graphicsDevice.Activate();
                    sceneToLoad.InitialiseGraphicsResources(this.graphicsDevice);
                    sync.Join();
                }
                finally
                {
                    this.graphicsDevice.Deactivate();
                }

                sceneToLoad.InitialiseSoundResources(this.audioDevice);
                sync.Join();
            }
           
            sceneToLoad.ClientInitialised(initState);

            this.host = host;
            this.scene = sceneToLoad;
        }

        private void LoadSceneWithHost(int scene, ClientHost host, object initState)
        {
            var sceneToLoad = this.application.RegisteredScenes[scene]();

            sceneToLoad.PreInitialise(initState);

            using (var sync = new ScopedSynchronizationContext(true))
            {
                sceneToLoad.Initialise(this.application, this.application.RegisterAssetManager());
                sync.Join();

                SynchronizationContext.SetSynchronizationContext(null);
                host.LoadScene(sceneToLoad, initState);
                SynchronizationContext.SetSynchronizationContext(sync);

                sceneToLoad.ClientInitialiseAfterShared(this);
                sync.Join();

                sceneToLoad.InitialiseChannels(host.Dispatcher);
                sync.Join();

                try
                {
                    this.graphicsDevice.Activate();
                    sceneToLoad.InitialiseGraphicsResources(this.graphicsDevice);
                    sync.Join();
                }
                finally
                {
                    this.graphicsDevice.Deactivate();
                }

                sceneToLoad.InitialiseSoundResources(this.audioDevice);
                sync.Join();
            }

            sceneToLoad.ClientInitialised(initState);

            try
            {
                Threads.VerifyCorrectThread = false;
                this.SyncData(sceneToLoad, host);
            }
            finally
            {
                Threads.VerifyCorrectThread = true;
            }
            
           
            this.host = host;
            this.scene = sceneToLoad;
        }

        private void SyncData(Scene scene, Host host)
        {
            SpinWait.SpinUntil(() => host.State == HostState.WaitingOnClients || host.State == HostState.RunningScene);

            try
            {
                this.graphicsDevice.Activate();
                scene.SynchroniseClientUpStream();
            }
            finally
            {
                this.graphicsDevice.Deactivate();
            }

            host.Dispatcher.Synchronise();

            if (host.IsLocal)
            {
                scene.SynchroniseHostUpStream();
                scene.SynchroniseHostDownStream();
                host.Dispatcher.Synchronise();
            }

            try
            {
                this.graphicsDevice.Activate();
                scene.SynchroniseClientDownStream();
            }
            finally
            {
                this.graphicsDevice.Deactivate();
            }
        }

        public void ChangeScene(int scene, object initsate = null) => this.ChangeScene(scene, this.host, initsate);
        
        public void ChangeScene(int scene, ClientHost host, object initsate = null)
        {
            if (host is NullHost == false && object.ReferenceEquals(this.host, host) && host.IsRemote && host.IsConnected)
            {
                throw new ArgumentException();
            }

            if(scene < 0 || scene >= this.application.RegisteredScenes.Length)
            {
                throw new ArgumentException($"scene {scene} does not exist");
            }

            host.Start(this);

            this.newSceneRequest = new NewSceneRequest()
            {
                Host = host,
                InitState = initsate,
                Scene = scene
            };
            this.state = ClientState.BeginLoadingScene;

        }

        public void Run(int scene, object initState = null) => this.Run(scene, this.host, initState);

        public void Run(int scene, ClientHost host, object initState = null)
        {           
            this.graphicsDevice.Activate();
            this.graphicsDevice.ClearColour = Colour.Black;
            var ls = this.application.RegisterLoadingScreen();

            if (ls != null)
            {
                using var assets = this.application.RegisterAssetManager();
                ls.InitialiseResources(assets);
                ls.InitialiseGraphicsResources(this.graphicsDevice, assets);
                this.loadingScreen = ls;
            }
            this.window.Show();
            this.graphicsDevice.Clear();
            this.graphicsDevice.Display();
                
            this.ChangeScene(scene, host, initState);

            this.Run();
        }

        private void Run()
        {
            var clientThread = Thread.CurrentThread;
            clientThread.Name = $"{this.application.Name} Client Thread";
            Threads.ClientThread = clientThread;

            this.upsCounter.Start();
            this.fpsCounter.Start();


            var runner = this as IRunner<ClientState, ClientState>;
            runner.BeginRun();

            while (this.window.IsOpen)
            {
                var state = this.CheckForStateChange();

                var updateDuration = runner.Run(state, state);

                this.Metrics.UpdateLag = 1d * (updateDuration.TotalMilliseconds / updateTime.TotalMilliseconds);
            }

            runner.StopRun();

            this.UnloadScene();
            this.host.Dispose();
        }

        void IRunner<ClientState, ClientState>.Simulate(GameTime time, ClientState state)
        {
            this.Update(state, time);            
        }

        void IRunner<ClientState, ClientState>.Interpolate(GameTime time, ClientState state)
        {            
            this.Render(state, time);
        }

        private void Update(ClientState state, GameTime time)
        {
            this.Metrics.UpdatesPerSecond = this.upsCounter.Frequency;
            
            try
            {
                SynchronizationContext.SetSynchronizationContext(this.inputContext);
                this.inputContext.Yield();
                this.window.ProcessEvents();                
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
                       
            if (state > ClientState.BeginLoadingScene)
            {
                this.loadingScreen?.Update(time);
            }
            else
            {
                this.host.SynchroniseClientUpdate();
                this.scene.Update(time);
            }

            this.upsCounter.Count();
        }

        private void Render(ClientState state, GameTime time)
        {
            this.Metrics.FramesPerSecond = this.fpsCounter.Frequency;

            if (state > ClientState.BeginLoadingScene)
            {
                if (this.loadingScreen != null && this.graphicsDevice.Activate(this.updateTime))
                {
                    try
                    {
                        this.loadingScreen.Render(this.graphicsDevice, time);
                        this.graphicsDevice.Display();
                        this.fpsCounter.Count();
                    }
                    finally
                    {
                        this.graphicsDevice.Deactivate();
                    }
                }
            }
            else
            {
                this.scene.Render(this.graphicsDevice, time);
                this.graphicsDevice.Display();
                this.fpsCounter.Count();
            }
        }

        private ClientState CheckForStateChange()
        {
            var state = this.state;

            switch (state)
            {
                case ClientState.WaitingOnHost:
                    
                    if(this.host.IsHostLoaded)
                    {
                        this.graphicsDevice.Activate();
                        state = ClientState.RunningScene;
                        this.state = state;

                        var runner = this as IRunner<ClientState, ClientState>;

                        runner.StopRun();
                        runner.BeginRun();

                        this.scene.Initialised();
                        this.scene.SubscribeInputs(this.window);
                    }
                    else
                    {
                        Thread.Yield();
                    }
                    break;

                case ClientState.EndLoadingScene:

                    state = ClientState.WaitingOnHost;
                    this.state = state;
                    this.newSceneRequest = default;
                    break;

                case ClientState.BeginLoadingScene:

                    this.graphicsDevice.Deactivate();
                    state = ClientState.LoadingScene;
                    this.state = state;

                    var loaderThread = new Thread(new ThreadStart(() => this.LoadScene()));
                    loaderThread.IsBackground = false;
                    loaderThread.Name = $"{this.application.Name} Loader Thread";
                    loaderThread.Start();                    
                    break;
            }

            return state;
        }

        public void Close() => this.window.Close();

        protected override void Dispose(bool disposing)
        {
            var ls = this.loadingScreen;

            if (ls != null)
            {
                ls.DisposeGraphicsResources(this.graphicsDevice);
                ls.DisposeResources();
            }

            this.graphicsDevice.Dispose();
            this.window.Dispose();

            Threads.ClientThread = null;

            base.Dispose(disposing);
        }
    }

    // Order sensitive
    public enum ClientState : byte
    {
        Constructed = 0,

        RunningScene,

        BeginLoadingScene,

        LoadingScene,

        EndLoadingScene,

        WaitingOnHost
    }
}