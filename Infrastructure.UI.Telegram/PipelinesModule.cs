using Autofac;
using Infrastructure.UI.TelegramBot.MessagePipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.TelegramBot
{
	public class PipelinesModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AddNotePipeline>();
			base.Load(builder);
		}
	}
}
