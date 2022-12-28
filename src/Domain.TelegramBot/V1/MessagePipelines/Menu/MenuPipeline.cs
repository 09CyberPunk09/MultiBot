using Application.TelegramBot.Pipelines.Old.MessagePipelines.Notes;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Questions;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Reminder;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.TimeTracker;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.ToDoList.ToDo;
using Application.TextCommunication.Core.Routing;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Menu
{
    //note: if you are going to change the alternative route,please, change also the copy in the Pipeline type, it cannot be accessed through GetRoute<>() because of assembly cycle reference issue
    //TODO: IMplement getitng a list of done todos
    [Route("/menu", "🏡Home")]
    public class MenuPipeline : MessagePipelineBase
    {
        public MenuPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(() =>
            {
                var t = new KeyboardButton[][]
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(GetAlternativeRoute<TrackerInfoPipeline>()),
                    new KeyboardButton(GetAlternativeRoute<ReminderInfoPipeline>()),
                    new KeyboardButton(GetAlternativeRoute<ToDoinfoPipeline>())
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(GetAlternativeRoute<QuestionInfoPipeline>()),
                    new KeyboardButton(GetAlternativeRoute<NoteInfoPipeline>())
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("List of all endpoints(Not implemented yet)"),
                    //todo:make it dependent of user access level
                    new KeyboardButton(GetAlternativeRoute<DeveloperMenuPipeline>())
                }
                };
                var rkm = new ReplyKeyboardMarkup(t)
                {
                    ResizeKeyboard = true
                };

                return new()
                {
                    Menu = rkm
                };
            });
        }


    }
}
