using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using System.Data;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _context.Positions.ToListAsync();
            return View(positions);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = _context.Authors.Any(a => a.Name.Trim() == positionVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda position movcuddur");
                return View();
            }

            Position position = new Position
            {
                Name = positionVM.Name
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Position position = await _context.Positions.FirstOrDefaultAsync(d => d.Id == id);

            if (position is null) return NotFound();

            UpdatePositionVM positionVM = new UpdatePositionVM
            {
                Name = position.Name,
            };

            return View(positionVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View(positionVM);
            Position existed = await _context.Positions.FirstOrDefaultAsync(d => d.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Positions.Any(c => c.Name == positionVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda position artiq movcuddur");
                return View();
            }
            existed.Name = positionVM.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Position existed = await _context.Positions.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Positions.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Position position = await _context.Positions.FirstOrDefaultAsync(d => d.Id == id);
            if (position == null) return NotFound();

            return View(position);
        }
    }
}
