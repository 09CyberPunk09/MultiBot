using Microsoft.AspNetCore.Mvc;

namespace LifeTracker.Web.Host.Controllers
{
    public class LifeTrackerController : ControllerBase
    {
        protected Guid userId { get => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value); }
    }
}
