using Autofac;
using Infrastructure.UI.TelegramBot.MessagePipelines;
using Infrastructure.UI.TelegramBot.MessagePipelines.Sets;

namespace Infrastructure.UI.TelegramBot
{
	public class PipelinesModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// todo:rewrite to assembly scanning
			builder.RegisterType<AddNotePipeline>();
			builder.RegisterType<AddSetPipeline>();
			builder.RegisterType<GetNotesPipeline>();
			base.Load(builder);
		}
	}
}
