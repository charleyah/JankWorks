using System;

namespace JankWorks.Graphics
{
    public interface IRenderTarget
    {
        void Activate();
        void Deactivate();
        void Render();

        event Action<Rectangle> OnResize;
    }
}
