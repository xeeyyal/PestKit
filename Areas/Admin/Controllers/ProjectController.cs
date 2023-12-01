using Microsoft.AspNetCore.Mvc;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateProjectVM projectVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = _context.Projects.Any(a => a.Name.Trim() == projectVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda project movcuddur");
                return View();
            }

            Project project = new Project
            {
                Name = projectVM.Name
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Project project = await _context.Projects.FirstOrDefaultAsync(d => d.Id == id);

            if (project is null) return NotFound();

            UpdatePojectVM pojectVM = new UpdatePojectVM
            {
                Name = project.Name,
            };

            return View(projectVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateAuthorVM authorVM)
        {
            if (!ModelState.IsValid) return View(authorVM);
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
