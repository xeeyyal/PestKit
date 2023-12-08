using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using PestKitAB104.ViewModels;

namespace PestKitAB104.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;
        private HttpRequest _request;

        public LayoutService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
            _request = _http.HttpContext.Request;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
             var settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
             return settings;
        }

        public async Task<List<BasketItemVM>> GetBasketItemsAsync()
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
