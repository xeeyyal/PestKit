using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PestKitAB104.Models;
using PestKitAB104.Utilities.Extensions.Enum;
using PestKitAB104.ViewModels;

namespace PestKitAB104.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _roleManager = roleManager;
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
			await _userManager.AddToRoleAsync(user, "Member");
			await _signInManager.SignInAsync(user, isPersistent: false);
			return RedirectToAction("Index", "Home");
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginVM loginVM,string? returnUrl)
		{
			if (!ModelState.IsValid) return View();

			AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
				if (user == null)
				{
                    ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect");
                    return View();
                }
			}
			var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemebered, true);
			if (result.IsLockedOut)
			{
				ModelState.AddModelError(String.Empty, "Login failed. Is blocked pls try later");
				return View();
			}
			if (!result.Succeeded)
			{
                ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect");
                return View();
            }
			if(returnUrl is null)
			{
				return RedirectToAction("Index", "Home");
			}
			return Redirect(returnUrl);
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> CreateRoles()
		{
			foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
			{
				if (!await _roleManager.RoleExistsAsync(role.ToString()))
				{
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
