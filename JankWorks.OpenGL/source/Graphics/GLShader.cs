using System;
using System.Text;
using System.Numerics;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Functions;
using static JankWorks.Drivers.OpenGL.Native.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLShader : Shader
    {
        internal uint ProgramId { get; private set; }
        internal uint Vao { get; private set; }

        private const int initialSamplerCapacity = 16;
        private Sampler[] samplers;
        private int samplerCount;

        private Encoder uniformNameEncoder;

        public GLShader(uint programId)
        {
            this.ProgramId = programId;
            this.samplers = new Sampler[initialSamplerCapacity];
            this.samplerCount = 0;
                
            var utf = Encoding.GetEncoding("utf-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());
            this.uniformNameEncoder = utf.GetEncoder();
        }

        private void AddTexture2DSampler(GLTexture2D texture, int unit)
        {
            var sampler = new Sampler();
            sampler.unit = unit;
            sampler.texture = texture.Id;
            sampler.type = GL_TEXTURE_2D;

            for (int index = 0; index < this.samplerCount; index++)
            {
                ref var currentSampler = ref this.samplers[index];

                if(currentSampler.unit == unit)
                {
                    currentSampler = sampler;
                    return;
                }
            }

            if (samplerCount >= this.samplers.Length)
            {
                Array.Resize(ref this.samplers, this.samplers.Length + initialSamplerCapacity);
            }

            this.samplers[samplerCount++] = sampler;
        }

        public override void Reset()
        {
            this.UnBind();
            this.ClearUniformTextures();                
            this.Vao = 0;
        }

        public override void ClearUniformTextures()
        {
            Array.Clear(this.samplers, 0, this.samplerCount);
            this.samplerCount = 0;
        }

        internal void BindTextures()
        {
            foreach(var sampler in this.samplers)
            {
                glActiveTexture(GL_TEXTURE0 + sampler.unit);
                glBindTexture(sampler.type, sampler.texture);
            }
        }

        internal void Bind()
        {
            glBindVertexArray(this.Vao);
            glUseProgram(this.ProgramId);
        }

        internal void UnBind()
        {
            if (this.samplers.Length > 0)
            {
                glBindTexture(GL_TEXTURE_2D, 0);
            }
                
            glBindVertexArray(0);
            glUseProgram(0);
        }

        public override IntPtr GetUniformNameHandle(string name)
        {
            var handle = this.GetUniformLocation(name);
            return (IntPtr)handle;
        }

        private int GetUniformLocation(string name)
        {
            glUseProgram(this.ProgramId);

            var utfLength = this.uniformNameEncoder.GetByteCount(name, false) + 1;

            var loc = -1;
            unsafe
            {
                byte* utfName = stackalloc byte[utfLength];                
                utfName[utfLength - 1] = 0;

                fixed (char* namePtr = name)
                {
                    this.uniformNameEncoder.GetBytes(namePtr, name.Length, utfName, utfLength, true);
                }
                loc = glGetUniformLocation(this.ProgramId, utfName);
            }

            if (loc == -1)
            {
                throw new ArgumentException();
            }
            return loc;
        }

        public override void SetUniform(string name, int value)
        {
            var loc = this.GetUniformLocation(name);
            glUniform1i(loc, value);
        }

        public override void SetUniform(string name, uint value)
        {
            var loc = this.GetUniformLocation(name);
            glUniform1ui(loc, value);
        }

        public override void SetUniform(string name, float value)
        {
            var loc = this.GetUniformLocation(name);
            glUniform1f(loc, value);
        }

        public override void SetUniform(string name, RGBA value) => this.SetUniform(name, (Vector4)value);

        public override void SetUniform(string name, Vector2 value)
        {
            var loc = this.GetUniformLocation(name);
            glUniform2f(loc, value.X, value.Y);
        }

        public override void SetUniform(string name, Vector3 value)
        {
            var loc = this.GetUniformLocation(name);
            glUniform3f(loc, value.X, value.Y, value.Z);
        }

        public override void SetUniform(string name, Vector4 value)
        {
            var loc = this.GetUniformLocation(name);
            glUniform4f(loc, value.X, value.Y, value.Z, value.W);
        }

        public override void SetUniform(string name, Matrix3x2 value)
        {
            var loc = this.GetUniformLocation(name);
            unsafe { glUniformMatrix3x2fv(loc, 1, false, (float*)&value); }
        }

        public override void SetUniform(string name, Matrix4x4 value)
        {
            var loc = this.GetUniformLocation(name);
            unsafe { glUniformMatrix4fv(loc, 1, false, (float*)&value); }
        }

        public override void SetUniform(string name, Texture2D texture, int unit)
        {
            this.AddTexture2DSampler((GLTexture2D)texture, unit);
            var loc = this.GetUniformLocation(name);
            glUniform1i(loc, unit);
        }

        public override void SetUniform(IntPtr nameHandle, int value)
        {
            glUseProgram(this.ProgramId);
            glUniform1i(nameHandle.ToInt32(), value);
        }

        public override void SetUniform(IntPtr nameHandle, uint value)
        {
            glUseProgram(this.ProgramId);
            glUniform1ui(nameHandle.ToInt32(), value);
        }

        public override void SetUniform(IntPtr nameHandle, float value)
        {
            glUseProgram(this.ProgramId);
            glUniform1f(nameHandle.ToInt32(), value);
        }

        public override void SetUniform(IntPtr nameHandle, RGBA value) => this.SetUniform(nameHandle, (Vector4)value);

        public override void SetUniform(IntPtr nameHandle, Vector2 value)
        {
            glUseProgram(this.ProgramId);
            glUniform2f(nameHandle.ToInt32(), value.X, value.Y);
        }

        public override void SetUniform(IntPtr nameHandle, Vector3 value)
        {
            glUseProgram(this.ProgramId);
            glUniform3f(nameHandle.ToInt32(), value.X, value.Y, value.Z);
        }

        public override void SetUniform(IntPtr nameHandle, Vector4 value)
        {
            glUseProgram(this.ProgramId);
            glUniform4f(nameHandle.ToInt32(), value.X, value.Y, value.Z, value.W);
        }

        public override void SetUniform(IntPtr nameHandle, Matrix3x2 value)
        {
            glUseProgram(this.ProgramId);
            unsafe { glUniformMatrix3x2fv(nameHandle.ToInt32(), 1, false, (float*)&value); }
        }

        public override void SetUniform(IntPtr nameHandle, Matrix4x4 value)
        {
            glUseProgram(this.ProgramId);
            unsafe { glUniformMatrix4fv(nameHandle.ToInt32(), 1, false, (float*)&value); }
        }

        public override void SetUniform(IntPtr nameHandle, Texture2D texture, int unit)
        {            
            this.AddTexture2DSampler((GLTexture2D)texture, unit);
            glUseProgram(this.ProgramId);            
            glUniform1i(nameHandle.ToInt32(), unit);
        }


        public override void SetVertexData<T>(VertexBuffer<T> buffer, VertexLayout layout)
        {
            var vbo = (GLVertexBuffer<T>)buffer;
            var vao = (GLVertexLayout)layout;        
            this.Vao = vao.Id;

            this.Bind();
            glBindBuffer(GL_ARRAY_BUFFER, vbo.Id);
            vao.ApplyAttributes();
            this.UnBind();

            glBindBuffer(GL_ARRAY_BUFFER, 0);
        }

        public override void SetVertexData<T>(VertexBuffer<T> buffer, VertexLayout layout, IndexBuffer indexes)
        {
            var vbo = (GLVertexBuffer<T>)buffer;
            var vao = (GLVertexLayout)layout;
            var ebo = (GLIndexBuffer)indexes;
            this.Vao = vao.Id;

            this.Bind();
            glBindBuffer(GL_ARRAY_BUFFER, vbo.Id);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo.Id);
            vao.ApplyAttributes();
            this.UnBind();

            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        }

        internal void SetVertexData<T>(GLBuffer<T> buffer, VertexLayout layout) where T : unmanaged
        {
            var vao = (GLVertexLayout)layout;

            this.Vao = vao.Id;

            this.Bind();
            glBindBuffer(GL_ARRAY_BUFFER, buffer.BufferId);
            vao.ApplyAttributes();
            this.UnBind();

            glBindBuffer(GL_ARRAY_BUFFER, 0);
        }

        internal void SetVertexData<T>(GLBuffer<T> buffer, VertexLayout layout, GLBuffer<uint> indexes) where T : unmanaged
        {
            var vao = (GLVertexLayout)layout;

            this.Vao = vao.Id;

            this.Bind();
            glBindBuffer(GL_ARRAY_BUFFER, buffer.BufferId);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexes.BufferId);
            vao.ApplyAttributes();
            this.UnBind();

            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        }
        protected override void Dispose(bool finalising)
        {
            this.ClearUniformTextures();
            this.UnBind();
            glDeleteProgram(this.ProgramId);
            base.Dispose(finalising);
        }

        

        

        private struct Sampler : IEquatable<Sampler>
        {
            public int unit;

            public uint texture;

            public int type;

            public bool Equals(Sampler other) => this.unit == other.unit && this.texture == other.texture && this.type == other.type;
        }
    }
}