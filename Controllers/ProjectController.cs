using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            List<Project> projects = await _context.Projects.Include(p => p.ProjectImages).ToListAsync();

            return View(projects);
       
        
        }


        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0) return BadRequest();

            return View();
        }
    }
}
