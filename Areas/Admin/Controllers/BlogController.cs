using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.Utilities.Extensions;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors=await _context.Authors.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = await _context.Authors.ToListAsync();
                return View();
            }

            bool result = _context.Blogs.Any(a => a.Title.Trim() == blogVM.Title.Trim());
            if (result)
            {
                ViewBag.Authors = await _context.Authors.ToListAsync();
                ModelState.AddModelError("Blog", "Bu adda blog movcuddur");
                return View();
            }

            string filename = await blogVM.Photo.CreateFile(_env.WebRootPath, "admin", "images");
            Blog blog = new Blog
            {
                Title = blogVM.Title,
                AuthorId = blogVM.AuthorId,
                Description= blogVM.Description,
                DateTime= blogVM.DateTime,
                ReplyCount= blogVM.ReplyCount,
                Image=filename
            };

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null) return NotFound();

            return View(blog);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            if (!ModelState.IsValid) return View();

            Blog existed = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Blogs.Any(c => c.Title == blog.Title && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda blog artiq movcuddur");
                return View();
            }
            existed.Title = blog.Title;
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
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(d => d.Id == id);
            if (blog == null) return NotFound();

            return View(blog);
        }
    }
}
