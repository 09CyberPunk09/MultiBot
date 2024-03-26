using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Linq;
using System.Text;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.System
{
    [Route("/get_idle_applications", "🎛 Applications")]
    public class GetApplicationsPipleine : MessagePipelineBase
    {
        private readonly CacheService _service;
        public GetApplicationsPipleine(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<CacheService>();
            RegisterStage(() =>
            {
                var message = new StringBuilder();
                var apps = _service.GetIdleApplications();

                message.AppendLine("Applicaitons:");

                apps
                .ToList()
                .ForEach(a =>
                {
                    message.AppendLine();
                    message.AppendLine($"🔹Name: {a.ApplicationName}");
                    message.AppendLine($"🔹Instance ID: {a.ApplicationInstanceID}");
                    var time = DateTime.Now - a.LastIdleTime;
                    message.AppendLine($"🔹Last Idle: {time.Hours}h {time.Minutes}m {time.Seconds}s ago");
                    var status = time < new TimeSpan(0, 0, 2, 0) ? "🟢Active" : "🔴Inactive";
                    message.AppendLine(status);
                });

                return new()
                {
                    Text = message.ToString()
                };
            });
        }
    }
}
