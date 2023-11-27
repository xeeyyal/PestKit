using Microsoft.AspNetCore.Mvc;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
