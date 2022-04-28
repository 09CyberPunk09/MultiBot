using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.TelegramBot.MessagePipelines.Tags.Chunks
{
    public class GetTagIdChunk : PipelineChunk
    {
        public const string TAGDICTIONARY_CACHEKEY = "TagDictionary";
        private readonly TagAppService _tagService;

        public GetTagIdChunk(TagAppService tagAppService, ILifetimeScope scope) : base(scope)
        {
            _tagService = tagAppService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(AskForSetName);
        }

        public ContentResult AskForSetName(MessageContext ctx)
        {
            var tags = _tagService.GetAll(GetCurrentUser(ctx).Id);

            var b = new StringBuilder();
            b.AppendLine("Enter a number near the tag you want to open:");

            var counter = 0;
            var dictionary = new Dictionary<int, Guid>();

            foreach (var item in tags)
            {
                ++counter;
                b.AppendLine($"🔷 {counter}. {item.Name}");
                dictionary[counter] = item.Id;
            }

            SetCachedValue(TAGDICTIONARY_CACHEKEY, dictionary, ctx.Recipient);

            return new ContentResult()
            {
                Text = b.ToString()
            };
        }
    }
}