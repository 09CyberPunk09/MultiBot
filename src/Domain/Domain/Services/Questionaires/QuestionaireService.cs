using Application.Services.Questionaires.Dto;
using Common.Entites;
using Common.Entites.Questionaires;
using Infrastructure.Queuing;
using Infrastructure.Queuing.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Persistence.Caching.Redis;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TelegramBot.ChatEngine.Commands.Caching;

namespace Application.Services.Questionaires;

//TODO: Create an interface for every service
public class QuestionaireService : AppService
{
    public const string QUESTIONAIREID_CACHEKEY = "QuestionaireToAnswerId";

    private readonly IRepository<Answer> _answerRepo;
    private readonly IRepository<Question> _questionRepo;
    private readonly IRepository<Questionaire> _questionaireRepo;
    private readonly IRepository<PredefinedAnswer> _predefinedAnswerRepo;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<QuestionaireSession> _questionaireSessionRepository;
    //TODO: Create a Type for piblishing questionaires
    private readonly QueuePublisher _questionaireSchedulerPublisher;
    public QuestionaireService(
        IRepository<Answer> answerRepo,
    IRepository<Question> questionRepo,
    IRepository<Questionaire> questionaireRepo,
    IRepository<PredefinedAnswer> predefinedAnswerRepo,
    IRepository<User> userRepository,
    IRepository<QuestionaireSession> questionaireSessionRepository,
    IConfiguration configuration
        )
    {
        _answerRepo = answerRepo;
        _questionRepo = questionRepo;
        _predefinedAnswerRepo = predefinedAnswerRepo;
        _questionaireRepo = questionaireRepo;
        _userRepository = userRepository;
        _questionaireSessionRepository = questionaireSessionRepository;

        _questionaireSchedulerPublisher = QueuingHelper.CreatePublisher(configuration["Application:Questionaires:ScheduleQuestionaireQueueName"]);

    }

    public void Delete(Guid questionaireId)
    {
        _questionaireRepo.Remove(questionaireId);
    }

    public Questionaire Get(Guid id)
    {
        return _questionaireRepo.Get(id);
    }

    //TODO: Create a telegram functionality service and move this method there
    public void SetQuestionaireForUser(Guid questionaireId, long[] chatIds)
    {
        var cache = new Cache(DatabaseType.Pipelines);
        foreach (var chatId in chatIds)
        {
            var cachedChatData = new CachedChatData
            {
                Data = cache.GetDictionary(chatId.ToString())
            };
            var chatDataFacade = new CachedChatDataWrapper(cachedChatData);
            chatDataFacade.Set(QUESTIONAIREID_CACHEKEY, questionaireId);
            cache.SetDictionary(chatId.ToString(), chatDataFacade.Data.Data);//
        }
    }

    public QuestionaireSession CreateQuestionaireSession(Guid questionaireId)
    {
        return _questionaireSessionRepository.Add(new()
        {
            QuestionaireId = questionaireId,
        });
    }

    public IEnumerable<Questionaire> GetAll(Guid? userId = null)
    {
        return userId == null ? _questionaireRepo.GetAll() : _questionaireRepo.Where(x => x.UserId == userId);
    }

    public void Update(Questionaire questionaire)
    {
        _questionaireRepo.Update(questionaire);
    }

    public Guid Create(CreateQuestionaireDto questionaireDto, List<CreateQuestionDto> questions)
    {
        #region Questionaire Creation
        var questionaire = _questionaireRepo.Add(new()
        {
            Name = questionaireDto.Text,
            IsActive = true,
            UserId = questionaireDto.UserId,
            //TODO: brainstorm how to fix it. Keeping serialized data is bad
            SchedulerExpression = JsonConvert.SerializeObject(questionaireDto.SchedulerExpression)
        });

        int position = 0;
        foreach (var q in questions)
        {
            var insertedQuestion = _questionRepo.Add(new()
            {
                Text = q.Text,
                QuestionaireId = questionaire.Id,
                AnswerType = q.AnswerType,
                RangeMax = q.RangeMax,
                RangeMin = q.RangeMin,
                Position = ++position
            });

            var predefinedAnswers = new List<PredefinedAnswer>();
            foreach (var a in q.PredefinedAnswers)
            {
                predefinedAnswers.Add(_predefinedAnswerRepo.Add(new()
                {
                    Text = a.Text,
                    QuestionId = insertedQuestion.Id
                }));
            }
        }
        #endregion

        var user = _userRepository.Get(questionaireDto.UserId);
        var chatIds = user.TelegramLogIns.Select(x => x.TelegramChatId).ToArray();

        _questionaireSchedulerPublisher.Publish(new SendQuestionaireJobPayload()
        {
            QuestionaireId = questionaire.Id,
            ScheduleExpression = questionaireDto.SchedulerExpression,
            TelegramChatIds = chatIds,
            QuestionaireName = questionaire.Name,
        });

        return questionaire.Id;
    }
}
