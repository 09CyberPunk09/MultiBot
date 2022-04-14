using System;

namespace Application
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
