using Microsoft.AspNetCore.Mvc;

namespace Windgram.Blogging.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
    }
}
