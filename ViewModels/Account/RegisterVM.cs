using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.ViewModels 
{
	public class RegisterVM
	{
		public string Username { get; set; }
		[Required]
		[MinLength(3,ErrorMessage ="Uzunlugu 3-den cox olmalidir")]
		[MaxLength(25,ErrorMessage ="Uzunlugu 25-den cox olmamalidir")]
		public string Name { get; set; }
		[Required]
		[MinLength(8, ErrorMessage = "Uzunlugu 8-dEn cox olmalidir")]
		[MaxLength(25, ErrorMessage = "Uzunlugu 25-den cox olmamalidir")]
		public string Surname { get; set; }
		[Required]
		[MinLength(15)]
		[MaxLength(25)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[MinLength(8)]
		[MaxLength(25)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[MinLength(8)]
		[MaxLength(25)]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }
		public GenderType Gender { get; set; }
	}
	public enum GenderType
	{
		Male,
		Female
	}
}
