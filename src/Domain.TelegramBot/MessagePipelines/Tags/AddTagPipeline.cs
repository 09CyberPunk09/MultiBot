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

        public ContentResult AskForTagName()
        {
            return Text("Enter Tag name:");
        }

        public ContentResult SaveTag()
        {
            _tagService.Create(MessageContext.Message.Text, MessageContext.User.Id);
            return Text("✅ Tag saved");
        }
    }
}