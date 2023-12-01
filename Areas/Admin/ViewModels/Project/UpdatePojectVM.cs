using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdatePojectVM
    {
        [Required]
        public string Name { get; set; }
    }
}
