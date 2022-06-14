using Application.Services;
using Common.Entites;
using LifeTracker.Web.Core.Models.IncomeModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TagsController : ControllerBase
    {
        private readonly TagAppService _service;

        public TagsController(TagAppService service)
        {
            _service = service;
        }

        [HttpPut]
        public Tag Update([FromBody] Tag tag)
        {
            return _service.Update(tag);
        }

        [HttpPost]
        public Tag Create([FromBody] CreateTagIncomeModel model)
        {
            return _service.Create(model.Name, model.UserId);
        }

        [HttpGet]
        public Tag Get([FromQuery] Guid id)
        {
            return _service.Get(id);
        }

        [HttpGet]
        public Tag Get([FromQuery] string text, [FromQuery] Guid userId)
        {
            return _service.Get(text, userId);
        }

        [HttpGet]
        public IEnumerable<Tag> GetAll([FromQuery] Guid userId)
        {
            return _service.GetAll(userId);
        }

        [HttpPost]
        public Note CreateNoteUnderTag([FromBody] CreateNoteUnderTagIncomeModel model)
        {
            return _service.CreateNoteUnderTag(model.TagId, model.Text, model.UserId);
        }
    }
}