using Autofac;
using Common.Entites;
using Persistence.Common.DataAccess;
using Persistence.Sql.Repositories;

namespace Persistence.Sql
{
    public class PersistenceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterType<LifeTrackerDbContext>().As<RelationalSchemaContext>().InstancePerLifetimeScope();
            _ = builder.RegisterType<SynchronizationDbContext>().InstancePerLifetimeScope();

            _ = builder.RegisterType<NoteRepositry>().As<LifeTrackerRepository<Note>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<ReminderRepository>().As<LifeTrackerRepository<Reminder>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<TimeTrackingActivityRepository>().As<LifeTrackerRepository<TimeTrackingActivity>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<TimeTrackingEntryRepository>().As<LifeTrackerRepository<TimeTrackingEntry>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<UserRepositry>().As<LifeTrackerRepository<User>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<TagRepository>().As<LifeTrackerRepository<Tag>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<PredefinedAnswerRepository>().As<LifeTrackerRepository<PredefinedAnswer>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<QuestionRepository>().As<LifeTrackerRepository<Question>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<AnswerRepository>().As<LifeTrackerRepository<Answer>>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}