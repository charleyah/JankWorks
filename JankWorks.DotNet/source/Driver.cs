using System.Drawing;
using System.IO;

using JankWorks.Graphics;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;
using JankWorks.Drivers.DotNet.Graphics;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.DotNet.Driver))]

namespace JankWorks.Drivers.DotNet
{
    public class Driver : IImageDriver
    {
        public JankWorks.Graphics.Image Create(Vector2i size, ImageFormat format) => new DotNetImage(new Bitmap(size.X, size.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb));       

        public JankWorks.Graphics.Image LoadFromStream(Stream stream, ImageFormat format) => new DotNetImage(new Bitmap(stream));        
    }
}