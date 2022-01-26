using System;

using JankWorks.Game;
using JankWorks.Game.Local;
using JankWorks.Graphics;

namespace ImagePreviewer
{
    sealed class PreviewerScene : Scene
    {
        public override void ClientInitialise(Client client)
        {
            this.RegisterClientObject(new ImageRenderer());
            base.ClientInitialise(client);
        }

        public override void Render(Surface surface, GameTime time)
        {
            surface.Clear();
            base.Render(surface, time);
        }
    }
}
