using Application.Services;
using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Tags
{
    [Route("/new-tagged-note")]
    [Description("Use this command for creating notes")]
    public class AddNoteUnderTagPipeline : MessagePipelineBase
    {
        private readonly TagAppService _tagService;
        public AddNoteUnderTagPipeline(TagAppService tagService,ILifetimeScope scope) : base(scope)
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
            if(!Guid.TryParse(ctx.Message.Text,out id))
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
