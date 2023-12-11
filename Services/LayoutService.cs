using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.ViewModels;
using System.Security.Claims;

namespace PestKitAB104.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _http;
        private HttpRequest _request;

        public LayoutService(AppDbContext context, IHttpContextAccessor http, UserManager<AppUser> userManager)
        {
            _context = context;
            _http = http;
            _userManager = userManager;
            _request = _http.HttpContext.Request;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            var settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }

        public async Task<List<BasketItemVM>> GetBasketItemsAsync()
        {
            List<BasketItemVM> basketVM = new List<BasketItemVM>();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .ThenInclude(bi => bi.Product)
                    .FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

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
                List<BasketItemVM> biList;

                if (_request.Cookies["basket"] is not null)
                {
                    List<BasketCookieItemVM> bciList = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(_request.Cookies["basket"]);

                    biList = new();

                    foreach (BasketCookieItemVM basketCookieItem in bciList)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketCookieItem.Id);

                        if (product is not null)
                        {
                            BasketItemVM basketItem = new()
                            {
                                Id = basketCookieItem.Id,
                                Quantity = basketCookieItem.Quantity,
                                Name = product.Name,
                                Price = product.Price,
                                SubTotal = product.Price * basketCookieItem.Quantity
                            };

                            biList.Add(basketItem);
                        }

                    }
                }
                else
                {
                    biList = new();
                }

                return biList;
            }

            return basketVM;

        }

        public async Task<List<BasketItemVM>> GetBasketItemsAsync(List<BasketCookieItemVM> bciList)
        {
            List<BasketItemVM> biList;

            biList = new();

            foreach (BasketCookieItemVM basketCookieItem in bciList)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketCookieItem.Id);

                if (product is not null)
                {
                    BasketItemVM basketItem = new()
                    {
                        Id = basketCookieItem.Id,
                        Quantity = basketCookieItem.Quantity,
                        Name = product.Name,
                        Price = product.Price,
                        SubTotal = product.Price * basketCookieItem.Quantity
                    };

                    biList.Add(basketItem);
                }

            }


            return biList;
        }
    }
}
