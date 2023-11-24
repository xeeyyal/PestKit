using Microsoft.AspNetCore.Mvc;

namespace PestKitAB104.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
