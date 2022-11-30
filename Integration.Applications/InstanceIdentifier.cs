using System;

namespace Integration.Applications
{
    public static class InstanceIdentifier
    {
        public static Guid Identifier { get; }
        static InstanceIdentifier()
        {
            Identifier = Guid.NewGuid();
        }
    }
}
