using System.Numerics;

using JankWorks.Graphics;

using JankWorks.Game;
using JankWorks.Game.Hosting;
using JankWorks.Game.Assets;
using JankWorks.Game.Diagnostics;
using JankWorks.Game.Local;

using Pong.UI;

namespace Pong.MainMenu
{
    sealed class MenuScene : Scene
    {
        private PerformanceInfo perfinfo;
        private UIContainer container;
        private OrthoCamera camera;

        private Fader fader;
        private Text title;
        private TextButton newgame;
        private TextButton exit;

        public override void ClientInitialise(Client client)
        {            
            this.camera = new OrthoCamera(client.Viewport.Size);

            this.container = new UIContainer(78, 170);
            this.container.Camera = this.camera;
            this.SetupUI(client);
            this.RegisterClientObject(this.container);


            this.perfinfo = new PerformanceInfo(client, null, new Asset("embedded", "ibm-plex-mono.regular.ttf"), 14);
            this.perfinfo.MaxDisplayCounters = 8;
            this.RegisterClientObject(this.perfinfo);

            base.ClientInitialise(client);
        }

        private void SetupUI(Client client)
        {
            var viewportSize = (Vector2)client.Viewport.Size;

            Vector2 center = viewportSize / 2;

            center.X -= 200;

            this.fader = new Fader()
            {
                Colour = Colours.Background,
                Position = Vector2.Zero,
                Size = viewportSize,
                ZOrder = 1
            };

            this.title = new Text()
            {
                Colour = Colours.Forground,
                IsTitle = true,
                Value = "PONG",
                Position = new Vector2(center.X, center.Y - 100),
                Origin = new Vector2(0f, 1f)
            };

            var btnPosition = center;
            const float btnSpacing = 100f;

            this.newgame = new TextButton()
            {
                Colour = Colours.Forground,
                HoverColour = Colours.Hover,
                Text = "New Game",
                Position = btnPosition,
                Origin = new Vector2(0f, 0.5f),
                OnClick = () => 
                { 
                    this.fader.FadeOut(() => client.ChangeScene(1, new OfflineHost(this.Application))); 
                    this.container.Interactive = false; 
                }
            };

            btnPosition.Y += btnSpacing;

            this.exit = new TextButton()
            {
                Colour = Colours.Forground,
                HoverColour = Colours.Hover,
                Text = "Exit",
                Position = btnPosition,
                Origin = new Vector2(0f, 0.5f),
                OnClick = () =>
                {
                    this.fader.FadeOut(client.Close);
                    this.container.Interactive = false;
                }
            };

            this.container.AddElement(this.fader);
            this.container.AddElement(this.title);
            this.container.AddControl(this.newgame);
            this.container.AddControl(this.exit);            
        }

        public override void InitialiseGraphicsResources(GraphicsDevice device)
        {
            device.ClearColour = Colours.Background;
            base.InitialiseGraphicsResources(device);
        }

        public override void ClientInitialised(object state)
        {
            this.container.Activate();
            base.ClientInitialised(state);
        }

        public override void Render(Surface surface, Frame frame)
        {
            surface.Clear();

            base.Render(surface, frame);
        }
    }
}