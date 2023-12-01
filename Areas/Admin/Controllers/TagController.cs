using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Tag> tag = await _context.Tags.ToListAsync();
            return View(tag);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM tagVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = _context.Tags.Any(t => t.Name.Trim() == tagVM.Name.Trim());

            if (result)
            {
                ModelState.AddModelError("Tag", "Bu addli tag movcuddur");
                return View();
            }

            Tag tag = new Tag
            {
                Name = tagVM.Name
            };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Tag tag = await _context.Tags.FirstOrDefaultAsync(d => d.Id == id);

            if (tag is null) return NotFound();

            UpdateTagVM tagVM = new UpdateTagVM
            {
                Name = tag.Name,
            };

            return View(tagVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateTagVM tagVM)
        {
            if (!ModelState.IsValid) return View(tagVM);
            Tag existed = await _context.Tags.FirstOrDefaultAsync(d => d.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Authors.Any(c => c.Name == tagVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda tag artiq movcuddur");
                return View();
            }
            existed.Name = tagVM.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            }
            public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Tag existed = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (existed is null) return NotFound();

            _context.Tags.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Tag tag = await _context.Tags.FirstOrDefaultAsync(d => d.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }
    }
}
