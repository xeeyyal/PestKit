using Microsoft.AspNetCore.Identity;

namespace PestKitAB104.Models
{
	public class AppUser:IdentityUser
	{
		public string Name { get; set; }
		public string surname { get; set; }
        public List<BasketItem> BasketItems { get; set; }
		List<Order> Orders { get; set; }
    }
}
