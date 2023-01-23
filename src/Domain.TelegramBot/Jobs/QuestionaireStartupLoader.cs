using Application.Services.Questionaires.Dto;
using Common;
using Common.Configuration;
using Common.Entites.Questionaires;
using Common.Entites.Scheduling;
using Infrastructure.Queuing;
using Infrastructure.Queuing.Core;
using Newtonsoft.Json;
using Persistence.Common.DataAccess.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Jobs;

/// <summary>
/// Loads questionaires on startup of job executor app
/// </summary>
public class QuestionaireStartupLoader
{
    private readonly IJobExecutor _jobExecutor;
    private readonly IRepository<Questionaire> _questionaireRepository;
    private QueueListener<SendQuestionaireJobPayload> _channelJobScheduler;
    public QuestionaireStartupLoader(IJobExecutor jobExecutor, IRepository<Questionaire> repository)
    {
        _jobExecutor = jobExecutor;
        _questionaireRepository = repository;
    }
    public void ScheduleJobsFromChannel()
    {
        var configuration = ConfigurationHelper.GetConfiguration();
        _channelJobScheduler = QueuingHelper.CreateListener<SendQuestionaireJobPayload>(configuration["Application:Questionaires:ScheduleQuestionaireQueueName"]);
        _channelJobScheduler.AddMessageHandler(channelMessage =>
        {
            var config = new SendQuestionaireJobConfiguration(channelMessage);
            _jobExecutor.ScheduleJob(config);
        });
        _channelJobScheduler.StartConsuming();
    }
    public void ScheduleJobsOnStartup()
    {
        //TODO: Change to service method. calling repos directly is a bad pattern
        var questionaires = _questionaireRepository.Where(q => q.IsActive && !string.IsNullOrEmpty(q.SchedulerExpression)).ToList();
        Parallel.ForEach(questionaires, q =>
        {
            var config = new SendQuestionaireJobConfiguration(new()
            {
                QuestionaireId = q.Id,
                ScheduleExpression = JsonConvert.DeserializeObject<ScheduleExpressionDto>(q.SchedulerExpression),
                QuestionaireName = q.Name,
            });
            _jobExecutor.ScheduleJob(config);
        });
    }
}
