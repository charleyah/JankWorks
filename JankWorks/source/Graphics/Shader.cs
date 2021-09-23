using System;
using System.Numerics;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public enum ShaderFormat
    {
        SPARV,
        GLSL,
        HLSL
    }

    public enum ShaderType
    {
        Vertex,
        Fragment,
        Geometry,
        Compute
    }
    public abstract class Shader : Disposable
    {
        public abstract void Reset();

        public abstract void SetVertexData<T>(VertexBuffer<T> buffer, VertexLayout layout) where T : unmanaged;
        public abstract void SetVertexData<T>(VertexBuffer<T> buffer, VertexLayout layout, IndexBuffer indexes) where T : unmanaged;

        public abstract IntPtr GetUniformNameHandle(string name);

        public abstract void SetUniform(string name, int value);
        public abstract void SetUniform(string name, uint value);
        public abstract void SetUniform(string name, float value);
        public abstract void SetUniform(string name, RGBA value);
        public abstract void SetUniform(string name, Vector2 value);
        public abstract void SetUniform(string name, Vector3 value);
        public abstract void SetUniform(string name, Vector4 value);
        public abstract void SetUniform(string name, Matrix3x2 value);
        public abstract void SetUniform(string name, Matrix4x4 value);
        public abstract void SetUniform(string name, Texture2D texture, int unit);


        public abstract void SetUniform(IntPtr nameHandle, int value);
        public abstract void SetUniform(IntPtr nameHandle, uint value);
        public abstract void SetUniform(IntPtr nameHandle, float value);
        public abstract void SetUniform(IntPtr nameHandle, RGBA value);
        public abstract void SetUniform(IntPtr nameHandle, Vector2 value);
        public abstract void SetUniform(IntPtr nameHandle, Vector3 value);
        public abstract void SetUniform(IntPtr nameHandle, Vector4 value);
        public abstract void SetUniform(IntPtr nameHandle, Matrix3x2 value);
        public abstract void SetUniform(IntPtr nameHandle, Matrix4x4 value);
        public abstract void SetUniform(IntPtr nameHandle, Texture2D texture, int unit);

        public abstract void ClearUniformTextures();
    }

    public sealed class InvalidShaderException : Exception
    {
        public InvalidShaderException(string message) : base(message) { }
    }
}
