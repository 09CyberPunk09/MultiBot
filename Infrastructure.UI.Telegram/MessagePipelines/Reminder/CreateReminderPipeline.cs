using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    public class CreateReminderPipeline : MessagePipelineBase
    {
        public CreateReminderPipeline(ILifetimeScope scope) : base(scope)
        {

        }

        public ContentResult AskName(MessageContext ctx)
        {
            return Text("Enter reminder text:");
        }
        public AcceptReminder(MessageContext ctx)
        {
            string text = ctx.Message.Text;
            return Text("Gotcha.Do you want")
        }
    }
}
