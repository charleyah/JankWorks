using System;
using System.IO;

using JankWorks.Core;
using JankWorks.Graphics;

namespace Tests
{
    abstract class Test : Disposable
    {
        protected Stream GetEmbeddedStream(string name)
        {
            var resource = $"Tests.{name}";
            return typeof(Test).Assembly.GetManifestResourceStream(resource) ?? throw new ArgumentException();
        }
            
        public abstract void Setup(GraphicsDevice device);
        public abstract void Draw(GraphicsDevice device);
    }
}
