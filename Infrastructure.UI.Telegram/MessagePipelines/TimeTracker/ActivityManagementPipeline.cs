using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Text;
using CallbackButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/activities", "🗄 Activities")]
    public class ActivityManagementPipeline : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public ActivityManagementPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(ListActivities);
        }

        public ContentResult ListActivities(MessageContext ctx)
        {
            StringBuilder sb = new();
            sb.AppendLine("Your Activities");
            sb.AppendLine();
            var activities = _service.GetAllActivities(GetCurrentUser().Id);
            activities.ForEach(activity =>
            {
                sb.AppendLine(activity.Name);
            });

            return new()
            {
                Text = sb.ToString(),
                Buttons = new(new[]
                {
                    CallbackButton.WithCallbackData("Add","/add_activity"),
                    CallbackButton.WithCallbackData("Remove","/remove_activity")
                }),
            };
        }
    }
}
