
namespace JankWorks.Game.Hosting
{
    public struct HostParameters
    {
        public float TickRate { get; set; }

        public static HostParameters Default => new HostParameters()
        {
            TickRate = 30
        };
    }
}