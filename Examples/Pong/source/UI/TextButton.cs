using System;
using System.Numerics;

using JankWorks.Graphics;
using JankWorks.Game;

namespace Pong.UI
{
    class TextButton : IControl
    {
        public int ZOrder { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 Position { get; set; }

        public RGBA Colour { get; set; }

        public RGBA HoverColour { get; set; }

        public Action OnClick { get; set; }

        public string Text { get; set; }
       
        private Bounds bounds;
        private RGBA renderColour;
        private bool hover;
        private float blendAmount;

        public TextButton()
        {
            this.hover = false;
            this.blendAmount = 0;
        }

        public Bounds GetBounds() => this.bounds;

        public void Enter() => this.hover = true;

        public void Leave() => this.hover = false;

        public void Click() => this.OnClick?.Invoke();

        public void Update(GameTime time)
        {
            if (this.hover)
            {
                this.blendAmount = MathF.Min(1f, this.blendAmount + 0.10f);
            }
            else
            {
                this.blendAmount = MathF.Max(0, this.blendAmount - 0.10f);
            }

            this.renderColour = JankWorks.Graphics.Colour.Blend(this.Colour, this.HoverColour, this.blendAmount);
        }

        public void Draw(TextRenderer textRenderer, ShapeRenderer shapeRenderer)
        {
            this.bounds = textRenderer.Draw(this.Text, this.Position, this.Origin, 0f, this.renderColour);
        }
    }
}