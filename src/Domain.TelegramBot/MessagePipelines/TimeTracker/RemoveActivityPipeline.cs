using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/remove_activity")]
    //todo: make an analyse where we can generalize the element selection
    public class RemoveActivityPipeline : MessagePipelineBase
    {
        private const string ID_CACHEKEY = "ActivityIdToRemove";
        private const string ACTIVITIES_CACHEKEY = "ActivitiesDicitonary";
        private readonly TimeTrackingAppService _service;
        public RemoveActivityPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();

            RegisterStage(AskForActivity);
            RegisterStage(Confirm);
            RegisterStage(Remove);
        }

        public ContentResult AskForActivity(MessageContext ctx)
        {
            var activities = _service.GetAllActivities(GetCurrentUser().Id);
            var b = new StringBuilder();
            b.AppendLine("Activities: ");
            var activitiesDict = new Dictionary<int, Guid>();
            int a = 1;

            activities.ForEach(activity =>
            {
                activitiesDict[a] = activity.Id;

                b.AppendLine($"🔸 {a}. {activity.Name}");

                a++;
            });

            SetCachedValue(ACTIVITIES_CACHEKEY, activitiesDict);

            return new()
            {
                Text = b.ToString(),
                MultiMessages = new()
                {
                    new()
                    {
                        Text = "Now, enter a number near your desicion to delete the activity"
                    }
                }
            };
        }

        public ContentResult Confirm(MessageContext ctx)
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(ACTIVITIES_CACHEKEY,true);
            if (!(int.TryParse(ctx.Message.Text, out var number) && (number >= 0 && number <= dict.Count)))
            {
                ForbidMovingNext();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var id = dict[number];
            SetCachedValue(ID_CACHEKEY, id.ToString());

            return new()
            {
                Text = "Are you sure you want to delete?",
                Buttons = new(new[]
                {
                    InlineKeyboardButton.WithCallbackData("🟩 Yes",true.ToString()),
                    InlineKeyboardButton.WithCallbackData("🟥 No",false.ToString()),
                })
            };
        }

        public ContentResult Remove(MessageContext ctx)
        {
            if (bool.TryParse(ctx.Message.Text, out var result))
            {
                if (result)
                {
                    _service.RemoveActivity(Guid.Parse(GetCachedValue<string>(ID_CACHEKEY)));
                    return Text("✅ Done");
                }
            }
            return Text("Canceled");
        }
    }
}
