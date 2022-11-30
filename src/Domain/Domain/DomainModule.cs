using Autofac;
using AutoMapper;
using System;
using System.Linq;

namespace Application
{
    public class DomainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var config = config.GetSection("MyOptions").Get<MyOptions>();
            var mapper = LoadMappers(builder);

            LoadServices(builder, mapper);

            base.Load(builder);
        }

        private void LoadServices(ContainerBuilder builder, IMapper mapper)
        {
            var serviceTypes = GetType().Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AppService))).ToArray();
            foreach (var serviceType in serviceTypes)
            {
                _ = builder.RegisterType(serviceType).WithProperty("Mapper", mapper);
            }
        }

        private IMapper LoadMappers(ContainerBuilder builder)
        {
            var config = new MapperConfiguration(cfg => {
                var profiles = AppDomain
                             .CurrentDomain
                             .GetAssemblies();
                cfg.AddMaps(profiles);
            });
            var mapper = config.CreateMapper();
            builder.RegisterInstance(mapper).SingleInstance();
            return mapper;
        }
    }
}