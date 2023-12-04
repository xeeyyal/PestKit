using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
