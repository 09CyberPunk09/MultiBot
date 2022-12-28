using Application.Services;
using Common.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionAppService _service;

        public QuestionController(QuestionAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public Question Create(Question q, Guid userId)
        {
            return _service.Create(q, userId);
        }

        [HttpGet]
        public Question Get(Guid id)
        {
            return _service.Get(id);
        }

        [HttpPut]
        public Question Update(Question q)
        {
            return _service.Update(q);
        }

        [HttpPost]
        public void AddSchedule(Guid questionId, string cron)
        {
            _service.AddSchedule(questionId, cron);
        }

        [HttpGet]
        public List<Question> GetQuestions(Guid userId)
        {
            return _service.GetQuestions(userId);
        }

        [HttpGet]
        public List<Question> GetScheduledQuestions()
        {
            return _service.GetScheduledQuestions();
        }
    }

}
