using Application.Chatting.Core;
using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.Routing;
using Application.TelegramBot.Commands.Core.Interfaces;
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

        public static IServiceCollection AddPipelines(this IServiceCollection services)
        {
            var command = typeof(ITelegramCommand);
            var allTypes = typeof(PipelinesServiceCollectionExtensions).Assembly.GetTypes();
            var commandsToRegister = allTypes.Where(t => t.GetInterfaces().Contains(command) && t.FullName != command.FullName);

            var stagesToRegister = allTypes.Where(t => t.GetInterfaces().Contains(typeof(IStage)) && t.FullName != command.FullName);

            foreach (var commandType in commandsToRegister)
            {
                services.AddTransient(commandType);
            }
            logger.Info($"Registered {commandsToRegister.Count()} commands");
            foreach (var stageType in stagesToRegister)
            {
                services.AddTransient(stageType);
            }
            logger.Info($"Registered {stagesToRegister.Count()} stages");

            return services;
        }
    }
}
