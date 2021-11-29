using System;
using System.Numerics;

namespace JankWorks.Graphics
{
    public abstract class Camera
    {
        public abstract Matrix4x4 GetProjection();

        public abstract Matrix4x4 GetView();
    }

    public class OrthoCamera : Camera
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

        public OrthoCamera(Surface surface) : this(surface.Viewport.Size) { }

        public OrthoCamera(Vector2i viewport) : this((Vector2)viewport, (Vector2)viewport) { }

        public OrthoCamera(Vector2i viewport, Vector2 size) : this((Vector2)viewport, size) { }

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
        
        public Vector2 TranslateScreenCoordinate(Vector2 pos)
        {
            Matrix4x4 mat = Matrix4x4.Identity;
            mat = mat * Matrix4x4.CreateScale(new Vector3(this.size / this.viewport, 0));
            mat = mat * Matrix4x4.CreateTranslation(-new Vector3(this.position, 0));

            return Vector2.Transform(pos, mat);
        }
    }

    public class PerspectiveCamera : Camera
    {
        public float VerticalFieldOfView
        {
            get => this.fov;
            set
            {
                this.fov = value;
                this.UpdateProjection();
            }
        }

        public Vector3 Position { get; set; }

        public Vector3 Target { get; set; }

        public Vector3 Up { get; set; }

        private float fov;
        private Matrix4x4 projection;

        private readonly Vector2 size;

        public PerspectiveCamera(Surface surface)
        {
            var size = (Vector2)surface.Viewport.Size;
            this.VerticalFieldOfView = 45;
            this.size = size;

            this.Position = Vector3.Zero;
            this.Up = Vector3.UnitY;
            this.Target = Vector3.Zero;

            this.UpdateProjection();
        }

        private void UpdateProjection()
        {
            this.projection = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 180f * this.VerticalFieldOfView, size.X / size.Y, 0.1f, 100.0f);
        }

        public override Matrix4x4 GetProjection() => this.projection;

        public override Matrix4x4 GetView() => Matrix4x4.CreateLookAt(this.Position, this.Target, this.Up);
    }
}