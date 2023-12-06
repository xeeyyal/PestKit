using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.ViewModels;
using System.Data;

namespace PestKitAB104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            List<Product> products=await _context.Products.ToListAsync();
            return View(products);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            bool result = await _context.Products.AnyAsync(p => p.Name.Trim() == productVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda product movcuddur");
                return View();
            }

            Product product = new Product
            {
                Name = productVM.Name,
                Price= productVM.Price,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(d => d.Id == id);

            if (product is null) return NotFound();

            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = product.Name,
                Price = product.Price,
            };

            return View(productVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateProductVM productVM)
        {
            if (!ModelState.IsValid) return View(productVM);
            Product existed = await _context.Products.FirstOrDefaultAsync(d => d.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Products.Any(c => c.Name == productVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda product artiq movcuddur");
                return View();
            }
            existed.Name = productVM.Name;
            existed.Price = productVM.Price;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Product existed = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Products.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(d => d.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }
    }
}
