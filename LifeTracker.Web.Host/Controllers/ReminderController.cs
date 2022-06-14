using Application.Services;
using Common.Entites;
using LifeTracker.Web.Core.Models.IncomeModels.Reminders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReminderController : ControllerBase
    {
        private readonly ReminderAppService _service;

        public ReminderController(ReminderAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Reminder> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPost]
        public Reminder Create([FromBody] CreateReminderIncomeModel model)
        {
            return _service.Create(model.Reminder, model.UserId);
        }

        [HttpPut]
        public Reminder Update([FromBody] Reminder reminder)
        {
            return _service.Update(reminder);
        }

        [HttpGet]
        public Reminder Get([FromQuery] Guid id)
        {
            return _service.Get(id);
        }
    }
}