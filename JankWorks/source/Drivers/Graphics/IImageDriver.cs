using System;
using System.IO;

using JankWorks.Graphics;

namespace JankWorks.Drivers.Graphics
{
    public interface IImageDriver : IDriver
    {
        Image LoadFromStream(Stream stream, ImageFormat format);
    }
}
