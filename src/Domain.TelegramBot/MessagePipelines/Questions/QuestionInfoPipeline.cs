using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
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
                Menu = new(new List<List<KeyboardButton>>
                {
                    new(){ GetRoute<CreateQuestionPipeline>().AlternativeRoute },
                    new(){ "List questions(not implemented already)" }
                    //TODO: implement question management
                })
                {
                    ResizeKeyboard = true
                }
            });
        }
    }
}
