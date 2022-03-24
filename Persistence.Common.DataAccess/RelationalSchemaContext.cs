using Common.Entites;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common.DataAccess
{
    public class RelationalSchemaContext : DbContext
    {
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<PredefinedAnswer> PredefinedAnswers { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
