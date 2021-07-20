using System;

using JankWorks.Core;

namespace JankWorks.Graphics
{
    public sealed class TextureFont : Disposable
    {
        public Texture2D Texture { get; private set; }

        private Func<char, Rectangle> charMapper;

        public TextureFont(Texture2D texture, Func<char, Rectangle> charmapper)
        {
            this.Texture = texture;
            this.charMapper = charmapper;                
        }

        public Rectangle GetCharacterPosition(char character) => this.charMapper(character);

        protected override void Dispose(bool finalising)
        {
            this.Texture.Dispose();
            base.Dispose(finalising);
        }
    }
}
