using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;
using System;

namespace Tests.KeyTest
{
    class KeyTester : Test
    {
        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            window.OnKeyPressed += Window_OnKeyPressed;
        }

        private void Window_OnKeyPressed(KeyEvent e)
        {
            if(!e.Repeated)
            {
                Console.WriteLine($"Key {e.Key}, Code {e.KeyCode}");
            }            
        }

        public override void Draw(GraphicsDevice graphics) { }       

        public override void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            window.OnKeyPressed -= Window_OnKeyPressed;
        }

    }
}
