using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
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
                Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(GetAlternativeRoute<AddNotePipeline>()),
                    MenuButtonRow(GetAlternativeRoute<GetNotesPipeline>())
                })
            });
        }

    }
}
