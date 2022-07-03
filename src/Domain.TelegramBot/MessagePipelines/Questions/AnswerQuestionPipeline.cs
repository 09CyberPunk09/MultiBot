using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;

namespace Domain.TelegramBot.MessagePipelines.Questions
{
    [Route("/answer_question")]
    public class AnswerQuestionPipeline : MessagePipelineBase
    {
        public const string QUESTIONID_CACHEKEY = "AskedQuestionId";
        private readonly QuestionAppService _service;
        public AnswerQuestionPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<QuestionAppService>();
            RegisterStage(AcceptAnswer);
        }

        public ContentResult AcceptAnswer(MessageContext ctx)
        {
            var questionId = GetCachedValue<Guid>(QUESTIONID_CACHEKEY,true);
            _service.SaveAnswer(new()
            {
                QuestionId = questionId,
                Content = ctx.Message.Text
            });
            return new()
            {
                Text = "✅ Question answered."
            };
        }
    }
}
