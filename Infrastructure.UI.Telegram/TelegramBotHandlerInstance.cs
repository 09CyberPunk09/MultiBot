using Autofac;
using Infrastructure.UI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
	public class TelegramBotHandlerInstance : IHandlerInstance
	{
		public IResultSender Sender { get; set; }

		public IMessageReceiver Receiver { get; }

		public void Start()
		{
			ContainerBuilder scopeBuilder = new ContainerBuilder();
			
			var configurationBuilder = new ConfigurationBuilder()
				.AddJsonFile("config.json");

			_ = scopeBuilder.RegisterInstance(configurationBuilder.Build()).SingleInstance();
		
			//Telegram bot direct deps
			_ = scopeBuilder.RegisterInstance<ITelegramBotClient>(ConfigureApiClient()).SingleInstance();
			_ = scopeBuilder.RegisterType<TelegramBotMessageReceiver>().As<IMessageReceiver>().SingleInstance();
			_ = scopeBuilder.RegisterType<MessageSender>().As<IResultSender>().SingleInstance();


		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		private TelegramBotClient ConfigureApiClient()
		{
			var client = new TelegramBotClient("none") { Timeout = TimeSpan.FromSeconds(10) };
			return client;
		}
	}
}
