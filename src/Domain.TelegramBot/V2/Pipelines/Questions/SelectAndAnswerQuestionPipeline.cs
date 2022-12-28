using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using CallbackButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Questions
{
    [Route("/force-answer")]
    public class SelectAndAnswerQuestionPipeline : MessagePipelineBase
    {
        private const string selectedQustionCacheKey = "SelectedQuestion";
        private readonly QuestionAppService _questionAppService;

        public SelectAndAnswerQuestionPipeline(QuestionAppService qs, ILifetimeScope scope) : base(scope)
        {
            _questionAppService = qs;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(SelectQuestion);
            RegisterStage(SendQuestion);
            RegisterStage(Answer);
        }

        public ContentResult SelectQuestion()
        {
            var questions = _questionAppService.GetQuestions(MessageContext.User.Id);
            var buttons = questions.Select(q => new CallbackButton[] { Button(q.Text, q.Id.ToString()) });
            return new ContentResult()
            {
                Text = "Pick a question to answer:",
                Buttons = new InlineKeyboardMarkup(buttons)
            };
        }

        public ContentResult SendQuestion()
        {
            if (!Guid.TryParse(MessageContext.Message.Text, out Guid input))
            {
                Response.ForbidNextStageInvokation();
                return Text("Please,select the question form the menu");
            }

            var question = _questionAppService.Get(input);
            List<List<CallbackButton>> buttons = new();
            if (question.HasPredefinedAnswers)
            {
                buttons = question.PredefinedAnswers.Select(q => new List<CallbackButton> { Button(q.Content, q.Content) }).ToList();
            }

            cache.SetValueForChat(selectedQustionCacheKey, question.Id, MessageContext.RecipientChatId);

            return new ContentResult()
            {
                Text = question.Text,
                Buttons = new InlineKeyboardMarkup(buttons)
            };
        }

        public ContentResult Answer()
        {
            var question = _questionAppService.Get(cache.GetValueForChat<Guid>(selectedQustionCacheKey, MessageContext.RecipientChatId));
            var answer = MessageContext.Message.Text;
            if (question.HasPredefinedAnswers)
            {
                if (question.PredefinedAnswers.Any(x => x.Content == answer))
                {
                    _questionAppService.SaveAnswer(new()
                    {
                        Content = answer,
                        Question = question,
                    });
                }
                else
                {
                    Response.ForbidNextStageInvokation();
                    return Text("You should select the answer from the menu.");
                }
            }
            else
            {
                _questionAppService.SaveAnswer(new()
                {
                    Content = answer,
                    Question = question,
                });
            }
            return Text("Got it!");
        }
    }
}