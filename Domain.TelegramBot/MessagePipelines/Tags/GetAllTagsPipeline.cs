using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.ComponentModel;
using System.Text;

namespace Infrastructure.TelegramBot.MessagePipelines.Tags
{
    [Route("/get-tags", "📋 List Tags")]
    public class GetAllTagsPipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;

        public GetAllTagsPipeline(TagAppService tagService, ILifetimeScope scope) : base(scope)
        {
            _tagService = tagService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage((ctx) =>
            {
                var tags = _tagService.GetAll(GetCurrentUser().Id);

                int counter = 0;
                StringBuilder b = new(Environment.NewLine);
                foreach (var item in tags)
                {
                    b.AppendLine(++counter + " " + item.Name);
                }

                return Text("All your tags:" + b.ToString());
            });
        }
    }
}