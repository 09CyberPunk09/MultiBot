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

        public ContentResult AskForNoteText()
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(GetTagIdChunk.TAGDICTIONARY_CACHEKEY,true);
            if (!(int.TryParse(MessageContext.Message.Text, out var number) && (number >= 0 && number <= dict.Count)))
            {
                Response.ForbidNextStageInvokation();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var id = dict[number];

            cache.SetValueForChat("AddSetItemSetId", id, MessageContext.RecipientChatId);
            return Text("Note text:");
        }

        private ContentResult SaveNote()
        {
            var tagId = cache.GetValueForChat<Guid>("AddSetItemSetId", MessageContext.RecipientChatId);
            _tagService.CreateNoteUnderTag(tagId, MessageContext.Message.Text, MessageContext.User.Id);
            return Text("✅ Note saved");
        }
    }
}