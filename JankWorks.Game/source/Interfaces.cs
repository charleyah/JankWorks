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

    public interface IParallelTickable
    {
        void ForkTick(ulong tick, TimeSpan delta) => this.ForkTick(tick, delta, null);

        void ForkTick(ulong tick, TimeSpan delta, Action<IParallelTickable> callback);

        void JoinTick(ulong tick, TimeSpan delta);
    }

    public interface IUpdatable
    {
        void Update(TimeSpan delta);
    }

    public interface IParallelUpdatable
    {
        void ForkUpdate(TimeSpan delta) => this.ForkUpdate(delta, null);

        void ForkUpdate(TimeSpan delta, Action<IParallelUpdatable> callback);

        void JoinUpdate(TimeSpan delta);
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

    public interface IParallelRenderable : IGraphicsResource
    {
        void ForkRender(Surface surface, Frame frame) => this.ForkRender(surface, frame, null);

        void ForkRender(Surface surface, Frame frame, Action<IParallelRenderable> callback);

        void JoinRender(Surface surface, Frame frame);
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