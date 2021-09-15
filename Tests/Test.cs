using System;
using System.IO;

using JankWorks.Core;
using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Tests
{
    abstract class Test
    {
        protected Stream GetEmbeddedStream(string name)
        {
            var resource = $"Tests.{name}";
            return typeof(Test).Assembly.GetManifestResourceStream(resource) ?? throw new ArgumentException();
        }
            
        public abstract void Setup(GraphicsDevice graphics, AudioDevice audio, Window window);

        public abstract void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window);

        public abstract void Draw(GraphicsDevice graphics);
    }
}
