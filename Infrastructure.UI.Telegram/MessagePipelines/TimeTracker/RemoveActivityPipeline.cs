using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/remove_activity")]
    public class RemoveActivityPipeline : MessagePipelineBase
    {
        private const string ID_CACHEKEY = "ActivityIdToRemove";
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

            return new()
            {
                Text = "Select activityto remove:",
                Buttons = new(activities.Select(x => InlineKeyboardButton.WithCallbackData(x.Name, x.Id.ToString()))),
            };
        }

        public ContentResult Confirm(MessageContext ctx)
        {
            if (Guid.TryParse(ctx.Message.Text, out var id))
            {
                SetCachedValue(ID_CACHEKEY, id.ToString(), ctx.Recipient);

                return new()
                {
                    Text = "Are you sure you want to delete",
                    Buttons = new(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Yes",true.ToString()),
                        InlineKeyboardButton.WithCallbackData("No",false.ToString()),
                    })
                };
            }
            else
            {
                ForbidMovingNext();
                return Text("Please,select an activity form list");
            }
        }

        public ContentResult Remove(MessageContext ctx)
        {
            if (bool.TryParse(ctx.Message.Text, out var result))
            {
                if (result)
                {
                    _service.RemoveActivity(Guid.Parse(GetCachedValue<string>(ID_CACHEKEY, ctx.Recipient)));
                    return Text("Done");
                }
            }
            return Text("Canceled");
        }
    }
}
