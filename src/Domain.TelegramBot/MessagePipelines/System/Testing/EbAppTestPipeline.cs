using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.System.Testing
{
    [Route("/webapp_test")]
    public class EbAppTestPipeline : MessagePipelineBase
    {
        public EbAppTestPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(ctx =>
            {
                return new()
                {
                    Text = "Hooray! This is a test of webapp!",
                    Buttons = new(InlineKeyboardButton.WithWebApp("Click ne to test!", new()
                    {
                        Url = "https://glittering-horse-c3d5ad.netlify.app/"
                        //use netlify
                    }))
                };
            });
        }
    }
}
