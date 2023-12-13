using Microsoft.AspNetCore.Mvc;

namespace PestKitAB104.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage(string error)
        {
            return View(model:error);
        }
    }
}
