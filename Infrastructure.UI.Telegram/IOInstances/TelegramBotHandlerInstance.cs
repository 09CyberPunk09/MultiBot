using Autofac;
using Domain;
using Infrastructure.UI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Persistence.Sql;
using System;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
	public class TelegramBotHandlerInstance : IHandlerInstance
	{
		public IResultSender Sender { get; set; }

		public IMessageReceiver Receiver { get; }

		//todo: remove 
		private ContainerBuilder _containerBuider;


		private void LoadModules()
		{
			_containerBuider = new ContainerBuilder();

			var configurationBuilder = new ConfigurationBuilder()
				.AddJsonFile("config.json");

			_ = _containerBuider.RegisterInstance(configurationBuilder.Build()).SingleInstance();

			//Telegram bot direct deps
			_ = _containerBuider.RegisterInstance<TelegramBotClient>(ConfigureApiClient()).As<ITelegramBotClient>().SingleInstance();
			_ = _containerBuider.RegisterType<TelegramBotMessageReceiver>().As<IMessageReceiver>().SingleInstance();
			_ = _containerBuider.RegisterType<MessageSender>().As<IResultSender>().SingleInstance();


			_ = _containerBuider.RegisterModule<PersistenceModule>();
			_ = _containerBuider.RegisterModule<PipelinesModule>();
			//handlers
			_ = _containerBuider.RegisterModule<DomainModule>();


		}


		public void Start()
		{
			LoadModules();
			var container = _containerBuider.Build();
			//TODO: Resolve
			var accessor = container.Resolve<DependencyAccessor>();


			container.Resolve<IMessageReceiver>().Start();
			//container.Resolve<IResultSender>().Start();

		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		private TelegramBotClient ConfigureApiClient()
		{
			var client = new TelegramBotClient("1740254100:AAGW32c6AWAqilo1xNYLUim5zsgTXn8g9x4") { Timeout = TimeSpan.FromSeconds(10) };
			return client;
		}
	}
}
