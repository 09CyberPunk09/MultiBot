using Application.Services;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System;
using System.ComponentModel;
using System.Text;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Tags
{
    [Route("/get-tags")]
    [Description("Use this command for getting tag data")]
    public class GetAllTagsPipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;
        public GetAllTagsPipeline(TagAppService tagService)
        {
            _tagService = tagService;
        }

        public override void RegisterPipelineStages()
        {
            Stages.Add(AskForSetName);
        }

        public ContentResult AskForSetName(MessageContext ctx)
        {
            var tags = _tagService.GetAll(GetCurrentUser().Id);

            int counter = 0;
            StringBuilder b = new(Environment.NewLine);
            foreach (var item in tags)
            {
                b.AppendLine(++counter + " " + item.Name);
            }

            return Text("All your tags:" + b.ToString());
        }
    }
}
