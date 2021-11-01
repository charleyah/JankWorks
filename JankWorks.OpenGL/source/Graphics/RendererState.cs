using System;
using System.Numerics;

using JankWorks.Graphics;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    internal struct RendererState
    {
        public Matrix4x4 projection;
        public Matrix4x4 view;
        public DrawState? drawState;
        public bool drawing;

        public void Setup()
        {
            this.projection = Matrix4x4.Identity;
            this.view = Matrix4x4.Identity;
            this.drawState = null;
            this.drawing = false;
        }

        public void BeginDraw(Camera camera, DrawState? state)
        {
            if (this.drawing) { throw new InvalidOperationException(); }

            this.projection = camera.GetProjection();
            this.view = camera.GetView();
            this.drawState = state;
            this.drawing = true;
        }

        public bool CanReDraw(Camera camera)
        {
            if (this.drawing) { throw new InvalidOperationException(); }

            return this.projection.Equals(camera.GetProjection()) && this.view.Equals(camera.GetView());
        }

        public void EndDraw()
        {
            if (!this.drawing) { throw new InvalidOperationException(); }

            this.drawing = false;
        }
    }
}