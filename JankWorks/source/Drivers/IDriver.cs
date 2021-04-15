
namespace JankWorks.Drivers
{
    public interface IDriver
    {
        string Name => this.GetType().FullName;
    }
}
