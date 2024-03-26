using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.Repsonses;
using Application.Services.Questionaires;
using Application.Services.Questionaires.Dto;
using Application.Telegram.Implementations;
using Application.TelegramBot.Commands.Core;
using Common;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.TelegramBot.Commands.Jobs;

public class SendQuestionaireJobConfiguration : IConfiguredJob<SendQuestionaireJobPayload>
{
    public SendQuestionaireJobConfiguration(SendQuestionaireJobPayload payload)
    {
        AdditionalData = new()
        {
            [QUESTIONAIREID_KEY] = payload.QuestionaireId.ToString(),
            [TGCHATIDS_KEY] = payload.TelegramChatIds.ToJson(),
            [QUESTIONAIRE_TEXT] = payload.QuestionaireName
        };
        Payload = payload;
    }
    public const string QUESTIONAIREID_KEY = "questionaireId";
    public const string TGCHATIDS_KEY = "chatIds";
    public const string QUESTIONAIRE_TEXT = "QuestionaireText";
    public SendQuestionaireJobPayload Payload { get; init; }
    public Dictionary<string, string> AdditionalData { get; set; }

    public IJobDetail BuildJob()
    {
        var builder = JobBuilder.Create<SendQuestionaireJob>();

        builder.UsingJobData(nameof(SendQuestionaireJobPayload.QuestionaireId), Payload.QuestionaireId);

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

public class SendQuestionaireJob : IJob
{
    private readonly IMessageSender<SentTelegramMessage> _sender;
    private readonly QuestionaireService _questionaireService;

    public SendQuestionaireJob(IMessageSender<SentTelegramMessage> sender, QuestionaireService qService)
    {
        _sender = sender;
        _questionaireService = qService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var chatIds = ((string)context.JobDetail.JobDataMap[SendQuestionaireJobConfiguration.TGCHATIDS_KEY]).FromJson<long[]>();
        var questionaireId = Guid.Parse(((string)context.JobDetail.JobDataMap[SendQuestionaireJobConfiguration.QUESTIONAIREID_KEY]));
        var questionaireText = (string)context.JobDetail.JobDataMap[SendQuestionaireJobConfiguration.QUESTIONAIRE_TEXT];
        _questionaireService.SetQuestionaireForUser(questionaireId, chatIds);
        var tasks = chatIds.Select(x => _sender.SendMessageAsync(new AdressedContentResult()
        {
            ChatId = x,
            Text = $"⏰⏰⏰Answer the \"{questionaireText}\" questionaire. Press the button below to answer it",
            Menu = new(MenuType.MessageMenu, new[]
            {
                new[]
                {
                    new Button("🗳 Answer!","/answer_questionaire")
                }
            })
        }));

        await Task.WhenAll(tasks);
    }
}
