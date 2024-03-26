using Application.Services.Reminders;
using Common.Entites;
using LifeTracker.Web.Host.Models.IncomeModels.Reminders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReminderController : ControllerBase
    {
        private readonly ReminderService _service;

        public ReminderController(ReminderService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Reminder> GetAll()
        {
            return _service.GetAll();
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