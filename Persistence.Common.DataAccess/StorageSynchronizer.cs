using System;

namespace Persistence.Common.DataAccess
{
    public enum SynchronizationMode
    {
        SyncMaster,
        SyncFromMaster,
        CrosDataSource
    }

    public class StorageSynchronizer
    {
        public SynchronizationMode Mode { get; }
        public StorageSynchronizer(SynchronizationMode mode)
        {
            Mode = mode;
        }

        public void Synchronize()
        {
            switch (Mode)
            {
                case SynchronizationMode.SyncMaster:
                    break;
                case SynchronizationMode.SyncFromMaster:
                    break ;
                case SynchronizationMode.CrosDataSource:
                    break;
                default: 
                    throw new ArgumentOutOfRangeException("mode");
            }
        }
    }
}
