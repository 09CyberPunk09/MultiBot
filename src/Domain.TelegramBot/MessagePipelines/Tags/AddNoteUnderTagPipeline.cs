using Application.Services;
using Autofac;
using Infrastructure.TelegramBot.MessagePipelines.Tags.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;

namespace Infrastructure.TelegramBot.MessagePipelines.Tags
{
    [Route("/new_tagged_note", "➕ New Tagged Note")]
    public class AddNoteUnderTagPipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;

        public AddNoteUnderTagPipeline(TagAppService tagService, ILifetimeScope scope) : base(scope)
        {
            _tagService = tagService;
        }

        public override void RegisterPipelineStages()
        {
            IntegrateChunkPipeline<GetTagIdChunk>();
            RegisterStage(AskForNoteText);
            RegisterStage(SaveNote);
        }

        public ContentResult AskForNoteText(MessageContext ctx)
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(GetTagIdChunk.TAGDICTIONARY_CACHEKEY,true);
            if (!(int.TryParse(ctx.Message.Text, out var number) && (number >= 0 && number <= dict.Count)))
            {
                ForbidMovingNext();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var id = dict[number];

            cache.SetValueForChat("AddSetItemSetId", id, ctx.RecipientChatId);
            return Text("Note text:");
        }

        private ContentResult SaveNote(MessageContext ctx)
        {
            var tagId = cache.GetValueForChat<Guid>("AddSetItemSetId", ctx.RecipientChatId);
            _tagService.CreateNoteUnderTag(tagId, ctx.Message.Text, GetCurrentUser().Id);
            return Text("✅ Note saved");
        }
    }
}