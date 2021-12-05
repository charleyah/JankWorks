using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


using JankWorks.Graphics;
using JankWorks.Game;
using JankWorks.Game.Assets;

namespace ImagePreviewer
{
    sealed class ImageRenderer : IUpdatable, IRenderable
    {
        private SpriteRenderer renderer;
        private Texture2D texture;

        private bool processingImage;

        public ImageRenderer()
        {
            this.processingImage = false;
        }

        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets)
        {
            this.renderer = device.CreateSpriteRenderer(new OrthoCamera(device));
            this.texture = device.CreateTexture2D(new Vector2i(1000), PixelFormat.RGBA);
        }

        public void DisposeGraphicsResources(GraphicsDevice device)
        {
            this.texture.Dispose();
            this.renderer.Dispose();
        }

        public async void Update(TimeSpan delta)
        {
            if(!this.processingImage)
            {
                this.processingImage = true;
                Console.WriteLine("Enter a url to an image to display...");
                var loc = await Task.Run(Console.ReadLine);

                try
                {
                    var uri = new Uri(loc);
                    Console.WriteLine("...");

                    var image = await GetImage(uri);

                    image.CopyTo(texture);
                    this.renderer.Clear();
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error getting image \n { e }");
                    Console.WriteLine();               
                }
                finally
                {
                    this.processingImage = false;
                }                
            }
        }

        private async Task<Image> GetImage(Uri url)
        {
            using var client = new HttpClient();

            using var response = await client.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            var mediaType = response.Content.Headers.ContentType.MediaType.Trim().ToLower();

            ImageFormat format = mediaType switch
            {
                "image/jpeg" => ImageFormat.JPG,
                "image/png" => ImageFormat.PNG,
                "image/bmp" => ImageFormat.BMP,
                _ => throw new Exception($"unsupported image format {mediaType}")
            };

            return await Task.Run(() => Image.Load(response.Content.ReadAsStream(), format));
        }

        public void Render(Surface surface, Frame frame)
        {
            if(!this.renderer.ReDraw(surface))
            {
                var halfviewport = (Vector2)surface.Viewport.Size / 2;

                var isBiggerThanViewport = this.texture.Size.X > surface.Viewport.Size.X || this.texture.Size.Y > surface.Viewport.Size.Y;

                this.renderer.BeginDraw();
                this.renderer.Draw(this.texture, halfviewport, isBiggerThanViewport ? halfviewport : (Vector2)this.texture.Size, new Vector2(0.5f));
                this.renderer.EndDraw(surface);
            }
        }        
    }
}