using Autofac;
using Common.Entites;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Interfaces;
using Persistence.Master.Repositories;

namespace Persistence.Master
{
    public class PersistenceModule : Module
    {
        public bool RegisterRepositorieaAsBaseTypes { get; }

        public PersistenceModule(bool registerRepositorieaAsBaseTypes)
        {
            RegisterRepositorieaAsBaseTypes = registerRepositorieaAsBaseTypes;
        }
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterType<LifeTrackerDbContext>().As<RelationalSchemaContext>().InstancePerLifetimeScope();

            //TODO: REVIEW AND REFACTOR
            //DO NOT FORGET TO ADD TWO VERSIONS OF COMPONENT REGISTRATION WHEN YOU ADD A REPOSITORY
            if (RegisterRepositorieaAsBaseTypes)
            {
                _ = builder.RegisterType<ToDoItemRepository>()
                    .As<IRepository<ToDoItem>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<ToDoCategoryRepository>()
                    .As<IRepository<ToDoCategory>>()
                    .InstancePerLifetimeScope();


                _ = builder.RegisterType<NoteRepositry>()
                    .As<IRepository<Note>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<ReminderRepository>()
                    .As<IRepository<Reminder>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<TimeTrackingActivityRepository>()
                    .As<IRepository<TimeTrackingActivity>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<TimeTrackingEntryRepository>()
                    .As<IRepository<TimeTrackingEntry>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<UserRepositry>()
                    .As<IRepository<User>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<TagRepository>()
                    .As<IRepository<Tag>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<PredefinedAnswerRepository>()
                    .As<IRepository<PredefinedAnswer>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<QuestionRepository>()
                    .As<IRepository<Question>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<AnswerRepository>()
                    .As<IRepository<Answer>>()
                    .InstancePerLifetimeScope();

            }
            else
            {
                _ = builder.RegisterType<ToDoItemRepository>()
                    .As<LifeTrackerRepository<ToDoItem>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<ToDoCategoryRepository>()
                    .As<LifeTrackerRepository<ToDoCategory>>()
                    .InstancePerLifetimeScope();


                _ = builder.RegisterType<NoteRepositry>()
                      .As<LifeTrackerRepository<Note>>()
                      .InstancePerLifetimeScope();

                _ = builder.RegisterType<ReminderRepository>()
                    .As<LifeTrackerRepository<Reminder>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<TimeTrackingActivityRepository>()
                    .As<LifeTrackerRepository<TimeTrackingActivity>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<TimeTrackingEntryRepository>()
                    .As<LifeTrackerRepository<TimeTrackingEntry>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<UserRepositry>()
                    .As<LifeTrackerRepository<User>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<TagRepository>()
                    .As<LifeTrackerRepository<Tag>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<PredefinedAnswerRepository>()
                    .As<LifeTrackerRepository<PredefinedAnswer>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<QuestionRepository>()
                    .As<LifeTrackerRepository<Question>>()
                    .InstancePerLifetimeScope();

                _ = builder.RegisterType<AnswerRepository>()
                    .As<LifeTrackerRepository<Answer>>()
                    .InstancePerLifetimeScope();
            }

            base.Load(builder);
        }
    }
}