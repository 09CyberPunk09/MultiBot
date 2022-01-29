using Autofac;
using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
using Persistence.Sql.Repositories;

namespace Persistence.Sql
{
    public class PersistenceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterType<SqlServerDbContext>().InstancePerLifetimeScope();

            _ = builder.RegisterType<NoteRepositry>().As<Repository<Note>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<UserRepositry>().As<Repository<User>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<TagRepository>().As<Repository<Tag>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<PredefinedAnswerRepository>().As<Repository<PredefinedAnswer>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<QuestionRepository>().As<Repository<Question>>().InstancePerLifetimeScope();
            _ = builder.RegisterType<AnswerRepository>().As<Repository<Answer>>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
