using System;
using System.IO;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Tests.MusicTest
{
    class MusicTest : Test
    {
        private Music musicPlayer;

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.musicPlayer = audio.LoadMusic(File.OpenRead("bangin.wav"), AudioFormat.Wav);
            this.musicPlayer.Play();
        }


        public override void Draw(GraphicsDevice graphics)
        {
            this.musicPlayer.Stream();
        }


        public override void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.musicPlayer.Dispose();
        }
    }
}
