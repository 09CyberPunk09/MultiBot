using Autofac;
using Domain.TelegramBot.MessagePipelines.System;
using Domain.TelegramBot.MessagePipelines.ToDoList;
using Infrastructure.TelegramBot.MessagePipelines.Notes;
using Infrastructure.TelegramBot.MessagePipelines.Questions;
using Infrastructure.TelegramBot.MessagePipelines.Reminder;
using Infrastructure.TelegramBot.MessagePipelines.Tags;
using Infrastructure.TelegramBot.MessagePipelines.TimeTracker;
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
                var rkm = new ReplyKeyboardMarkup(t);

                return new()
                {
                    Text = "Menu:",
                    Menu = rkm
                };
            });
        }
    }
}
