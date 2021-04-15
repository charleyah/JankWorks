using System;
using JankWorks.Graphics;

using static OpenGL.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    static class GLExtensions
    {
        public static int GetGLType<T>()
        {
            var type = typeof(T);

            if (type == typeof(byte))
            {
                return GL_UNSIGNED_BYTE;
            }
            else if (type == typeof(ushort))
            {
                return GL_UNSIGNED_SHORT;
            }
            else if (type == typeof(uint))
            {
                return GL_UNSIGNED_INT;
            }
            else if (type == typeof(sbyte))
            {
                return GL_BYTE;
            }
            else if (type == typeof(short))
            {
                return GL_SHORT;
            }
            else if (type == typeof(int))
            {
                return GL_INT;
            }
            else if (type == typeof(float))
            {
                return GL_FLOAT;
            }
            else if(type == typeof(double))
            {
                return GL_DOUBLE;
            }

            throw new InvalidOperationException();
        }

        public static int GetGLPrimitive(this DrawPrimitiveType primitive)
        {

            return primitive switch
            {
                DrawPrimitiveType.Points => GL_POINTS,
                DrawPrimitiveType.Lines => GL_LINES,
                DrawPrimitiveType.LineLoop => GL_LINE_LOOP,
                DrawPrimitiveType.LineStrip => GL_LINE_STRIP,
                DrawPrimitiveType.Triangles => GL_TRIANGLES,
                DrawPrimitiveType.TriangleStrip => GL_TRIANGLE_STRIP,

                _ => throw new NotImplementedException()
            };
        }

        public static int GetGLPointerType(this VertexAttributeFormat type)
        {
            switch (type)
            {
                case VertexAttributeFormat.Byte: return GL_BYTE;
                case VertexAttributeFormat.Short: return GL_SHORT;
                case VertexAttributeFormat.Int: return GL_INT;
                case VertexAttributeFormat.Float: return GL_FLOAT;
                case VertexAttributeFormat.Double: return GL_DOUBLE;

                case VertexAttributeFormat.UByte: return GL_UNSIGNED_BYTE;
                case VertexAttributeFormat.UShort: return GL_UNSIGNED_SHORT;
                case VertexAttributeFormat.UInt: return GL_UNSIGNED_INT;

                case VertexAttributeFormat.Vector2f: return GL_FLOAT;
                case VertexAttributeFormat.Vector2i: return GL_INT;

                case VertexAttributeFormat.Vector3f: return GL_FLOAT;
                case VertexAttributeFormat.Vector3i: return GL_INT;

                case VertexAttributeFormat.Vector4f: return GL_FLOAT;
                case VertexAttributeFormat.Vector4i: return GL_INT;


                default: throw new NotImplementedException();
            }
        }

        public static int GetGLBufferUsage(this BufferUsage usage)
        {
            return usage switch
            {
                BufferUsage.Static => GL_STATIC_DRAW,
                BufferUsage.Dynamic => GL_DYNAMIC_DRAW,
                BufferUsage.Stream => GL_STREAM_DRAW,

                _ => throw new NotImplementedException()
            };
        }
    }
}
