using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdateProjectVM
    {
        [Required]
        public string Name { get; set; }
    }
}
