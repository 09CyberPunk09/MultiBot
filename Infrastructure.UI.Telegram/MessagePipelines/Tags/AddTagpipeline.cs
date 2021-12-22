using Application.Services;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Tags
{
    [Route("/new-tag")]
    [Description("Use this command for creating notes")]
    public class AddTagpipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;
        public AddTagpipeline(TagAppService tagService)
        {
            _tagService = tagService;
        }

        public override void RegisterPipelineStages()
        {
            Stages.Add(AskForTagName);
            Stages.Add(SaveTag);
            IsLooped = true;
        }

        public ContentResult AskForTagName(MessageContext ctx)
        {
            return Text("Enter Tag name:");
        }
        public ContentResult SaveTag(MessageContext ctx)
        {
            _tagService.Create(ctx.Message.Text, GetCurrentUser().Id);
            return Text("✅ Tag saved");
        }
    }
}
