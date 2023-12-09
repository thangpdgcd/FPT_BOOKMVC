using Microsoft.AspNetCore.Mvc;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
