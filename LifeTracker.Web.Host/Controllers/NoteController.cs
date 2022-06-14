using Application.Services;
using Common.Entites;
using LifeTracker.Web.Core.Models.IncomeModels.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NoteController : ControllerBase
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
        public Note Update([FromBody] Note note)
        {
            return _service.Update(note);
        }

        [HttpPost]
        public Note Create([FromBody] CreateNoteIncomeModel model)
        {
            return _service.Create(model.Text, model.UserId);
        }

        [HttpDelete]
        public void RemovePhysically(Note entity)
        {
            _service.RemovePhysically(entity);
        }

        [HttpGet]
        public IEnumerable<Note> GetByUserId([FromQuery] Guid userId)
        {
            return _service.GetByUserId(userId);
        }
    }
}