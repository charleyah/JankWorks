using System;

namespace JankWorks.Audio
{
    public abstract class AudioDevice
    {
        public static AudioDevice Create() { return null; /* doesn't throw to test other APIs in framework */ }
    }
}
