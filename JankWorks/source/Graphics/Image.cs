using System;
using System.IO;

using JankWorks.Core;
using JankWorks.Drivers;


namespace JankWorks.Graphics
{
    public abstract class Image : Disposable
    {
        public abstract Vector2i Size { get; }

        public abstract void CopyTo(Texture2D texture);

        public abstract void WriteFrom(Texture2D texture);

        public abstract void Save(Stream stream, ImageFormat format);

        public static Image Load(Stream stream, ImageFormat format) => DriverConfiguration.Drivers.imageApi.LoadFromStream(stream, format);

        public static Image Create(Vector2i size, ImageFormat format) => DriverConfiguration.Drivers.imageApi.Create(size, format);

        public static Texture2D LoadTexture(GraphicsDevice device, Stream stream, ImageFormat format, TextureFilter filter = TextureFilter.Linear, TextureWrap warp = TextureWrap.Clamp)
        {
            using var image = Image.Load(stream, format);

            var texture = device.CreateTexture2D(image.Size, PixelFormat.RGBA);
            texture.Filter = filter;
            texture.Wrap = warp;

            image.CopyTo(texture);

            return texture;
        }
    }


    public enum ImageFormat
    {
        BMP,
        JPG,
        PNG
    }
}
