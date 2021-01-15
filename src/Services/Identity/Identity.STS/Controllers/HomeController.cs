using Microsoft.AspNetCore.Mvc;

namespace Windgram.Identity.STS.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}