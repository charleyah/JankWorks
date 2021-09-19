using System;
using System.Text;
using System.IO;

using JankWorks.Graphics;
using JankWorks.Util;

using static JankWorks.Drivers.OpenGL.Native.Constants;
using static JankWorks.Drivers.OpenGL.Native.Functions;

namespace JankWorks.Drivers.OpenGL
{
    public static class GlslCompiler
    {
        private const int ErrorBufferSize = 512;

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
                    ReadOnlySpan<byte> span;

                    checked
                    {
                        span = new ReadOnlySpan<byte>(unmanagedStream.PositionPointer, (int)unmanagedStream.Length);
                    }
                    return CompileShader(span, type);
                }
            }
            else
            {
                MemoryStream ms;

                if(source is MemoryStream memoryStream)
                {
                    ms = memoryStream;
                }
                else
                {
                    int sourceLength;
                    checked
                    {
                        sourceLength = (int)source.Length;
                    }
                    ms = new MemoryStream(sourceLength);
                    source.CopyTo(ms);
                }

                return CompileShader(ms.GetBuffer(), type);
            }
        }

        public static uint CompileShader(ReadOnlySpan<byte> source, int type)
        {
            if (source.IsEmpty) { return 0; }

            var shaderId = glCreateShader(type);

            unsafe
            {
                byte** dataptr = stackalloc byte*[1];
                int length = source.Length;

                fixed (byte* data = source)
                {                    
                    dataptr[0] = data;                                         
                    glShaderSource(shaderId, 1, dataptr, &length);
                }
                
                glCompileShader(shaderId);
                int success = 0;
                glGetShaderiv(shaderId, GL_COMPILE_STATUS, &success);

                if (success == GL_FALSE)
                {  
                    var info = new byte[ErrorBufferSize];
                    var errorLength = 0;
                    fixed (byte* infobuffer = info)
                    {
                        glGetShaderInfoLog(shaderId, ErrorBufferSize, null, infobuffer);
                        errorLength = new CString(infobuffer).Length;                        
                    }

                    throw new InvalidShaderException(Encoding.UTF8.GetString(info, 0, errorLength));
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
                    var info = new byte[ErrorBufferSize];
                    var errorLength = 0;

                    fixed (byte* infobuffer = info)
                    {
                        glGetProgramInfoLog(programId, ErrorBufferSize, null, infobuffer);
                        errorLength = new CString(infobuffer).Length;
                    }

                    throw new InvalidShaderException(Encoding.UTF8.GetString(info, 0, errorLength));
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