using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _context.Authors.ToListAsync();
            return View(authors);
        }
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CreateAuthorVM authorVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = _context.Authors.Any(a => a.Name.Trim() == authorVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda author movcuddur");
                return View();
            }

            Author author=new Author 
            { 
                Name = authorVM.Name
            };

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Author author = await _context.Authors.FirstOrDefaultAsync(d => d.Id == id);

            if (author is null) return NotFound();

            UpdateAuthorVM authorVM = new UpdateAuthorVM
            {
                Name = author.Name,
            };

            return View(authorVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateAuthorVM authorVM)
        {
            if(!ModelState.IsValid) return View(authorVM);
            Author existed = await _context.Authors.FirstOrDefaultAsync(d => d.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Authors.Any(c => c.Name == authorVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda author artiq movcuddur");
                return View();
            }
            existed.Name = authorVM.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Author existed = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Authors.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Author author = await _context.Authors.FirstOrDefaultAsync(d => d.Id == id);
            if (author == null) return NotFound();

            return View(author);
        }
    }
}
