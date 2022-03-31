using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/create_reminder")]
    public class CreateReminderPipeline : MessagePipelineBase
    {
        public const string REMINDERID_CACHEKEY = "NewlyCreatedReminderId";
        private readonly ReminderAppService _service;
        public CreateReminderPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<ReminderAppService>();

            RegisterStage(AskName);
            RegisterStage(AcceptReminder);
        }

        public ContentResult AskName(MessageContext ctx)
            => Text("Enter reminder text:");
        
        public ContentResult AcceptReminder(MessageContext ctx)
        {
            string text = ctx.Message.Text;
            var res = _service.Create(new()
            {
                Name = text
            },GetCurrentUser().Id);

            SetCachedValue(REMINDERID_CACHEKEY, res.Id, ctx.Recipient);

            return new()
            {
                Text = "✅Gotcha. Do ypu want to make your reminder recurent or fire-and-forget?",
                Buttons = new(new[]
                {
                    InlineKeyboardButton.WithCallbackData("⏱Fire-and-forget",GetCommand<MakeReminderOneTimeFiringPipeline>()),
                    InlineKeyboardButton.WithCallbackData("♻️Recurent",GetCommand<MakeReminderRecurentPipeline>())
                })
            };
        }
    }
}
