using PestKitAB104.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdateAuthorVM
    {
        [Required]
        public string Name { get; set; }
    }
}
