using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Linq;
using CallbackButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/track_in", "📥 Track In")]
    public class TrackInPipeline : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public TrackInPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(AskForActivity);
            RegisterStage(AcceptTrackIn);
        }

        public ContentResult AskForActivity(MessageContext ctx)
        {
            var data = _service.GetAllActivities(GetCurrentUser().Id);
            if (data == null || data.Count == 0)
            {
                var activity = _service.CreateTimeTrackingActivity("Default", GetCurrentUser().Id);
                data = new() { activity };
            }

            return new()
            {
                Text = "Activity you want to track in:",
                Buttons = new(data.Select(a => CallbackButton.WithCallbackData(a.Name, a.Id.ToString())).ToList())
            };
        }

        public ContentResult AcceptTrackIn(MessageContext ctx)
        {
            if (Guid.TryParse(ctx.Message.Text, out var result))
            {
                _service.TrackIn(result, DateTime.Now, GetCurrentUser().Id);
                return Text($"✅Done. Started tracking at ⏱{DateTime.Now.ToString("HH:mm dd:MM:yyyy")}");
            }
            else
            {
                ForbidMovingNext();
                return Text("Select activity from the list");
            }
        }
    }
}
