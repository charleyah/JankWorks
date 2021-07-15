using System;
using System.Diagnostics;

using Thread = System.Threading.Thread;
using ThreadPool = System.Threading.ThreadPool;

using JankWorks.Core;
using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

using JankWorks.Game.Configuration;
using JankWorks.Game.Assets;
using JankWorks.Game.Hosting;


using JankWorks.Game.Platform;

namespace JankWorks.Game.Local
{
    public sealed class Client : Disposable
    {
        private struct NewSceneRequest
        {
            public string? SceneName;
            public Host? Host;
            public object? InitState;
        }

        public Settings Settings { get; private set; }

        public TimeSpan Lag { get; private set; }

        public float UpdatesPerSecond { get; private set; }

        public float FramesPerSecond { get; private set; }

        private Application application;

        private Host host;

        private AssetManager assetManager;

        private Window window;

        private GraphicsDevice graphicsDevice;

        private AudioDevice audioDevice;

        private LoadingScreen? loadingScreen;

        private Scene scene;

        private volatile ClientState state;

        private TimeSpan targetDelta;
        private TimeSpan targetFrameRateDelta;

        private NewSceneRequest newSceneRequest;

#pragma warning disable CS8618
        public Client(Application application, ClientConfgiuration config, Host host)
        {
            this.state = ClientState.Constructed;
            this.application = application;
            this.host = host;
            this.assetManager = application.RegisterAssetManager();

            var settings = application.GetClientSettings();
            config.Load(settings);
            this.Settings = settings;

            var parms = application.ClientParameters;

            this.targetDelta = TimeSpan.FromMilliseconds((1f / parms.UpdateRate) * 1000);          
            this.targetFrameRateDelta = TimeSpan.FromMilliseconds((1f / config.FrameRate) * 1000);

            var winds = new WindowSettings()
            {
                Title = application.Name,
                Monitor = config.Monitor,
                ShowCursor = parms.ShowCursor,
                Style = config.WindowStyle,
                VideoMode = config.VideoMode,
                VSync = config.Vsync
            };

            var surfs = new SurfaceSettings()
            {
                ClearColour = parms.ClearColour,
                Size = config.VideoMode.Viewport.Size
            };

            this.window = Window.Create(winds);
            this.graphicsDevice = GraphicsDevice.Create(surfs, this.window);
            this.audioDevice = AudioDevice.Create();
        }
#pragma warning restore CS8618

        private void LoadScene()
        {
            var changeState = this.newSceneRequest;
            string scene = changeState.SceneName ?? throw new ApplicationException();
            Host host = this.newSceneRequest.Host ?? throw new ApplicationException();
            object? initstate = this.newSceneRequest.InitState;


            if (!object.ReferenceEquals(this.host, host))
            {
                this.host.Dispose();
                this.host = host;
            }


            if(this.host is RemoteHost remoteHost)
            {
                this.LoadSceneWithRemoteHost(scene, remoteHost, initstate);
            }
            else if(this.host is LocalHost localHost)
            {
                this.LoadSceneWithLocalHost(scene, localHost, initstate);
            }
            else
            {
                throw new NotImplementedException();
            }
            this.state = ClientState.EndLoadingScene;
        }

        private void LoadSceneWithRemoteHost(string scene, RemoteHost host, object? initState)
        {
            if (this.scene != null)
            {
                this.scene.PreDispose();
                this.scene.UnsubscribeInputs(this.window);
                this.scene.DisposeSoundResources(this.audioDevice);
                this.scene.DisposeGraphicsResources(this.graphicsDevice);
                this.scene.ClientDispose(this);
                this.scene.Dispose(this.application);
            }

            if(!host.IsConnected)
            {
                host.Connect();
            }

            host.LoadScene(scene, initState);

            this.scene = this.application.Scenes[scene]();

            this.scene.PreInitialise(initState);
            this.scene.Initialise(this.application, this.assetManager);
            this.scene.ClientInitialise(this, this.assetManager);
            this.scene.InitialiseGraphicsResources(this.graphicsDevice, this.assetManager);
            this.scene.InitialiseSoundResources(this.audioDevice, this.assetManager);
            this.scene.ClientInitialised(initState);

            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        }

        private void LoadSceneWithLocalHost(string scene, LocalHost host, object? initState)
        {
            if (this.scene != null)
            {
                this.scene.UnsubscribeInputs(this.window);
                this.scene.DisposeSoundResources(this.audioDevice);
                this.scene.DisposeGraphicsResources(this.graphicsDevice);
                this.scene.ClientDispose(this);
            }

            this.scene = this.application.Scenes[scene]();

            if (!host.IsConnected)
            {
                host.Connect();
            }

            host.LoadScene(this.scene, initState);

            this.scene.ClientInitialise(this, this.assetManager);
            this.scene.InitialiseGraphicsResources(this.graphicsDevice, this.assetManager);
            this.scene.InitialiseSoundResources(this.audioDevice, this.assetManager);
            this.scene.ClientInitialised(initState);            
        }

        public void ChangeScene(string scene, object? initsate = null) => this.ChangeScene(scene, this.host, initsate);     
        public void ChangeScene(string scene, Host host, object? initsate = null)
        {
            if (object.ReferenceEquals(this.host, host) && host.IsRemote && host.IsConnected)
            {
                throw new ArgumentException();
            }

            this.newSceneRequest = new NewSceneRequest()
            {
                Host = host,
                InitState = initsate,
                SceneName = scene
            };
            this.state = ClientState.BeginLoadingScene;
        }

        public void Run(string scene, object? initState = null) => this.Run(scene, this.host, initState);
        public void Run(string scene, Host host, object? initState = null)
        {
            this.graphicsDevice.Activate();

            var ls = this.application.RegisterLoadingScreen();

            if (ls != null)
            {
                ls.InitialiseResources(this.assetManager);
                ls.InitialiseGraphicsResources(this.graphicsDevice, this.assetManager);
                this.loadingScreen = ls;
            }
            this.window.Show();
            this.ChangeScene(scene, host, initState);
            this.Run();
        }
        private void Run()
        {
            var timer = new Stopwatch();
            var target = this.targetDelta;
            timer.Start();

            var lag = TimeSpan.Zero;
            var lastrun = timer.Elapsed;

            while (this.window.IsOpen)
            {
                var state = this.CheckForStateChange();

                TimeSpan now = timer.Elapsed;
                TimeSpan since = now - lastrun;
                lag += since;
                this.Lag = lag;

                if(lag < this.targetFrameRateDelta)
                {
                    PlatformApi.Instance.Sleep(this.targetFrameRateDelta - lag);
                    lastrun = now;
                    continue;
                }

                while (lag >= target)
                {
                    var delta = (lag > target) ? target : lag;

                    this.Update(state, delta);

                    lag -= target;
                    this.UpdatesPerSecond = Convert.ToSingle(Math.Round(1000 /delta.TotalMilliseconds, 0));
                }

                var frame = new Frame(lag.TotalMilliseconds / target.TotalMilliseconds);
                this.Render(state, frame);
                this.FramesPerSecond = Convert.ToSingle(Math.Round(1000 / since.TotalMilliseconds, 0));

                lastrun = now;
            }
        }

        private void Update(ClientState state, TimeSpan delta)
        {
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

        private void Render(ClientState state, Frame frame)
        {
            if (state > ClientState.BeginLoadingScene)
            {
                if (this.loadingScreen != null && this.graphicsDevice.Activate(this.targetDelta))
                {
                    try
                    {
                        this.loadingScreen.Render(this.graphicsDevice, frame);
                        this.graphicsDevice.Display();
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

                        if(this.host.IsRemote)
                        {
                            this.scene.Initialised();
                        }
                        
                        this.scene.SubscribeInputs(this.window);
                    }
                   
                    break;

                case ClientState.EndLoadingScene:

                    state = ClientState.WaitingOnHost;
                    this.state = state;
                    this.host.NotifyClientLoaded();
                    this.newSceneRequest = default;
                    break;

                case ClientState.BeginLoadingScene:

                    this.graphicsDevice.Deactivate();
                    state = ClientState.LoadingScene;
                    this.state = state;

#pragma warning disable CS8602
#pragma warning disable CS8600
                    ThreadPool.QueueUserWorkItem((client) => ((Client)client).LoadScene(), this);                    
#pragma warning restore CS8600
#pragma warning restore CS8602

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
