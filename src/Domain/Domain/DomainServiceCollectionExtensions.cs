using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Persistence.Master;
using System;
using System.Linq;

namespace Application
{
    public static class DomainServiceCollectionExtensions
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var profiles = AppDomain
                             .CurrentDomain
                             .GetAssemblies();
                cfg.AddMaps(profiles);
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            logger.Info("Mapper registered");
            return services;
        }
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var serviceTypes = typeof(DomainServiceCollectionExtensions).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AppService)));
            foreach (var serviceType in serviceTypes)
            {
                services.AddTransient(serviceType);
            }

            services.AddSqlServerPersistence();
            logger.Info("Services registered");
            return services;
        }
    }
}
