using Microsoft.Build.Framework;
using PestKitAB104.Models;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class CreateDepartmentVM
    {
        [Required]
        public string Name { get; set; }
        public List<Employee>? Employees { get; set; }
        public IFormFile? Photo { get; set; }

    }
}
