using System.Numerics;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Tests.ShapesTest
{
    sealed class ShapesTest : Test
    {
        private OrthoCamera camera;
        private ShapeRenderer renderer;

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.camera = new OrthoCamera(graphics.Viewport.Size);
            graphics.ClearColour = new RGBA(25, 25, 25);
            this.renderer = graphics.CreateShapeRenderer(this.camera);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            this.renderer.BeginDraw();
            this.renderer.DrawRectangle(new Vector2(100), new Vector2(200), Colour.White);
            this.renderer.DrawTriangle(new Vector2(100), new Vector2(600), new Vector2(0), 180f, Colour.White);
            this.renderer.EndDraw(graphics);
        }

        public override void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.renderer.Dispose();
            graphics.ClearColour = default;
        }

    }
}
