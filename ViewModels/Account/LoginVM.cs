using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MaxLength(25,ErrorMessage ="Uzunlugu 25-den cox olmamalidir!")]
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(8,ErrorMessage ="Uzunluugu 8-den cox olmalidir!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemebered { get; set; }
    }
}
