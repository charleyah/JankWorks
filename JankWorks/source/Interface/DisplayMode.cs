using System;

using JankWorks.Graphics;

namespace JankWorks.Interface
{
    public readonly struct DisplayMode : IEquatable<DisplayMode>
    {
        public readonly uint Width;
        public readonly uint Height;
        public readonly uint BitsPerPixel;
        public readonly uint RefreshRate;

        public DisplayMode(uint width, uint height, uint bitsPerPixel, uint refreshRate)
        {
            this.Width = width;
            this.Height = height;
            this.BitsPerPixel = bitsPerPixel;
            this.RefreshRate = refreshRate;
        }

        public Rectangle Viewport
        {
            get
            {
                checked
                {
                    return new Rectangle(0, 0, (int)this.Width, (int)this.Height);
                }
            }
        }

        public override string ToString() => $"{this.Width}x{this.Height} {this.BitsPerPixel} Bits {this.RefreshRate} Hz";

        public bool Equals(DisplayMode other) => this == other;
        public override bool Equals(object obj) => obj is DisplayMode other && this == other;
        
        // what is this... F#?
        public override int GetHashCode() 
        => 
        this.Width.GetHashCode() ^ 
        this.Height.GetHashCode() ^ 
        this.BitsPerPixel.GetHashCode() ^ 
        this.RefreshRate.GetHashCode();

        public static bool operator ==(DisplayMode a, DisplayMode b) 
        => 
        a.Width == b.Width && 
        a.Height == b.Height && 
        a.BitsPerPixel == b.BitsPerPixel && 
        a.RefreshRate == b.RefreshRate;

        public static bool operator !=(DisplayMode a, DisplayMode b) 
        => 
        a.Width != b.Width || 
        a.Height != b.Height || 
        a.BitsPerPixel != b.BitsPerPixel || 
        a.RefreshRate != b.RefreshRate;
    }
}
