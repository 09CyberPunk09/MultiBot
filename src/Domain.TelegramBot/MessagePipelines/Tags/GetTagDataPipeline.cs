using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Tags
{
    [Route("/get_tag", "📁 Get Tag Data")]
    public class GetTagDataPipeline : MessagePipelineBase
    {
        private const string TAGDICTIONARY_CACHEKEY = "TagsDictionary";

        private readonly TagAppService _tagService;

        public GetTagDataPipeline(TagAppService tagService, ILifetimeScope scope) : base(scope)
        {
            _tagService = tagService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(AskForSetName);
            RegisterStage(ReturnNotes);
        }

        public ContentResult AskForSetName(MessageContext ctx)
        {
            var tags = _tagService.GetAll(GetCurrentUser().Id);

            var b = new StringBuilder();
            b.AppendLine("Enter a number near the tag you want to open:");

            int counter = 0;
            var dictionary = new Dictionary<int, Guid>();

            foreach (var item in tags)
            {
                ++counter;
                b.AppendLine($"🔷 {counter}. {item.Name}");
                dictionary[counter] = item.Id;
            }

            SetCachedValue(TAGDICTIONARY_CACHEKEY, dictionary);

            return new ContentResult()
            {
                Text = b.ToString()
            };
        }

        public ContentResult ReturnNotes(MessageContext ctx)
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(TAGDICTIONARY_CACHEKEY);
            if (!(int.TryParse(ctx.Message.Text, out var number) && (number >= 0 && number <= dict.Count())))
            {
                ForbidMovingNext();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var id = dict[number];
            var tag = _tagService.Get(id);
            var counter = 0;
            var b = new StringBuilder(Environment.NewLine);
            foreach (var item in tag.Notes)
            {
                b.AppendLine($"🔸 {++counter}. {item.Text}");
            }

            return Text($"{tag.Name}: " + b.ToString());
        }
    }
}