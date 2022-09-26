using Autofac;
using Infrastructure.TelegramBot.MessagePipelines.Tags;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Notes
{
    [Route("/notes", "📓 Notes")]
    public class NoteInfoPipeline : MessagePipelineBase
    {
        public NoteInfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage((ctx) => new()
            {
                Text = "Menu of notes",
                Menu = new(new List<List<KeyboardButton>>()
                {
                    new()
                    { 
                        GetAlternativeRoute<AddNotePipeline>(),
                        GetAlternativeRoute<GetNotesPipeline>()
                    },
                    new(){GetAlternativeRoute<TagInfoPipeline>() },
                })
                {
                    ResizeKeyboard = true
                }
            });
        }

    }
}
