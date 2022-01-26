using System;
using System.Numerics;

using JankWorks.Graphics;
using JankWorks.Game;

namespace Pong.UI
{
    class Fader : IElement
    {
        public int ZOrder { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public RGBA Colour { get; set; }

        public byte FateRate { get; set; }

        private byte renderedAlpha;
        private FadeState state;

        private Action callback;

        public Fader()
        {
            this.FateRate = 5;
        }

        public void FadeIn(Action callback)
        {
            this.callback = callback;
            this.state = FadeState.In;
            this.renderedAlpha = byte.MaxValue;
        }

        public void FadeOut(Action callback)
        {
            this.callback = callback;
            this.state = FadeState.Out;
            this.renderedAlpha = byte.MinValue;
        }

        public void Update(GameTime time)
        {
            switch(this.state)
            {
                case FadeState.In:
                    this.renderedAlpha = (byte)Math.Max(this.renderedAlpha - this.FateRate, byte.MinValue);

                    if (this.renderedAlpha == byte.MinValue)
                    {
                        this.state = FadeState.None;
                        this.callback?.Invoke();
                        this.callback = null;
                    }

                    break;

                case FadeState.Out:
                    this.renderedAlpha = (byte)Math.Min(this.renderedAlpha + this.FateRate, byte.MaxValue);

                    if(this.renderedAlpha == byte.MaxValue)
                    {
                        this.state = FadeState.None;
                        this.callback?.Invoke();
                        this.callback = null;
                    }

                    break;
            }
        }

        public void Draw(TextRenderer textRenderer, ShapeRenderer shapeRenderer)
        {
            if(this.renderedAlpha > 0)
            {
                var colour = this.Colour;
                colour.A = this.renderedAlpha;

                shapeRenderer.DrawRectangle(this.Size, this.Position, colour);
            }
        }

        private enum FadeState
        {
            None,
            In,
            Out
        }
    }
}