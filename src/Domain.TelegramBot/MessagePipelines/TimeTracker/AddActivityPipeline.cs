using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;

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

        public ContentResult AskName()
        {
            return Text("Enater new activity name:");
        }

        public ContentResult Save()
        {
            _service.CreateTimeTrackingActivity(MessageContext.Message.Text, MessageContext.User.Id);
            return Text("✅ Done");
        }

    }
}
