using Autofac;
using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.DataAccess.Extensions;
using System;

namespace Persistence.Common.DataAccess
{
    public class RelationalSchemaContext : DbContext
    {
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<PredefinedAnswer> PredefinedAnswers { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<TimeTrackingActivity> TimeTrackingActivities { get; set; }
        public virtual DbSet<TimeTrackingEntry> TimeTrackingEntries { get; set; }

        public static ContainerBuilder RegisterRepositories(ContainerBuilder builder, Type repositoryType)
        {
            var type = repositoryType;
            _ = builder.RegisterRepository<Note>(type);
            _ = builder.RegisterRepository<Reminder>(type);
            _ = builder.RegisterRepository<PredefinedAnswer>(type);
            _ = builder.RegisterRepository<Answer>(type);
            _ = builder.RegisterRepository<Question>(type);
            _ = builder.RegisterRepository<TimeTrackingActivity>(type);
            _ = builder.RegisterRepository<User>(type);
            _ = builder.RegisterRepository<Note>(type);
            _ = builder.RegisterRepository<TimeTrackingEntry>(type);
            return builder;
        }
    }
}