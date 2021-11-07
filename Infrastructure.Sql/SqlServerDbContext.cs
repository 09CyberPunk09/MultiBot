using Microsoft.EntityFrameworkCore;
using Persistence.Sql.Entites;

namespace Persistence.Sql
{
    public class SqlServerDbContext : DbContext
	{
		//todo: put into config.json
		private static readonly string connectionstring = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog= MultiBot-devDb;";

		public DbSet<Note> Notes { get; set; }
		public DbSet<Set> Sets { get; set; }
		public DbSet<SetItem> SetDatas { get; set; }

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
