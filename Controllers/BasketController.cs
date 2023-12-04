using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.ViewModels;

namespace PestKitAB104.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basketVM = new List<BasketItemVM>();
            if (Request.Cookies["Basket"] is not null)
            {
                List<BasketCookieItemVM> basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);

                foreach (BasketCookieItemVM basketCookieItem in basket)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketCookieItem.Id);

                    if (product is not null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Count = basketCookieItem.Count,
                            SubTotal = product.Price * basketCookieItem.Count,
                        };

                        basketVM.Add(basketItemVM);
                    }
                }
            }
            return View(basketVM);
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            List<BasketCookieItemVM> basket;

            if (Request.Cookies["Basket"] is not null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);

                BasketCookieItemVM itemVM = basket.FirstOrDefault(b => b.Id == id);
                if (itemVM is null)
                {
                    BasketCookieItemVM basketCookieItemVM = new BasketCookieItemVM
                    {
                        Id = id,
                        Count = 1
                    };

                    basket.Add(basketCookieItemVM);
                }
                else
                {
                    itemVM.Count++;
                }
            }
            else
            {
                basket = new List<BasketCookieItemVM>();

                BasketCookieItemVM basketCookieItemVM = new BasketCookieItemVM
                {
                    Id = id,
                    Count = 1
                };

                basket.Add(basketCookieItemVM);
            }

            string json = JsonConvert.SerializeObject(basket);

            Response.Cookies.Append("Basket", json);

            return RedirectToAction(nameof(Index), "Home");
        }
        
        public async Task<IActionResult> RemoveBasket(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            List<BasketCookieItemVM> basket;
            if (Request.Cookies["Basket"] is not null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);

                var item = basket.FirstOrDefault(b => b.Id == id);
                if (item is not null)
                {
                    basket.Remove(item);

                    string json = JsonConvert.SerializeObject(basket);
                    Response.Cookies.Append("Basket", json);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
