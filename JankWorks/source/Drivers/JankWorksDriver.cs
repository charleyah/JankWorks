using System;

namespace JankWorks.Drivers
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class JankWorksDriver : Attribute
    {
        public Type DriverType { get; private set; }

        public JankWorksDriver(Type driverType) { this.DriverType = driverType; }
    }
}
