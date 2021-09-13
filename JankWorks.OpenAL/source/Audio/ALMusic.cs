using System;
using System.IO;
using JankWorks.Audio;

namespace JankWorks.Drivers.OpenAL.Audio
{
    sealed class ALMusic : Music
    {
        public ALBuffer buffer;

        private ALEmitter emitter;

        public ALMusic()
        {
            this.buffer.Create();
            this.emitter = new ALEmitter(this);            
        }

        public override float Volume { get => this.emitter.Volume; set => this.emitter.Volume = value; }

        public override bool Loop { get => this.emitter.Loop; set => this.emitter.Loop = value; }

        public override PlayState State => this.emitter.State;

        public override void Pause() => this.emitter.Pause();

        public override void Play() => this.emitter.Play();

        public override void Resume() => this.emitter.Resume();

        public override void Stop() => this.emitter.Stop();

        public override void Write(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency)
        {
            this.buffer.Write(pcm, channels, samples, frequency);
            this.Channels = channels;
            this.Samples = samples;
        }

        public override void ChangeTrack(Stream stream, AudioFormat format)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool finalising)
        {            
            this.emitter.Dispose();
            this.buffer.Dispose();
            base.Dispose(finalising);
        }
    }
}