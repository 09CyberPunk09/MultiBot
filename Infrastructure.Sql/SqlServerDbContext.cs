using Microsoft.EntityFrameworkCore;
using Persistence.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Sql
{
	public class SqlServerDbContext : DbContext
    {
        //todo: put into config.json
        private static readonly string connectionstring = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog= MultiBot-devDb;";

        public DbSet<Note> Notes { get; set; }

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
