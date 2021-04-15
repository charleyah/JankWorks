using System;
using System.Drawing;
using System.IO;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;
using JankWorks.Graphics;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.DotNet.Driver))]

namespace JankWorks.Drivers.DotNet
{
    public class Driver : IImageDriver
    {
        public JankWorks.Graphics.Image LoadFromStream(Stream stream, ImageFormat format)
        {
            return new Graphics.DotNetImage(new Bitmap(stream));
        }
    }
}
