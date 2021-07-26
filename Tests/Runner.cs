using System;
using System.Collections.Generic;
using System.Linq;

using JankWorks.Drivers;
using JankWorks.Graphics;
using JankWorks.Interface;


#pragma warning disable CS8602, CS8600

namespace Tests
{
    class Runner
    {
        static void Main(string[] args)
        {
            DriverConfiguration.Initialise("JankWorks.Glfw", "JankWorks.OpenGL", "JankWorks.FreeType", "JankWorks.DotNet");

            var windowsettings = WindowSettings.Default;
            windowsettings.VideoMode = new VideoMode(1024, 768, 32, 60);
            windowsettings.VSync = true;
            windowsettings.Style = WindowStyle.Windowed;

            var surfacesettings = new SurfaceSettings()
            {
                ClearColour = Colour.Black,
                Size = windowsettings.VideoMode.Viewport.Size
            };

            using var window = Window.Create(windowsettings);
            using var device = GraphicsDevice.Create(surfacesettings, window);


            var tests = Runner.GetTestClasses().GetEnumerator();
            var testsRemaining = tests.MoveNext();
            Test? test = null;

            if (testsRemaining)
            {
                test = (Test)Activator.CreateInstance(tests.Current);
                test.Setup(device);
            }

            window.OnKeyReleased += (ke) =>
            {
                if (ke.Key == Key.Escape)
                {
                    window.Close();
                }
                else if (ke.Key == Key.Enter)
                {
                    testsRemaining = tests.MoveNext();

                    if (testsRemaining)
                    {
                        test?.Dispose();
                        test = (Test)Activator.CreateInstance(tests.Current);
                        test.Setup(device);
                    }
                }
            };

            window.Show();

            while(window.IsOpen)
            {
                window.ProcessEvents();

                device.Clear();

                test?.Draw(device);

                device.Display();
            }

            window.Hide();
        }

        private static IEnumerable<Type> GetTestClasses() => typeof(Test).Assembly.GetTypes().Where((t) => t.BaseType == typeof(Test));
    }
}