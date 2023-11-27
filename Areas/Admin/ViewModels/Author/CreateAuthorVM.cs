using PestKitAB104.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class CreateAuthorVM
    {
        [Required]
        public string Name { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
