using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Persistence.Common.DataAccess;
using Persistence.Master;
using System.Text;

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting...");


IServiceCollection services = new ServiceCollection();
services.AddSqlServerPersistence();
var provider = services.BuildServiceProvider();

var ctx = provider.GetService<RelationalSchemaContext>();
var pendingMigrations = ctx.Database.GetPendingMigrations();
if(pendingMigrations!= null && pendingMigrations.Count() > 0)
{
    StringBuilder sb = new();
    sb.AppendLine($"Found {pendingMigrations.Count()} not applied migrations");
    pendingMigrations.ToList().ForEach(m =>
    {
            sb.AppendLine(m);
    });
    logger.Info(sb.ToString());
    logger.Info("Started applying migrations");
    try
    {
        ctx.Database.Migrate();
        logger.Info("Done");
    }
    catch (Exception ex)
    {
        logger.Error(ex);
    }

}
else
{
    logger.Info("All migrations are applied to the database");
}