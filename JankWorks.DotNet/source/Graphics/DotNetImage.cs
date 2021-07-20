using System;
using System.Drawing;
using System.Drawing.Imaging;

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

            if (DriverConfiguration.Drivers.graphicsApi.GraphicsApi == GraphicsApi.OpenGL)
            {
                this.bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
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

        protected override void Dispose(bool finalising)
        {
            this.bitmap.Dispose();
            base.Dispose(finalising);
        }
    }
}
