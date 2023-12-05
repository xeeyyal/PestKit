using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class CreateProjectVM
    {
        [Required]
        public string Name { get; set; }
        public IFormFile MainPhoto { get; set; }
        [Required]
        public List<IFormFile>? Photos { get; set; }
    }
}
