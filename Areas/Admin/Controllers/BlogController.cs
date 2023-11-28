using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = _context.Blogs.Any(a => a.Title.Trim() == blogVM.Title.Trim());
            if (result)
            {
                ModelState.AddModelError("Blog", "Bu adda blog movcuddur");
                return View();
            }

            Blog blog = new Blog
            {
                Title = blogVM.Title
            };

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Blog existed = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Blogs.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
