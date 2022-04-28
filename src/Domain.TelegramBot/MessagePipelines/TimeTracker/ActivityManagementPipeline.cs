using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using System;
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
            var activities = _service.GetAllActivities(GetCurrentUser().Id);
            var b = new StringBuilder();
            b.AppendLine("Activities: ");
            int a = 1;

            activities.ForEach(activity =>
            {
                b.AppendLine($"🔸 {a}. {activity.Name}");
                a++;
            });

            return new()
            {
                Text = b.ToString(),
                Buttons = new(new[]
                {
                    //todo: change to use getroute
                    CallbackButton.WithCallbackData("Add","/add_activity"),
                    CallbackButton.WithCallbackData("Remove","/remove_activity")
                }),
            };
        }
    }
}
