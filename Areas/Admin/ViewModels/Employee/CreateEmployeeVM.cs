using Microsoft.Build.Framework;
using PestKitAB104.Models;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class CreateEmployeeVM
    {
        [Required]
        public string Name { get; set; }
        public string? InstaLink { get; set; }
        public string? FbLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? LinkedinLink { get; set; }

        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
