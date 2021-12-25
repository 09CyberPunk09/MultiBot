using Application.Services;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
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
        public AddNoteUnderTagPipeline(TagAppService tagService)
        {
            _tagService = tagService;
        }

        public override void RegisterPipelineStages()
        {
            Stages.Add(AskForSetName);
            Stages.Add(AskForNoteText);
            Stages.Add(SaveNote);
        }


        public ContentResult AskForSetName(MessageContext ctx)
        {
            var tags = _tagService.GetAll(GetCurrentUser().Id);
            
            var markups = new List<InlineKeyboardButton>();
            foreach (var item in tags)
            {
                markups.Add(InlineKeyboardButton.WithCallbackData(item.Name, item.Id.ToString()));
            }

            return new BotMessage()
            {
                Text = "Choose the set you want to extend:",
                Buttons = new InlineKeyboardMarkup(markups.ToArray())
            };
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
