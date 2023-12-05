using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.Utilities.Extensions;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

            Project project = await _context.Projects.Include(p=>p.ProjectImages).FirstOrDefaultAsync(d => d.Id == id);

            if (project is null) return NotFound();

            UpdateProjectVM projectVM = new UpdateProjectVM
            {
                Name = project.Name,
                ProjectImages = project.ProjectImages
            };
            return View(projectVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateProjectVM projectVM)
        {
            Project existed = await _context.Projects
                .Include(pi => pi.ProjectImages)

                .FirstOrDefaultAsync(x => x.Id == id);

            projectVM.ProjectImages = existed.ProjectImages;
            if (!ModelState.IsValid) { return View(projectVM); }
            if (existed is null) { return NotFound(); }

            if (projectVM.MainPhoto is not null)
            {
                if (!projectVM.MainPhoto.ValidateType())
                {
                    ModelState.AddModelError("Photo", "File Not supported");
                    return View(projectVM);
                }
                if (!projectVM.MainPhoto.ValidateType())
                {
                    ModelState.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return View(projectVM);
                }
            }


            if (projectVM.MainPhoto is not null)
            {
                string fileName = await projectVM.MainPhoto.CreateFile(_env.WebRootPath, "img");
                ProjectImage prMain = existed.ProjectImages.FirstOrDefault(pi => pi.IsPrimary == true);

                prMain.ImgUrl.DeleteFile(_env.WebRootPath, "img");
                _context.ProjectImages.Remove(prMain);

                existed.ProjectImages.Add(new ProjectImage
                {
                    IsPrimary = true,
                    ImgUrl = fileName
                });
            }



            if (existed.ProjectImages is null) { existed.ProjectImages = new List<ProjectImage>(); }

            if (projectVM.ImageIds is null) projectVM.ImageIds = new List<int>();

            List<ProjectImage> remove = existed.ProjectImages.Where(pi => pi.IsPrimary == null && !projectVM.ImageIds.Exists(imgId => imgId == pi.Id)).ToList();

            foreach (ProjectImage image in remove)
            {
                image.ImgUrl.DeleteFile(_env.WebRootPath, "img");
                existed.ProjectImages.Remove(image);
            }

            if (projectVM.Photos is not null)
            {
                TempData["Message"] = "";

                foreach (IFormFile photo in projectVM.Photos)
                {
                    if (!photo.ValidateType())
                    {
                        TempData["Message"] += $"<p class=\"text-danger\">{photo.Name} type is not suitable</p>";
                        continue;
                    }

                    if (!photo.ValidateSize(10))
                    {
                        TempData["Message"] += $"<p class=\"text-danger\">{photo.Name} the size is not suitable</p>";
                        continue;
                    }

                    existed.ProjectImages.Add(new ProjectImage
                    {
                        IsPrimary = null,
                        ImgUrl = await photo.CreateFile(_env.WebRootPath, "img")
                    });
                }
            }

            existed.Name = projectVM.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Project existed = await _context.Projects.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Projects.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Project project = await _context.Projects.FirstOrDefaultAsync(d => d.Id == id);
            if (project == null) return NotFound();

            return View(project);
        }
    }
}
