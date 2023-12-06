using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Department> departments=await _context.Departments.ToListAsync();
            return View(departments);
        }
        public async Task<IActionResult> Details(int id)
        {
            
                if (id <= 0)
                {
                    return BadRequest();
                }
                bool result = await _context.Departments.AnyAsync(d => d.Id == id);
                if (!result)
                {
                    return NotFound();
                }
                List<Employee> employees = await _context.Employees.Where(e => e.DepartmentId == id).ToListAsync();
            
            return View(employees);
        }
    }
}
