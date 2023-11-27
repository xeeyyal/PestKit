using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Area.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
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

            Department department = new Department
            {
                Name = departmentVM.Name
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
