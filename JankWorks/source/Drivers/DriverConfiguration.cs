using System;
using System.IO;
using System.Reflection;

using JankWorks.Drivers.Interface;
using JankWorks.Drivers.Graphics;


namespace JankWorks.Drivers
{
    public sealed class DriverConfiguration
    {
        public readonly IMonitorDriver monitorApi;
        public readonly IWindowDriver windowApi;

        public readonly IImageDriver imageApi;
        public readonly IFontDriver fontApi;
        public readonly IGraphicsDriver graphicsApi;


        static DriverConfiguration()
        {
            DriverConfiguration.Drivers = new DriverConfiguration();
        }

        public static DriverConfiguration Drivers { get; private set; }

        public DriverConfiguration
        (
            IMonitorDriver monitorDriver = null, 
            IWindowDriver windowDriver = null, 
            IImageDriver imageDriver = null,
            IFontDriver fontDriver = null,
            IGraphicsDriver graphicsDriver = null
        )
        {
            this.monitorApi = monitorDriver ?? DriverUnitialisedException.driver;
            this.windowApi = windowDriver ?? DriverUnitialisedException.driver;
            this.imageApi = imageDriver ?? DriverUnitialisedException.driver;
            this.fontApi = fontDriver ?? DriverUnitialisedException.driver;
            this.graphicsApi = graphicsDriver ?? DriverUnitialisedException.driver;
            
        }

        public void PrintDrivers(TextWriter writer)
        {
            PrintDriver(this.monitorApi, writer);
            PrintDriver(this.windowApi, writer);
            PrintDriver(this.imageApi, writer);
            PrintDriver(this.graphicsApi, writer);

            void PrintDriver<T>(T driver, TextWriter writer) where T : IDriver
            {
                writer.Write(typeof(T).Name.PadRight(15));
                writer.Write(" - ");
                writer.WriteLine(driver.Name);
            }
        }

        public static IDisposable Initialise(DriverConfiguration configuration)
        {
            DriverConfiguration.Drivers = configuration;
            return new ScopedConfiguration(configuration);
        }

        public static DriverConfiguration Initialise(params string[] assemblyNames)
        {
            foreach(var asmName in assemblyNames)
            {
                AppDomain.CurrentDomain.Load(asmName);
            }

            IMonitorDriver monitorDriver = null;
            IWindowDriver windowDriver = null;
            IImageDriver imageDriver = null;
            IFontDriver fontDriver = null;
            IGraphicsDriver graphicsDriver = null;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var jwdriver = assembly.GetCustomAttribute<JankWorksDriver>();

                if(jwdriver != null)
                {
                    var driverInstance = Activator.CreateInstance(jwdriver.DriverType);

                    SetDriverApi(ref monitorDriver, driverInstance);
                    SetDriverApi(ref windowDriver, driverInstance);
                    SetDriverApi(ref imageDriver, driverInstance);
                    SetDriverApi(ref fontDriver, driverInstance);
                    SetDriverApi(ref graphicsDriver, driverInstance);
                }
            }

            var config = new DriverConfiguration
            (
                monitorDriver,
                windowDriver,
                imageDriver,
                fontDriver,
                graphicsDriver
            );
            DriverConfiguration.Drivers = config;
            return config;

            void SetDriverApi<T>(ref T api, object driver)
            {
                if (api == null && driver is T driverApi) { api = driverApi; }
            }
        }

        public static void Shutdown()
        {
            var drivers = DriverConfiguration.Drivers;
            DriverConfiguration.Drivers = new DriverConfiguration();
            DriverConfiguration.Shutdown(drivers);
        }

        private static void Shutdown(DriverConfiguration drivers)
        {
            MaybeDispose(drivers.monitorApi);
            MaybeDispose(drivers.windowApi);
            MaybeDispose(drivers.graphicsApi);            

            void MaybeDispose(IDriver driver)
            {
                if (driver is IDisposable disposable) { disposable.Dispose(); }
            }
        }

        private sealed class ScopedConfiguration : IDisposable
        {
            public readonly DriverConfiguration config;

            public ScopedConfiguration(DriverConfiguration config) { this.config = config; }
            public void Dispose() => DriverConfiguration.Shutdown(config);
        }
    }
}
