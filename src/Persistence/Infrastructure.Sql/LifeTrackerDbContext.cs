using Common.Configuration;
using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Common.DataAccess;

namespace Persistence.Master;

public class LifeTrackerDbContext : RelationalSchemaContext
{
    public LifeTrackerDbContext() : base()
    {
    }

    public DbContextOptions<LifeTrackerDbContext> _options;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = ConfigurationHelper.GetConfiguration();
        optionsBuilder.UseSqlServer(config.GetConnectionString("LifeTrackerDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<Tag>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<TimeTrackingActivity>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Note>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Reminder>().HasQueryFilter(x => !x.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}