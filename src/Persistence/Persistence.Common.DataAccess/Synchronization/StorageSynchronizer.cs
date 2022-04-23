using Autofac;
using Common.BaseTypes;
using Common.Entites;
using Persistence.Common.DataAccess.Interfaces;
using System.Linq;

namespace Persistence.Common.DataAccess
{
    public class StorageSynchronizer
    {
        public void Synchronize()
        {
            SynchronizaEntity<Note>();
            SynchronizaEntity<Reminder>();
            SynchronizaEntity<TimeTrackingActivity>();
            SynchronizaEntity<TimeTrackingEntry>();
            SynchronizaEntity<User>();
            SynchronizaEntity<Tag>();
            SynchronizaEntity<PredefinedAnswer>();
            SynchronizaEntity<Question>();
            SynchronizaEntity<Answer>();
        }

        public ILifetimeScope SourceContainer { get; }
        public ILifetimeScope DestinationContainer { get; }

        public StorageSynchronizer(ILifetimeScope sourceContainer, ILifetimeScope destinationContainer)
        {
            SourceContainer = sourceContainer;
            DestinationContainer = destinationContainer;
        }


        private void SynchronizaEntity<TEntity>() where TEntity : AuditableEntity
        {
            var sourceRepo = SourceContainer.Resolve<IRepository<TEntity>>();
            var destRepo = DestinationContainer.Resolve<IRepository<TEntity>>();

            var sourceData = sourceRepo.GetAll().ToList();
            var destData = destRepo.GetAll().ToList();

            var datasToInsert = sourceData.Where(s => !destData.Any(d => d.Id == s.Id)).ToList();
            destData.AddRange(datasToInsert);

            var dataToupdate = sourceData.Where(s =>
            {
                var t = destData.FirstOrDefault(d => d.Id == s.Id);
                return t != null && t.LastModificationDate != s.LastModificationDate;
            });

            destRepo.AddRange(datasToInsert);
            destRepo.UpdateRange(dataToupdate);
        }
    }
}
