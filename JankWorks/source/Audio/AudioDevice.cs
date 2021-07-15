using System;

namespace JankWorks.Audio
{
    public abstract class AudioDevice
    {
        public static AudioDevice Create() { throw new NotImplementedException(); }
    }
}
