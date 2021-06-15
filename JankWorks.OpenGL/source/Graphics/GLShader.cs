using System;
using System.Collections.Generic;
using System.Numerics;

using JankWorks.Graphics;

using static OpenGL.Constants;
using static OpenGL.Functions;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLShader : Shader
    {
        internal uint ProgramId { get; private set; }
        internal uint Vao { get; private set; }

        private List<Sampler> samplers;

        public GLShader(uint programId)
        {
            this.ProgramId = programId;
            this.samplers = new List<Sampler>(8);
        }

        private void AddTexture2DSampler(GLTexture2D texture, int unit)
        {
            var sampler = new Sampler();
            sampler.unit = unit;
            sampler.texture = texture.Id;
            sampler.type = GL_TEXTURE_2D;

            var index = this.samplers.FindIndex((s) => s.unit == unit);

            if(index > -1)
            {
                this.samplers[index] = sampler;
            }
            else
            {
                this.samplers.Add(sampler);
            }
        }

        public override void Reset()
        {
            this.UnBind();
            this.ClearUniformTextures();                
            this.Vao = 0;
        }

        public override void ClearUniformTextures()
        {
            this.samplers.Clear();
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
            if (this.samplers.Count > 0)
            {
                glBindTexture(GL_TEXTURE_2D, 0);
            }
                
            glBindVertexArray(0);
            glUseProgram(0);
        }

        private int GetUniformLocation(string name)
        {
            glUseProgram(this.ProgramId);
            var loc = glGetUniformLocation(this.ProgramId, name);

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
