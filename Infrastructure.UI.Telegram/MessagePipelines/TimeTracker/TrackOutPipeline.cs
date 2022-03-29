using Application.Services;
using Autofac;
using Common.Enums;
using Infrastructure.TextUI.Core.Attributes;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.MessagePipelines;
using Infrastructure.TextUI.Core.Types;
using System;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/track_out")]
    //TODO: Add ability to select time where the track was out
    public class TrackOutPipeline : MessagePipelineBase
    {
        private readonly TimeTrackingAppService _service;
        public TrackOutPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<TimeTrackingAppService>();
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(TrackOut);
        }

        public ContentResult TrackOut(MessageContext ctx)
        {
            var user = GetCurrentUser();
            var lastActivity = _service.GetLastTrackedActivity(user.Id);
            _service.Track(lastActivity.Id, EntryType.Out, user.Id);
            return Text($"✅ Stopped tracking at ⏱{DateTime.Now.ToString("HH:mm dd:MM:yyyy")}");
        }
    }
}
