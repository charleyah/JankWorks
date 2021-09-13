using System;

using JankWorks.Audio;

namespace JankWorks.Drivers.OpenAL.Audio
{
    sealed class ALSound : Sound
    {      
        public ALBuffer buffer;

        public ALSound()
        {
            this.buffer.Create();
        }

        public override void Write(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency)
        {
            this.buffer.Write(pcm, channels, samples, frequency);
            this.Channels = channels;
            this.Samples = samples;
        }
       
        protected override void Dispose(bool finalising)
        {
            this.buffer.Dispose();
            base.Dispose(finalising);
        }
    }
}