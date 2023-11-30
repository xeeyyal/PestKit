using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.Utilities.Extensions;

namespace PestKitAB104.Area.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _context.Departments.ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateDepartmentVM departmentVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = _context.Departments.Any(a => a.Name.Trim() == departmentVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda department movcuddur");
                return View();
            }

            string filename = await departmentVM.Photo.CreateFile(_env.WebRootPath, "admin", "images");
            Department department = new Department
            {
                Name = departmentVM.Name,
                Image=filename
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department is null) return NotFound();

            UpdateDepartmentVM departmentVM = new UpdateDepartmentVM
            {
                Name = department.Name,
                ImgUrl=department.Image
            };

            return View(departmentVM);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateDepartmentVM departmentVM)
        {
            if (!ModelState.IsValid) return View(departmentVM);

            Department existed = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Departments.Any(c => c.Name == departmentVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda department artiq movcuddur");
                return View();
            }
            existed.Name = departmentVM.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Department existed = await _context.Departments.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Departments.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null) return NotFound();

            return View(department);
        }
    }
}
