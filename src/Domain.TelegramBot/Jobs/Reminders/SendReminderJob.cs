﻿using Application.Chatting.Core.Interfaces;
using Application.Services.Questionaires;
using Application.Services.Reminders.Dto;
using Application.Telegram.Implementations;
using Application.TelegramBot.Commands.Core;
using Common;
using Kernel;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Jobs.Reminders;

public class SendReminderJobConfiguration : IConfiguredJob<SendReminderJobPayload>
{
    public SendReminderJobPayload Payload { get; init; }
    public Dictionary<string, string> AdditionalData { get; set; }

    public SendReminderJobConfiguration(SendReminderJobPayload payload)
    {
            AdditionalData = new()
            {
                [TGCHATIDS_KEY] = payload.TelegramChatIds.ToJson(),
                [REMINDER_TEXT] = payload.Text
            };
            Payload = payload;
    }
    public const string TGCHATIDS_KEY = "chatIds";
    public const string REMINDER_TEXT = "ReminderText";

    public IJobDetail BuildJob()
    {
        var builder = JobBuilder.Create<SendReminderJob>();

        foreach (var item in AdditionalData)
        {
            builder.UsingJobData(item.Key, item.Value);
        }
        return builder.Build();
    }

    public ITrigger GetTrigger()
    {
        var builder = TriggerBuilder.Create().StartNow();
        var expression = Payload.ScheduleExpression;
        if (expression.FireAndForget)
        {
            //TODO
            //foreach (var item in expression.FireAndForgetDates)
            //{
            //    builder.WithSimpleSchedule(x => x.)
            //}
        }
        else
        {
            foreach (var cron in expression.Crons)
            {
                builder.WithCronSchedule(cron);
            }
        }

        return builder.Build();
    }
}

public class SendReminderJob : IJob
{
    private readonly IMessageSender<SentTelegramMessage> _sender;

    public SendReminderJob(IMessageSender<SentTelegramMessage> sender)
    {
        _sender = sender;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var chatIds = ((string)context.JobDetail.JobDataMap[SendReminderJobConfiguration.TGCHATIDS_KEY]).FromJson<long[]>();
        var questionaireText = (string)context.JobDetail.JobDataMap[SendReminderJobConfiguration.REMINDER_TEXT];
        List<Task> tasks = new();
        for (int i = 0; i < 4; i++)
        {
            tasks.AddRange(chatIds.Select(x => _sender.SendMessageAsync(new AdressedContentResult()
            {
                ChatId = x,
                Text = $@"
[Reminder]
⏰⏰⏰ 
{questionaireText}",
            })));

        }
        await Task.WhenAll(tasks);
    }
}
