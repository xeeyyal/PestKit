using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;
using PestKitAB104.Models;

namespace PestKitAB104.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return View(products);
        }
    }
}
