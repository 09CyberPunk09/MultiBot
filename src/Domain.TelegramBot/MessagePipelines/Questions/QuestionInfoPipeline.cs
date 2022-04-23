using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Questions
{
    [Route("/question_menu", "❔ Questions")]
    public class QuestionInfoPipeline : MessagePipelineBase
    {
        public QuestionInfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage((ctx) => new()
            {
                Text = "Questions menu",
                Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(GetRoute<CreateQuestionPipeline>().AlternativeRoute),
                    MenuButtonRow("List questions(not implemented already)")
                    //TODO: implement question management
                })
            });
        }
    }
}
