using Application.Services;
using Autofac;
using Common.Entites;
using Infrastructure.TelegramBot.MessagePipelines.Scheduling.Chunks;
using Infrastructure.TextUI.Core.Attributes;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.MessagePipelines;
using Infrastructure.TextUI.Core.Types;
using Kernel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using CallbackButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;

namespace Infrastructure.TelegramBot.MessagePipelines.Questions
{
    [Route("/new-q")]
    [Description("Use this command for creating questions")]
    public class CreateQuestionPipeline : MessagePipelineBase
    {
        private enum AnswerSelectionOptions
        {
            YesNo = -45634986,
            Confirm,
            CancelLast,
        }

        private const string answersKey = "SelectedAnswers";
        private const string questionTextKey = "QuestionText";
        private const string questionIdKey = "QuestionId";
        private const string hasPredefinedAnswersKey = "HasPredefinedAnswers";

        private readonly QuestionAppService _questionAppService;

        public CreateQuestionPipeline(QuestionAppService qs, ILifetimeScope scope) : base(scope)
        {
            _questionAppService = qs;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(AskForQuestionText);
            RegisterStage(AcceptQuestionTextAndConfirmAnswers);
            RegisterStage(TryAcceptAnswers);
            IntegrateChunkPipeline<CreateScheduleChunk>();
            RegisterStage(SaveSchedule);
        }

        public ContentResult AskForQuestionText(MessageContext ctx)
        {
            return Text("Enter question text:");
        }

        public ContentResult AcceptQuestionTextAndConfirmAnswers(MessageContext ctx)
        {
            string questionText = ctx.Message.Text;
            cache.SetValueForChat(questionTextKey, questionText, ctx.Recipient);

            List<CallbackButton> buttons = new()
            {
                Button("Yes/No", ((int)AnswerSelectionOptions.YesNo).ToString()),
                Button("Confirm", ((int)AnswerSelectionOptions.Confirm).ToString())
            };

            return new BotMessage()
            {
                Text = "Now, let's define the answers for the question.You can choose the type from buttons,or type custom answers.",
                Buttons = new InlineKeyboardMarkup(buttons)
            };
        }

        public ContentResult TryAcceptAnswers(MessageContext ctx)
        {
            string input = ctx.Message.Text;
            if (Enum.TryParse(typeof(AnswerSelectionOptions), input, out object choice))
            {
                switch ((AnswerSelectionOptions)choice)
                {
                    case AnswerSelectionOptions.YesNo:

                        cache.SetValueForChat(answersKey, new List<string>() { "Yes", "No" }, ctx.Recipient);
                        return ConfirmResults(ctx);

                    case AnswerSelectionOptions.Confirm:
                        return ConfirmResults(ctx);

                    default:
                        return Text("Unkonown result");
                }
            }
            else
            {
                cache.SetValueForChat(hasPredefinedAnswersKey, true, ctx.Recipient);
                return AddAnswer(ctx);
            }
        }

        public ContentResult AddAnswer(MessageContext ctx)
        {
            string input = ctx.Message.Text;
            if (Enum.TryParse(typeof(AnswerSelectionOptions), input, out object choice))
            {
                switch ((AnswerSelectionOptions)choice)
                {
                    case AnswerSelectionOptions.Confirm:
                        return ConfirmResults(ctx);

                    case AnswerSelectionOptions.CancelLast:
                        return CancelLast(ctx);

                    default:
                        ForbidMovingNext();
                        return Text("Unkown result");
                }
            }
            else
            {
                ForbidMovingNext();
                var savedAnswers = cache.GetValueForChat<List<string>>(answersKey, ctx.Recipient) ?? new();
                savedAnswers.Add(input);
                cache.SetValueForChat(answersKey, savedAnswers, ctx.Recipient);

                StringBuilder output = savedAnswers.ToListString("Your answers:");

                List<CallbackButton> butotns = new()
                {
                    Button("Confirm", ((int)AnswerSelectionOptions.Confirm).ToString()),
                    Button("Remove last", ((int)AnswerSelectionOptions.CancelLast).ToString())
                };

                return new BotMessage()
                {
                    Text = output.ToString(),
                    Buttons = new InlineKeyboardMarkup(butotns)
                };
            }
        }

        public ContentResult ConfirmResults(MessageContext ctx)
        {
            var predefinedAnswers = cache.GetValueForChat<List<string>>(answersKey, ctx.Recipient) ?? new List<string>();
            var question = _questionAppService.Create(new Question()
            {
                Text = cache.GetValueForChat<string>(questionTextKey, ctx.Recipient),
                HasPredefinedAnswers = predefinedAnswers.Count > 0,
            }, GetCurrentUser().Id);

            SetCachedValue(questionIdKey, question.Id, ctx.Recipient);

            List<PredefinedAnswer> answers = predefinedAnswers
                                    .Select(t => new PredefinedAnswer()
                                    {
                                        Content = t,
                                        Question = question
                                    })
                                    .ToList();
            _questionAppService.InsertAnswers(answers);

            cache.SetValueForChat(answersKey, null, ctx.Recipient);
            return new EmptyResult();
        }

        public ContentResult SaveSchedule(MessageContext ctx)
        {
            var cronExpr = cache.GetValueForChat<string>(CreateScheduleChunk.CRONEXPR_CACHEKEY, ctx.Recipient);
            var questionId = GetCachedValue<Guid>(questionIdKey, ctx.Recipient);
            _questionAppService.AddSchedule(questionId, cronExpr);
            return Text("✅Done. Question saved!");
        }

        public ContentResult CancelLast(MessageContext ctx)
        {
            ForbidMovingNext();
            var savedAnswers = cache.GetValueForChat<List<string>>(answersKey, ctx.Recipient);
            savedAnswers.SmartRemove(savedAnswers[^1]);
            cache.Set(answersKey, savedAnswers);

            StringBuilder output = savedAnswers.ToListString("Your answers:");

            List<CallbackButton> butotns = new()
            {
                Button("Confirm", ((int)AnswerSelectionOptions.Confirm).ToString()),
                Button("Remove last", ((int)AnswerSelectionOptions.CancelLast).ToString())
            };

            return new BotMessage()
            {
                Text = output.ToString(),
                Buttons = new InlineKeyboardMarkup(butotns)
            };
        }
    }
}