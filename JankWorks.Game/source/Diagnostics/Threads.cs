using System.Threading;

namespace JankWorks.Game.Diagnostics
{
    internal static class Threads
    {
        public static Thread ClientThread { get; set; }

        public static Thread HostThread { get; set; }
    }
}