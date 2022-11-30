using Autofac;
using Common.Entites;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Interfaces;
using Persistence.Common.DataAccess.Interfaces.Repositories;
using Persistence.Master.Repositories;

namespace Persistence.Master
{
    public class PersistenceModule : Module
    {

        public PersistenceModule()
        {
        }
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder
                .RegisterType<LifeTrackerDbContext>()
                .As<RelationalSchemaContext>()
                .InstancePerLifetimeScope();


            _ = builder
                .RegisterType<RelationalSchemaRepository<ToDoItem>>()
                .As<IRepository<ToDoItem>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<ToDoCategory>>()
                .As<IRepository<ToDoCategory>>()
                .InstancePerLifetimeScope();


            _ = builder
                .RegisterType<NoteRepositry>()
                  .As<INoteRepository>()
                  .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<Reminder>>()
                .As<IRepository<Reminder>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<TimeTrackingActivity>>()
                .As<IRepository<TimeTrackingActivity>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<TimeTrackingEntry>>()
                .As<IRepository<TimeTrackingEntry>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<TelegramLogIn>>()
                .As<IRepository<TelegramLogIn>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<UserRepositry>()
                .As<IRepository<User>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<UserFeatureFlag>>()
                .As<IRepository<UserFeatureFlag>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<TagRepository>()
                .As<IRepository<Tag>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<PredefinedAnswer>>()
                .As<IRepository<PredefinedAnswer>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<QuestionRepository>()
                .As<IRepository<Question>>()
                .InstancePerLifetimeScope();

            _ = builder
                .RegisterType<RelationalSchemaRepository<Answer>>()
                .As<IRepository<Answer>>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}