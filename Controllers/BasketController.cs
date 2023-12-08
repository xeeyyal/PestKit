using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.Services;
using PestKitAB104.ViewModels;

namespace PestKitAB104.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _layoutService;

        public BasketController(AppDbContext context, LayoutService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> AddItemAsync(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            List<BasketCookieItemVM> bciList;

            BasketCookieItemVM bci;

            if (Request.Cookies["basket"] is not null)
            {
                bciList = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["basket"]);


                bci = bciList.FirstOrDefault(bci => bci.Id == id);

                if (bci is null)
                {
                    bci = new() { Id = id, Quantity = 1 };

                    bciList.Add(bci);
                }
                else
                {
                    bci.Quantity++;
                }
            }
            else
            {
                bciList = new()
                {
                    new(){ Id=id, Quantity=1}
                };
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(bciList));

            List<BasketItemVM> basketItems = await _layoutService.GetBasketItemsAsync(bciList);

            return RedirectToAction("Index","Home");
        }

        public IActionResult RemoveItem(int id)
        {
            List<BasketCookieItemVM> bciList = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["basket"]);


            BasketCookieItemVM basketCookieItem = bciList.FirstOrDefault(bci => bci.Id == id);

            if (basketCookieItem is not null)
            {
                bciList = bciList.FindAll(bci => bci.Id != id);
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(bciList));

            return RedirectToAction("Index", "Home");
        }
    }
}
