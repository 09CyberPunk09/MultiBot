using Common.Entites;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Persistence.Common.DataAccess;
using Persistence.Common.DataAccess.Interfaces;
using Persistence.Common.DataAccess.Interfaces.Repositories;
using Persistence.Master.Repositories;

namespace Persistence.Master;

public static class DataStorageServiceCollectionExtensions
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static IServiceCollection AddSqlServerPersistence(this IServiceCollection services) 
    {
        services.AddScoped<RelationalSchemaContext, LifeTrackerDbContext>();

        services.AddScoped<IRepository<ToDoItem>, RelationalSchemaRepository<ToDoItem>>();          
        services.AddScoped<IRepository<ToDoCategory>, RelationalSchemaRepository<ToDoCategory>>();                      
        services.AddScoped<INoteRepository, NoteRepositry>();
        services.AddScoped<IRepository<Reminder>, RelationalSchemaRepository<Reminder>>();                    
        services.AddScoped<IRepository<TimeTrackingActivity>, RelationalSchemaRepository<TimeTrackingActivity>>();                    
        services.AddScoped<IRepository<TimeTrackingEntry>, RelationalSchemaRepository<TimeTrackingEntry>>();                    
        services.AddScoped<IRepository<TelegramLogIn>, RelationalSchemaRepository<TelegramLogIn>>();                           
        services.AddScoped<IRepository<User>, UserRepositry>();
        services.AddScoped<IRepository<UserFeatureFlag>, RelationalSchemaRepository<UserFeatureFlag>>();
        services.AddScoped<IRepository<Tag>, TagRepository>();
        services.AddScoped<IRepository<PredefinedAnswer>, RelationalSchemaRepository<PredefinedAnswer>>();
        services.AddScoped<IRepository<Question>, QuestionRepository>();
        services.AddScoped<IRepository<Answer>, RelationalSchemaRepository<Answer>>();
        logger.Info("SqlServer persistence registered");
        return services;
    }
}
