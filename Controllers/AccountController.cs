using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PestKitAB104.Models;
using PestKitAB104.ViewModels;

namespace PestKitAB104.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM userVM)
		{
			if (!ModelState.IsValid) return View();

			AppUser user = new AppUser
			{
				Email = userVM.Email,
				Name = userVM.Name,
				surname = userVM.Surname,
				UserName = userVM.Username
			};

			IdentityResult result = await _userManager.CreateAsync(user, userVM.Password);

			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(String.Empty, error.Description);
				}
				return View();
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
