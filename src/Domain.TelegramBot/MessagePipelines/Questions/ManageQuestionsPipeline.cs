using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.TelegramBot.MessagePipelines.Questions
{
    [Route("/manage_questions", "⚙️ Manage Questions")]
    public class ManageQuestionsPipeline : MessagePipelineBase
    {
        //todo: виокремити виведення питань в окремий папйплайн з командою і викликати його тут
        public const string QUESTIONSORDER_CACHEKEY = "QuestionsOrder";
        private readonly QuestionAppService _service;
        public ManageQuestionsPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(ctx =>
            {
                var questions = _service.GetQuestions(GetCurrentUser().Id);
                var sb  = new StringBuilder();
                sb.AppendLine("Your questions: ");
                int counter = 0;
                var orders = new Dictionary<int, Guid>();
                foreach (var question in questions)
                {
                    sb.AppendLine($"🔶{++counter}. {question.Text}");
                    //todo: add quesiton enabled status, and show it here and additional pipleine to enable\disable it 
                    orders.Add(counter, question.Id);
                    if (question.HasPredefinedAnswers)
                    {
                        foreach (var answer in question.PredefinedAnswers)
                        {
                            sb.AppendLine($"   🔸 {question.Text}");
                        }
                    }
                }


                return new();
            });
        }
    }
}
