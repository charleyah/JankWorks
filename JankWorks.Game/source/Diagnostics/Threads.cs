using System.Threading;

namespace JankWorks.Game.Diagnostics
{
    internal static class Threads
    {
        public static Thread ClientThread { get; set; }

        public static Thread HostThread { get; set; }

        public static bool VerifyCorrectThread { get; set; }

        static Threads()
        {
            VerifyCorrectThread = true;
        }
    }
}