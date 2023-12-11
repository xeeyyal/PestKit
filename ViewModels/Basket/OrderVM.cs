using PestKitAB104.Models;

namespace PestKitAB104.ViewModels
{
    public class OrderVM
    {
        public string Address { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
    }
}
