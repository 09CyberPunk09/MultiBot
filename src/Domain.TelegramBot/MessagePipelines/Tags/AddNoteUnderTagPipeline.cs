using Application.Services;
using Autofac;
using Infrastructure.TelegramBot.MessagePipelines.Tags.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;

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
            Guid id;
            if (!Guid.TryParse(ctx.Message.Text, out id))
            {
                ctx.MoveNext = false;
                ctx.PipelineStageSucceeded = false;
                return Text("Pick the set you want to modify,do not enter custom text.");
            }
            cache.SetValueForChat("AddSetItemSetId", id, ctx.Recipient);
            return Text("Note text:");
        }

        private ContentResult SaveNote(MessageContext ctx)
        {
            var tagId = cache.GetValueForChat<Guid>("AddSetItemSetId", ctx.Recipient);
            _tagService.CreateNoteUnderTag(tagId, ctx.Message.Text, GetCurrentUser().Id);
            return Text("✅ Note saved");
        }
    }
}