using Autofac;
using Infrastructure.UI.Core.MessagePipelines;
using System.Linq;

namespace Infrastructure.UI.TelegramBot
{
    public class PipelinesModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			_ = builder.RegisterTypes(GetType().Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(MessagePipelineBase))).ToArray());
			base.Load(builder);
		}
	}
}
