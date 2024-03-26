using LifeTracker.FileStorage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.FileStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        public StorageController()
        {

        }

        [HttpPost("UploadFile")]
        public ActionResult UploadFile([FromForm] UploadFilesModel model)
        {

            return Ok();
        }
    }
}
