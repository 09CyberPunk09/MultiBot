using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Routing;

namespace TelegramBot.ChatEngine.Setup;

public static class  IServiceCollectionExtensions
{
    public static IServiceCollection AddCommandsAndStages(this IServiceCollection services)
    {
       // logger.Info("Host configured");

        var routingTableBuilder = new RoutingTableBuilder();
        //routingTableBuilder
        var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
        var commandsToRegister = allTypes.Where(t => t.GetInterfaces().Contains(typeof(ITelegramCommand)));
        foreach (var command in commandsToRegister)
        {
            routingTableBuilder.AddCommand(command);
        }

        var stagesToRegister = allTypes.Where(t => t.GetInterfaces().Contains(typeof(IStage)));//NOW: ICommand and IStage interfaces by itself are included into this variable. Fix it
        foreach (var stage in stagesToRegister)
        {
            routingTableBuilder.AddStage(stage);
        }

        var routingTable = routingTableBuilder.Build();

        services.AddSingleton(routingTable);
        // logger.Info($"Detected {commandsToRegister.Count()} commands with routes.");


        services.RegisterCommandsAndStages();
        return services;
    }

    private static IServiceCollection RegisterCommandsAndStages(this IServiceCollection services)
    {
        var command = typeof(ITelegramCommand);
        var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
        var commandsToRegister = allTypes.Where(t => t.GetInterfaces().Contains(command) && t.FullName != command.FullName);

        var stageType = typeof(ITelegramStage);
        var stagesToRegister = allTypes.Where(t => t.GetInterfaces().Contains(stageType) && t.FullName != command.FullName && t.FullName != stageType.FullName);

        foreach (var commandType in commandsToRegister)
        {
            services.AddTransient(commandType);
        }
        //logger.Info($"Registered {commandsToRegister.Count()} commands");
        foreach (var stageTypeToRegister in stagesToRegister)
        {
            services.AddTransient(stageTypeToRegister);
        }
        //logger.Info($"Registered {stagesToRegister.Count()} stages");

       // services.AddSingleton<AuthentificationMiddleware>();

        return services;
    }

}
