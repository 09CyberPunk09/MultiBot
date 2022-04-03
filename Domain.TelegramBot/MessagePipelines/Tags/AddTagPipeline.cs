using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TelegramBot.MessagePipelines.Tags
{
    [Route("/new_tag", "➕ Create Tag")]
    public class AddTagPipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;

        public AddTagPipeline(TagAppService tagService, ILifetimeScope scope) : base(scope)
        {
            _tagService = tagService;
            RegisterStage(AskForTagName);
            RegisterStage(SaveTag);
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