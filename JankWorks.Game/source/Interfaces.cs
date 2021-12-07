using System;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

using JankWorks.Game.Assets;

using JankWorks.Game.Hosting.Messaging;

// This file defines all the interfaces an arbitrary game object can implement

namespace JankWorks.Game
{
    /// <summary>
    /// Interface for providing a name for a game object. Used in performance metrics and debugging.
    /// </summary>
    public interface INameable
    {
        string GetName() => this.GetType().Name;
    }

    /// <summary>
    /// Interface for any game object that reads/loads game assets
    /// </summary>
    public interface IResource
    {
        void InitialiseResources(AssetManager assets);

        void DisposeResources();
    }

    /// <summary>
    /// Interface for client-side game objects that listen for user inputs.
    /// </summary>
    public interface IInputListener
    {
        void SubscribeInputs(IInputManager inputManager);

        void UnsubscribeInputs(IInputManager inputManager);
    }

    /// <summary>
    /// Interface for tickable host-side game objects.
    /// </summary>
    public interface ITickable : INameable
    {
        IntervalBehaviour TickInterval => IntervalBehaviour.Asynchronous;

        void Tick(ulong tick, TimeSpan delta);
    }

    /// <summary>
    /// Interface for tickable host-side game objects that are ticked in parallel.
    /// </summary>
    public interface IParallelTickable : INameable
    {
        void ForkTick(ulong tick, TimeSpan delta) => this.ForkTick(tick, delta, null);

        void ForkTick(ulong tick, TimeSpan delta, Action<IParallelTickable> callback);

        void JoinTick(ulong tick, TimeSpan delta);
    }

    /// <summary>
    /// Interface for updatable client-side game objects.
    /// </summary>
    public interface IUpdatable : INameable
    {
        IntervalBehaviour UpdateInterval => IntervalBehaviour.Asynchronous;

        void Update(TimeSpan delta);
    }

    /// <summary>
    /// Interface for updatable client-side game objects that are updated in parallel.
    /// </summary>
    public interface IParallelUpdatable : INameable
    {
        void ForkUpdate(TimeSpan delta) => this.ForkUpdate(delta, null);

        void ForkUpdate(TimeSpan delta, Action<IParallelUpdatable> callback);

        void JoinUpdate(TimeSpan delta);
    }

    /// <summary>
    /// Interface for game objects that communicate between client and host.
    /// </summary>
    public interface IDispatchable
    {
        void InitialiseChannels(Dispatcher dispatcher);

        void UpSynchronise();

        void DownSynchronise();
    }

    /// <summary>
    /// Interface for client-side game objects that emit sounds.
    /// </summary>
    public interface ISoundResource
    {
        void InitialiseSoundResources(AudioDevice device, AssetManager assets);

        void DisposeSoundResources(AudioDevice device);
    }

    /// <summary>
    /// Interface for client-side game objects that utilise graphics.
    /// </summary>
    public interface IGraphicsResource
    {
        void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets);

        void DisposeGraphicsResources(GraphicsDevice device);
    }

    /// <summary>
    /// Interface for client-side game objects that are rendered.
    /// </summary>
    public interface IRenderable : IGraphicsResource, INameable
    {
        IntervalBehaviour RenderInterval => IntervalBehaviour.NoAsync;

        void Render(Surface surface, Frame frame);
    }

    /// <summary>
    /// Interface for client-side game objects that are rendered in parallel.
    /// </summary>
    public interface IParallelRenderable : IGraphicsResource, INameable
    {
        void ForkRender(Surface surface, Frame frame) => this.ForkRender(surface, frame, null);

        void ForkRender(Surface surface, Frame frame, Action<IParallelRenderable> callback);

        void JoinRender(Surface surface, Frame frame);
    }


    /// <summary>
    /// Defines how interval methods (update, tick and render) are invoked.
    /// </summary>
    public enum IntervalBehaviour
    {
        /// <summary>
        /// Method is invoked every interval and does not support async method invocation. Used to indicate no SynchronizationContext is required.
        /// </summary>
        NoAsync,

        /// <summary>
        /// Method is invoked every interval. If the method is async it can be invoked multiple times while awaiting tasks.
        /// </summary>
        Overlapped,

        /// <summary>
        /// Method is invoked every interval. If the method is async it will block until all awaited tasks are completed.
        /// </summary>
        Synchronous,

        /// <summary>
        /// Method is invoked or resumes awaited tasks on interval.
        /// </summary>
        Asynchronous
    }

    /// <summary>
    /// Provides infomation about a given frame to render
    /// </summary>
    public readonly struct Frame : IEquatable<Frame>
    {
        public static readonly Frame Complete = new Frame(1d);

        public readonly double Interpolation;

        public Frame(double interpolation)
        {
            this.Interpolation = interpolation;
        }

        public override string ToString() => this.Interpolation.ToString();

        public override int GetHashCode() => this.Interpolation.GetHashCode();

        public override bool Equals(object obj) => obj is Frame other && this == other;

        public bool Equals(Frame other) => this == other;

        public static bool operator ==(Frame left, Frame right) => left.Interpolation == right.Interpolation;

        public static bool operator !=(Frame left, Frame right) => left.Interpolation != right.Interpolation;
    } 
}