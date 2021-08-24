using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using JankWorks.Drivers.Graphics;
using JankWorks.Graphics;

namespace JankWorks.Drivers.DotNet.Graphics
{
    sealed class DotNetImage : JankWorks.Graphics.Image
    {
        public override Vector2i Size => new Vector2i(this.bitmap.Width, this.bitmap.Height);

        private Bitmap bitmap;

        public DotNetImage(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public override void CopyTo(Texture2D texture)
        {
            var size = this.Size;
            var data = this.bitmap.LockBits(new System.Drawing.Rectangle(0, 0, size.X, size.Y), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                ReadOnlySpan<ARGB32> pixels;

                unsafe
                {
                    pixels = new ReadOnlySpan<ARGB32>(data.Scan0.ToPointer(), size.X * size.Y);
                }

                texture.SetPixels(size, pixels);
            }
            finally
            {
                this.bitmap.UnlockBits(data);
            }
        }

        public override void WriteFrom(Texture2D texture)
        {
            var size = this.Size;

            if(size != texture.Size)
            {
                throw new ArgumentException("texture is not same size as image");
            }

            var data = this.bitmap.LockBits(new System.Drawing.Rectangle(0, 0, size.X, size.Y), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    var pixels = new Span<ARGB32>(data.Scan0.ToPointer(), size.X * size.Y);
                    texture.CopyTo(pixels);
                }                                
            }
            finally
            {
                this.bitmap.UnlockBits(data);
            }

            if (DriverConfiguration.Drivers.graphicsApi.GraphicsApi == GraphicsApi.OpenGL)
            {
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
        }

        public override void Save(Stream stream, JankWorks.Graphics.ImageFormat format) => this.bitmap.Save(stream, format.GetDotNetImageFormat());
        
        protected override void Dispose(bool finalising)
        {
            this.bitmap.Dispose();
            base.Dispose(finalising);
        }
    }
}