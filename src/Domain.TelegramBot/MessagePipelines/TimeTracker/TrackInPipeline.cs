using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/track_in", "📥 Track In")]
    public class TrackInPipeline : MessagePipelineBase
    {
        private const string ACTIVITIES_CACHEKEY = "activitiesDictionary";
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


            var b = new StringBuilder();
            b.AppendLine("Enter a number near the Activity you want to track in:");

            var counter = 0;
            var dictionary = new Dictionary<int, Guid>();

            foreach (var item in data)
            {
                ++counter;
                b.AppendLine($"🔸 {counter}. {item.Name}");
                dictionary[counter] = item.Id;
            }

            SetCachedValue(ACTIVITIES_CACHEKEY, dictionary, ctx.Recipient);

            return new ContentResult()
            {
                Text = b.ToString()
            };
        }

        public ContentResult AcceptTrackIn(MessageContext ctx)
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(ACTIVITIES_CACHEKEY);
            if (!(int.TryParse(ctx.Message.Text, out var number) && (number >= 0 && number <= dict.Count)))
            {
                ForbidMovingNext();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var id = dict[number];
            _service.TrackIn(id, DateTime.Now, GetCurrentUser().Id);

            //todo: make track in possible only if the previous entries are complete
            //todo: make track out possible only if the previous entries are not complete
            return Text($"✅Done. Started tracking at ⏱{DateTime.Now:HH:mm dd:MM:yyyy}");
        }
    }
}
