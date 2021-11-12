using System;


using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

using JankWorks.Game.Assets;

using JankWorks.Game.Hosting.Messaging;

namespace JankWorks.Game
{
    public interface IResource
    {
        void InitialiseResources(AssetManager assets);

        void DisposeResources();
    }

    public interface IInputListener
    {
        void SubscribeInputs(IInputManager inputManager);

        void UnsubscribeInputs(IInputManager inputManager);
    }

    public interface ITickable
    {
        void Tick(ulong tick, TimeSpan delta);
    }

    public interface IAsyncTickable
    {
        void BeginTick(ulong tick, TimeSpan delta) => this.BeginTick(tick, delta, null);

        void BeginTick(ulong tick, TimeSpan delta, Action<IAsyncTickable> callback);

        void EndTick(ulong tick, TimeSpan delta);
    }

    public interface IUpdatable
    {
        void Update(TimeSpan delta);
    }

    public interface IAsyncUpdatable
    {
        void BeginUpdate(TimeSpan delta) => this.BeginUpdate(delta, null);

        void BeginUpdate(TimeSpan delta, Action<IAsyncUpdatable> callback);

        void EndUpdate(TimeSpan delta);
    }

    public interface IDispatchable
    {
        void InitialiseChannels(Dispatcher dispatcher);

        void UpSynchronise();

        void DownSynchronise();
    }

    public interface ISoundResource
    {
        void InitialiseSoundResources(AudioDevice device, AssetManager assets);

        void DisposeSoundResources(AudioDevice device);
    }

    public interface IGraphicsResource
    {
        void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets);

        void DisposeGraphicsResources(GraphicsDevice device);
    }

    public interface IDrawable : IGraphicsResource
    {
        void Draw(Surface surface);
    }

    public interface IRenderable : IGraphicsResource
    {
        void Render(Surface surface, Frame frame);
    }

    public interface IAsyncRenderable : IGraphicsResource
    {
        void BeginRender(Surface surface, Frame frame) => this.BeginRender(surface, frame, null);

        void BeginRender(Surface surface, Frame frame, Action<IAsyncRenderable> callback);

        void EndRender(Surface surface, Frame frame);
    }

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