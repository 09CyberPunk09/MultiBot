using Application.Telegram.Implementations;
using Application.TelegramBot.Pipelines.Old.Helpers;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Questions;
using Application.TextCommunication.Core.Interfaces;
using Application.TextCommunication.Core.Routing;
using Common;
using Common.Entites;
using Newtonsoft.Json;
using NLog;
using Persistence.Caching.Redis.TelegramCaching;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.TelegramBot.Pipelines.Old.Jobs
{
    //todo: refactor. create a static cretae method which takes in the additionaldata and return a ready-to-use object
    public class QuestionJobConfiguration : IConfiguredJob
    {
        public IJob Job { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<SendQustionJob>()
                                     .WithIdentity("questionSender" + AdditionalData[SendQustionJob.Question], "telegram-questions");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            .WithIdentity("question-trigger" + AdditionalData[SendQustionJob.Question], "telegram-questions")
            .StartNow()
            .WithCronSchedule(AdditionalData[JobsConsts.cron])
            //.WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(2)).Build();
            .Build();
        }
    }

    public class SendQustionJob : IJob
    {
        public const string Question = "question";
        public const string ChatId = "chatId";

        private readonly IMessageSender _sender;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TelegramCache _cache = new();

        public SendQustionJob(IMessageSender sender)
        {
            _sender = sender;
        }

        public virtual Task Execute(IJobExecutionContext context)
        {
            var question = JsonConvert.DeserializeObject<Question>(context.JobDetail.JobDataMap[Question] as string);
            var chatId = Convert.ToInt64(context.JobDetail.JobDataMap[ChatId]);
            var route = typeof(AnswerQuestionPipeline).GetCustomAttribute<RouteAttribute>().Route;

            InlineKeyboardMarkup buttons = default;
            if (question.PredefinedAnswers != null || question.PredefinedAnswers.Count >= 0)
            {
                buttons = new(question.PredefinedAnswers.Select(a =>
                    new[] { InlineKeyboardButton.WithCallbackData($"{a.Content}", $"{a.Content}") }));
            }

            _sender.SendMessage(
                new AdressedContentResult()
                {
                    Text = question.Text,
                    ChatId = chatId,
                    //   Buttons = buttons
                }
            );

            PipelineAssignHelper.SetPipeline<AnswerQuestionPipeline>(chatId);
            _cache.SetValueForChat(AnswerQuestionPipeline.QUESTIONID_CACHEKEY, question.Id, chatId);

            logger.Trace($"Question {question.Id} sent to {chatId}");
            return Task.CompletedTask;
        }
    }
}