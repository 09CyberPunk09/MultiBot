using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Tags
{
    [Route("/tag_menu", "📂 Tags")]
    public class TagInfoPipeline : MessagePipelineBase
    {
        public TagInfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage((ctx) => new()
            {
                Text = "Tags menu",
                Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(MenuButton(GetAlternativeRoute<AddTagPipeline>()),
                                  MenuButton(GetAlternativeRoute<GetAllTagsPipeline>())),
                    MenuButtonRow(MenuButton(GetAlternativeRoute<AddNoteUnderTagPipeline>()),
                                  MenuButton(GetAlternativeRoute<GetTagDataPipeline>())),
                   // MenuButtonRow("List questions(not implemented already)")
                })
            });
        }
    }
}
