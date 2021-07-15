using System;
using System.Threading;

namespace JankWorks.Game.Platform
{
    internal sealed class FallbackApi : PlatformApi
    {
        public override void Sleep(TimeSpan time) => Thread.Sleep(time);
    }
}
