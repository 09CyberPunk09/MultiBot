using Common;
using Infrastructure.TelegramBot.IOInstances;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.TelegramBot.Jobs
{
    public class QuestionJobConfiguration : IConfiguredJob
    {
        public IJob Job { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<SendQustionJob>()
                                     .WithIdentity("questionSender" + AdditionalData[SendQustionJob.QuestionId], "telegram-questions");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            .WithIdentity("question-trigger" + AdditionalData[SendQustionJob.QuestionId], "telegram-questions")
            .StartNow()
            .WithCronSchedule(AdditionalData[JobsConsts.cron])
            //.WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(2)).Build();
            .Build();
        }
    }

    public class SendQustionJob : IJob
    {
        public const string QuestionId = "questionId";
        public const string UserId = "userId";
        public const string ChatId = "chatId";
        private readonly MessageResponsePublisher _sender;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SendQustionJob()
        {
            _sender = new();
        }

        public virtual Task Execute(IJobExecutionContext context)
        {
            //TODO: Add better formatting for questions

            _sender.SendMessage(
                new ContentResult()
                {
                    Text = context.JobDetail.JobDataMap[QuestionId] as string,
                    RecipientChatId = Convert.ToInt64(context.JobDetail.JobDataMap[ChatId])
                }
                );
            logger.Trace($"Question {context.JobDetail.JobDataMap[QuestionId]} sent");
            return Task.CompletedTask;
        }
    }
}