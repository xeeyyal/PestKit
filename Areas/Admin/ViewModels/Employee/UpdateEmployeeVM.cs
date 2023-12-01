using PestKitAB104.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class UpdateEmployeeVM
    {
        [Required]
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }

    }
}
