
namespace JankWorks.Game.Configuration
{
    public abstract class SettingsSource
    {
        public abstract void Flush(Settings settings);

        public abstract void Refresh(Settings settings);

        private sealed class TransientSource : SettingsSource
        {
            public override void Flush(Settings settings) { }

            public override void Refresh(Settings settings) { }
        }

        public static readonly SettingsSource Transient = new TransientSource();
    }
}
