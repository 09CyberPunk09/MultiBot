using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/reminder_info", "📅 Reminders")]
    public class ReminderInfoPipeline : MessagePipelineBase
    {
        public ReminderInfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(Method);
        }

        public ContentResult Method()
        {
            return new()
            {
                Text = "Reminders menu",
                Menu = new(new List<List<KeyboardButton>>
                {
                    new(){ GetAlternativeRoute<CreateReminderPipeline>() },
                    new(){ "List reminders(not implemented already)" }
                    //TODO: implement reminder management
                })
                {
                    ResizeKeyboard = true
                }
            };
        }
    }
}
