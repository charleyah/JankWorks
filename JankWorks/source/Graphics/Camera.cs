using System;
using System.Numerics;

namespace JankWorks.Graphics
{
    public abstract class Camera
    {
        public abstract Matrix4x4 GetProjection();

        public abstract Matrix4x4 GetView();

        public abstract Vector3 TranslateScreenCoordinate(Vector2 pos);
    }

    public sealed class OrthoCamera : Camera
    {
        public Vector2 Position 
        {
            get => this.position; 
            set
            {
                this.position = value;
                this.UpdateView();
            }
        }

        private Vector2 position;
        private readonly Vector2 size;
        private readonly Vector2 viewport;
                
        private Matrix4x4 view;
        private readonly Matrix4x4 projection;

        public OrthoCamera(Vector2 viewport) : this(viewport, viewport) { }

        public OrthoCamera(Vector2 viewport, Vector2 size) : this(viewport, size, -1, 1) { }

        public OrthoCamera(Vector2 viewport, Vector2 size, float nearZ, float farZ)
        {
            this.projection = Matrix4x4.CreateOrthographicOffCenter(0, size.X, size.Y, 0, nearZ, farZ);
            this.size = size;
            this.viewport = viewport;
            this.Position = Vector2.Zero;
        }

        private void UpdateView()
        {
            this.view = Matrix4x4.CreateTranslation(new Vector3(this.position, 0));
        }

        public override Matrix4x4 GetProjection() => this.projection;

        public override Matrix4x4 GetView() => this.view;
        


        public override Vector3 TranslateScreenCoordinate(Vector2 pos)
        {
            Matrix4x4 mat = Matrix4x4.Identity;

            mat = mat * Matrix4x4.CreateScale(new Vector3(this.size / this.viewport, 0));
            mat = mat * Matrix4x4.CreateTranslation(-new Vector3(this.position, 0));

            return new Vector3(Vector2.Transform(pos, mat), 0);
        }
    }
}