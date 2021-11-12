using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using JankWorks.Audio;
using JankWorks.Interface;
using JankWorks.Graphics;

using JankWorks.Game.Local;
using JankWorks.Game.Hosting;
using JankWorks.Game.Hosting.Messaging;
using JankWorks.Game.Assets;
using JankWorks.Game.Diagnostics;

namespace JankWorks.Game
{    
    public abstract class ApplicationScene
    {
        internal const int InitialObjectContainerCount = 8;

        internal bool PerformanceMetricsEnabled { get; private set; }

        protected Application Application { get; private set; }

        protected AssetManager Assets { get; private set; }

        internal ManualResetEvent sharedSignal;

        protected ApplicationScene()
        {
            this.sharedSignal = new ManualResetEvent(true);
        }

        public virtual void PreInitialise(object state) => this.sharedSignal.Reset();
             
        public virtual void Initialise(Application app, AssetManager assets) 
        {
            this.PerformanceMetricsEnabled = app.Configuration.PerformanceMetricsEnabled;
            this.Application = app;
            this.Assets = assets;
        }

        public virtual void Initialised() { }

        public virtual void PreDispose() => this.sharedSignal.Reset();
        
        public virtual void Dispose(Application app) 
        {
            this.Assets.Dispose();
        }
    }

    public abstract class HostScene : ApplicationScene, ITickable
    {
        private List<object> hostObjects;

        private IResource[] resources;

        private IDispatchable[] dispatchables;

        private IDisposable[] disposables;

        private ITickable[] tickables;

        private IAsyncTickable[] asyncTickables;

        internal MetricCounter[] TickMetricCounters { get; private set; }

        internal MetricCounter[] AsyncTickMetricCounters { get; private set; }

        public HostScene()
        {            
            this.hostObjects = new List<object>(ApplicationScene.InitialObjectContainerCount);
            this.resources = Array.Empty<IResource>();
            this.dispatchables = Array.Empty<IDispatchable>();
            this.disposables = Array.Empty<IDisposable>();
            this.tickables = Array.Empty<ITickable>();
            this.asyncTickables = Array.Empty<IAsyncTickable>();
            this.TickMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncTickMetricCounters = Array.Empty<MetricCounter>();
        }                

        protected void RegisterHostObject(object obj) => this.hostObjects.Add(obj);

        public virtual void SharedInitialise(Host host, Client client) => this.InternalHostInitialise();

        public virtual void HostInitialise(Host host) => this.InternalHostInitialise();

        private void InternalHostInitialise()
        {
            this.BuildHostObjectContainers();

            for (int index = 0; index < this.resources.Length; index++)
            {
                this.resources[index].InitialiseResources(this.Assets);
            }
        }

        public virtual void SharedInitialised(object state) 
        {
            this.sharedSignal.Set();
        }

        public virtual void InitialiseChannels(Dispatcher dispatcher)
        {
            for (int index = 0; index < this.dispatchables.Length; index++)
            {
                this.dispatchables[index].InitialiseChannels(dispatcher);
            }
        }

        internal void SynchroniseHostUpStream()
        {
            for (int index = 0; index < this.dispatchables.Length; index++)
            {
                this.dispatchables[index].UpSynchronise();
            }
        }

        internal void SynchroniseHostDownStream()
        {
            for (int index = 0; index < this.dispatchables.Length; index++)
            {
                this.dispatchables[index].DownSynchronise();
            }
        }

        public virtual void HostInitialised(object state) { }
        
        private void BuildHostObjectContainers()
        {
            this.resources = (from obj in this.hostObjects where obj is IResource select (IResource)obj).Reverse().ToArray();            
            this.disposables = (from obj in this.hostObjects where obj is IDisposable select (IDisposable)obj).Reverse().ToArray();
            this.dispatchables = (from obj in this.hostObjects where obj is IDispatchable select (IDispatchable)obj).ToArray();

            if (this.PerformanceMetricsEnabled)
            {                
                this.tickables = (from obj in this.hostObjects where obj is ITickable select new TickableMetricCounter((ITickable)obj)).ToArray();
                this.asyncTickables = (from obj in this.hostObjects where obj is IAsyncTickable select new AsyncTickableMetricCounter((IAsyncTickable)obj)).ToArray();

                this.TickMetricCounters = (MetricCounter[])this.tickables.Clone();
                this.AsyncTickMetricCounters = (MetricCounter[])this.asyncTickables.Clone();
            }
            else
            {
                this.tickables = (from obj in this.hostObjects where obj is ITickable select (ITickable)obj).ToArray();
                this.asyncTickables = (from obj in this.hostObjects where obj is IAsyncTickable select (IAsyncTickable)obj).ToArray();
                this.TickMetricCounters = Array.Empty<MetricCounter>();
                this.AsyncTickMetricCounters = Array.Empty<MetricCounter>();
            }            
        }

        public virtual void SharedDispose(Host host, Client client)
        {
            try
            {
                this.InternalHostDispose(host);
            }
            finally
            {
                this.sharedSignal.Set();
            }                       
        }

        public virtual void HostDispose(Host host) => this.InternalHostDispose(host);

        private void InternalHostDispose(Host host)
        {
            this.TickMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncTickMetricCounters = Array.Empty<MetricCounter>();

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }

            foreach(var resource in this.resources)
            {
                resource.DisposeResources();
            }

            this.disposables = Array.Empty<IDisposable>();
            this.dispatchables = Array.Empty<IDispatchable>();
            this.resources = Array.Empty<IResource>();
            this.tickables = Array.Empty<ITickable>();
            this.asyncTickables = Array.Empty<IAsyncTickable>();
            this.hostObjects.Clear();
        }

        public virtual void Tick(ulong tick, TimeSpan delta)
        {
            for(int index = 0; index < this.asyncTickables.Length; index++)
            {
                this.asyncTickables[index].BeginTick(tick, delta);
            }

            for (int index = 0; index < this.tickables.Length; index++)
            {
                this.tickables[index].Tick(tick, delta);
            }

            for (int index = 0; index < this.asyncTickables.Length; index++)
            {
                this.asyncTickables[index].EndTick(tick, delta);
            }
        }
    }

    public abstract class Scene : HostScene
    {
        private List<object> clientObjects;

        private IResource[] resources;

        private IDispatchable[] dispatchables;

        private IGraphicsResource[] graphicsResources;

        private ISoundResource[] soundResources;

        private IDisposable[] disposables;

        private IInputListener[] inputlisteners;

        private IUpdatable[] updatables;

        private IAsyncUpdatable[] asyncUpdatables;

        private IRenderable[] renderables;

        private IAsyncRenderable[] asyncRenderables;

        internal MetricCounter[] UpdatableMetricCounters { get; private set; }

        internal MetricCounter[] AsyncUpdatableMetricCounters { get; private set; }

        internal MetricCounter[] RenderableMetricCounters { get; private set; }

        internal MetricCounter[] AsyncRenderableMetricCounters { get; private set; }

        protected Scene() : base()
        {
            this.UpdatableMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncUpdatableMetricCounters = Array.Empty<MetricCounter>();
            this.RenderableMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncRenderableMetricCounters = Array.Empty<MetricCounter>();

            this.clientObjects = new List<object>(ApplicationScene.InitialObjectContainerCount);
            this.resources = Array.Empty<IResource>();
            this.dispatchables = Array.Empty<IDispatchable>();
            this.graphicsResources = Array.Empty<IGraphicsResource>();
            this.soundResources = Array.Empty<ISoundResource>();
            this.disposables = Array.Empty<IDisposable>();

            this.inputlisteners = Array.Empty<IInputListener>();
            this.updatables = Array.Empty<IUpdatable>();
            this.asyncUpdatables = Array.Empty<IAsyncUpdatable>();

            this.renderables = Array.Empty<IRenderable>();
            this.asyncRenderables = Array.Empty<IAsyncRenderable>();
        }

        protected void RegisterClientObject(object obj) => this.clientObjects.Add(obj);

        internal void ClientInitialiseAfterShared(Client client)
        {
            this.sharedSignal.WaitOne();
            this.ClientInitialise(client);
        }

        public virtual void ClientInitialise(Client client)
        {
            this.BuildClientObjectContainers();

            for (int index = 0; index < this.resources.Length; index++)
            {
                this.resources[index].InitialiseResources(this.Assets);
            }
        }

        public override void InitialiseChannels(Dispatcher dispatcher)
        {
            base.InitialiseChannels(dispatcher);

            for (int index = 0; index < this.dispatchables.Length; index++)
            {
                this.dispatchables[index].InitialiseChannels(dispatcher);
            }
        }

        internal void SynchroniseClientUpStream()
        {
            for (int index = 0; index < this.dispatchables.Length; index++)
            {
                this.dispatchables[index].UpSynchronise();
            }
        }

        internal void SynchroniseClientDownStream()
        {
            for (int index = 0; index < this.dispatchables.Length; index++)
            {
                this.dispatchables[index].DownSynchronise();
            }
        }

        public virtual void ClientInitialised(object state) { }
        
        private void BuildClientObjectContainers()
        {
            this.resources = (from obj in this.clientObjects where obj is IResource select (IResource)obj).Reverse().ToArray();
            this.graphicsResources = (from obj in this.clientObjects where obj is IGraphicsResource select (IGraphicsResource)obj).Reverse().ToArray();
            this.soundResources = (from obj in this.clientObjects where obj is ISoundResource select (ISoundResource)obj).Reverse().ToArray();
            this.disposables = (from obj in this.clientObjects where obj is IDisposable select (IDisposable)obj).Reverse().ToArray();

            this.dispatchables = (from obj in this.clientObjects where obj is IDispatchable select (IDispatchable)obj).ToArray();

            this.inputlisteners = (from obj in this.clientObjects where obj is IInputListener select (IInputListener)obj).ToArray();

            if (this.PerformanceMetricsEnabled)
            {
                this.updatables = (from obj in this.clientObjects where obj is IUpdatable select new UpdatableMetricCounter((IUpdatable)obj)).ToArray();               
                this.renderables = (from obj in this.clientObjects where obj is IRenderable select new RenderableMetricCounter((IRenderable)obj)).ToArray();
                this.asyncUpdatables = (from obj in this.clientObjects where obj is IAsyncUpdatable select new AsyncUpdatableMetricCounter((IAsyncUpdatable)obj)).ToArray();
                this.asyncRenderables = (from obj in this.clientObjects where obj is IAsyncRenderable select new AsyncRenderableMetricCounter((IAsyncRenderable)obj)).ToArray();

                this.UpdatableMetricCounters = (MetricCounter[])this.updatables.Clone();
                this.AsyncUpdatableMetricCounters = (MetricCounter[])this.asyncUpdatables.Clone();
                this.RenderableMetricCounters = (MetricCounter[])this.renderables.Clone();
                this.AsyncRenderableMetricCounters = (MetricCounter[])this.asyncRenderables.Clone();
            }
            else
            {
                this.updatables = (from obj in this.clientObjects where obj is IUpdatable select (IUpdatable)obj).ToArray();               
                this.renderables = (from obj in this.clientObjects where obj is IRenderable select (IRenderable)obj).ToArray();
                this.asyncUpdatables = (from obj in this.clientObjects where obj is IAsyncUpdatable select (IAsyncUpdatable)obj).ToArray();
                this.asyncRenderables = (from obj in this.clientObjects where obj is IAsyncRenderable select (IAsyncRenderable)obj).ToArray();

                this.UpdatableMetricCounters = Array.Empty<MetricCounter>();
                this.AsyncUpdatableMetricCounters = Array.Empty<MetricCounter>();
                this.RenderableMetricCounters = Array.Empty<MetricCounter>();
                this.AsyncRenderableMetricCounters = Array.Empty<MetricCounter>();
            }            
        }

        internal void ClientDisposeAfterShared(Client client)
        {
            this.sharedSignal.WaitOne();
            this.ClientDispose(client);
        }

        public virtual void ClientDispose(Client client) 
        {
            this.UpdatableMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncUpdatableMetricCounters = Array.Empty<MetricCounter>();

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }

            foreach (var resource in this.resources)
            {
                resource.DisposeResources();
            }

            this.resources = Array.Empty<IResource>();                       
            this.disposables = Array.Empty<IDisposable>();
            this.dispatchables = Array.Empty<IDispatchable>();

            this.inputlisteners = Array.Empty<IInputListener>();
            this.updatables = Array.Empty<IUpdatable>();
            this.asyncUpdatables = Array.Empty<IAsyncUpdatable>();
            this.clientObjects.Clear();
        }

        public virtual void SubscribeInputs(IInputManager inputManager) 
        {
            for (int index = 0; index < this.inputlisteners.Length; index++)
            {
                this.inputlisteners[index].SubscribeInputs(inputManager);
            }
        }

        public virtual void UnsubscribeInputs(IInputManager inputManager) 
        {
            for (int index = 0; index < this.inputlisteners.Length; index++)
            {
                this.inputlisteners[index].UnsubscribeInputs(inputManager);
            }
        }

        public virtual void InitialiseGraphicsResources(GraphicsDevice device) 
        {
            for (int index = 0; index < this.graphicsResources.Length; index++)
            {
                this.graphicsResources[index].InitialiseGraphicsResources(device, this.Assets);
            }
        }

        public virtual void DisposeGraphicsResources(GraphicsDevice device) 
        {
            foreach(var graphicsResource in this.graphicsResources)
            {
                graphicsResource.DisposeGraphicsResources(device);
            }

            this.graphicsResources = Array.Empty<IGraphicsResource>();
            this.renderables = Array.Empty<IRenderable>();
            this.asyncRenderables = Array.Empty<IAsyncRenderable>();

            this.RenderableMetricCounters = Array.Empty<MetricCounter>();
            this.AsyncRenderableMetricCounters = Array.Empty<MetricCounter>();
        }

        public virtual void InitialiseSoundResources(AudioDevice device) 
        {
            for (int index = 0; index < this.soundResources.Length; index++)
            {
                this.soundResources[index].InitialiseSoundResources(device, this.Assets);
            }
        }

        public virtual void DisposeSoundResources(AudioDevice device) 
        {
            foreach (var soundResource in this.soundResources)
            {
                soundResource.DisposeSoundResources(device);
            }

            this.soundResources = Array.Empty<ISoundResource>();
        }

        public virtual void Update(TimeSpan delta) 
        {
            for (int index = 0; index < this.asyncUpdatables.Length; index++)
            {
                this.asyncUpdatables[index].BeginUpdate(delta);
            }

            for (int index = 0; index < this.updatables.Length; index++)
            {
                this.updatables[index].Update(delta);
            }

            for (int index = 0; index < this.asyncUpdatables.Length; index++)
            {
                this.asyncUpdatables[index].EndUpdate(delta);
            }
        }

        public virtual void Render(Surface surface, Frame frame) 
        {
            for(int index = 0; index < this.asyncRenderables.Length; index++)
            {
                this.asyncRenderables[index].BeginRender(surface, frame);
            }

            for (int index = 0; index < this.renderables.Length; index++)
            {
                this.renderables[index].Render(surface, frame);
            }

            for (int index = 0; index < this.asyncRenderables.Length; index++)
            {
                this.asyncRenderables[index].EndRender(surface, frame);
            }
        }
    }
}