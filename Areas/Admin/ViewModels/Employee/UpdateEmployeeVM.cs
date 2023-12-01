using PestKitAB104.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels.Employee
{
    public class UpdateEmployeeVM
    {
        [Required]
        public string Name { get; set; }

    }
}
