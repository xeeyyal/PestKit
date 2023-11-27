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

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _context.Authors.ToListAsync();
            return View(authors);
        }

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
    }
}
