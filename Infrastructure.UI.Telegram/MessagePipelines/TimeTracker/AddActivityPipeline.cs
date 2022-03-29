using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.Attributes;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.MessagePipelines;
using Infrastructure.TextUI.Core.Types;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/add_activity")]
    public class AddActivityPipeline : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public AddActivityPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();

            RegisterStage(AskName);
            RegisterStage(Save);
        }

        public ContentResult AskName(MessageContext ctx)
        {
            return Text("Enater new activity name:");
        }

        public ContentResult Save(MessageContext ctx)
        {
            _service.CreateTimeTrackingActivity(ctx.Message.Text,GetCurrentUser().Id);
            return Text("✅ Done");
        }

    }
}
