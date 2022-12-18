using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using Telegram.Bot;

namespace Application.TelegramBot.Pipelines
{
    public static class TelegramAPIServiceCollectionExtensions
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static IServiceCollection AddTelegramClient(this IServiceCollection services, string botToken)
        {
            var client = new TelegramBotClient(botToken)
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            services.AddSingleton((ITelegramBotClient)client);
            logger.Info("Telegram API Client registered");
            return services;
        }
    }
}
