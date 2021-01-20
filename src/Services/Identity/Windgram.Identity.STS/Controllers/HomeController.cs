using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Windgram.Identity.STS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer _localizer;
        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var accessToken =await HttpContext.GetTokenAsync("access_token");
            var cookies = HttpContext.Request.Cookies;
            ViewData["Message"] = _localizer["Message"];
            return View();
        }
    }
}