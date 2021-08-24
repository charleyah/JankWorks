using System;

namespace JankWorks.Drivers.DotNet.Graphics
{
    internal static class Extensions
    {
        public static System.Drawing.Imaging.ImageFormat GetDotNetImageFormat(this JankWorks.Graphics.ImageFormat format) => format switch
        {
            JankWorks.Graphics.ImageFormat.JPG => System.Drawing.Imaging.ImageFormat.Jpeg,
            JankWorks.Graphics.ImageFormat.BMP => System.Drawing.Imaging.ImageFormat.Bmp,
            JankWorks.Graphics.ImageFormat.PNG => System.Drawing.Imaging.ImageFormat.Png,
            _ => throw new NotImplementedException()
        };      
    }
}