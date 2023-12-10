using Microsoft.AspNetCore.Mvc;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
