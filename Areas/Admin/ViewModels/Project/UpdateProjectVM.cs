using PestKitAB104.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdateProjectVM
    {
        [Required]
        public string Name { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public List<ProjectImage>? ProjectImages { get; set; }
        public List<int>? ImageIds { get; set; }
    }
}
