using Application.Chatting.Core;
using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.Routing;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Data;
using System.Linq;

namespace Application.TelegramBot.Commands
{
    public static class PipelinesServiceCollectionExtensions
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static IServiceCollection AddHost(this IServiceCollection services)
        {
            logger.Info("Host configured");
            services.AddSingleton<IHost, Host>();

            var routingTableBuilder = new RoutingTableBuilder();
            //routingTableBuilder
            var allTypes = typeof(PipelinesServiceCollectionExtensions).Assembly.GetTypes();
            var commandsToRegister = allTypes.Where(t => t.GetInterfaces().Contains(typeof(ITelegramCommand)));
            foreach (var command in commandsToRegister)
            {
                routingTableBuilder.AddCommand(command);
            }

            var stagesToRegister = allTypes.Where(t => t.GetInterfaces().Contains(typeof(IStage)));
            foreach (var stage in stagesToRegister)
            {
                routingTableBuilder.AddStage(stage);
            }

            var routingTable = routingTableBuilder.Build();
            services.AddSingleton(routingTable);
            logger.Info($"Detected {commandsToRegister.Count()} commands with routes.");

            return services;
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            var command = typeof(ITelegramCommand);
            var allTypes = typeof(PipelinesServiceCollectionExtensions).Assembly.GetTypes();
            var commandsToRegister = allTypes.Where(t => t.GetInterfaces().Contains(command) && t.FullName != command.FullName);

            var stageType = typeof(ITelegramStage);
            var stagesToRegister = allTypes.Where(t => t.GetInterfaces().Contains(stageType) && t.FullName != command.FullName && t.FullName != stageType.FullName);

            foreach (var commandType in commandsToRegister)
            {
                services.AddTransient(commandType);
            }
            logger.Info($"Registered {commandsToRegister.Count()} commands");
            foreach (var stageTypeToRegister in stagesToRegister)
            {
                services.AddTransient(stageTypeToRegister);
            }
            logger.Info($"Registered {stagesToRegister.Count()} stages");

            services.AddSingleton<AuthentificationMiddleware>();

            return services;
        }
    }
}
