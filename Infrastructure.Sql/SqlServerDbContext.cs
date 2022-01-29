using Microsoft.EntityFrameworkCore;
using Persistence.Sql.Entites;

namespace Persistence.Sql
{
    public class SqlServerDbContext : DbContext
    {
        //todo: put into config.json
        private static readonly string connectionstring = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog= MultiBot-Db;";

        public DbSet<Note> Notes { get; set; }
        public DbSet<PredefinedAnswer> PredefinedAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        public SqlServerDbContext() : base()
        {
        }

        public DbContextOptions<SqlServerDbContext> _options;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionstring);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
