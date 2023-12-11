using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.Services;
using PestKitAB104.ViewModels;
using System.Security.Claims;

namespace PestKitAB104.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _layoutService;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, LayoutService layoutService, UserManager<AppUser> userManager)
        {
            _context = context;
            _layoutService = layoutService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basketItems = new List<BasketItemVM>();

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
                            Quantity = basketCookieItem.Quantity,
                            SubTotal = product.Price * basketCookieItem.Quantity,
                        };
                        basketItems.Add(basketItemVM);
                    }
                }
            }
            return View(basketItems);
        }

        public async Task<IActionResult> AddItemAsync(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            if (User.Identity.IsAuthenticated)
            { //Database ils ishleyen
                AppUser user = await _userManager.Users.Include(u => u.BasketItems.Where(bi => bi.OrderId == null)).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user is null) return NotFound();

                BasketItem item = user.BasketItems.FirstOrDefault(b => b.ProductId == id);

                if (item is null)
                {
                    item = new BasketItem
                    {
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        Price = product.Price,
                        Count = 1,
                    };
                    user.BasketItems.Add(item);
                }
                else
                {
                    item.Count++;
                }

                await _context.SaveChangesAsync();
                var basket = await _context.BasketItems.Include(x=>x.Product).ToListAsync();
                List<BasketItemVM> vms = new();
                foreach (var i in basket)
                {
                    BasketItemVM vm = new()
                    {
                        Id = i.Id,

                        Name = i.Product.Name,
                        Price = i.Price,
                        Quantity = i.Count,
                        SubTotal = i.Price * i.Count

                    };
                    vms.Add(vm);
                }
                return PartialView("_BasketItemPartial", vms);


            }
            else
            {
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

                return PartialView("_BasketItemPartial", basketItems);
            }

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
        public async Task<IActionResult> Checkout()
        {
            return View();
        }
    }
}
