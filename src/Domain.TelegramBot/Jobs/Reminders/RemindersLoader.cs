using Application.Services.Reminders.Dto;
using Common;
using Common.Configuration;
using Common.Entites;
using Common.Entites.Scheduling;
using Infrastructure.Queuing;
using Infrastructure.Queuing.Core;
using Newtonsoft.Json;
using Persistence.Common.DataAccess.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Jobs.Reminders;

public class RemindersLoader
{
    private readonly IJobExecutor _jobExecutor;
    private readonly IRepository<Reminder> _reminderRepository;
    private QueueListener<SendReminderJobPayload> _channelJobScheduler;
    public RemindersLoader(IJobExecutor jobExecutor, IRepository<Reminder> repository)
    {
        _jobExecutor = jobExecutor;
        _reminderRepository = repository;
    }
    public void ScheduleJobsFromChannel()
    {
        var configuration = ConfigurationHelper.GetConfiguration();
        _channelJobScheduler = QueuingHelper.CreateListener<SendReminderJobPayload>(configuration["Application:Reminders:ScheduleReminderQueueName"]);
        _channelJobScheduler.AddMessageHandler(channelMessage =>
        {
            var config = new SendReminderJobConfiguration(channelMessage);
            _jobExecutor.ScheduleJob(config);
        });
        _channelJobScheduler.StartConsuming();
    }
    public void ScheduleJobsOnStartup()
    {
        //TODO: Change to service method. calling repos directly is a bad pattern

        var questionaires = _reminderRepository.Where(q => /*/*TODO:q.IsActive &&*/ !string.IsNullOrEmpty(q.SchedulerExpression)).ToList();
        Parallel.ForEach(questionaires, q =>
        {
            var config = new SendReminderJobConfiguration(new()
            {
                ScheduleExpression = JsonConvert.DeserializeObject<ScheduleExpressionDto>(q.SchedulerExpression),
                 Text = q.Name,
                  //TODO: !!!!!!!!!!!!!!!!TelegramChatIds =
                  //IN QUESTION LOADER TOO
            });
            _jobExecutor.ScheduleJob(config);
        });
    }
}
