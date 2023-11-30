using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels.Position
{
    public class UpdatePositionVM
    {
        [Required]
        public string Name { get; set; }
    }
}
