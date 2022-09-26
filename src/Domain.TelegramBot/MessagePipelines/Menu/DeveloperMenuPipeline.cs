using Autofac;
using Domain.TelegramBot.MessagePipelines.System;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.Menu
{
    //todo: add to user property isDeveloper and enpoint to set it true
    [Route("/developer_menu", "💻 For Developer")]
    public class DeveloperMenuPipeline : MessagePipelineBase
    {
        public DeveloperMenuPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage((ctx) =>
            {
                var t = new KeyboardButton[][]
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(GetAlternativeRoute<GetApplicationsPipleine>()),
                }
                };
                var rkm = new ReplyKeyboardMarkup(t)
                {
                    ResizeKeyboard = true
                };

                return new()
                {
                    Text = "Menu:",
                    Menu = rkm
                };
            });
        }
    }
}
