
using System;
using System.Collections.Generic;
using System.Linq;

using JankWorks.Core;
using JankWorks.Audio;
using JankWorks.Interface;
using JankWorks.Graphics;

using JankWorks.Game.Local;
using JankWorks.Game.Hosting;
using JankWorks.Game.Assets;

namespace JankWorks.Game
{    
    public abstract class ApplicationScene
    {
        internal const int InitialObjectContainerCount = 8;

        internal protected bool PerfMetrics;

        protected ApplicationScene()
        {
            this.PerfMetrics = false;
        }

        public virtual void PreInitialise(object? state) { }
        public virtual void Initialise(Application app, AssetManager assets) 
        {
            this.PerfMetrics = app.Settings.GetEntry(Application.PerfMetricsEntry, (ent) => bool.Parse(ent), defaultValue: false);
        }
        public virtual void Initialised() { }

        public virtual void PreDispose() { }
        public virtual void Dispose(Application app) { }
    }

    public abstract class HostScene : ApplicationScene, ITickable
    {
        private List<object> hostObjects;

        private IResource[] resources;

        private IDisposable[] disposables;

        private ITickable[] tickables;

        private IAsyncTickable[] asyncTickables;

        public HostScene()
        {
            this.hostObjects = new List<object>(ApplicationScene.InitialObjectContainerCount);
            this.resources = Array.Empty<IResource>();
            this.disposables = Array.Empty<IDisposable>();
            this.tickables = Array.Empty<ITickable>();
            this.asyncTickables = Array.Empty<IAsyncTickable>();
        }

        protected void RegisterHostObject(object obj) => this.hostObjects.Add(obj);


        public virtual void HostInitialise(Host host, AssetManager assets) { }
        public virtual void HostInitialised(object? state) => this.BuildHostObjectContainers();
        
        private void BuildHostObjectContainers()
        {
            this.resources = (from obj in this.hostObjects where obj is IResource select (IResource)obj).Reverse().ToArray();
            this.disposables = (from obj in this.hostObjects where obj is IDisposable select (IDisposable)obj).Reverse().ToArray();

            this.tickables = (from obj in this.hostObjects where obj is ITickable select (ITickable)obj).ToArray();
            this.asyncTickables = (from obj in this.hostObjects where obj is IAsyncTickable select (IAsyncTickable)obj).ToArray();
        }

        public virtual void HostDispose(Host host) 
        {
            Array.ForEach(this.disposables, (d) => d.Dispose());
            Array.ForEach(this.resources, (r) => r.DisposeResources());
           
            this.disposables = Array.Empty<IDisposable>();
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

    public abstract class Scene : HostScene, IGraphicsResource, ISoundResource, IRenderable, IUpdatable, IInputListener
    {
        private List<object> clientObjects;

        private IResource[] resources;

        private IGraphicsResource[] graphicsResources;

        private ISoundResource[] soundResources;

        private IDisposable[] disposables;

        private IInputListener[] inputlisteners;

        private IUpdatable[] updatables;

        private IAsyncUpdatable[] asyncUpdatables;

        private IRenderable[] renderables;

        private IDrawable[] drawables;

        protected Scene() : base()
        {
            this.clientObjects = new List<object>(ApplicationScene.InitialObjectContainerCount);
            this.resources = Array.Empty<IResource>();
            this.graphicsResources = Array.Empty<IGraphicsResource>();
            this.soundResources = Array.Empty<ISoundResource>();
            this.disposables = Array.Empty<IDisposable>();

            this.inputlisteners = Array.Empty<IInputListener>();
            this.updatables = Array.Empty<IUpdatable>();
            this.asyncUpdatables = Array.Empty<IAsyncUpdatable>();

            this.renderables = Array.Empty<IRenderable>();
            this.drawables = Array.Empty<IDrawable>();
        }

        protected void RegisterClientObject(object obj) => this.clientObjects.Add(obj);

        public virtual void ClientInitialise(Client client, AssetManager assets) { }
        public virtual void ClientInitialised(object? state) => this.BuildClientObjectContainers();
        
        private void BuildClientObjectContainers()
        {
            this.resources = (from obj in this.clientObjects where obj is IResource select (IResource)obj).Reverse().ToArray();
            this.graphicsResources = (from obj in this.clientObjects where obj is IGraphicsResource select (IGraphicsResource)obj).Reverse().ToArray();
            this.soundResources = (from obj in this.clientObjects where obj is ISoundResource select (ISoundResource)obj).Reverse().ToArray();
            this.disposables = (from obj in this.clientObjects where obj is IDisposable select (IDisposable)obj).Reverse().ToArray();

            this.inputlisteners = (from obj in this.clientObjects where obj is IInputListener select (IInputListener)obj).ToArray();
            this.updatables = (from obj in this.clientObjects where obj is IUpdatable select (IUpdatable)obj).ToArray();
            this.asyncUpdatables = (from obj in this.clientObjects where obj is IAsyncUpdatable select (IAsyncUpdatable)obj).ToArray();

            this.renderables = (from obj in this.clientObjects where obj is IRenderable select (IRenderable)obj).ToArray();
            this.drawables = (from obj in this.clientObjects where obj is IDrawable select (IDrawable)obj).ToArray();
        }

        public virtual void ClientDispose(Client client) 
        {
            Array.ForEach(this.disposables, (d) => d.Dispose());
            Array.ForEach(this.resources, (r) => r.DisposeResources());

            this.resources = Array.Empty<IResource>();                       
            this.disposables = Array.Empty<IDisposable>();

            this.inputlisteners = Array.Empty<IInputListener>();
            this.updatables = Array.Empty<IUpdatable>();
            this.asyncUpdatables = Array.Empty<IAsyncUpdatable>();
        }

        public virtual void SubscribeInputs(Window window) 
        {
            for (int index = 0; index < this.inputlisteners.Length; index++)
            {
                this.inputlisteners[index].SubscribeInputs(window);
            }
        }
        public virtual void UnsubscribeInputs(Window window) 
        {
            for (int index = 0; index < this.inputlisteners.Length; index++)
            {
                this.inputlisteners[index].UnsubscribeInputs(window);
            }
        }

        public virtual void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets) { }
        public virtual void DisposeGraphicsResources(GraphicsDevice device) 
        {
            Array.ForEach(this.graphicsResources, (gr) => gr.DisposeGraphicsResources(device));
            this.graphicsResources = Array.Empty<IGraphicsResource>();
            this.renderables = Array.Empty<IRenderable>();
            this.drawables = Array.Empty<IDrawable>();
        }

        public virtual void InitialiseSoundResources(AudioDevice device, AssetManager assets) { }
        public virtual void DisposeSoundResources(AudioDevice device) 
        {
            Array.ForEach(this.soundResources, (sr) => sr.DisposeSoundResources(device));
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
            for (int index = 0; index < this.renderables.Length; index++)
            {
                this.renderables[index].Render(surface, frame);
            }
        }

        protected void Draw(Surface surface)
        {
            for (int index = 0; index < this.drawables.Length; index++)
            {
                this.drawables[index].Draw(surface);
            }
        }
    }
}
