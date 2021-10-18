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

namespace JankWorks.Game.Local
{
    public sealed class Client : Disposable
    {
        private struct NewSceneRequest
        {
            public int Scene;
            public ClientHost Host;
            public object InitState;
        }

        public Settings Settings { get; private set; }

        public ClientConfgiuration Configuration { get; private set; }

        public TimeSpan Lag { get; private set; }

        public float UpdatesPerSecond { get; private set; }

        public float FramesPerSecond { get; private set; }

        internal ClientState State => this.state;

        private Application application;

        private ClientHost host;

        private Window window;

        private GraphicsDevice graphicsDevice;

        private AudioDevice audioDevice;

        private LoadingScreen loadingScreen;

        private Scene scene;

        private volatile ClientState state;

        private ClientParameters parameters;

        private NewSceneRequest newSceneRequest;

        private Counter upsCounter;
        private Counter fpsCounter;

        public Client(Application application, ClientHost host)
        {
            var second = TimeSpan.FromSeconds(1);
            this.upsCounter = new Counter(second);
            this.fpsCounter = new Counter(second);

            this.state = ClientState.Constructed;
            this.application = application;
            this.host = host;

            var settings = application.GetClientSettings();

            var config = application.DefaultClientConfiguration;

            config.Load(settings);
            this.Configuration = config;
            this.Settings = settings;

            var parms = application.ClientParameters;
            this.parameters = parms;

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

        public MetricCounter[] GetMetrics() => this.scene.ClientMetricCounters;

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

                scene.PreDispose();
                
                if(this.host is NullHost)
                {
                    scene.DisposeSoundResources(this.audioDevice);
                    scene.DisposeGraphicsResources(this.graphicsDevice);
                    scene.ClientDispose(this);
                }
                else
                {
                    this.host.UnloadScene();
                    scene.DisposeSoundResources(this.audioDevice);
                    scene.DisposeGraphicsResources(this.graphicsDevice);
                    scene.ClientDisposeAfterShared(this);                    
                }

                scene.Dispose(this.application);
            }
        }

        private void LoadSceneWithNoHost(int scene, NullHost host, object initState)
        {            
            var sceneToLoad = this.application.RegisteredScenes[scene]();

            sceneToLoad.PreInitialise(initState);
            sceneToLoad.Initialise(this.application, this.application.RegisterAssetManager());
            sceneToLoad.ClientInitialise(this);
            sceneToLoad.InitialiseGraphicsResources(this.graphicsDevice);
            sceneToLoad.InitialiseSoundResources(this.audioDevice);
            sceneToLoad.ClientInitialised(initState);

            this.host = host;
            this.scene = sceneToLoad;
        }

        private void LoadSceneWithHost(int scene, ClientHost host, object initState)
        {
            var sceneToLoad = this.application.RegisteredScenes[scene]();
            
            sceneToLoad.PreInitialise(initState);
            sceneToLoad.Initialise(this.application, this.application.RegisterAssetManager());

            host.LoadScene(sceneToLoad, initState);

            sceneToLoad.ClientInitialiseAfterShared(this);
            sceneToLoad.InitialiseGraphicsResources(this.graphicsDevice);
            sceneToLoad.InitialiseSoundResources(this.audioDevice);
            sceneToLoad.ClientInitialised(initState);

            this.host = host;
            this.scene = sceneToLoad;
        }

        public void ChangeScene(int scene, object initsate = null) => this.ChangeScene(scene, this.host, initsate);
        
        public void ChangeScene(int scene, ClientHost host, object initsate = null)
        {
            if (host is NullHost == false && object.ReferenceEquals(this.host, host) && host.IsRemote && host.IsConnected)
            {
                throw new ArgumentException();
            }

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

            var ls = this.application.RegisterLoadingScreen();

            if (ls != null)
            {
                using var assets = this.application.RegisterAssetManager();
                ls.InitialiseResources(assets);
                ls.InitialiseGraphicsResources(this.graphicsDevice, assets);
                this.loadingScreen = ls;
            }
            this.window.Show();
            this.ChangeScene(scene, host, initState);
            this.Run();
        }

        private void Run()
        {
            Thread.CurrentThread.Name = $"{this.application.Name} Client Thread";

            var updateTime = TimeSpan.FromMilliseconds((1f / this.parameters.UpdateRate) * 1000);
            var frameTime = TimeSpan.FromMilliseconds((1f / this.Configuration.FrameRate) * 1000);

            var timer = new Stopwatch();
            timer.Start();

            var accumulator = TimeSpan.Zero;
            var lag = TimeSpan.Zero;
            var lastrun = timer.Elapsed;

            this.upsCounter.Start();
            this.fpsCounter.Start();

            while (this.window.IsOpen)
            {
                var state = this.CheckForStateChange();

                TimeSpan now = timer.Elapsed;
                TimeSpan since = now - lastrun;
                accumulator += since;
                lag += since;
                this.Lag = lag;

                if (accumulator >= frameTime)
                {
                    while (lag >= updateTime)
                    {
                        var delta = (lag > updateTime) ? updateTime : lag;

                        this.Update(state, delta);

                        lag -= updateTime;
                    }

                    var frame = new Frame(accumulator.TotalMilliseconds / updateTime.TotalMilliseconds);

                    this.Render(state, frame, updateTime);

                    accumulator -= frameTime;
                }
                else
                {
                    var remaining = frameTime - accumulator;

                    if (remaining > TimeSpan.Zero)
                    {
                        PlatformApi.Instance.Sleep(remaining);
                    }
                }

                lastrun = now;
            }

            this.UnloadScene();
            this.host.DisposeAsync();
        }

        private void Update(ClientState state, TimeSpan delta)
        {
            this.UpdatesPerSecond = this.upsCounter.Frequency;
            this.upsCounter.Count();

            this.window.ProcessEvents();
            if (state > ClientState.BeginLoadingScene)
            {
                this.loadingScreen?.Update(delta);
            }
            else
            {
                this.scene.Update(delta);
            }
        }

        private void Render(ClientState state, Frame frame, TimeSpan timeout)
        {
            this.FramesPerSecond = this.fpsCounter.Frequency;            

            if (state > ClientState.BeginLoadingScene)
            {
                if (this.loadingScreen != null && this.graphicsDevice.Activate(timeout))
                {
                    try
                    {
                        this.loadingScreen.Render(this.graphicsDevice, frame);
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
                this.scene.Render(this.graphicsDevice, frame);
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
        
        protected override void Dispose(bool finalising)
        {
            var ls = this.loadingScreen;

            if (ls != null)
            {
                ls.DisposeGraphicsResources(this.graphicsDevice);
                ls.DisposeResources();
            }

            this.graphicsDevice.Dispose();
            this.window.Dispose();

            base.Dispose(finalising);
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