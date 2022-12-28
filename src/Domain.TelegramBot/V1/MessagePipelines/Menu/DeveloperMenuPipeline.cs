using Application.TelegramBot.Pipelines.Old.MessagePipelines.System;
using Application.TextCommunication.Core.Routing;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Menu
{
    //todo: add to user property isDeveloper and enpoint to set it true
    [Route("/developer_menu", "💻 For Developer")]
    public class DeveloperMenuPipeline : MessagePipelineBase
    {
        public DeveloperMenuPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(() =>
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
