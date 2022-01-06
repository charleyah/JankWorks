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

        public override void Write(ReadOnlySpan<byte> pcm, short channels, short sampleSize, int frequency)
        {
            this.buffer.Write(pcm, channels, sampleSize, frequency);
            this.Channels = channels;
            this.Samples = sampleSize;
        }
       
        protected override void Dispose(bool disposing)
        {
            this.buffer.Dispose();
            base.Dispose(disposing);
        }
    }
}