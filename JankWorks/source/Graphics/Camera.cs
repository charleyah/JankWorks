using System;
using System.Numerics;

namespace JankWorks.Graphics
{
    public abstract class Camera
    {
        public abstract Matrix4x4 GetProjection();

        public abstract Matrix4x4 GetView();

        public abstract Vector3 TranslateScreenCoordinate(Vector2i viewport, Vector2 pos);
    }

    public sealed class OrthoCamera : Camera
    {
        public Vector2 Position { get; set; }

        public Vector2 Origin 
        {
            get => (this.originOffset == Vector2.Zero) ? Vector2.Zero : this.size / this.originOffset;
            set => this.originOffset = this.size * value; 
        }

        private Vector2 TopLeft => this.Position - this.originOffset;

        private Vector2 originOffset;

        private readonly Vector2 size;
        private readonly Matrix4x4 projection;

        public OrthoCamera(Vector2i size) : this((Vector2)size, -1f, 1f) { }

        public OrthoCamera(Vector2 size) : this(size, -1f, 1f) { }

        public OrthoCamera(Vector2i size, float nearZ, float farZ) : this((Vector2)size, nearZ, farZ) { }

        public OrthoCamera(Vector2 size, float nearZ, float farZ)
        {
            this.projection = Matrix4x4.CreateOrthographicOffCenter(0, size.X, size.Y, 0, nearZ, farZ);
            this.size = size;

            this.Origin = Vector2.Zero;
            this.Position = Vector2.Zero;
        }

        public override Matrix4x4 GetProjection() => this.projection;

        public override Matrix4x4 GetView() => Matrix4x4.CreateTranslation(new Vector3(this.TopLeft, 0));
       
        public override Vector3 TranslateScreenCoordinate(Vector2i viewport, Vector2 pos)
        {                        
            var scale = (Vector2)viewport / this.size;
            var translated = this.TopLeft + (pos * scale);
            return new Vector3(translated, 0);
        }
    }
}