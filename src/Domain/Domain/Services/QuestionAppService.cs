using Common.Entites;
using Microsoft.EntityFrameworkCore;
using Persistence.Master;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class QuestionAppService : AppService
    {
        private readonly LifeTrackerRepository<Question> _questionRepo;
        private readonly LifeTrackerRepository<PredefinedAnswer> _predefinedAnswerRepo;
        private readonly LifeTrackerRepository<Answer> _answerRepo;

        public QuestionAppService(
            LifeTrackerRepository<Question> questionRepo,
            LifeTrackerRepository<PredefinedAnswer> predanswerRepo,
            LifeTrackerRepository<Answer> answerRepo)
        {
            _questionRepo = questionRepo;
            _predefinedAnswerRepo = predanswerRepo;
            _answerRepo = answerRepo;
        }

        public Question Create(Question q, Guid userId)
        {
            q.UserId = userId;
            return _questionRepo.Add(q);
        }

        public Question Get(Guid id)
        {
            return _questionRepo.Get(id);
        }

        public Question Update(Question q)
        {
            return _questionRepo.Update(q);
        }

        public List<PredefinedAnswer> InsertAnswers(List<PredefinedAnswer> answers)
        {
            return answers.Select(a => _predefinedAnswerRepo.Add(a)).ToList();
        }

        public void AddSchedule(Guid questionId, string cron)
        {
            var q = _questionRepo.Get(questionId);
            q.CronExpression = cron;
            _questionRepo.GetTable().Update(q);
            _questionRepo.GetContext().SaveChanges();
        }

        public Answer SaveAnswer(Answer answer)
        {
            return _answerRepo.Add(answer);
        }

        public List<Question> GetQuestions(Guid userId)
        {
            var questions = _questionRepo
                                    .GetTable()
                                    .Include(q => q.PredefinedAnswers)
                                    .Where(q => q.UserId == userId);
            return questions.ToList();
        }

        public List<Question> GetScheduledQuestions()
        {
            return _questionRepo
                       .GetAll()
                       .Where(q => q.CronExpression != null)
                       .ToList();
        }
    }
}