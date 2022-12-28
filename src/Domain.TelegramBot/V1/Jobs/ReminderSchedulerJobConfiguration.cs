using Application.Services;
using Application.Services.Users;
using Application.Telegram.Implementations;
using Application.TextCommunication.Core.Interfaces;
using Autofac;
using Common;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.Old.Jobs
{
    public class ReminderSchedulerJobConfiguration : IConfiguredJob
    {
        public IJob Job { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; } = new();

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<ScheduleRemindersJob>();
            //.WithIdentity("reminderSenderSetup", "telegram-reminders-setup");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            //.WithIdentity("reminder-setup-trigger", "telegram-reminders-setup")
            .StartNow()
            .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(1)).Build();
        }
    }

    public class ScheduleRemindersJob : IJob
    {
        private ILifetimeScope _scope;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ScheduleRemindersJob(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var executor = _scope.Resolve<IJobExecutor>();
            var reminderService = _scope.Resolve<ReminderAppService>();
            var users = _scope.Resolve<UserAppService>().GetAll();
            var reminders = reminderService
                                            .GetAll()
                                            .Where(r => !r.ReminderTime.HasValue || r.ReminderTime.HasValue && r.ReminderTime > DateTime.Now)
                                            .Where(r => r.SchedulerInstanceId != executor.InstanceId)
                                            .ToList();

            logger.Trace("Started scheduling reminders");
            reminders.ForEach(async r =>
            {
                var currentUser = users.FirstOrDefault(u => u.Id == r.UserId);
                var dictionary = new Dictionary<string, string>
                {
                    { SendReminderJob.CHATID, currentUser.TelegramChatId.ToString() },
                    { SendReminderJob.TEXT,r.Name },
                    { SendReminderJob.REMINDERID, r.Id.ToString() }
                };

                if (r.Recuring)
                {
                    await executor.ScheduleJob(new RecuringReminderJobConfiguration(r.RecuringCron)
                    {
                        AdditionalData = dictionary
                    });
                }
                else
                {
                    if (r.ReminderTime.HasValue)
                    {
                        await executor.ScheduleJob(new FireAndForgetReminderJobConfiguration(r.ReminderTime.Value)
                        {
                            AdditionalData = dictionary
                        });
                    }
                    else
                    {
                        logger.Error($"Reminder {r.Id} has not any schedule");
                    }
                }

                r.SchedulerInstanceId = executor.InstanceId;
                reminderService.Update(r);

                logger.Trace($"Reminder {r.Name} scheduled");
            });
        }
    }

    public class FireAndForgetReminderJobConfiguration : IConfiguredJob
    {
        public Dictionary<string, string> AdditionalData { get; set; }
        public IJob Job { get; set; }
        private readonly DateTime _dateTime;
        public FireAndForgetReminderJobConfiguration(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<SendReminderJob>()
                                     .WithIdentity($"reminder{AdditionalData[SendReminderJob.REMINDERID]}", "telegram-reminders");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            .WithIdentity("reminder", "telegram-reminders")
            .StartAt(_dateTime.ToUniversalTime())
            //.WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(2)).Build();
            .Build();
        }
    }

    public class RecuringReminderJobConfiguration : IConfiguredJob
    {
        public Dictionary<string, string> AdditionalData { get; set; }
        public IJob Job { get; set; }
        private readonly string _cron;
        public RecuringReminderJobConfiguration(string cron)
        {
            _cron = cron;
        }

        public IJobDetail BuildJob()
        {
            var builder = JobBuilder.Create<SendReminderJob>();
            //   .WithIdentity($"reminder{AdditionalData[SendReminderJob.REMINDERID]}", "telegram-reminders");
            foreach (var item in AdditionalData)
            {
                builder.UsingJobData(item.Key, item.Value);
            }
            return builder.Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
            //  .WithIdentity("reminder", "telegram-reminders")
            .StartNow()
            .WithCronSchedule(_cron)
            //.WithSchedule(SimpleScheduleBuilder.).Build();
            .Build();
        }
    }

    public class SendReminderJob : IJob
    {
        public const string TEXT = "text";
        public const string CHATID = "ChatId";
        public const string REMINDERID = "ReminderId";

        private readonly IMessageSender _sender;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public SendReminderJob(IMessageSender sender)
        {
            _sender = sender;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _sender.SendMessage(new AdressedContentResult()
            {
                Text = context.JobDetail.JobDataMap[TEXT] as string,
                ChatId = long.Parse(context.JobDetail.JobDataMap[CHATID] as string),
            });

            return Task.CompletedTask;
        }
    }
}
