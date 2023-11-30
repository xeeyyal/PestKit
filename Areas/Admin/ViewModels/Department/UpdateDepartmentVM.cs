using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdateDepartmentVM
    {
        [Required]
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
