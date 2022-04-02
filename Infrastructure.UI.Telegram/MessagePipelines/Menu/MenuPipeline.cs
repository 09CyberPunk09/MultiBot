using Autofac;
using Infrastructure.TelegramBot.MessagePipelines.Notes;
using Infrastructure.TelegramBot.MessagePipelines.Questions;
using Infrastructure.TelegramBot.MessagePipelines.Reminder;
using Infrastructure.TelegramBot.MessagePipelines.Tags;
using Infrastructure.TelegramBot.MessagePipelines.TimeTracker;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Menu
{
    //note: if you are going to change the alternative route,please, change also the copy in the Pipeline type, it cannot be accessed through GetRoute<>() because of assembly cycle reference issue
    [Route("/menu", "🏡Home")]
    public class MenuPipeline : MessagePipelineBase
    {
        public MenuPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage((ctx) =>
            {
                var t = new KeyboardButton[][]
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(GetAlternativeRoute<TrackerInfoPipeline>()),
                    new KeyboardButton(GetAlternativeRoute<NoteInfoPipeline>())
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(GetAlternativeRoute<ReminderInfoPipeline>()),
                    new KeyboardButton(GetAlternativeRoute<QuestionInfoPipeline>()),
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(GetAlternativeRoute<TagInfoPipeline>()),
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("List of all endpoints(Not implemented yet)"),
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
