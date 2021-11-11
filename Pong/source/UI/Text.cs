using System;
using System.Numerics;

using JankWorks.Graphics;

namespace Pong.UI
{
    class Text : IElement
    {
        public int ZOrder { get; set; }

        public bool IsTitle { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 Position { get; set; }

        public RGBA Colour { get; set; }
    
        public string Value { get; set; }

        public void Update(TimeSpan delta) { }

        public void Draw(TextRenderer textRenderer, ShapeRenderer shapeRenderer)
        {
            textRenderer.Draw(this.Value, this.Position, this.Origin, 0f, this.Colour);
        }        
    }
}