using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Linq;
using System.Text;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Tags
{
    [Route("/get-tags", "📋 List Tags")]
    public class GetAllTagsPipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;

        public GetAllTagsPipeline(TagAppService tagService, ILifetimeScope scope) : base(scope)
        {
            _tagService = tagService;
            RegisterStage(() =>
            {
                var tags = _tagService.GetAll(MessageContext.User.Id)
                                      .Where(t => !t.IsSystem);

                var b = new StringBuilder();
                b.AppendLine("All your tags:");

                var counter = 0;
                foreach (var item in tags)
                {
                    ++counter;
                    b.AppendLine($"🔷 {counter}. {item.Name}");
                }

                return new ContentResult()
                {
                    Text = b.ToString()
                };
            });
        }
    }
}