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
            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = await _context.Authors.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();

                return View();
            }

            bool result = _context.Blogs.Any(a => a.Title.Trim() == blogVM.Title.Trim());
            if (result)
            {
                ViewBag.Authors = await _context.Authors.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ModelState.AddModelError("Blog", "Bu adda blog movcuddur");
                return View();
            }

            string filename = await blogVM.Photo.CreateFile(_env.WebRootPath, "admin", "images");
            Blog blog = new Blog
            {
                Title = blogVM.Title,
                AuthorId = blogVM.AuthorId,
                Description = blogVM.Description,
                DateTime = DateTime.Now,
                ReplyCount = blogVM.ReplyCount,
                Image = filename,
                BlogTags = new()
        };
            foreach (var id in blogVM.TagIds)
            {
                BlogTag bt = new()
                {

                    TagId = id,
                };
        blog.BlogTags.Add(bt);
            }
    await _context.Blogs.AddAsync(blog);
    await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
}

public async Task<IActionResult> Update(int id)
{
    if (id <= 0) return BadRequest();

    Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

    if (blog is null) return NotFound();
    UpdateBlogVM vm = new UpdateBlogVM
    {
        Name = blog.Title,
        AuthorId = blog.AuthorId,
        Description = blog.Description,
        DateTime = blog.DateTime,
        ReplyCount = blog.ReplyCount,
        ImgUrl=blog.Image

    };

    ViewBag.Authors = await _context.Authors.ToListAsync();
    ViewBag.Tags = await _context.Tags.ToListAsync();

    return View(vm);
}

[HttpPost]
public async Task<IActionResult> Update(int id, UpdateBlogVM blogVM)
{
    if (!ModelState.IsValid)
    {
        ViewBag.Authors = await _context.Authors.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();

        return View();
    }


    Blog existed = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);

    if (existed is null) return NotFound();

    bool result = _context.Blogs.Any(c => c.Title == blogVM.Title && c.Id != id);
    if (result)
    {
        ViewBag.Authors = await _context.Authors.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();
        ModelState.AddModelError("Name", "Bu adda blog artiq movcuddur");
        return View();
    }
    existed.Title = blogVM.Title;
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

