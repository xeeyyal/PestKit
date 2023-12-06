using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
