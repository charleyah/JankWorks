
using JankWorks.Graphics;

namespace JankWorks.Game.Local
{
    public struct ClientParameters
    {
        public RGBA ClearColour { get; set; }

        public bool ShowCursor { get; set; }

        public static ClientParameters Default => new ClientParameters()
        {
            ClearColour = Colour.Black,
            ShowCursor = true
        };
    }
}