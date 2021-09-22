using System;
using System.Numerics;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

#pragma warning disable CS8618

namespace Tests.RendererTest
{
    class RendererTest : Test
    {
        private OrthoCamera camera;
        private SpriteRenderer spriteRenderer;
        private Texture2D smiley;

        private Vector2 mousePos;
        private Vector2i viewportSize;
        private Bounds cursorTextureMap;

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.viewportSize = graphics.Viewport.Size;
            this.cursorTextureMap = Bounds.One;

            this.camera = new OrthoCamera(this.viewportSize);
            this.spriteRenderer = graphics.CreateSpriteRenderer(this.camera);
            this.smiley = graphics.CreateTexture2D(GetEmbeddedStream("RendererTest.smiley.png"), ImageFormat.PNG);

            graphics.DefaultDrawState = new DrawState()
            {
                Blend = BlendMode.Alpha,
                DepthTest = DepthTestMode.None
            };

            window.OnMouseMoved += this.MouseMoved;
        }

        private void MouseMoved(Vector2 screenPos)
        {
            var translated = this.camera.TranslateScreenCoordinate(this.viewportSize, screenPos);
            var pos = new Vector2(translated.X, translated.Y);

            if(this.mousePos.X < pos.X)
            {
                this.cursorTextureMap = new Bounds(1, 0, 1, 0);
            }
            else if (this.mousePos.X > pos.X)
            {
                this.cursorTextureMap = Bounds.One;
            }

            this.mousePos = pos;
        }

        public override void Draw(GraphicsDevice graphics)
        {
            this.spriteRenderer.BeginDraw();

            this.spriteRenderer.Draw(this.smiley, (Vector2)this.viewportSize / 2, new Vector2(200), new Vector2(0.5f), 0, Colour.Red, Bounds.One);
            this.spriteRenderer.Draw(this.smiley, this.mousePos, new Vector2(100), new Vector2(0.5f), 0, Colour.White, this.cursorTextureMap);

            this.spriteRenderer.EndDraw(graphics);
        }

        public override void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            window.OnMouseMoved -= this.MouseMoved;

            this.spriteRenderer.Dispose();
            this.smiley.Dispose();
        }
    }
}