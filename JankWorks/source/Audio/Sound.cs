using System;

using JankWorks.Core;

namespace JankWorks.Audio
{
    public abstract class Sound : Disposable
    {
        public virtual short Channels { get; protected set; }

        public virtual short Samples { get; protected set; }

        public abstract void Write(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency);

        public abstract Emitter CreateEmitter();
    }
}