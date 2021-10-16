
namespace JankWorks.Game.Hosting
{
    public struct HostParameters
    {
        public ushort TickRate { get; set; }

        public static HostParameters Default => new HostParameters()
        {
            TickRate = 30
        };
    }
}