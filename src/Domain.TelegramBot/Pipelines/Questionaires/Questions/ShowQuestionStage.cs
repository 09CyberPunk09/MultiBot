using Application.Chatting.Core.Repsonses;
using Application.Services.Questionaires.Dto;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;
using Common.Entites.Questionaires;
using ServiceStack.Text;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Questions
{
    public class ShowQuestionStage : ITelegramStage
    {
        public Task<StageResult> Execute(TelegramMessageContext ctx)
        {
            var questionaireDto = ctx.Cache.Get<NewQuestionaireDto>();
            ctx.Cache.Set(questionaireDto);
            var lastQuestion = questionaireDto.Questions.Last();

            var sb = new StringBuilder();
                sb.AppendLine($"🔷 {lastQuestion.Text}");


                switch (lastQuestion.AnswerType)
                {
                    case AnswerType.WithPredefinedAnswers:
                    lastQuestion.PredefinedAnswers.ForEach(y => sb.AppendLine($"     🔹 {y.Text}"));
                        break;
                    case AnswerType.WithoutPredefinedAnswers:
                        break;
                    case AnswerType.Numeric:
                        sb.AppendLine($"🔹 {lastQuestion.NumericRange.Item1} - {lastQuestion.NumericRange.Item2}");
                        break;
                    default:
                        break;
                }

            return ContentResponse.New(new()
            {
                Text = sb.ToString(),
            });
        }
    }
}
