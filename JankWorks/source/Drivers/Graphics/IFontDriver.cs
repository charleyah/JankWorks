using System;
using System.IO;
using JankWorks.Graphics;

namespace JankWorks.Drivers.Graphics
{
    public interface IFontDriver : IDriver
    {
        Font LoadFontFromStream(Stream stream, FontFormat format);
    }
}
