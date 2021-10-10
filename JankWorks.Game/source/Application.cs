using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using JankWorks.Core;
using JankWorks.Drivers;
using JankWorks.Game.Assets;
using JankWorks.Game.Configuration;
using JankWorks.Game.Local;
using JankWorks.Game.Hosting;

namespace JankWorks.Game
{
    public abstract class Application : Disposable
    {
        protected readonly DirectoryInfo DataFolder;
        protected readonly DirectoryInfo SaveFolder;

        public IReadOnlyDictionary<string, Func<Scene>> Scenes { get; private set; }
        public DriverConfiguration Drivers { get; private set; }

        public Settings Settings { get; private set; }

        protected Application()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), this.Name);
            this.DataFolder = new DirectoryInfo(path);
            if(!this.DataFolder.Exists)
            {
                this.DataFolder.Create();
            }

            path = Path.Combine(path, "saves");
            this.SaveFolder = new DirectoryInfo(path);

            if(!this.SaveFolder.Exists)
            {
                this.SaveFolder.Create();
            }

            this.Scenes = this.RegisterScenes();

            this.Drivers = this.RegisterDrivers();

            this.Settings = this.GetApplicationSettings();
        }

        public abstract string Name { get; }

        protected abstract DriverConfiguration RegisterDrivers();

        protected abstract IReadOnlyDictionary<string, Func<Scene>> RegisterScenes();

        public abstract AssetManager RegisterAssetManager();

        public abstract LoadingScreen RegisterLoadingScreen();


        public virtual ApplicationParameters ApplicationParameters => ApplicationParameters.Default;
        public virtual ClientParameters ClientParameters => ClientParameters.Default;
        public virtual HostParameters HostParameters => HostParameters.Default;

        public virtual Settings GetApplicationSettings()
        {
            var path = Path.Combine(this.DataFolder.FullName, "app.ini");
            var settings = new Settings(new IniSettingsSource(path, Encoding.UTF8));
            settings.Load();
            return settings;
        }
        public virtual Settings GetClientSettings()
        {
            var path = Path.Combine(this.DataFolder.FullName, "client.ini");
            var settings = new Settings(new IniSettingsSource(path, Encoding.UTF8));
            settings.Load();
            return settings;
        }
        public virtual Settings GetHostSettings()
        {
            var path = Path.Combine(this.DataFolder.FullName, "host.ini");
            var settings = new Settings(new IniSettingsSource(path, Encoding.UTF8));
            settings.Load();
            return settings;
        }


        public static void Run<App>(string scene, object initstate) where App : Application, new()
        {
            using var app = new App();
            var host = new OfflineHost(app);
            host.Start();
            Application.Run(app, host, scene, initstate);
        }

        public static void Run<App>(Host host, string scene, object initstate) where App : Application, new()
        {
            using var app = new App();
            Application.Run(app, host, scene, initstate);
        }

        public static void Run(Application app, Host host, string scene, object initstate)
        {
            Application.Run(app, host, ClientConfgiuration.Default, scene, initstate);
        }

        public static void Run(Application app, Host host, ClientConfgiuration config, string scene, object initstate)
        {
            using var client = new Client(app, config, host);            
            client.Run(scene, host, initstate);
        }
    }

    public struct ApplicationParameters
    {
        public bool ParseArguments { get; set; }
        public ArgumentOptions ParseOptions { get; set; }
        public bool EnableConsole { get; set; }
        public static ApplicationParameters Default => new ApplicationParameters()
        {
            ParseArguments = false,
            ParseOptions = ArgumentOptions.Parse,
            EnableConsole = false
        };

        [Flags]
        public enum ArgumentOptions
        {           
            Parse = 1,

            OverrideAppSettings = 2,

            OverrideClientSettings = 4,

            OverrideHostSettings = 8
        }
    }
}
