using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.DAL;
using PestKitAB104.Interfaces;
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
        private readonly IEmailService _emailService;

        public BasketController(AppDbContext context, LayoutService layoutService, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _layoutService = layoutService;
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basketVM = new List<BasketItemVM>();

            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .ThenInclude(bi => bi.Product)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                foreach (BasketItem item in user.BasketItems)
                {
                    basketVM.Add(new BasketItemVM()
                    {
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Quantity = item.Count,
                        SubTotal = item.Count * item.Product.Price,
                        Id = item.Product.Id
                    });
                }
            }
            else
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
            return View(basketVM);
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
                var basket = await _context.BasketItems.Include(x => x.Product).ToListAsync();
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

        public async Task<IActionResult> RemoveItem(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            List<BasketCookieItemVM> basket;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (user is null) return NotFound();

                BasketItem basketItem = user.BasketItems.FirstOrDefault(bi => bi.ProductId == id);

                if (basketItem is null)
                {
                    return NotFound();
                }
                else
                {
                    user.BasketItems.Remove(basketItem);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                if (Request.Cookies["Basket"] is not null)
                {
                    basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);

                    BasketCookieItemVM item = basket.FirstOrDefault(b => b.Id == id);

                    if (item is not null)
                    {
                        basket.Remove(item);

                        string json = JsonConvert.SerializeObject(basket);
                        Response.Cookies.Append("Basket", json);
                    }
                }
            }
            return Redirect(Request.Headers["Referer"]);
        }
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.Users
                .Include(u => u.BasketItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            OrderVM orderVM = new OrderVM
            {
                BasketItems = user.BasketItems
            };

            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            var user = await _userManager.Users
                .Include(u => u.BasketItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                orderVM.BasketItems = user.BasketItems;
                return View(orderVM);
            }


            decimal total = 0;
            foreach (BasketItem item in user.BasketItems)
            {
                item.Price = item.Product.Price;
                total = item.Count * item.Price;
            }

            Order order = new Order
            {
                Status = null,
                Address = orderVM.Address,
                PurchaseAt = DateTime.Now,
                AppUserId = user.Id,
                BasketItems = user.BasketItems,
                TotalPrice = total
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            string body = @"<table border=""1"">
                              <thead>
                                <tr>
                                  <th>Name</th>
                                  <th>Price</th>
                                  <th>Count</th>
                                </tr>
                              </thead>
                              <tbody>";

            foreach (var item in order.BasketItems)
            {
                body += @$"<tr>
                           <td>{item.Product.Name}</td>
                           <td>{item.Price}</td>
                           <td>{item.Count}</td>
                         </tr>";
            }

            body += @"  </tbody>
                     </table>";

            await _emailService.SendMailAsync(user.Email, "Your Order", body, true);

            return RedirectToAction("Index", "Home");
        }
    }
}
