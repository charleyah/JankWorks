using System;
using System.Numerics;

namespace JankWorks.Graphics
{
    public abstract class Camera
    {
        public abstract Matrix4x4 GetProjection();

        public abstract Matrix4x4 GetView();

        public abstract Vector3 TranslateScreenCoordinate(Vector2i screen, Vector2 pos);
    }

    public sealed class OrthoCamera : Camera
    {
        public Vector2 Position 
        {
            get => this.position; 
            set
            {
                this.position = value;
                this.view = Matrix4x4.CreateTranslation(new Vector3(value, 0));
            }
        }


        private Vector2 position;
        private readonly Vector2 size;
                
        private Matrix4x4 view;
        private readonly Matrix4x4 projection;

        public OrthoCamera(Vector2i size) : this((Vector2)size, -1f, 1f) { }

        public OrthoCamera(Vector2 size) : this(size, -1f, 1f) { }

        public OrthoCamera(Vector2i size, float nearZ, float farZ) : this((Vector2)size, nearZ, farZ) { }

        public OrthoCamera(Vector2 size, float nearZ, float farZ)
        {
            this.projection = Matrix4x4.Identity * Matrix4x4.CreateOrthographicOffCenter(0, size.X, size.Y, 0, nearZ, farZ);
            this.size = size;
            this.Position = Vector2.Zero;
        }

        public override Matrix4x4 GetProjection() => this.projection;

        public override Matrix4x4 GetView() => this.view;
       
        public override Vector3 TranslateScreenCoordinate(Vector2i screen, Vector2 pos)
        {                        
            var scale = (Vector2)screen / this.size;
            var translated = Vector2.Transform(pos * scale, this.view);
            return new Vector3(translated, 0);
        }
    }
}