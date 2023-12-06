using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;

namespace PestKitAB104.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(b=>b.Author).ToListAsync();
            return View(blogs);
        }
    }
}
