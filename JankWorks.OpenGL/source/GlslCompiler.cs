using System;

using System.Text;
using System.IO;

using JankWorks.Graphics;

using static OpenGL.Constants;
using static OpenGL.Functions;

namespace JankWorks.Drivers.OpenGL
{
    public static class GlslCompiler
    {
        public static uint CompileShader(Stream source, int type)
        {
            if(source == null)
            {
                return 0;
            }
            else if (source is UnmanagedMemoryStream unmanagedStream)
            {
                unsafe
                {
                    ReadOnlySpan<byte> span = default;

                    checked
                    {
                        span = new ReadOnlySpan<byte>(unmanagedStream.PositionPointer, (int)unmanagedStream.Length);
                    }
                    return CompileShader(span, type);
                }
            }
            else if (source is MemoryStream memoryStream)
            {
                return CompileShader(memoryStream.GetBuffer(), type);
            }
            else
            {
                int sourceLength = 0;
                checked
                {
                    sourceLength = (int)source.Length;
                }
                var ms = new MemoryStream(sourceLength);
                source.CopyTo(ms);
                return CompileShader(ms.GetBuffer(), type);
            }
        }

        public static uint CompileShader(ReadOnlySpan<byte> source, int type)
        {
            if (source.IsEmpty) { return 0; }

            var shaderId = glCreateShader(type);

            unsafe
            {
                fixed (byte* data = source)
                {
                    byte** dataptr = stackalloc byte*[1];
                    int* lengths = stackalloc int[1];
                    dataptr[0] = data;
                    lengths[0] = source.Length;

                    glShaderSource(shaderId, 1, dataptr, lengths);
                }

                glCompileShader(shaderId);
                int success = 0;
                glGetShaderiv(shaderId, GL_COMPILE_STATUS, &success);

                if (success == GL_FALSE)
                {
                    const int bufferSize = 512;

                    Span<byte> info = stackalloc byte[bufferSize];

                    fixed (byte* infobuffer = info)
                    {
                        glGetShaderInfoLog(shaderId, bufferSize, null, infobuffer);
                    }

                    throw new InvalidShaderException(Encoding.UTF8.GetString(info));
                }
            }

            return shaderId;
        }

        public static uint LinkProgram(uint vertex, uint fragment, uint geometry, bool deleteShaders)
        {
            if (vertex == 0 || fragment == 0)
            {
                throw new ArgumentException();
            }

            var programId = glCreateProgram();

            glAttachShader(programId, vertex);
            glAttachShader(programId, fragment);

            if (geometry != 0) { glAttachShader(programId, geometry); }

            glLinkProgram(programId);

            unsafe
            {
                int success = 0;
                glGetProgramiv(programId, GL_LINK_STATUS, &success);

                if (success == GL_FALSE)
                {
                    const int bufferSize = 512;

                    Span<byte> info = stackalloc byte[bufferSize];

                    fixed (byte* infobuffer = info)
                    {
                        glGetProgramInfoLog(programId, bufferSize, null, infobuffer);
                    }

                    throw new InvalidShaderException(Encoding.UTF8.GetString(info));
                }
            }

            if(deleteShaders)
            {
                if (geometry != 0) 
                {
                    glDeleteShader(geometry); 
                }
                glDeleteShader(fragment);
                glDeleteShader(vertex);
            }
            
            return programId;
        }
    }
}
