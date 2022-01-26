using System;
using JankWorks.Drivers;
using JankWorks.Interface;

using JankWorks.Game;
using JankWorks.Game.Assets;
using JankWorks.Game.Local;

namespace ImagePreviewer
{
    sealed class App : Application
    {
        public override string Name => "ImagePreviewer";

        public override AssetManager RegisterAssetManager() => new AssetManager();

        protected override DriverConfiguration RegisterDrivers() => DriverConfiguration.Initialise
        (
            "JankWorks.Glfw",
            "JankWorks.DotNet",
            "JankWorks.FreeType",
            "JankWorks.OpenGL",
            "JankWorks.OpenAL"
        );
        
        protected override Func<Scene>[] RegisterScenes()
        {
            return new Func<Scene>[]
            {
                () => new PreviewerScene()
            };
        }

        public override ClientConfgiuration DefaultClientConfiguration
        {
            get
            {
                var conf = base.DefaultClientConfiguration;
                conf.DisplayMode = new DisplayMode(1024, 768, 32, conf.DisplayMode.RefreshRate);
                conf.Vsync = false;
                conf.UpdateRate = conf.DisplayMode.RefreshRate;
                conf.WindowStyle = WindowStyle.Windowed;
                return conf;
            }
        }

        static void Main(string[] args) => Application.RunWithoutHost<App>(0);
    }
}
