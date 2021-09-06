using System;

namespace JankWorks.Audio
{
    public sealed class AudioException : Exception
    {
        public AudioException(string msg) : base(msg) { }
    }
}
