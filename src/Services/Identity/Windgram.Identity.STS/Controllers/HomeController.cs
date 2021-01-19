using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Windgram.Identity.STS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer _localizer;
        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _localizer["Message"];
            return View();
        }
    }
}