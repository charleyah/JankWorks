using System;
using System.Numerics;

using JankWorks.Graphics;

namespace Pong.UI
{
    interface IElement
    {
        int ZOrder { get; set; }

        Vector2 Origin { get; set; }

        Vector2 Position { get; set; }
       
        RGBA Colour { get; set; }

        void Update(TimeSpan delta);

        void Draw(TextRenderer textRenderer, ShapeRenderer shapeRenderer);        
    }
}