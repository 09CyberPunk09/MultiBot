using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/reminder_info", "📅 Reminders")]
    public class ReminderInfoPipeline : MessagePipelineBase
    {
        public ReminderInfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage((ctx) => new()
            {
                Text = "Reminders menu",
                Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(GetAlternativeRoute<CreateReminderPipeline>()),
                    MenuButtonRow("List reminders(not implemented already)")
                    //TODO: implement reminder management
                })
            });
        }
    }
}
