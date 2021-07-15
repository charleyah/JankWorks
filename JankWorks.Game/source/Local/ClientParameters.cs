using System;

using JankWorks.Graphics;

namespace JankWorks.Game.Local
{
    public struct ClientParameters
    {
        public float UpdateRate { get; set; }

        public RGBA ClearColour { get; set; }

        public bool ShowCursor { get; set; }

        public static ClientParameters Default => new ClientParameters()
        {
            UpdateRate = 30,
            ClearColour = Colour.Black,
            ShowCursor = true
        };
    }
}
