using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/track_out", "📥 Track out")]
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

        public ContentResult TrackOut()
        {
            var user = MessageContext.User;
            var lastActivity = _service.GetLastTrackedActivity(user.Id);
            _service.TrackOut(lastActivity.Id, DateTime.Now);
            return Text($"✅ Stopped tracking at ⏱{DateTime.Now.ToString("HH:mm dd:MM:yyyy")}");
        }
    }
}
