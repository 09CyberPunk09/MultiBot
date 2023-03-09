using Application.Services;
using Common.Entites;
using LifeTracker.Web.Host.Models.IncomeModels.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NoteController : LifeTrackerController
    {
        private readonly NoteAppService _service;

        public NoteController(NoteAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public Note Get([FromQuery] Guid id)
        {
            return _service.Get(id);
        }

        [HttpPut]
        public Note Update([FromBody] UpdateNoteIncomeModel model)
        {
            return _service.Update(model.Id, model.Text);
        }

        [HttpPost]
        public Note Create([FromBody] CreateNoteIncomeModel model)
        {
            return _service.Create(userId, model.Text);
        }

        [HttpDelete("{id}")]
        public void Remove(Guid id)
        {
            _service.Remove(id);
        }

        [HttpGet]
        public IEnumerable<Note> GetAllByUserId()
        {
            return _service.GetAllByUserId(userId);
        }
    }
}
