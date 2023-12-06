using Microsoft.Build.Framework;
using PestKitAB104.Models;
using System.Drawing.Printing;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class CreateEmployeeVM
    {
        [Required]
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public List<Department>? Departments { get; set; }
        public List<CreatePositionVM>? Positions { get; set; }
        public IFormFile? Photo { get; set; }

    }
}
